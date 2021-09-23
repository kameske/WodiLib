// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     単一文字列オブジェクトに付与する属性プロパティ生成用クラス
    /// </summary>
    internal class StringValueObjectAttribute : SingleValueObjectAttribute
    {
        public static readonly PropertyInfo IsAllowEmpty = new()
        {
            Name = nameof(IsAllowEmpty),
            Type = "bool",
            Summary = "空文字許容フラグ",
            Remarks = @"指定した場合、空文字をエラーとする。",
            DefaultValue = true
        };

        public static readonly PropertyInfo IsAllowNewLine = new()
        {
            Name = nameof(IsAllowNewLine),
            Type = "bool",
            Summary = "改行コード許容フラグ",
            Remarks = @"指定した場合、改行コードを含む文字列をエラーとする。",
            DefaultValue = true
        };

        public static readonly PropertyInfo Pattern = new()
        {
            Name = nameof(Pattern),
            Type = "string",
            Summary = "正規表現パターン",
            Remarks = @"指定した場合、この正規表現に一致しない文字列をエラーとする。",
            DefaultValue = null,
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo SafetyPattern = new()
        {
            Name = nameof(SafetyPattern),
            Type = "string",
            Summary = "推奨する正規表現パターン",
            Remarks = @"指定した場合、この正規表現に一致しない文字列に対して警告ログを出力する。",
            DefaultValue = null,
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo PatternOption = new()
        {
            Name = nameof(PatternOption),
            Type = typeof(RegexOptions).FullName,
            Summary = "正規表現パターンオプション",
            Remarks = @"入力値を正規表現パターンで検証する際、このオプションを使用する。",
            DefaultValue = $"{typeof(RegexOptions).FullName}.{nameof(RegexOptions.None)}",
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo ByteLengthEncoding = new()
        {
            Name = nameof(ByteLengthEncoding),
            Type = "string",
            Summary = "バイナリサイズ文字エンコーディング",
            Remarks = @"文字データサイズ検証時、この文字コードでデータサイズを測る。<br/>
デフォルトは ""utf-8""。",
            DefaultValue = "utf-8"
        };

        public static readonly PropertyInfo ByteMaxLength = new()
        {
            Name = nameof(ByteMaxLength),
            Type = "int",
            Summary = "バイナリサイズ上限",
            Remarks = $@"指定した場合、バイナリデータ化した際のサイズがこの値を超える文字列をエラーとする。<br/>
バイナリデータ化する際のエンコードには {Tag.See.Cref(nameof(ByteLengthEncoding))} で指定された文字コードを使用する。",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo ByteMinLength = new()
        {
            Name = nameof(ByteMinLength),
            Type = "int",
            Summary = "バイナリサイズ下限",
            Remarks = $@"指定した場合、バイナリデータ化した際のサイズがこの値未満の文字列をエラーとする。<br/>
バイナリデータ化する際のエンコードには {Tag.See.Cref(nameof(ByteLengthEncoding))} で指定された文字コードを使用する。",
            DefaultValue = 0
        };

        public static readonly PropertyInfo MaxLength = new()
        {
            Name = nameof(MaxLength),
            Type = "int",
            Summary = "文字数上限",
            Remarks = @"指定した場合、文字列長がこの値を超える文字列をエラーとする。",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true
        };

        public static readonly PropertyInfo MinLength = new()
        {
            Name = nameof(MinLength),
            Type = "int",
            Summary = "文字数下限",
            Remarks = @"指定した場合、文字列長がこの値未満の文字列をエラーとする。",
            DefaultValue = 0
        };

        public override string NameSpace => GenerationConst.NameSpaces.Attributes;
        public override string AttributeName => nameof(StringValueObjectAttribute);
        public override string Summary => "単一の文字列を表すValueObject";

        public override IEnumerable<PropertyInfo> Properties()
            => base.Properties().Concat(new[]
            {
                IsAllowEmpty,
                IsAllowNewLine,
                Pattern,
                SafetyPattern,
                PatternOption,
                ByteLengthEncoding,
                ByteMaxLength,
                ByteMinLength,
                MaxLength,
                MinLength
            });

        public static StringValueObjectAttribute Instance { get; } = new();
    }
}
