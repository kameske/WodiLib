// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorker.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     構文処理
    /// </summary>
    internal partial class SyntaxWorker : SyntaxWorkerCore
    {
        /// <inheritdoc/>
        public override bool CanResolve(string typeFullName)
            => TypeDefinitionDict.ContainsKey(typeFullName);

        /// <inheritdoc/>
        public override TypeDefinitionInfo Resolve(string typeFullName)
            => TypeDefinitionDict[typeFullName];

        /// <inheritdoc/>
        private protected override ISyntaxAnalyzer? SyntaxAnalyzer { get; }
            = new CustomizedSyntaxAnalyzer();

        /// <summary>
        ///     ソース自動生成情報解決処理
        /// </summary>
        protected IExecuteInfoResolver ExecuteInfoResolver { get; }

        /// <summary>
        ///     型情報
        /// </summary>
        protected Dictionary<string, TypeDefinitionInfo> TypeDefinitionDict { get; } =
            new();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="executeInfoResolver">ソース自動生成情報解決処理</param>
        /// <param name="logger">ロガー</param>
        public SyntaxWorker(IExecuteInfoResolver executeInfoResolver, ILogger? logger)
        {
            ExecuteInfoResolver = executeInfoResolver;
            Logger = logger;
        }
    }
}
