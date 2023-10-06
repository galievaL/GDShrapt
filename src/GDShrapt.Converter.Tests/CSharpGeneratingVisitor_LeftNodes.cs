using System;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        public void Left(GDVariableDeclaration d)
        {
        }

        public void Left(GDWhileStatement s)
        {
            throw new NotImplementedException();
        }

        public void Left(GDVariableDeclarationStatement s)
        {
            throw new NotImplementedException();
        }

        public void Left(GDMatchStatement s)
        {
            throw new NotImplementedException();
        }

        public void Left(GDForStatement s)
        {
            throw new NotImplementedException();
        }

        public void Left(GDIfStatement s)
        {
            throw new NotImplementedException();
        }

        public void Left(GDExpressionStatement s)
        {
            throw new NotImplementedException();
        }

        public void Left(GDToolAtribute a)
        {
            ////////
        }

        public void Left(GDClassNameAtribute a)
        {
            ////////
        }

        public void Left(GDExtendsAtribute a)
        {
            ////////
        }

        public void Left(GDMethodDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Left(GDInnerClassDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Left(GDParameterDeclaration d)
        {
            throw new NotImplementedException();
        }

        public void Left(GDClassDeclaration d)
        {
            ////////
        }

        public void Left(GDDictionaryKeyValueDeclaration decl)
        {
            throw new NotImplementedException();
        }

        public void Left(GDEnumDeclaration decl)
        {
            throw new NotImplementedException();
        }

        public void Left(GDEnumValueDeclaration decl)
        {
            throw new NotImplementedException();
        }

        public void Left(GDExportDeclaration decl)
        {
            throw new NotImplementedException();
        }

        public void Left(GDMatchCaseDeclaration decl)
        {
            throw new NotImplementedException();
        }

        public void Left(GDSignalDeclaration decl)
        {
            throw new NotImplementedException();
        }

        public void Left(GDClassAtributesList list)
        {
            ////////
        }

        public void Left(GDClassMembersList list)
        {
            ////
        }

        public void Left(GDDictionaryKeyValueDeclarationList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDElifBranchesList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDEnumValuesList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDExportParametersList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDExpressionsList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDMatchCasesList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDParametersList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDPathList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDLayersList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDStatementsList list)
        {
            throw new NotImplementedException();
        }

        public void Left(GDIfBranch branch)
        {
            throw new NotImplementedException();
        }

        public void Left(GDElseBranch branch)
        {
            throw new NotImplementedException();
        }

        public void Left(GDElifBranch branch)
        {
            throw new NotImplementedException();
        }

        public void Left(GDArrayInitializerExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDBoolExpression e)
        {
            ////
        }

        public void Left(GDBracketExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDYieldExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDStringExpression e)
        {
            ////
        }

        public void Left(GDSingleOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDReturnExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDPassExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDNumberExpression e)
        {
            ///GDNumberExpression '42'
        }

        public void Left(GDBreakExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDNodePathExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDMemberOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDMatchDefaultOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDMatchCaseVariableExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDIndexerExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDBreakPointExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDIfExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDIdentifierExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDGetNodeExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDDictionaryInitializerExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDContinueExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDCallExpression e)
        {
            throw new NotImplementedException();
        }

        public void Left(GDDualOperatorExpression e)
        {
            throw new NotImplementedException();
        }

        public void LeftListChild(GDNode node)
        {
            ////////
        }

        public void LeftNode()
        {
            ////////
        }
    }
}