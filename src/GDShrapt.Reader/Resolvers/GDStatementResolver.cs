﻿using System.Text;

namespace GDShrapt.Reader
{
    internal class GDStatementResolver : GDIntendedResolver
    {
        bool _waitForNewLine;

        readonly StringBuilder _sequenceBuilder = new StringBuilder();

        new IStatementsReceiver Owner { get; }

        public GDStatementResolver(IStatementsReceiver owner, int lineIntendation)
            : base(owner, lineIntendation)
        {
            Owner = owner;
        }

        internal override void HandleCharAfterIntendation(char c, GDReadingState state)
        {
            if (_waitForNewLine)
            {
                if (IsSpace(c))
                {
                    Owner.HandleReceivedToken(state.Push(new GDSpace()));
                    state.PassChar(c);
                    return;
                }

                Owner.ResolveInvalidToken(c, state, x => !x.IsSpace());
                state.PassChar(c);
                return;
            }

            if (_sequenceBuilder.Length == 0)
            {
                if (IsSpace(c))
                {
                    Owner.HandleReceivedToken(state.Push(new GDSpace()));
                    state.PassChar(c);
                    return;
                }

                if (char.IsLetter(c))
                {
                    _sequenceBuilder.Append(c);
                }
                else
                {
                    Owner.HandleReceivedToken(state.Push(new GDInvalidToken(x => char.IsLetter(x))));
                    state.PassChar(c);
                }
            }
            else
            {
                if (char.IsLetter(c))
                {
                    _sequenceBuilder.Append(c);
                }
                else
                {
                    CompleteAsStatement(state, _sequenceBuilder.ToString());
                    state.PassChar(c);
                }
            }
        }

        internal override void HandleNewLineChar(GDReadingState state)
        {
            if (_sequenceBuilder.Length > 0)
            {
                var sequence = _sequenceBuilder.ToString();
                _sequenceBuilder.Clear();
                CompleteAsStatement(state, sequence);
                state.PassNewLine();
                return;
            }

            if (_waitForNewLine)
                _waitForNewLine = false;
            else
                SendIntendationToOwner();
            Owner.HandleReceivedToken(new GDNewLine());
            ResetIntendation();
        }

        private GDStatement CompleteAsStatement(GDReadingState state, string sequence)
        {
            _sequenceBuilder.Clear();
            _waitForNewLine = true;
            GDStatement statement = null;

            switch (sequence)
            {
                case "if":
                    {
                        var s = new GDIfStatement(LineIntendationThreshold);
                        s.SendKeyword(new GDIfKeyword());
                        statement = s;
                        break;
                    }
                case "for":
                    {
                        var s = new GDForStatement(LineIntendationThreshold);
                        s.SendKeyword(new GDForKeyword());
                        statement = s;
                        break;
                    }
                case "while":
                    {
                        var s = new GDWhileStatement(LineIntendationThreshold);
                        s.SendKeyword(new GDWhileKeyword());
                        statement = s;
                        break;
                    }
                case "match":
                    {
                        var s = new GDMatchStatement(LineIntendationThreshold);
                        s.SendKeyword(new GDMatchKeyword());
                        statement = s;
                        break;
                    }
                case "var":
                    {
                        var s = new GDVariableDeclarationStatement(LineIntendationThreshold);
                        s.SendKeyword(new GDVarKeyword());
                        statement = s;
                        break;
                    }
                default:
                    {
                        return CompleteAsExpressionStatement(state, sequence);
                    }
            }

            SendIntendationToOwner();
            Owner.HandleReceivedToken(statement);
            state.Push(statement);
            return statement;
        }

        private GDExpressionStatement CompleteAsExpressionStatement(GDReadingState state, string sequence)
        {
            var statement = new GDExpressionStatement();

            SendIntendationToOwner();
            Owner.HandleReceivedToken(statement);
            state.Push(statement);

            for (int i = 0; i < sequence.Length; i++)
                state.PassChar(sequence[i]);

            return statement;
        }

        internal override void ForceComplete(GDReadingState state)
        {
            if (_sequenceBuilder.Length > 0)
            {
                var sequence = _sequenceBuilder.ToString();
                _sequenceBuilder.Clear();

                CompleteAsStatement(state, sequence);
                return;
            }

            base.ForceComplete(state);
        }
    }
}