// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PropertyInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Core.Dtos
{
    /// <summary>
    ///     プロパティ情報
    /// </summary>
    internal class PropertyInfo
    {
        private static readonly Regex RegexTypeIsString = new("^[Ss]tring\\??");

        /// <summary>プロパティ名</summary>
        public string Name { get; init; } = default!;

        /// <summary>プロパティタイプ</summary>
        public string Type { get; init; } = default!;

        /// <summary>ドキュメントコメント：Summary</summary>
        public string Summary { get; init; } = default!;

        /// <summary>ドキュメントコメント：Remarks</summary>
        public string? Remarks { get; init; }

        /// <summary>ドキュメントコメント：SeeAlso</summary>
        public SeeAlsoInfo[]? SeeAlsos { get; init; }

        /// <summary>デフォルト値</summary>
        public object? DefaultValue { get; init; }

        /// <summary>
        ///     <see cref="DefaultValue"/> に文字列が指定された場合、それをソースコードとして埋め込むかどうか
        /// </summary>
        /// <remarks>
        ///     <see langword="true"/>を指定した場合、<see cref="DefaultValue"/> を <see langword="string"/> として扱わず、
        ///     文字列をそのままソースコードとして出力する。
        /// </remarks>
        /// <value></value>
        public bool DefaultValueAsSourceCode { get; init; }

        /// <returns>自身の情報から生成した <see cref="SourceFormatTarget"/> 配列</returns>
        public SourceFormatTargetBlock ToSourceFormatTargets()
        {
            return SourceTextFormatter.Format("",
                SourceFormatTargetSummaryBody(),
                SourceFormatTargetRemarks(),
                SourceFormatTargetSeeAlso(), new[]
                {
                    ($@"[{typeof(DefaultValueAttribute).FullName}({(SourceTextDefaultValue())})]"),
                    ($@"public virtual {Type} {Name} {{ get; init; }} = default!;"),
                    SourceFormatTarget.Empty
                });
        }

        /// <returns>デフォルト値</returns>
        public PropertyValue SourceTextDefaultValue(bool wrapDoubleQuoteIfString = true)
        {
            if (DefaultValue is null) return new PropertyValue("null");

            if (DefaultValue is bool bValue)
            {
                return new PropertyValue(bValue ? "true" : "false");
            }

            if (DefaultValueAsSourceCode) return new PropertyValue($"{DefaultValue}");

            var isStringValue = RegexTypeIsString.IsMatch(Type);
            return new PropertyValue(isStringValue && wrapDoubleQuoteIfString
                ? $@"""{DefaultValue}"""
                : $"{DefaultValue}");
        }

        /// <returns>デフォルト値（プロパティデフォルト値ディクショナリ用）</returns>
        public string? DefaultValueForDict()
        {
            if (DefaultValue is null) return null;

            if (DefaultValue is bool bValue)
            {
                return bValue ? "true" : "false";
            }

            if (DefaultValueAsSourceCode) return $"{DefaultValue}";

            var isStringValue = RegexTypeIsString.IsMatch(Type);
            return isStringValue
                ? $@"""{DefaultValue}"""
                : $"{DefaultValue}";
        }

        /// <returns>Summaryタグ部</returns>
        private SourceFormatTargetBlock SourceFormatTargetSummaryBody()
            => new SourceFormatTarget[]
            {
                $@"/// {Tag.Summary(Summary.TrimNewLine())}"
            };

        /// <returns>Remarksタグ部</returns>
        private SourceFormatTargetBlock SourceFormatTargetRemarks()
        {
            if (Remarks is null) return Array.Empty<SourceFormatTarget>();

            return new SourceFormatTarget[]
            {
                $@"/// {Tag.Remarks(Remarks.TrimNewLine())}"
            };
        }

        /// <returns>seealsoタグ部</returns>
        private SourceFormatTargetBlock SourceFormatTargetSeeAlso()
        {
            if (SeeAlsos is null) return Array.Empty<SourceFormatTarget>();

            return SeeAlsos.Select(item => new SourceFormatTarget(item.ToDocumentComment())).ToArray();
        }

        /// <summary>
        ///     {seealso} タグ情報
        /// </summary>
        /// <param name="cref">参照先</param>
        /// <param name="body">タグ本文</param>
        public record SeeAlsoInfo(string cref, string? body)
        {
            /// <summary>
            ///     ドキュメントコメント文字列を取得する。
            /// </summary>
            /// <returns></returns>
            public string ToDocumentComment()
                => $"/// {Tag.SeeAlso.Cref(cref, body)}";

            public static implicit operator SeeAlsoInfo(string cref)
                => new(cref, null);

            public static implicit operator SeeAlsoInfo((string cref, string body) tuple)
                => new(tuple.cref, tuple.body);
        }
    }
}
