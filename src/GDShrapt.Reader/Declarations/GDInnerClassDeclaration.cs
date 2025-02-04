﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDShrapt.Reader
{
    public class GDInnerClassDeclaration : GDClassMember
    {
        readonly int _lineIntendation;
        bool _membersChecked;

        public List<GDClassMember> Members { get; } = new List<GDClassMember>();

        public GDType ExtendsClass => Members.OfType<GDExtendsAtribute>().FirstOrDefault()?.Type;
        public GDIdentifier Name => Members.OfType<GDClassNameAtribute>().FirstOrDefault()?.Identifier;
        public bool IsTool => Members.OfType<GDToolAtribute>().Any();

        internal GDInnerClassDeclaration(int lineIntendation)
        {
            _lineIntendation = lineIntendation;
        }

        public GDInnerClassDeclaration()
        {
        }

        internal override void HandleChar(char c, GDReadingState state)
        {
            if (!_membersChecked)
            {
                _membersChecked = true;
                state.PushNode(new GDClassMemberResolver(_lineIntendation + 1, member => Members.Add(member)));
                state.PassChar(c);
                return;
            }

            // Complete reading
            state.PopNode();
        }

        internal override void HandleLineFinish(GDReadingState state)
        {
            if (!_membersChecked)
            {
                _membersChecked = true;
                state.PushNode(new GDClassMemberResolver(_lineIntendation + 1, member => Members.Add(member)));
                state.PassLineFinish();
                return;
            }

            // Complete reading
            state.PopNode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < Members.Count; i++)
                builder.AppendLine(Members[i].ToString());

            return builder.ToString();
        }
    }
}
