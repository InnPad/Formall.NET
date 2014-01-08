using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Navigation
{
    public class RouteOption
    {
        public CultureInfo Culture
        {
            get;
            set;
        }

        public string Original
        {
            get;
            set;
        }

        public string Pattern
        {
            get;
            set;
        }

        public string Redirect
        {
            get;
            set;
        }

        public static IEnumerable<RouteOption> FromHost(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                yield return new RouteOption { Original = host, Pattern = "*" };
                yield break;
            }

            var original = host;

            host = host.ToLower();

            var absolute = host.Split('.');

            CultureInfo culture = null;
            string redirect = null;

            var tag = absolute[0].Split('-');

            switch (tag.Length)
            {
                case 1:
                    try
                    {
                        culture = CultureInfo.GetCultureInfoByIetfLanguageTag(tag[0]);
                    }
                    catch (CultureNotFoundException)
                    {
                    }
                    break;

                case 2:
                    try
                    {
                        culture = CultureInfo.GetCultureInfoByIetfLanguageTag(tag[0] + '-' + tag[1].ToUpper());
                    }
                    catch (CultureNotFoundException)
                    {
                        try
                        {
                            culture = CultureInfo.GetCultureInfoByIetfLanguageTag(tag[0]);
                            redirect = culture.Name + '.' + string.Join(".", absolute.Skip(1));
                        }
                        catch (CultureNotFoundException)
                        {
                        }
                    }
                    break;
            }

            if (culture != null)
            {
                absolute = absolute.Skip(1).ToArray();
            }

            if (absolute.Length < 2)
            {
                yield return new RouteOption { Culture = culture, Original = host, Pattern = "*" };
                yield break;
            }

            string[] relative = null;

            if (absolute.Last() != "*")
            {
                relative = new string[absolute.Length - 1];
                Array.Copy(absolute, relative, relative.Length - 1);
                relative[relative.Length - 1] = "*";
            }

            yield return new RouteOption { Culture = culture, Original = original, Pattern = string.Join(".", absolute), Redirect = redirect };
            yield return new RouteOption { Culture = culture, Original = original, Pattern = string.Join(".", relative), Redirect = redirect };

            for (var i = 0; i < absolute.Length - 2; i++)
            {
                if (i > 0)
                {
                    redirect = string.Join(".", absolute.Skip(i));

                    if (culture != null)
                    {
                        redirect = culture.Name + '.' + redirect;
                    }
                }

                absolute[i] = "*";
                yield return new RouteOption { Culture = culture, Original = original, Pattern = string.Join(".", absolute.Skip(i)), Redirect = redirect };

                if (relative != null)
                {
                    relative[i] = "*";
                    yield return new RouteOption { Culture = culture, Original = original, Pattern = string.Join(".", relative.Skip(i)), Redirect = redirect };
                }
            }

            redirect = absolute.Length > 2 ? string.Join(".", absolute.Skip(absolute.Length - 2)) : null;

            if (culture != null)
            {
                redirect = culture.Name + '.' + redirect;
            }

            if (relative != null)
            {
                yield return new RouteOption { Culture = culture, Original = original, Pattern = string.Join(".", absolute.Skip(absolute.Length - 2)), Redirect = redirect };
            }

            yield return new RouteOption { Culture = culture, Original = original, Pattern = "*", Redirect = redirect };

            yield break;
        }
    }
}
