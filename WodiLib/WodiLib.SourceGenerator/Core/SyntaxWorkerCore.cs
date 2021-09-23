// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorkerCore.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     構文処理中枢
    /// </summary>
    internal abstract class SyntaxWorkerCore : ISyntaxWorker
    {
        /// <summary>ロガー</summary>
        public ILogger? Logger { get; protected init; }

        /// <summary>構文解析器</summary>
        private protected abstract ISyntaxAnalyzer? SyntaxAnalyzer { get; }

        /// <inheritdoc/>
        public abstract bool CanResolve(string typeFullName);

        /// <inheritdoc/>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            Logger?.AppendLine("--- Start OnVisitSyntaxNode ---");
            Logger?.AppendLine(context.Node.ToFullString());

            SyntaxAnalyzer?.Analyze(context, this);
        }

        /// <inheritdoc/>
        public abstract TypeDefinitionInfo Resolve(string typeFullName);

        /// <summary>
        ///     構文解析器インタフェース
        /// </summary>
        internal interface ISyntaxAnalyzer
        {
            /// <summary>
            ///     構文解析を行う。
            /// </summary>
            /// <param name="outer">自身を呼び出した <see cref="SyntaxWorkerCore"/> インスタンス</param>
            /// <param name="context">コンテキスト</param>
            void Analyze(GeneratorSyntaxContext context, SyntaxWorkerCore outer);
        }
    }
}
