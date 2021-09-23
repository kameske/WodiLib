// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorker.IExecuteInfoResolver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal partial class SyntaxWorker
    {
        /// <summary>
        ///     ソース自動生成処理解決処理インタフェース
        /// </summary>
        internal interface IExecuteInfoResolver
        {
            /// <summary>
            ///     型名からソース自動生成に必要な情報を取得する。
            /// </summary>
            /// <param name="targetAttribute">対象属性シンボル</param>
            /// <returns>ソース自動生成処理情報</returns>
            IMainSourceAddable? Resolve(INamedTypeSymbol targetAttribute);

            /// <summary>
            ///     ソース自動生成対象に付与する属性のルートである場合trueを返す。
            /// </summary>
            /// <param name="targetAttribute">判定属性シンボル</param>
            /// <returns></returns>
            bool IsRootAttribute(INamedTypeSymbol targetAttribute);
        }
    }
}
