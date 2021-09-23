// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InitializeEnumSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize
{
    /// <summary>
    ///     <see cref="ISourceGenerator.Initialize"/> で
    ///     列挙型を追加する処理を行うためのテンプレートクラス
    /// </summary>
    internal abstract class InitializeEnumSourceAddable : IInitializeSourceAddable
    {
        /// <summary>名前空間</summary>
        public abstract string NameSpace { get; }

        /// <summary>型名</summary>
        public string TypeName => EnumName;

        /// <summary>型名（フル）</summary>
        public string TypeFullName => $"{NameSpace}.{TypeName}";

        /// <summary>列挙名</summary>
        public abstract string EnumName { get; }

        /// <summary>Summary</summary>
        public abstract string Summary { get; }

        /// <summary>列挙メンバ</summary>
        public abstract IEnumerable<EnumMember> Members();

        /// <summary><see cref="System.FlagsAttribute"/> 付与フラグ</summary>
        public virtual bool IsFlags => false;

        /// <inheritDoc/>
        public void AddSource(GeneratorPostInitializationContext context)
        {
            try
            {
                context.AddSource(HintName(), Source());
            }
            catch (Exception ex)
            {
                context.AddSource(HintName(), ex.ToString());
            }
        }

        /// <summary>
        ///     列挙メンバー情報
        /// </summary>
        public class EnumMember
        {
            /// <summary>列挙メンバー名</summary>
            public string MemberName { get; private init; } = default!;

            /// <summary>Summary</summary>
            public string Summary { get; private init; } = default!;

            /// <summary>値</summary>
            public int Value { get; private init; }

            public static implicit operator EnumMember((string memberName, string summary, int value) info)
                => new()
                {
                    MemberName = info.memberName,
                    Summary = info.summary,
                    Value = info.value
                };
        }

        /// <returns>SourceGenerator用ヒント名</returns>
        private string HintName()
            => $"{$"{NameSpace}.{EnumName}".CompressNameSpace()}.cs";

        /// <returns>SourceGenerator出力ソースコード</returns>
        private string Source()
            => SourceTextFormatter.Format(new SourceFormatTarget[]
                {
                    ($@"namespace {NameSpace}"),
                    ($@"{{")
                }, SourceTextFormatter.Format(IndentSpace,
                    new SourceFormatTarget[]
                    {
                        ($@"/// <summary>"),
                        ($@"/// {__}{Summary}"),
                        ($@"/// </summary>"),
                        ($@"[System.Flags]", IsFlags),
                        ($@"public enum {EnumName}"),
                        ($@"{{")
                    },
                    SourceTextFormatter.Reduce(IndentSpace, ",",
                        Members().Select(member => new SourceFormatTargetBlock(
                            ($@"/// <summary>{member.Summary}</summary>"),
                            ($@"{member.MemberName} = {member.Value}", !IsFlags),
                            ($@"{member.MemberName} = 0x{member.Value:X}", IsFlags),
                            SourceFormatTarget.Empty
                        )).ToArray()
                    ).TrimLastEmptyLine()
                ),
                new SourceFormatTarget[]
                {
                    ($@"    }}"),
                    ($@"}}")
                });
    }
}
