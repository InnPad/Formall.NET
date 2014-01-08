using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Presentation
{
    using Dictionary = Dictionary<string, object>;

    public class AngularSyntaxTransformation : TemplateSyntaxTransformation
    {
        public static implicit operator AngularSyntaxTransformation(Dictionary dictionary)
        {
            return new AngularSyntaxTransformation(dictionary);
        }

        public AngularSyntaxTransformation()
        {
        }

        public AngularSyntaxTransformation(Dictionary dictionary)
            : base(dictionary)
        {
        }

        public override object GetDefaultValue(string key)
        {
            return "{" + key + "}";
        }
    }
}
