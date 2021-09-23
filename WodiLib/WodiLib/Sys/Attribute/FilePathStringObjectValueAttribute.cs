// ========================================
// Project Name : WodiLib
// File Name    : FilePathStringObjectValueAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Text.RegularExpressions;

namespace WodiLib.Sys
{
    /// <summary>
    /// 汎用ファイルパス文字列オブジェクトの設定属性（継承元クラス用）
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FilePathStringObjectValueAttribute : CommonOneLineStringValueObjectAttribute
    {
        /// <inheritDoc/>
        [DefaultValueAttribute(260)]
        public override int MaxLength { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(true)]
        public override bool IsAllowEmpty { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(RegexOptions.IgnoreCase)]
        public override RegexOptions PatternOption { get; init; } = default!;
    }
}
