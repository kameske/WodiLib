// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.ISourceCustomizer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    internal abstract partial class SingleValueObjectGeneratorTemplate
    {
        /// <summary>
        ///     出力ソース個別カスタマイズ処理
        /// </summary>
        private interface ISourceCustomizer
        {
            /// <summary>
            ///     object 型のインスタンスとの比較コード
            /// </summary>
            /// <param name="workResult"></param>
            /// <param name="typeDefinitionInfoResolver"></param>
            /// <returns></returns>
            SourceFormatTarget SourceFormatTargetEqualsObject(PropertyValues workResult,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver);

            /// <summary>
            ///     同じ型のインスタンスとの比較コード
            /// </summary>
            /// <param name="workResult"></param>
            /// <param name="typeDefinitionInfoResolver"></param>
            /// <returns></returns>
            SourceFormatTarget SourceFormatTargetEqualsOther(PropertyValues workResult,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver);
        }
    }
}
