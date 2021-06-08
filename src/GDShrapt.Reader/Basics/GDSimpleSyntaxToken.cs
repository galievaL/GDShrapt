﻿using System.Text;

namespace GDShrapt.Reader
{
    public abstract class GDSimpleSyntaxToken : GDSyntaxToken
    {
        public override void AppendTo(StringBuilder builder)
        {
            builder.Append(ToString());
        }

        internal override void ForceComplete(GDReadingState state)
        {
            state.DropReadingToken();
        }

        internal override void HandleLineFinish(GDReadingState state)
        {
            state.DropReadingToken();
            state.PassLineFinish();
        }

        internal override void HandleSharpChar(GDReadingState state)
        {
            state.DropReadingToken();
            state.PassChar('#');
        }
    }
}
