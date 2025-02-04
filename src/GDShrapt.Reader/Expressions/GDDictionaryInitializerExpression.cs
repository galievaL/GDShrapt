﻿using System.Collections.Generic;
using System.Linq;

namespace GDShrapt.Reader
{
    public class GDDictionaryInitializerExpression : GDExpression
    {
        public override int Priority => GDHelper.GetOperationPriority(GDOperationType.DictionaryInitializer);
        public List<GDDictionaryKeyValueDeclaration> KeyValues { get; } = new List<GDDictionaryKeyValueDeclaration>();

        internal override void HandleChar(char c, GDReadingState state)
        {
            if (IsSpace(c))
                return;

            if (c == '}')
            {
                state.PopNode();
                return;
            }

            var decl = new GDDictionaryKeyValueDeclaration();
            KeyValues.Add(decl);
            state.PushNode(decl);

            if (c != ',')
                state.PassChar(c);
        }

        internal override void HandleLineFinish(GDReadingState state)
        {
            state.PopNode();
            state.PassLineFinish();
        }

        public override string ToString()
        {
            return $"{{{string.Join(", ", KeyValues.Select(x => x.ToString()))}}}";
        }
    }
}
