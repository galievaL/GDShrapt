﻿using System.Collections.Generic;
using System.Linq;

namespace GDShrapt.Reader
{
    public sealed class GDParametersExpression : GDExpression, IExpressionsReceiver
    {
        bool _parametersChecked;

        public override int Priority => GDHelper.GetOperationPriority(GDOperationType.Parameters);
        public List<GDExpression> Parameters { get; } = new List<GDExpression>();

        internal override void HandleChar(char c, GDReadingState state)
        {
            if (IsSpace(c))
                return;

            if (c == ',')
            {
                state.Push(new GDExpressionResolver(this));
                return;
            }

            if (c == ')')
            {
                state.Pop();
                return;
            }

            if (!_parametersChecked)
            {
                _parametersChecked = true;
                state.Push(new GDExpressionResolver(this));
                state.PassChar(c);
                return;
            }

            state.Pop();
            state.PassChar(c);
        }

        internal override void HandleLineFinish(GDReadingState state)
        {
            state.Pop();
            state.PassLineFinish();
        }

        public override string ToString()
        {
            if (Parameters.Count == 0)
                return "";

            return $"{string.Join(", ", Parameters.Select(x=> x.ToString()))}";
        }

        public void HandleReceivedToken(GDExpression token)
        {
            throw new System.NotImplementedException();
        }

        public void HandleReceivedExpressionSkip()
        {
            throw new System.NotImplementedException();
        }

        public void HandleReceivedToken(GDComment token)
        {
            throw new System.NotImplementedException();
        }

        public void HandleReceivedToken(GDNewLine token)
        {
            throw new System.NotImplementedException();
        }

        public void HandleReceivedToken(GDSpace token)
        {
            throw new System.NotImplementedException();
        }

        public void HandleReceivedToken(GDInvalidToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}