// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IMainSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Exceptions;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     <see cref="GeneratorExecutionContext"/> に自動生成クラスのソースを追加できることを示すインタフェース
    /// </summary>
    internal interface IMainSourceAddable
    {
        /// <summary>ロガー</summary>
        ILogger? Logger { get; }

        /// <summary>
        ///     ソースコードを追加する。
        /// </summary>
        /// <param name="context">ソースコード追加先</param>
        /// <param name="typeDefinitionInfoResolver">型情報解決処理</param>
        /// <exception cref="DuplicateHintNameException">ソースコード登録時にHintNameが重複した場合</exception>
        void AddSource(GeneratorExecutionContext context, ITypeDefinitionInfoResolver typeDefinitionInfoResolver);

        /// <summary>
        ///     構文処理結果をセットする。
        /// </summary>
        /// <param name="workResult">構文処理結果</param>
        void PutSyntaxWorkResult(SyntaxWorkResult workResult);
    }
}
