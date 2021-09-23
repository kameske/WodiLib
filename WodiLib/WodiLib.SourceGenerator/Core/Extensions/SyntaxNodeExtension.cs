// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxNodeExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core.Enums;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="SyntaxNode"/> 拡張クラス
    /// </summary>
    internal static class SyntaxNodeExtension
    {
        /// <returns>オブジェクト種別</returns>
        public static ObjectType GetObjectType(this SyntaxNode src)
        {
            return src switch
            {
                InterfaceDeclarationSyntax => ObjectType.Interface,
                ClassDeclarationSyntax => ObjectType.Class,
                StructDeclarationSyntax => ObjectType.Struct,
                RecordDeclarationSyntax => ObjectType.Record,
                PropertyDeclarationSyntax => ObjectType.Property,
                _ => ObjectType.Other
            };
        }
    }
}
