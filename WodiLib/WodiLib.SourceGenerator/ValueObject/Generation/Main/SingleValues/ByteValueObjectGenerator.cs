// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ByteValueObjectGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues
{
    /// <summary>
    ///     単一 <see cref="byte"/> 値を持つ値オブジェクトジェネレータ
    /// </summary>
    internal class ByteValueObjectGenerator : IntegralValueObjectGeneratorTemplate
    {
        /// <inheritdoc/>
        public override InitializeAttributeSourceAddable TargetAttribute => ByteValueObjectAttribute.Instance;

        /// <inheritdoc/>
        private protected override Type WrapType => typeof(byte);

        /// <summary>インスタンス（シングルトン）</summary>
        public static ByteValueObjectGenerator Instance { get; } = new();
    }
}
