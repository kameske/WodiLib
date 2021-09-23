// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ISyntaxWorker.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     コード解析Workerインタフェース
    /// </summary>
    internal interface ISyntaxWorker : ISyntaxContextReceiver, ITypeDefinitionInfoResolver
    {
        /// <summary>ロガー</summary>
        ILogger? Logger { get; }
    }
}
