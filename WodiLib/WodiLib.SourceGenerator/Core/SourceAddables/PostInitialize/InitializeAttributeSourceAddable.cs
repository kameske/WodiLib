// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InitializeAttributeSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize
{
    /// <summary>
    ///     <see cref="ISourceGenerator.Initialize"/> で
    ///     属性を追加する処理を行うためのテンプレートクラス
    /// </summary>
    internal abstract class InitializeAttributeSourceAddable : IInitializeSourceAddable
    {
        /// <summary>属性名</summary>
        public abstract string AttributeName { get; }

        /// <summary>属性付与可能対象</summary>
        public abstract AttributeTargets AttributeTargets { get; }

        /// <summary>Summary</summary>
        public abstract string Summary { get; }

        /// <summary>Remarks</summary>
        public virtual string? Remarks => null;

        /// <summary>派生クラスへの継承フラグ</summary>
        public virtual bool Inherited { get; } = true;

        /// <summary>複数属性付与可能フラグ</summary>
        public virtual bool AllowMultiple { get; }

        /// <summary>名前空間</summary>
        public abstract string NameSpace { get; }

        /// <summary>型名</summary>
        public string TypeName => AttributeName;

        /// <summary>型名（フル）</summary>
        public string TypeFullName => $"{NameSpace}.{TypeName}";

        /// <inheritDoc/>
        public void AddSource(GeneratorPostInitializationContext context)
        {
            try
            {
                context.AddSource(HintName(), Source());
            }
            catch (Exception ex)
            {
                context.AddSource($"{HintName()}_Error.log", ex.ToString());
            }
        }

        /// <summary>プロパティ一覧</summary>
        public abstract IEnumerable<PropertyInfo> Properties();

        /// <summary>
        ///     <see cref="PropertyValues"/> インスタンスを生成する。
        /// </summary>
        /// <param name="data">属性データ</param>
        /// <returns>生成したインスタンス</returns>
        public PropertyValues MakePropertyValues(AttributeData data)
        {
            var workResult = new SyntaxWorkResult(null, data);
            return new PropertyValues(
                workResult,
                MakeDefaultValueDict(data)
            );
        }

        /// <returns>SourceGenerator用ヒント名</returns>
        private string HintName()
            => $"{$"{NameSpace}.{AttributeName}".CompressNameSpace()}.cs";

        /// <returns>SourceGenerator出力ソースコード</returns>
        private string Source()
        {
            return SourceTextFormatter.Format(
                new SourceFormatTarget[]
                {
                    $@"namespace {NameSpace}",
                    $@"{{"
                },
                SourceTextFormatter.Format(
                    IndentSpace,
                    new SourceFormatTarget[]
                    {
                        $@"/// <summary>",
                        $@"/// {__}{Summary}",
                        $@"/// </summary>"
                    },
                    RemarksSourceText(),
                    new SourceFormatTarget[]
                    {
                        $@"[System.AttributeUsage({AttributeTargets.ToSource()}, Inherited = {Inherited.ToString().ToLower()}, AllowMultiple = {AllowMultiple.ToString().ToLower()})]",
                        $@"internal class {AttributeName} : System.Attribute",
                        $@"{{"
                    },
                    SourceTextFormatter.ReduceMany(
                            IndentSpace,
                            Properties()
                                .Select(
                                    prop =>
                                        prop.ToSourceFormatTargets()
                                )
                                .ToArray()
                        )
                        .TrimLastEmptyLine()
                ),
                new SourceFormatTarget[]
                {
                    $@"{__}}}",
                    $@"}}"
                }
            );
        }

        /// <returns>ドキュメントコメント：Remarks</returns>
        private SourceFormatTargetBlock RemarksSourceText()
        {
            if (Remarks is null) return Array.Empty<SourceFormatTarget>();

            return SourceTextFormatter.Format(
                IndentSpace,
                new SourceFormatTarget[]
                {
                    $@"/// <remarks>",
                    $@"/// {__}{Remarks.TrimNewLine()}",
                    $@"/// </remarks>"
                }
            );
        }

        /// <returns>デフォルト値ディクショナリ</returns>
        private IReadOnlyDictionary<string, PropertyValue> MakeDefaultValueDict(AttributeData data)
        {
            // 属性プロパティ
            var attrProperties = data.AttributeClass
                                     ?.GetMembers()
                                     .Where(member => member.Kind == SymbolKind.Property)
                                     .Cast<IPropertySymbol>()
                                     .ToList()
                                 ?? new List<IPropertySymbol>();

            return Properties()
                .ToDictionary(
                    prop => prop.Name,
                    prop =>
                    {
                        var defaultValueForAttr = attrProperties.FirstOrDefault(
                                attrProp
                                    => attrProp.Name.Equals(prop.Name)
                            )
                            ?.GetDefaultValue();
                        if (defaultValueForAttr.HasValue)
                        {
                            return new PropertyValue(defaultValueForAttr);
                        }

                        return prop.SourceTextDefaultValue(false);
                    }
                );
        }
    }
}
