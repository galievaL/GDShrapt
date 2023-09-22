using System;
using GDShrapt.Reader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GDShrapt.Converter.Tests
{
    internal class CSharpGeneratingVisitor2 : INodeVisitor
    {
        readonly ConversionSettings _conversionSettings;

        ClassDeclarationSyntax _partsCode;
        private CompilationUnitSyntax _compilationUnit;

        public CSharpGeneratingVisitor2(ConversionSettings conversionSettings)
        {
            _conversionSettings = conversionSettings;

            //var @cc = SyntaxFactory.ClassDeclaration("HTerrainDataSaver").AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword)); //добавляем класс и модификатор доступа
            //var c1 = @cc.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("ResourceFormatSaver"))); //наследование
            //var c2 = c1.AddAttributeLists(  //добавляем аттрибуты
            //        SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.ParseName("Tool")))),
            //        SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
            //            SyntaxFactory.Attribute(SyntaxFactory.ParseName("ClassName"),
            //                SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(
            //                    SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("HTerrainDataSaver")))))))));

            //var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Generated")).NormalizeWhitespace();
            //var compilationUnit = SyntaxFactory.CompilationUnit()
            //    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Godot")))
            //    .AddMembers(@namespace.AddMembers(c2))
            //    .NormalizeWhitespace();

            //var code = compilationUnit.ToFullString();
        }

        #region
        public void DidLeft(GDNode node)
        {
            ////////
            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Generated")).NormalizeWhitespace();

            _compilationUnit = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Godot")))
                .AddMembers(@namespace.AddMembers(_partsCode))
                .NormalizeWhitespace();
        }

        public void DidLeft(GDExpression expr)
        {
            throw new System.NotImplementedException();
        }

        public void EnterListChild(GDNode node)
        {
            ////////
        }

        public void EnterNode(GDNode node)
        {
            ////////
        }

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

        internal string BuildCSharpNormalisedCode()
        {
            ////////
            var code = _compilationUnit.ToFullString();
            return code;
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
            ////////++
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
            ////////++
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
            ////////++
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

        public void LeftUnknown(GDNode node)
        {
            throw new System.NotImplementedException();
        }

        public void LeftUnknown(GDExpression e)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        public void Visit(GDClassDeclaration d)
        {
            ////////
            var className = d.ClassName.ToString().Split(' ');//.Replace("class_name", "").Trim();

            if (className[0] == "class_name")
                _partsCode = SyntaxFactory.ClassDeclaration(className[1]).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
        }

        public void Visit(GDDictionaryKeyValueDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDEnumDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDEnumValueDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDExportDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDInnerClassDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchCaseDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDParameterDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDSignalDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDVariableDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIfBranch b)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDElseBranch b)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDElifBranch b)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDClassAtributesList list)
        {
            ////////
            //var lllii = list.Nodes;

            //for (int i = 0; i < list.Count; i++)
            //{
            //    var g = list[i].ToString();

            //    switch (g.Split(' ')[0])
            //    {
            //        case "":
            //            break;
            //        default:
            //            break;
            //    }
            //    //GDClassAtributesList 'tool \n class_name HTerrainDataSaver \n extends ResourceFormatSaver\n'
            //}

            _partsCode = _partsCode.AddBaseListTypes(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("ResourceFormatSaver"))); //наследование
            _partsCode = _partsCode.AddAttributeLists(  //добавляем аттрибуты
                       SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.ParseName("Tool")))),
                       SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(SyntaxFactory.ParseName("ClassName"), SyntaxFactory.AttributeArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("HTerrainDataSaver")))))))));
        }

        public void Visit(GDClassMembersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDDictionaryKeyValueDeclarationList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDElifBranchesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDEnumValuesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDExportParametersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDExpressionsList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchCasesList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDParametersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDPathList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDLayersList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDStatementsList list)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMethodDeclaration d)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDToolAtribute a)
        {
            ////////
        }

        public void Visit(GDClassNameAtribute a)
        {
            ////////
        }

        public void Visit(GDExtendsAtribute a)
        {
            ////////
        }

        public void Visit(GDExpressionStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIfStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDForStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDVariableDeclarationStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDWhileStatement s)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDArrayInitializerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBoolExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBracketExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBreakExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDBreakPointExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDCallExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDContinueExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDDictionaryInitializerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDDualOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDGetNodeExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIdentifierExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIfExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDIndexerExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchCaseVariableExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMatchDefaultOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDNodePathExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDNumberExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDMemberOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDPassExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDReturnExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDSingleOperatorExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDStringExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(GDYieldExpression e)
        {
            throw new System.NotImplementedException();
        }

        public void VisitUnknown(GDNode node)
        {
        }

        public void VisitUnknown(GDExpression e)
        {
        }

        public void WillVisit(GDNode node)
        {
        }

        public void WillVisit(GDExpression expr)
        {
        }
    }
}