using System;
using System.Linq;
using System.Text.RegularExpressions;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GDShrapt.Converter.Tests
{
    internal partial class CSharpGeneratingVisitor : INodeVisitor
    {
        public void Left(GDWhileStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDVariableDeclarationStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMatchStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDForStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDIfStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDExpressionStatement s)
        {
            throw new System.NotImplementedException();
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

        public void Left(GDVariableDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMethodDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDInnerClassDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDParameterDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDClassDeclaration d)
        {
            ////////
        }

        public void Left(GDDictionaryKeyValueDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDEnumDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDEnumValueDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDExportDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMatchCaseDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDSignalDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDClassAtributesList list)
        {
            ////////
        }

        public void Left(GDClassMembersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDDictionaryKeyValueDeclarationList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDElifBranchesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDEnumValuesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDExportParametersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDExpressionsList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMatchCasesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDParametersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDPathList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDLayersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDStatementsList list)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDIfBranch branch)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDElseBranch branch)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDElifBranch branch)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDArrayInitializerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDBoolExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDBracketExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDYieldExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDStringExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDSingleOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDReturnExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDPassExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDNumberExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDBreakExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDNodePathExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMemberOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMatchDefaultOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDMatchCaseVariableExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDIndexerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDBreakPointExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDIfExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDIdentifierExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDGetNodeExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDDictionaryInitializerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDContinueExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDCallExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Left(GDDualOperatorExpression e)
        {
            throw new System.NotImplementedException();
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