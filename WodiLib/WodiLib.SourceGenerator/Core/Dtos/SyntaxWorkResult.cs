// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorkResult.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Dtos
{
    /// <summary>
    ///     <see cref="ISyntaxReceiver"/>処理結果
    /// </summary>
    internal class SyntaxWorkResult
    {
        /// <summary>対象シンボル</summary>
        public INamedTypeSymbol? TargetSymbol { get; }

        /// <summary>情報取得のきっかけとなった属性名（フル）</summary>
        public string SrcAttributeName => SrcAttributeData.AttributeClass!.FullName();

        /// <summary>配置する名前空間名</summary>
        public string Namespace => TargetSymbol?.Namespace() ?? "";

        /// <summary>配置するクラス名</summary>
        public string ClassName => TargetSymbol?.ClassName() ?? "";

        /// <summary>名前空間.クラス名</summary>
        public string FullName => TargetSymbol?.ToString() ?? "";

        /// <summary>情報取得のきっかけとなった属性データ</summary>
        public AttributeData SrcAttributeData { get; }

        /// <summary>名前付き引数</summary>
        private ImmutableArray<KeyValuePair<string, TypedConstant>> NamedArgs => SrcAttributeData.NamedArguments;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="targetClassSymbol">対象シンボル</param>
        /// <param name="srcAttributeData">情報取得のきっかけとなった属性データ</param>
        public SyntaxWorkResult(INamedTypeSymbol? targetClassSymbol,
            AttributeData srcAttributeData)
        {
            TargetSymbol = targetClassSymbol;
            SrcAttributeData = srcAttributeData;
        }

        /// <summary>
        ///     属性プロパティ値を取得する。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>文字列化した値</returns>
        public string? GetNamedArgument(string propertyName)
            => NamedArgs.FirstOrDefault(pair => pair.Key.Equals(propertyName))
                .Value.ToValueString();

        /// <summary>
        ///     プロパティ名/値 のディクショナリを作成する。
        /// </summary>
        /// <returns>プロパティ名と値のDictionary</returns>
        public IReadOnlyDictionary<string, PropertyValue> MakePropertyValuesDict()
        {
            var result = new Dictionary<string, PropertyValue>();

            foreach (var namedArg in NamedArgs)
            {
                var key = namedArg.Key;
                var value = namedArg.Value;
                result[key] = new PropertyValue(value);
            }

            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{{ Namespace: {Namespace}, ClassName: {ClassName}, "
                   + $"NamedArgs: [ {string.Join(", ", NamedArgs.Select(arg => $"{{ Key: {arg.Key}, Value: {arg.Value.ToValueString()} }}"))} ]}}";
        }
    }
}
