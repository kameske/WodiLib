// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : AttributeTargetsExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.SourceGenerator.Core.Exceptions
{
    /// <summary>
    ///     自動生成コード登録時にHintNameが重複した場合の例外
    /// </summary>
    internal class DuplicateHintNameException : Exception
    {
        private ArgumentException Original { get; }

        public DuplicateHintNameException(ArgumentException original, string hintName) : base(ErrorMessage(original,
            hintName))
        {
            Original = original;
        }

        private static string ErrorMessage(ArgumentException original, string hintName)
            => $"{original.Message} (hintName: {hintName})";

        public override string StackTrace => Original.StackTrace;

        public override string Source
        {
            get => Original.Source;
            set => Original.Source = value;
        }

        public override string HelpLink
        {
            get => Original.HelpLink;
            set => Original.HelpLink = value;
        }
    }
}
