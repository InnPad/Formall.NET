using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

// http://erraticdev.blogspot.com/2011/01/custom-aspnet-mvc-route-class-with.html
// http://stackoverflow.com/questions/301230/using-the-greedy-route-parameter-in-the-middle-of-a-route-definition

namespace Formall.Web.Routing
{
    /// <summary>
    /// This route is used for cases where we want greedy route segments anywhere in the route URL definition
    /// </summary>
    public class GreedyRoute : Route
    {
        #region Properties

        /// <summary>Gets the URL pattern for the route.</summary>
        public new string Url { get; private set; }

        private LinkedList<GreedyRouteSegment> urlSegments = new LinkedList<GreedyRouteSegment>();

        private bool hasGreedySegment = false;

        /// <summary>Gets minimum number of segments that this route requires.</summary>
        public int MinRequiredSegments { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GreedyRoute"/> class, using the specified URL pattern and handler class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GreedyRoute(string url, IRouteHandler routeHandler)
            : this(url, null, null, null, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreedyRoute"/> class, using the specified URL pattern, handler class, and default parameter values.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GreedyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : this(url, defaults, null, null, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreedyRoute"/> class, using the specified URL pattern, handler class, default parameter values, and constraints.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GreedyRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : this(url, defaults, constraints, null, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreedyRoute"/> class, using the specified URL pattern, handler class, default parameter values, constraints, and custom values.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. The route handler might need these values to process the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GreedyRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url.Replace("*", ""), defaults, constraints, dataTokens, routeHandler)
        {
            this.Defaults = defaults ?? new RouteValueDictionary();
            this.Constraints = constraints;
            this.DataTokens = dataTokens;
            this.RouteHandler = routeHandler;
            this.Url = url;
            this.MinRequiredSegments = 0;

            // URL must be defined
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Route URL must be defined.", "url");
            }

            // correct URL definition can have AT MOST ONE greedy segment
            if (url.Split('*').Length > 2)
            {
                throw new ArgumentException("Route URL can have at most one greedy segment, but not more.", "url");
            }

            Regex rx = new Regex(@"^(?<isToken>{)?(?(isToken)(?<isGreedy>\*?))(?<name>[a-zA-Z0-9-_]+)(?(isToken)})$", RegexOptions.Compiled | RegexOptions.Singleline);
            foreach (string segment in url.Split('/'))
            {
                // segment must not be empty
                if (string.IsNullOrEmpty(segment))
                {
                    throw new ArgumentException("Route URL is invalid. Sequence \"//\" is not allowed.", "url");
                }

                if (segment.StartsWith("$"))
                {
                    var s = new GreedyRouteSegment
                    {
                        IsToken = false,
                        IsGreedy = false,
                        Name = segment
                    };
                    this.urlSegments.AddLast(s);

                    continue;
                }

                if (rx.IsMatch(segment))
                {
                    Match m = rx.Match(segment);
                    GreedyRouteSegment s = new GreedyRouteSegment
                    {
                        IsToken = m.Groups["isToken"].Value.Length.Equals(1),
                        IsGreedy = m.Groups["isGreedy"].Value.Length.Equals(1),
                        Name = m.Groups["name"].Value
                    };
                    this.urlSegments.AddLast(s);
                    this.hasGreedySegment |= s.IsGreedy;

                    continue;
                }
                throw new ArgumentException("Route URL is invalid.", "url");
            }

            // get minimum required segments for this route
            LinkedListNode<GreedyRouteSegment> seg = this.urlSegments.Last;
            int sIndex = this.urlSegments.Count;
            while (seg != null && this.MinRequiredSegments.Equals(0))
            {
                if (!seg.Value.IsToken || !this.Defaults.ContainsKey(seg.Value.Name))
                {
                    this.MinRequiredSegments = Math.Max(this.MinRequiredSegments, sIndex);
                }
                sIndex--;
                seg = seg.Previous;
            }

            // check that segments after greedy segment don't define a default
            if (this.hasGreedySegment)
            {
                LinkedListNode<GreedyRouteSegment> s = this.urlSegments.Last;
                while (s != null && !s.Value.IsGreedy)
                {
                    if (s.Value.IsToken && this.Defaults.ContainsKey(s.Value.Name))
                    {
                        throw new ArgumentException(string.Format("Defaults for route segment \"{0}\" is not allowed, because it's specified after greedy catch-all segment.", s.Value.Name), "defaults");
                    }
                    s = s.Previous;
                }
            }
        }

        #endregion

        #region GetRouteData
        /// <summary>
        /// Returns information about the requested route.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        /// An object that contains the values from the route definition.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + (httpContext.Request.PathInfo ?? string.Empty);

            RouteValueDictionary values = this.ParseRoute(httpContext, virtualPath);
            if (values == null)
            {
                return null;
            }

            RouteData result = new RouteData(this, this.RouteHandler);
            if (!this.ProcessConstraints(httpContext, values, RouteDirection.IncomingRequest))
            {
                return null;
            }

            // everything's fine, fill route data
            foreach (KeyValuePair<string, object> value in values)
            {
                result.Values.Add(value.Key, value.Value);
            }
            if (this.DataTokens != null)
            {
                foreach (KeyValuePair<string, object> token in this.DataTokens)
                {
                    result.DataTokens.Add(token.Key, token.Value);
                }
            }
            return result;
        }
        #endregion

        #region GetVirtualPath
        /// <summary>
        /// Returns information about the URL that is associated with the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains information about the URL that is associated with the route.
        /// </returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            RouteUrl url = this.Bind(requestContext.RouteData.Values, values);
            if (url == null)
            {
                return null;
            }
            if (!this.ProcessConstraints(requestContext.HttpContext, url.Values, RouteDirection.UrlGeneration))
            {
                return null;
            }

            VirtualPathData data = new VirtualPathData(this, url.Url);
            if (this.DataTokens != null)
            {
                foreach (KeyValuePair<string, object> pair in this.DataTokens)
                {
                    data.DataTokens[pair.Key] = pair.Value;
                }
            }
            return data;
        }
        #endregion

