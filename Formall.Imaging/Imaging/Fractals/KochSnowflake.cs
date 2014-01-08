using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formall.Imaging.Fractals
{
    public class KochSnowflake : AbstractFractal
    {
        public KochSnowflake(byte iterations)
            : base(iterations)
        {
        }

        public override bool Closed { get { return true; } }

        protected override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
