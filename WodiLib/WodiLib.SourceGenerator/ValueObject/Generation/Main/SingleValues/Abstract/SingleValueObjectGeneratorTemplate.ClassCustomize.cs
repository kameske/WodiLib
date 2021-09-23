// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.ClassCustomize.cs
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
        ///     対象がクラスの場合の出力カスタマイズ
        /// </summary>
        private class ClassCustomize : ISourceCustomizer
        {
            /// <summary>
            ///     インスタンス（シングルトン）
            /// </summary>
            public static ClassCustomize Instance { get; } = new();

            private ClassCustomize()
            {
            }

            /// <inheritdoc/>
            public SourceFormatTarget SourceFormatTargetEqualsObject(PropertyValues workResult,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver)
            {
                var className = workResult.Name;
                return (
                    $@"public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is {className} other && Equals(other);");
            }

            /// <inheritdoc/>
            public SourceFormatTarget SourceFormatTargetEqualsOther(PropertyValues workResult,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver)
            {
                var className = workResult.Name;
                var propertyName = workResult[MyAttr.PropertyName.Name];

                return ($@"public bool Equals({className}? other) => {propertyName}.Equals(other?.{propertyName});");
            }
        }
    }
}
