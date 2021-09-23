// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract.SingleValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    /// <summary>
    ///     単一値オブジェクトジェネレータのテンプレートクラス
    /// </summary>
    internal abstract partial class SingleValueObjectGeneratorTemplate : MainSourceAddableTemplate
    {
        /// <summary>
        ///     内部に保持する値の型
        /// </summary>
        private protected abstract Type WrapType { get; }

        /// <inheritdoc/>
        private protected sealed override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            WorkState workState)
        {
            var typeDefinitionInfo = workState.ResolveTypeDefinitionInfo(workState.FullName);
            var workResult = workState.PropertyValues;

            var className = workResult.Name;
            var propertyName = workResult[MyAttr.PropertyName.Name];
            var castType = workResult[MyAttr.CastType.Name]!;
            var isExtended = workState.IsExtended;

            var implInterfaces = GetImplementInterfaceSentence(className, workState);
            var canOperation = CastType.CanOperation(castType);
            var castOperation = CastType.ToSourceText(castType);

            var sourceCustomizer = GetSourceCustomizer(typeDefinitionInfo);

            var isExtendedRawValue = workState.GetParentPropertyValue(MyAttr.PropertyName.Name)?.Equals(propertyName) ??
                                     false;
            var newModifierRawValue = isExtendedRawValue ? "new " : "";

            var wrapTypeIsClass = WrapType.IsClass;

            return SourceTextFormatter.Format("", new SourceFormatTarget[]
                {
                    ($@"{ClassDefinitionSource(typeDefinitionInfo)} {className} {implInterfaces.PrefixIfNotEmpty(" : ")}"),
                    ($@"{{")
                }, SourceTextFormatter.Format(IndentSpace,
                    SourceFormatTargetsPublicStaticProperties(workState), new[]
                    {
                        ($@"/// <summary>{WrapType}値</summary>"),
                        ($@"public {newModifierRawValue}{WrapType} {propertyName} {{ get; private set; }}"),
                        SourceFormatTarget.Empty,
                        ($@"/// <summary>"),
                        ($@"/// {__}コンストラクタ"),
                        ($@"/// </summary>"),
                        ($@"/// {Tag.Param("value", "設定値")}")
                    }, SourceFormatTargetsConstructorException(workState), new SourceFormatTarget[]
                    {
                        ($@"public {className}({WrapType} value)", !isExtended),
                        ($@"public {className}({WrapType} value) : base(value)", isExtended),
                        ($@"{{")
                    }, SourceTextFormatter.Format(IndentSpace,
                        SourceFormatTargetsConstructorBody(workState)), new[]
                    {
                        ($@"}}"),
                        SourceFormatTarget.Empty
                    }, SourceTextFormatter.If(IsOverrideBasicMethods(workState),
                        new[]
                        {
                            ($@"/// {Tag.InheritDoc()}"),
                            ($@"public override string ToString() => {propertyName}.ToString();"),
                            SourceFormatTarget.Empty,
                            ($@"/// {Tag.InheritDoc()}"),
                            sourceCustomizer.SourceFormatTargetEqualsObject(workResult,
                                workState.TypeDefinitionInfoResolver),
                            SourceFormatTarget.Empty,
                            ($@"/// {Tag.InheritDoc()}"),
                            ($@"public override int GetHashCode() => {propertyName}.GetHashCode();"),
                            SourceFormatTarget.Empty
                        }
                    ), SourceTextFormatter.If(IsImplementEquatable(workState),
                        new[]
                        {
                            ($@"/// {Tag.InheritDoc("System.IEquatable{T}.Equals(T)")}"),
                            sourceCustomizer.SourceFormatTargetEqualsOther(workResult,
                                workState.TypeDefinitionInfoResolver),
                            SourceFormatTarget.Empty,
                            ($@"/// {Tag.Summary("== 演算子")}"),
                            ($@"/// {Tag.Param("left", "左項")}"),
                            ($@"/// {Tag.Param("right", "右項")}"),
                            ($@"/// {Tag.Returns($"{Tag.ParamRef("left")} と {Tag.ParamRef("right")} が同一要素である場合 {Tag.See.Langword_True}")}"),
                            ($@"public static bool operator ==({className}? left, {className}? right) => Equals(left, right);"),
                            ($@"/// {Tag.Summary("!= 演算子")}"),
                            ($@"/// {Tag.Param("left", "左項")}"),
                            ($@"/// {Tag.Param("right", "右項")}"),
                            ($@"/// {Tag.Returns($"{Tag.ParamRef("left")} と {Tag.ParamRef("right")} が同一要素ではない場合 {Tag.See.Langword_True}")}"),
                            ($@"public static bool operator !=({className}? left, {className}? right) => !Equals(left, right);"),
                            SourceFormatTarget.Empty
                        }
                    ), SourceTextFormatter.If(IsImplementFormattable(workState), () =>
                    {
                        var needNewModifier = ParentIsImplementFormattable(workState);
                        var newModifierStr = needNewModifier ? "new " : "";

                        return new[]
                        {
                            ($@"/// {Tag.InheritDoc("int.ToString(string)")}"),
                            ($@"public {newModifierStr}string ToString(string format) => {propertyName}.ToString(format);"),
                            SourceFormatTarget.Empty,
                            ($@"/// {Tag.InheritDoc("int.ToString(System.IFormatProvider)")}"),
                            ($@"public {newModifierStr}string ToString(System.IFormatProvider formatProvider) => {propertyName}.ToString(formatProvider);"),
                            SourceFormatTarget.Empty,
                            ($@"/// {Tag.InheritDoc("System.IFormattable.ToString(string?, System.IFormatProvider?)")}"),
                            ($@"public {newModifierStr}string ToString(string? format, System.IFormatProvider? formatProvider) => {propertyName}.ToString(format, formatProvider);"),
                            SourceFormatTarget.Empty
                        };
                    }), SourceTextFormatter.If(IsImplementComparable(workState), () =>
                    {
                        return new[]
                        {
                            ($@"/// {Tag.InheritDoc($"System.IComparable{{T}}.CompareTo")}"),
                            ($@"public int CompareTo({className}? other) => {propertyName}.CompareTo(other?.{propertyName});"),
                            SourceFormatTarget.Empty
                        };
                    }), SourceTextFormatter.If(canOperation,
                        new[]
                        {
                            ($@"/// {Tag.Summary($"{WrapType} から {className} への {CastType.ToDocumentText(castType)}な型変換")}",
                                !typeDefinitionInfo.IsAbstract),
                            ($@"/// {Tag.Param("value", "変換対象")}", !typeDefinitionInfo.IsAbstract),
                            ($@"/// {Tag.Returns("変換結果")}", !typeDefinitionInfo.IsAbstract),
                            ($@"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(""value"")]",
                                !typeDefinitionInfo.IsAbstract),
                            ($@"public static {castOperation} operator {className}?({WrapType}? value) => value is null ? null : new {className}(({WrapType}) value);",
                                !typeDefinitionInfo.IsAbstract),
                            ($@"", !typeDefinitionInfo.IsAbstract),
                            ($@"/// {Tag.Summary($"{className} から {WrapType} への {CastType.ToDocumentText(castType)}な型変換")}"),
                            ($@"/// {Tag.Param("value", "変換対象")}"),
                            ($@"/// {Tag.Returns("変換結果")}"),
                            ($@"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(""value"")]"),
                            ($@"public static {castOperation} operator {WrapType}?({className}? value) => value?.{propertyName};"),
                            SourceFormatTarget.Empty
                        }
                    ), SourceTextFormatter.If(canOperation && !wrapTypeIsClass,
                        // 構造体の場合のみ、nullable ではいない場合のキャストを別途定義
                        new[]
                        {
                            ($@"/// {Tag.Summary($"{WrapType} から {className} への {CastType.ToDocumentText(castType)}な型変換")}",
                                !typeDefinitionInfo.IsAbstract),
                            ($@"/// {Tag.Param("value", "変換対象")}", !typeDefinitionInfo.IsAbstract),
                            ($@"/// {Tag.Returns("変換結果")}", !typeDefinitionInfo.IsAbstract),
                            ($@"public static {castOperation} operator {className}({WrapType} value) => new {className}(value);",
                                !typeDefinitionInfo.IsAbstract),
                            ($@"", !typeDefinitionInfo.IsAbstract),
                            ($@"/// {Tag.Summary($"{className} から {WrapType} への {CastType.ToDocumentText(castType)}な型変換")}"),
                            ($@"/// {Tag.Param("value", "変換対象")}"),
                            ($@"/// {Tag.Returns("変換結果")}"),
                            ($@"public static {castOperation} operator {WrapType}({className} value) => value.{propertyName};"),
                            SourceFormatTarget.Empty
                        }
                    ), SourceFormatTargetsExtendBody(workState)
                ).TrimLastEmptyLine(),
                new SourceFormatTarget[]
                {
                    ($@"}}")
                }
            );
        }

        /// <summary>
        ///     <see cref="IEquatable{T}"/> 実装可否
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns><see cref="IEquatable{T}"/> を実装する場合 <see langword="true"/></returns>
        private protected virtual bool IsImplementEquatable(WorkState workState)
            => !workState.CurrentTypeDefinitionInfo.IsRecord &&
               bool.Parse(workState.PropertyValues[MyAttr.ImplementEquatable.Name]!);

        /// <summary>
        ///     <see cref="IFormattable"/> 実装可否
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns><see cref="IFormattable"/> を実装する場合 <see langword="true"/></returns>
        private protected abstract bool IsImplementFormattable(WorkState workState);

        /// <summary>
        ///     親クラスが <see cref="IFormattable"/> を継承しているかどうかを返す。
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        private protected abstract bool ParentIsImplementFormattable(WorkState workState);

        /// <summary>
        ///     <see cref="IComparable{T}"/> 実装可否
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns><see cref="IComparable{T}"/> を実装する場合 <see langword="true"/></returns>
        private protected virtual bool IsImplementComparable(WorkState workState)
            => bool.Parse(workState.PropertyValues[MyAttr.IsComparable.Name]!);

        /// <summary>
        ///     親クラスが <see cref="IComparable{T}"/> を継承しているかどうかを返す。
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        private protected virtual bool ParentIsImplementComparable(WorkState workState)
            => bool.Parse(workState.GetOrDefaultParentPropertyValue(MyAttr.IsComparable.Name,
                MyAttr.IsComparable.DefaultValue!.ToString()));

        /// <summary>
        ///     コンストラクタ XML ドキュメント例外説明
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>コード文字列情報</returns>
        private protected abstract SourceFormatTargetBlock SourceFormatTargetsConstructorException(
            WorkState workState);

        /// <summary>
        ///     コンストラクタ本体ソースコード
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>コード文字列情報</returns>
        private protected abstract SourceFormatTargetBlock SourceFormatTargetsConstructorBody(
            WorkState workState);

        /// <summary>
        ///     Object 基本メソッドオーバーライド要否
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns><see cref="object"/> の基本メソッドを継承する場合 <see langword="true"/></returns>
        private protected virtual bool IsOverrideBasicMethods(WorkState workState)
            => !workState.CurrentTypeDefinitionInfo.IsRecord &&
               bool.Parse(workState.PropertyValues[MyAttr.OverrideBasicMethods.Name]!);

        /// <summary>
        ///     public static property 定義コード
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソースコード文字列情報</returns>
        private protected virtual SourceFormatTargetBlock SourceFormatTargetsPublicStaticProperties(
            WorkState workState)
            => Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     クラス定義本体拡張コード
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソースコード文字列情報</returns>
        private protected virtual SourceFormatTargetBlock SourceFormatTargetsExtendBody(
            WorkState workState)
            => Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     ソースコードカスタマイズ用処理を取得する。
        /// </summary>
        /// <param name="typeDefinitionInfo">型定義情報</param>
        /// <returns><see cref="ISourceCustomizer"/> インスタンス</returns>
        private static ISourceCustomizer GetSourceCustomizer(TypeDefinitionInfo typeDefinitionInfo)
        {
            if (typeDefinitionInfo.IsStruct)
            {
                return StructCustomize.Instance;
            }

            return ClassCustomize.Instance;
        }

        /// <summary>
        ///     実装インタフェース宣言ソース文字列を取得する。
        /// </summary>
        /// <param name="className">クラス名</param>
        /// <param name="workState">ワーク情報</param>
        /// <returns>ソースコード文字列</returns>
        private string GetImplementInterfaceSentence(string className, WorkState workState)
        {
            return new[]
            {
                IsImplementEquatable(workState) ? $"System.IEquatable<{className}>" : null,
                IsImplementFormattable(workState) ? "System.IFormattable" : null,
                IsImplementComparable(workState) ? $"System.IComparable<{className}?>" : null
            }.JoinWithoutEmpty(",");
        }
    }
}
