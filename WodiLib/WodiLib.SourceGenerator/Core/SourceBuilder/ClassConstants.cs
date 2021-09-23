// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ClassConstants.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     クラス定数ソース情報出力用
    /// </summary>
    internal static class ClassConstants
    {
        /// <summary>
        ///     クラス定数定義ソース情報を出力する。
        /// </summary>
        /// <param name="info">定数情報</param>
        /// <returns>ソースコード文字列情報</returns>
        public static SourceFormatTargetBlock Generate(ConstantInfo info)
            => info.GenuineInfo is null
                ? GenerateWithoutGenuine(info)
                : GenerateWithGenuine(info);

        private static SourceFormatTargetBlock GenerateWithGenuine(ConstantInfo info)
        {
            var genuine = info.GenuineInfo!;
            var constName = $"{genuine.Prefix}{info.Name}{genuine.Suffix}";
            var newModifier = info.HasNewModifier ? "new " : "";

            var editorBrowsableStateName = genuine.EditorBrowsableState is not null
                ? Enum.GetName(typeof(EditorBrowsableState), genuine.EditorBrowsableState)
                : "";

            return SourceTextFormatter.Format("", new[]
            {
                (@$"/// <summary>{info.Summary}</summary>"),
                (@$"public static {newModifier}{info.Type} {info.Name} => {constName};"),
                (@$"/// <summary>{info.Summary}（定数）</summary>"),
                (@$"[{typeof(EditorBrowsableAttribute).FullName}({typeof(EditorBrowsableState).FullName}.{editorBrowsableStateName})]"),
                (@$"public {newModifier}const {info.Type} {constName} = {info.Value};"),
                SourceFormatTarget.Empty
            });
        }

        private static SourceFormatTargetBlock GenerateWithoutGenuine(ConstantInfo info)
        {
            var newModifier = info.HasNewModifier ? "new " : "";

            return SourceTextFormatter.Format("", new[]
            {
                (@$"/// <summary>{info.Summary}</summary>"),
                (@$"public static {newModifier}{info.Type} {info.Name} {{ get; }} = {info.Value};"),
                SourceFormatTarget.Empty
            });
        }

        /// <summary>
        ///     定数情報
        /// </summary>
        /// <remarks>
        ///     この情報をもとに public static な GetOnly のクラスフィールドを定義する。
        /// </remarks>
        public class ConstantInfo
        {
            /// <summary>定数名</summary>
            public string Name { get; init; } = default!;

            /// <summary>Summary</summary>
            public string Summary { get; init; } = default!;

            /// <summary>定数名</summary>
            public string? Remarks { get; init; } = default!;

            /// <summary>型</summary>
            public string Type { get; init; } = default!;

            /// <summary>値</summary>
            public string Value { get; init; } = default!;

            /// <summary>"new"修飾子付与フラグ</summary>
            public bool HasNewModifier { get; init; }

            /// <summary>真の定数情報</summary>
            public GenuineConstantInfo? GenuineInfo { get; init; }

            /// <summary>
            ///     プロパティ情報から定数情報を生成する。
            /// </summary>
            /// <param name="propertyInfo">プロパティ情報</param>
            /// <param name="value">定数値</param>
            /// <param name="hasNewModifier">"new"キーワード付与フラグ</param>
            /// <param name="state">Const定数のEditorBrowsable（未指定時Const定数を定義しない）</param>
            /// <returns></returns>
            public static ConstantInfo FromPropertyInfo(PropertyInfo propertyInfo, string value,
                bool hasNewModifier = false, EditorBrowsableState? state = EditorBrowsableState.Advanced)
                => new()
                {
                    Name = propertyInfo.Name,
                    Summary = propertyInfo.Summary,
                    Type = propertyInfo.Type,
                    Value = value,
                    HasNewModifier = hasNewModifier,
                    GenuineInfo = state is null
                        ? null
                        : new GenuineConstantInfo
                        {
                            EditorBrowsableState = state
                        }
                };

            /// <summary>
            ///     真・定数情報
            /// </summary>
            /// <remarks>
            ///     この情報をもとに public const を定義する。
            /// </remarks>
            public class GenuineConstantInfo
            {
                /// <summary>定数名につけるPrefix</summary>
                public string Prefix { get; init; } = "Const_";

                /// <summary>定数名につけるSuffix</summary>
                public string Suffix { get; init; } = "";

                /// <summary>値</summary>
                public EditorBrowsableState? EditorBrowsableState { get; init; }
            }
        }
    }
}