        #region Private methods

        #region ProcessConstraints
        /// <summary>
        /// Processes constraints.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="values">Route values.</param>
        /// <param name="direction">Route direction.</param>
        /// <returns><c>true</c> if constraints are satisfied; otherwise, <c>false</c>.</returns>
        private bool ProcessConstraints(HttpContextBase httpContext, RouteValueDictionary values, RouteDirection direction)
        {
            if (this.Constraints != null)
            {
                foreach (KeyValuePair<string, object> constraint in this.Constraints)
                {
                    if (!this.ProcessConstraint(httpContext, constraint.Value, constraint.Key, values, direction))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region ParseRoute
        /// <summary>
        /// Parses the route into segment data as defined by this route.
        /// </summary>
        /// <param name="virtualPath">Virtual path.</param>
        /// <returns>Returns <see cref="System.Web.Routing.RouteValueDictionary"/> dictionary of route values.</returns>
        private RouteValueDictionary ParseRoute(HttpContextBase httpContext, string virtualPath)
        {
            var parts = new Stack<string>(
                virtualPath
                .Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Reverse() // we have to reverse it because parsing starts at the beginning not the end.
            );

            // number of request route parts must match route URL definition
            if (parts.Count < this.MinRequiredSegments)
            {
                return null;
            }

            var prospect = new RouteValueDictionary();

            // start parsing from the beginning
            bool finished = false;

            LinkedListNode<GreedyRouteSegment> currentSegment;

            for (currentSegment = this.urlSegments.First; !finished && !currentSegment.Value.IsGreedy; finished = null == (currentSegment = currentSegment.Next))
            {
                object p = parts.Count > 0 ? parts.Pop() : null;
                if (currentSegment.Value.IsToken)
                {
                    p = p ?? this.Defaults[currentSegment.Value.Name];
                    prospect.Add(currentSegment.Value.Name, p);
                }
                else if (!currentSegment.Value.Name.Equals(p as string, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            var trail = new Stack<string>(parts); // this will reverse stack elements
            var result = new RouteValueDictionary(prospect);

            for (var lastSegment = this.urlSegments.Last; !finished && lastSegment != null && !lastSegment.Value.IsGreedy; lastSegment = lastSegment.Previous)
            {
                // continue from the end if needed
                trail = new Stack<string>(parts); // this will reverse stack elements
                result = new RouteValueDictionary(prospect);

                for (currentSegment = lastSegment; !finished && !currentSegment.Value.IsGreedy; finished = null == (currentSegment = currentSegment.Previous))
                {
                    object p = trail.Count > 0 ? trail.Pop() : null;
                    if (currentSegment.Value.IsToken)
                    {
                        p = p ?? this.Defaults[currentSegment.Value.Name];
                        result.Add(currentSegment.Value.Name, p);
                    }
                    else if (!currentSegment.Value.Name.Equals(p as string, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }

                if (this.ProcessConstraints(httpContext, result, RouteDirection.IncomingRequest))
                {
                    break;
                }

                if (!this.Defaults.ContainsKey(lastSegment.Value.Name))
                {
                    return null;
                }

                trail = new Stack<string>(parts); // this will reverse stack elements
                result = new RouteValueDictionary(prospect);
            }

            // fill in the greedy catch-all segment
            if (!finished)
            {
                object remaining = string.Join("/", trail.Reverse().ToArray()) ?? this.Defaults[currentSegment.Value.Name];
                result.Add(currentSegment.Value.Name, remaining);
            }

            // add remaining default values
            foreach (var def in this.Defaults)
            {
                if (!result.ContainsKey(def.Key))
                {
                    result.Add(def.Key, def.Value);
                }
            }

            return result;
        }
        #endregion

        #region Bind
        /// <summary>
        /// Binds the specified current values and values into a URL.
        /// </summary>
        /// <param name="currentValues">Current route data values.</param>
        /// <param name="values">Additional route values that can be used to generate the URL.</param>
        /// <returns>Returns a URL route string.</returns>
        private RouteUrl Bind(RouteValueDictionary currentValues, RouteValueDictionary values)
        {
            currentValues = currentValues ?? new RouteValueDictionary();
            values = values ?? new RouteValueDictionary();

            HashSet<string> required = new HashSet<string>(this.urlSegments.Where(seg => seg.IsToken).ToList().ConvertAll(seg => seg.Name), StringComparer.OrdinalIgnoreCase);
            RouteValueDictionary routeValues = new RouteValueDictionary();

            object dataValue = null;
            foreach (string token in new List<string>(required))
            {
                dataValue = values[token] ?? currentValues[token] ?? this.Defaults[token];
                if (this.IsUsable(dataValue))
                {
                    string val = dataValue as string;
                    if (val != null)
                    {
                        val = val.StartsWith("/") ? val.Substring(1) : val;
                        val = val.EndsWith("/") ? val.Substring(0, val.Length - 1) : val;
                    }
                    routeValues.Add(token, val ?? dataValue);
                    required.Remove(token);
                }
            }

            // this route data is not related to this route
            if (required.Count > 0)
            {
                return null;
            }

            // add all remaining values
            foreach (KeyValuePair<string, object> pair1 in values)
            {
                if (this.IsUsable(pair1.Value) && !routeValues.ContainsKey(pair1.Key))
                {
                    routeValues.Add(pair1.Key, pair1.Value);
                }
            }

            // add remaining defaults
            foreach (KeyValuePair<string, object> pair2 in this.Defaults)
            {
                if (this.IsUsable(pair2.Value) && !routeValues.ContainsKey(pair2.Key))
                {
                    routeValues.Add(pair2.Key, pair2.Value);
                }
            }

            // check that non-segment defaults are the same as those provided
            RouteValueDictionary nonRouteDefaults = new RouteValueDictionary(this.Defaults);
            foreach (GreedyRouteSegment seg in this.urlSegments.Where(ss => ss.IsToken))
            {
                nonRouteDefaults.Remove(seg.Name);
            }
            foreach (KeyValuePair<string, object> pair3 in nonRouteDefaults)
            {
                if (!routeValues.ContainsKey(pair3.Key) || !this.RoutePartsEqual(pair3.Value, routeValues[pair3.Key]))
                {
                    // route data is not related to this route
                    return null;
                }
            }

            StringBuilder sb = new StringBuilder();
            RouteValueDictionary valuesToUse = new RouteValueDictionary(routeValues);
            bool mustAdd = this.hasGreedySegment;

            // build URL string
            LinkedListNode<GreedyRouteSegment> s = this.urlSegments.Last;
            object segmentValue = null;
            while (s != null)
            {
                if (s.Value.IsToken)
                {
                    segmentValue = valuesToUse[s.Value.Name];
                    mustAdd = mustAdd || !this.RoutePartsEqual(segmentValue, this.Defaults[s.Value.Name]);
                    valuesToUse.Remove(s.Value.Name);
                }
                else
                {
                    segmentValue = s.Value.Name;
                    mustAdd = true;
                }

                if (mustAdd)
                {
                    sb.Insert(0, sb.Length > 0 ? "/" : string.Empty);
                    sb.Insert(0, Uri.EscapeUriString(Convert.ToString(segmentValue, CultureInfo.InvariantCulture)));
                }

                s = s.Previous;
            }

            // add remaining values
            if (valuesToUse.Count > 0)
            {
                bool first = true;
                foreach (KeyValuePair<string, object> pair3 in valuesToUse)
                {
                    // only add when different from defaults
                    if (!this.RoutePartsEqual(pair3.Value, this.Defaults[pair3.Key]))
                    {
                        sb.Append(first ? "?" : "&");
                        sb.Append(Uri.EscapeDataString(pair3.Key));
                        sb.Append("=");
                        sb.Append(Uri.EscapeDataString(Convert.ToString(pair3.Value, CultureInfo.InvariantCulture)));
                        first = false;
                    }
                }
            }

            return new RouteUrl
            {
                Url = sb.ToString(),
                Values = routeValues
            };
        }
        #endregion

        #region IsUsable
        /// <summary>
        /// Determines whether an object actually is instantiated or has a value.
        /// </summary>
        /// <param name="value">Object value to check.</param>
        /// <returns>
        ///     <c>true</c> if an object is instantiated or has a value; otherwise, <c>false</c>.
        /// </returns>
        private bool IsUsable(object value)
        {
            string val = value as string;
            if (val != null)
            {
                return val.Length > 0;
            }
            return value != null;
        }
        #endregion

        #region RoutePartsEqual
        /// <summary>
        /// Checks if two route parts are equal
        /// </summary>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        /// <returns><c>true</c> if both values are equal; otherwise, <c>false</c>.</returns>
        private bool RoutePartsEqual(object firstValue, object secondValue)
        {
            string sFirst = firstValue as string;
            string sSecond = secondValue as string;
            if ((sFirst != null) && (sSecond != null))
            {
                return string.Equals(sFirst, sSecond, StringComparison.OrdinalIgnoreCase);
            }
            if ((firstValue != null) && (secondValue != null))
            {
                return firstValue.Equals(secondValue);
            }
            return (firstValue == secondValue);
        }
        #endregion

        #endregion
    }
}
