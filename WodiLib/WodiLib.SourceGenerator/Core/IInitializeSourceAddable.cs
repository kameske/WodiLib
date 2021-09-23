// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IInitializeSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     <see cref="GeneratorPostInitializationContext"/> に関連クラスのソースを追加できることを示すインタフェース
    /// </summary>
    internal interface IInitializeSourceAddable
    {
        /// <summary>配置する名前空間</summary>
        public string NameSpace { get; }

        /// <summary>型名</summary>
        public string TypeName { get; }

        /// <summary>型名（フル）</summary>
        public string TypeFullName { get; }

        /// <summary>
        ///     ソースを追加する。
        /// </summary>
        /// <param name="context">追加先コンテキスト</param>
        public void AddSource(GeneratorPostInitializationContext context);
    }
}
