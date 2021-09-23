// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.StructCustomize.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract.SingleValueObjectAttribute;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    internal abstract partial class SingleValueObjectGeneratorTemplate
    {
        /// <summary>
        ///     対象が構造体の場合の出力カスタマイズ
        /// </summary>
        private class StructCustomize : ISourceCustomizer
        {
            /// <summary>
            ///     インスタンス（シングルトン）
            /// </summary>
            public static StructCustomize Instance { get; } = new();

            private StructCustomize()
            {
            }

            /// <inheritdoc/>
            public SourceFormatTarget SourceFormatTargetEqualsObject(PropertyValues workResult,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver)
            {
                var className = workResult.Name;
                return ($@"public override bool Equals(object? obj) => obj is {className} other && Equals(other);");
            }

            /// <inheritdoc/>
            public SourceFormatTarget SourceFormatTargetEqualsOther(PropertyValues workResult,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver)
            {
                var className = workResult.Name;
                var propertyName = workResult[MyAttr.PropertyName.Name];

                return ($@"public bool Equals({className} other) => {propertyName}.Equals(other.{propertyName});");
            }
        }
    }
}
