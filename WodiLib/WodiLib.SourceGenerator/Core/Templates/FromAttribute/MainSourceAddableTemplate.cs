// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MainSourceAddableTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Enums;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     属性情報からソースコードを自動生成する処理のテンプレートクラス
    /// </summary>
    internal abstract partial class MainSourceAddableTemplate : IMainSourceAddable
    {
        /// <summary>
        ///     自動生成対象とするクラスに付与する属性情報
        /// </summary>
        public abstract InitializeAttributeSourceAddable TargetAttribute { get; }

        /// <summary>
        ///     ロガー
        /// </summary>
        public virtual ILogger? Logger { get; } = new DefaultLogger();

        private IPropertyValueKeyResolver? keyResolver;

        /// <summary>
        ///     プロパティ値ディクショナリキー名解決処理
        /// </summary>
        private protected virtual IPropertyValueKeyResolver KeyResolver
        {
            get
            {
                if (keyResolver is null)
                {
                    keyResolver = new BasicPropertyValueKeyResolver(Logger);
                }

                return keyResolver!;
            }
        }

        private ISyntaxWorkResultDictionary? syntaxWorkResultDict;

        /// <summary>
        ///     解析結果ディクショナリ
        /// </summary>
        private protected virtual ISyntaxWorkResultDictionary SyntaxWorkResultDict
        {
            get
            {
                syntaxWorkResultDict ??= new BasicSyntaxWorkResultDictionary(KeyResolver, Logger);

                return syntaxWorkResultDict!;
            }
        }

        private IPropertyValueDictionary? propertyValuesDict;

        /// <summary>
        ///     出力処理要プロパティ値リスト
        /// </summary>
        private protected virtual IPropertyValueDictionary PropertyValuesDict
        {
            get
            {
                if (propertyValuesDict is null)
                {
                    propertyValuesDict = new BasicPropertyValueDictionary(KeyResolver, Logger);
                }

                return propertyValuesDict!;
            }
        }

        /// <summary>
        ///     属性プロパティデフォルト値ディクショナリ
        /// </summary>
        private protected AnalyzedPropertyValueDictionary PropertyDefaultValueDict { get; } = new();

        /// <inheritdoc/>
        public virtual void AddSource(GeneratorExecutionContext context,
            ITypeDefinitionInfoResolver typeDefinitionInfoResolver)
        {
            Logger?.AppendLine($"start AddSource from attribute: {TargetAttribute.TypeFullName}");
            Logger?.AppendLine($"SyntaxWorkResultDict Count: {SyntaxWorkResultDict.Count}");
            var workState = new WorkState(
                context, typeDefinitionInfoResolver,
                SyntaxWorkResultDict, PropertyDefaultValueDict,
                PropertyValuesDict, Logger);
            foreach (var workResult in SyntaxWorkResultDict)
            {
                if (!workState.CanGenerateSource(workResult))
                {
                    Logger?.AppendLine($"Skip AddSource {workResult.FullName}.");
                    continue;
                }

                try
                {
                    workState.SetCurrent(workResult);
                }
                catch (KeyNotFoundException ex)
                {
                    Logger?.AppendLine($"[WARNING] Cannot SetCurrent for workState.");
                    Logger?.AppendLine(ex.ToString());
                    Logger?.AppendLine(ex.HelpLink);
                    continue;
                }

                var hintName = HintName(workState);
                var source = GenerateSource(workState);

                try
                {
                    context.AddSource(hintName, source);
                    Logger?.AppendLine($"Complete AddSource {hintName}");
                }
                catch (ArgumentException)
                {
                    Logger?.AppendLine($"[WARNING] Duplicate HintName {hintName}");
                    /* TODO: 複数プロジェクトから利用すると重複登録される？ */
                    // throw new DuplicateHintNameException(ex, hintName);
                }
            }
        }

        /// <inheritdoc/>
        public void PutSyntaxWorkResult(SyntaxWorkResult workResult)
        {
            Logger?.AppendLine($"PutSyntaxWorkResult: {workResult}");
            SyntaxWorkResultDict.Add(workResult);
        }

        /// <inheritdoc/>
        public void PutPropertyDefaultValues(string attributeName, Dictionary<string, PropertyValue> defaultValues,
            string? parentAttrName)
        {
            Logger?.AppendLine($"PutPropertyDefaultValue (Target Attr: {attributeName}): ");
            foreach (var pair in defaultValues)
            {
                Logger?.AppendLine($"    {pair.Key} = {pair.Value}");
            }

            PropertyDefaultValueDict.Set(attributeName, defaultValues, parentAttrName);
        }

        /// <summary>
        ///     ヒント名を生成する。
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ヒント名</returns>
        private protected virtual string HintName(WorkState workState)
            => workState.FullName.ReplaceAngleBracketsToUnderscore();

        /// <summary>
        ///     ソース文字列を生成する。
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソース文字列</returns>
        private string GenerateSource(WorkState workState)
        {
            try
            {
                var nameSpace = workState.Namespace;

                return SourceTextFormatter.Format(new SourceFormatTarget[]
                {
                    ($@"namespace {nameSpace}"),
                    ($@"{{")
                }, SourceTextFormatter.Format(SourceConstants.IndentSpace,
                    GenerateBodyWithPartialOuter(workState)), new SourceFormatTarget[]
                {
                    ($@"}}")
                });
            }
            catch (Exception ex)
            {
                return string.Join(Environment.NewLine, ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        ///     外部クラスの定義で包んだ対象クラス本体のソースコードを生成する。
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソース文字列</returns>
        private SourceFormatTargetBlock GenerateBodyWithPartialOuter(WorkState workState)
        {
            var nextIndentSpaces = "";

            var nameSpace = workState.Namespace;
            var name = workState.Name;
            var fullName = workState.FullName;

            var outerTypeNames = ParseOuterTypePart(nameSpace, name, fullName);
            if (outerTypeNames.Length == 0)
            {
                return GenerateTypeDefinitionSource(workState, nextIndentSpaces);
            }

            var outerTypeFullNameBuilder = new StringBuilder();
            outerTypeFullNameBuilder.Append(nameSpace).Append(".");

            var frontSourceFormatTargets = new List<SourceFormatTarget>();
            var backSourceFormatTargets = new List<SourceFormatTarget>();

            foreach (var outerTypeName in outerTypeNames)
            {
                outerTypeFullNameBuilder.Append(outerTypeName);

                var typeDefinitionInfo = workState.ResolveTypeDefinitionInfo(outerTypeFullNameBuilder.ToString());

                var sourceTargets =
                    GeneratePartialOuterTypeOneSource(outerTypeName, typeDefinitionInfo, nextIndentSpaces);
                frontSourceFormatTargets.AddRange(sourceTargets.Item1);
                backSourceFormatTargets.AddRange(sourceTargets.Item2);

                nextIndentSpaces += SourceConstants.IndentSpace;
                outerTypeFullNameBuilder.Append(".");
            }

            var myTypeDefinitionSource =
                GenerateTypeDefinitionSource(workState, nextIndentSpaces);
            return frontSourceFormatTargets.Concat(myTypeDefinitionSource).Concat(backSourceFormatTargets).ToArray();
        }

        /// <summary>
        ///     外部クラスの partial 定義ソースを1クラス分生成する。
        /// </summary>
        /// <param name="typeName">外部クラスの型名</param>
        /// <param name="typeDefinitionInfo">型情報</param>
        /// <param name="indentSpace">インデント用スペース文字列</param>
        /// <returns>ソースコード文字列（クラス定義開始部、クラス定義終了部）</returns>
        private (SourceFormatTargetBlock, SourceFormatTargetBlock) GeneratePartialOuterTypeOneSource(string typeName,
            TypeDefinitionInfo typeDefinitionInfo, string indentSpace)
        {
            var defText = ClassDefinitionSource(typeDefinitionInfo);

            var forwardResult = SourceTextFormatter.Format(indentSpace, new SourceFormatTarget[]
            {
                ($@"{defText} {typeName}"),
                ($@"{{")
            });
            var backResult = SourceTextFormatter.Format(indentSpace, new SourceFormatTarget[]
            {
                ($@"}}")
            });

            return (forwardResult, backResult);
        }

        /// <summary>
        ///     クラスフル名をパースして外部クラス部分を取り出す。
        /// </summary>
        /// <param name="nameSpace">名前空間</param>
        /// <param name="typeName">出力タイプ名</param>
        /// <param name="typeFullName">出力タイプフル名</param>
        /// <returns>外部クラスのタイプ名配列</returns>
        private string[] ParseOuterTypePart(string nameSpace, string typeName, string typeFullName)
        {
            if (typeFullName.Equals($"{nameSpace}.{typeName}"))
            {
                return Array.Empty<string>();
            }

            var prefixRegex = new Regex($"^{nameSpace.Replace(".", "\\.")}\\.");
            var suffixRegex = new Regex($"\\.{typeName}$");

            var trimmed = prefixRegex.Replace(suffixRegex.Replace(typeFullName, ""), "");

            var split = trimmed.Split('.');
            return split;
        }

        /// <summary>
        ///     タイプ定義宣言部のソースコード出力
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <param name="indentSpaces">インデント用スペース文字列</param>
        /// <returns>ソースコード文字列情報</returns>
        private SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState, string indentSpaces)
            => SourceTextFormatter.Format(indentSpaces, GenerateTypeDefinitionSource(workState));

        /// <summary>
        ///     タイプ定義宣言部のソースコード出力
        /// </summary>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソースコード文字列情報</returns>
        private protected abstract SourceFormatTargetBlock GenerateTypeDefinitionSource(
            WorkState workState);

        /// <summary>
        ///     クラス定義宣言部のソースを生成する。
        /// </summary>
        /// <param name="typeDefinitionInfo">型定義情報</param>
        /// <returns>ソースコード文字列</returns>
        protected static string ClassDefinitionSource(TypeDefinitionInfo typeDefinitionInfo)
        {
            var resultBuilder = new StringBuilder();

            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);
            resultBuilder.Append(accessibility);

            if (
                typeDefinitionInfo.IsAbstract
                && typeDefinitionInfo.ObjectType != ObjectType.Interface // interface の場合 IsAbstract が true になる
            )
            {
                resultBuilder.Append(" abstract");
            }

            resultBuilder.Append(" partial ");

            var objTypeText = typeDefinitionInfo.ObjectType.ToString().ToLower();
            resultBuilder.Append(objTypeText);

            return resultBuilder.ToString();
        }
    }
}
