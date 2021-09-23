// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : CastType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums
{
    /// <summary>
    ///     キャスト種別
    /// </summary>
    internal class CastType : InitializeEnumSourceAddable
    {
        public static readonly int Code_None = 0;
        public static readonly int Code_Explicit = 1;
        public static readonly int Code_Implicit = 2;

        public override string NameSpace => GenerationConst.NameSpaces.Enums;
        public override string EnumName => nameof(CastType);
        public override string Summary => "キャスト種別";

        public override IEnumerable<EnumMember> Members()
            => new EnumMember[]
            {
                ("None", "キャスト不可", Code_None),
                ("Explicit", "明示的なキャスト可能", Code_Explicit),
                ("Implicit", "暗黙的なキャスト可能", Code_Implicit)
            };

        public static string ToSourceText(string code)
        {
            if (code.Equals(Code_None.ToString())) return "";
            if (code.Equals(Code_Explicit.ToString())) return "explicit";
            if (code.Equals(Code_Implicit.ToString())) return "implicit";
            throw new ArgumentOutOfRangeException(nameof(code), code, "not found");
        }

        public static string ToDocumentText(string code)
        {
            if (code.Equals(Code_None.ToString())) return "";
            if (code.Equals(Code_Explicit.ToString())) return "明示的";
            if (code.Equals(Code_Implicit.ToString())) return "暗黙的";
            throw new ArgumentOutOfRangeException(nameof(code), code, "not found");
        }

        public static bool CanOperation(string code)
            => (new List<string>
            {
                Code_Explicit.ToString(),
                Code_Implicit.ToString()
            }).Contains(code);

        private CastType()
        {
        }

        public static CastType Instance { get; } = new();
    }
}
