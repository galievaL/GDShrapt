﻿using System;
using System.Text;

namespace GDShrapt.Reader
{
    public class GDIdentifier : GDDataToken
    {
        public bool IsPi => string.Equals(Sequence, "PI", StringComparison.Ordinal);
        public bool IsTau => string.Equals(Sequence, "TAU", StringComparison.Ordinal);
        public bool IsInfinity => string.Equals(Sequence, "INF", StringComparison.Ordinal);
        public bool IsNaN => string.Equals(Sequence, "NAN", StringComparison.Ordinal);
        public bool IsTrue => string.Equals(Sequence, "true", StringComparison.Ordinal);
        public bool IsFalse => string.Equals(Sequence, "false", StringComparison.Ordinal);
        public bool IsSelf => string.Equals(Sequence, "self", StringComparison.Ordinal);
       
        public string Sequence { get; set; }

        StringBuilder _builder = new StringBuilder();

        internal override void HandleChar(char c, GDReadingState state)
        {
            if (_builder.Length == 0)
            {
                if (IsIdentifierStartChar(c))
                {
                    _builder.Append(c);
                }
                else
                {
                    state.PopAndPass(c);
                }
            }
            else
            {
                if (c == '_' || char.IsLetterOrDigit(c))
                {
                    _builder.Append(c);
                }
                else
                {
                    Sequence = _builder.ToString();
                    state.PopAndPass(c);
                }
            }
            
        }

        internal override void HandleNewLineChar(GDReadingState state)
        {
            if (_builder.Length > 0)
                Sequence = _builder.ToString();

            state.PopAndPassNewLine();
        }

        internal override void ForceComplete(GDReadingState state)
        {
            if (_builder.Length > 0)
                Sequence = _builder.ToString();
            base.ForceComplete(state);
        }

        public override string ToString()
        {
            return $"{Sequence}";
        }
    }
}