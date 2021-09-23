// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringValueObjectGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using WodiLib.SourceGenerator.ValueObject.Generation.Helper;
using WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract;
using MyAttr = WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.StringValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues
{
    /// <summary>
    ///     単一 <see cref="string"/> 値を持つ値オブジェクトジェネレータ
    /// </summary>
    internal class StringValueObjectGenerator : SingleValueObjectGeneratorTemplate
    {
        /// <inheritdoc/>
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        /// <inheritdoc/>
        private protected sealed override Type WrapType => typeof(string);

        /// <inheritdoc/>
        private protected override bool IsImplementFormattable(WorkState workState)
            => false;

        /// <inheritdoc/>
        private protected override bool ParentIsImplementFormattable(WorkState workState)
            => false;

        private protected override SourceFormatTargetBlock SourceFormatTargetsPublicStaticProperties(
            WorkState workState)
        {
            var workResult = workState.PropertyValues;

            var isValidatePattern = IsValidatePattern(workState);
            var isValidateSafetyPattern = IsValidateSafetyPattern(workState);
            var isValidateAnyPattern = isValidatePattern || isValidateSafetyPattern;
            var isValidateByteLength = IsValidateByteLength(workState);
            var isValidateLength = IsValidateLength(workState);

            if (new[]
            {
                isValidateAnyPattern,
                isValidateByteLength,
                isValidateLength
            }.All(b => !b)) return Array.Empty<SourceFormatTarget>();

            return SourceTextFormatter.Format("",
                SourceTextFormatter.If(isValidatePattern,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_String(
                        MyAttr.Pattern, workResult, workState)
                ),
                SourceTextFormatter.If(isValidateSafetyPattern,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_String(
                        MyAttr.SafetyPattern, workResult, workState)
                ),
                SourceTextFormatter.If(isValidateAnyPattern,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Enum(
                        MyAttr.PatternOption, workResult, workState,
                        workState.IsAnyPropertyOverwritten(MyAttr.Pattern.Name, MyAttr.SafetyPattern.Name,
                            MyAttr.PatternOption.Name))
                ),
                SourceTextFormatter.If(isValidateByteLength,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.ByteMaxLength, workResult, workState, "int.MaxValue",
                        workState.IsAnyPropertyOverwritten(MyAttr.ByteMaxLength.Name, MyAttr.ByteMinLength.Name)),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.ByteMinLength, workResult, workState, "0",
                        workState.IsAnyPropertyOverwritten(MyAttr.ByteMaxLength.Name, MyAttr.ByteMinLength.Name))
                ),
                SourceTextFormatter.If(isValidateLength,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MaxLength, workResult, workState, "int.MaxValue",
                        workState.IsAnyPropertyOverwritten(MyAttr.MaxLength.Name, MyAttr.MinLength.Name)),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant(
                        MyAttr.MinLength, workResult, workState, "0",
                        isOverwrittenProperty: workState.IsAnyPropertyOverwritten(MyAttr.MaxLength.Name,
                            MyAttr.MinLength.Name))
                )
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorException(
            WorkState workState)
        {
            return SourceTextFormatter.Format("",
                SourceFormatTargetsConstructorArgumentNullException(),
                SourceFormatTargetsConstructorArgumentOutOfRangeException(workState),
                SourceFormatTargetsConstructorArgumentException(workState)
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorBody(
            WorkState workState)
        {
            return SourceTextFormatter.Format("",
                SourceFormatTargetsValidateNotNull(),
                SourceFormatTargetsValidateNotEmpty(workState),
                SourceFormatTargetsValidatePattern(workState),
                SourceFormatTargetsValidateNewLine(workState),
                SourceFormatTargetsValidateByteLength(workState),
                SourceFormatTargetsValidateLength(workState),
                new SourceFormatTarget[]
                {
                    // DoConstructorExpansion メソッドで RawValue を更新する可能性があるので RawValue の初期化を先に行う
                    ($@"{workState.PropertyValues[MyAttr.PropertyName.Name]} = value;"),
                    ($@"DoConstructorExpansion(value);")
                }
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsExtendBody(WorkState workState)
        {
            var workResult = workState.PropertyValues;
            var typeDef = workState.ResolveTypeDefinitionInfo(workState.FullName);

            var operationOverloadCodeMaker = new OperationOverloadCodeMaker(workResult.Name, workResult.Namespace,
                workResult[MyAttr.PropertyName.Name]!, WrapType.FullName!);

            return SourceTextFormatter.Format("",
                operationOverloadCodeMaker.BinaryOperatorNewInstance("+", new[] { workResult.FullName },
                    !typeDef.IsAbstract),
                new SourceFormatTarget[]
                {
                    ($@"partial void DoConstructorExpansion(string value);")
                }
            );
        }

        /// <summary>インスタンス（シングルトン）</summary>
        public static StringValueObjectGenerator Instance { get; } = new();

        /// <returns>空文字チェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateEmpty(WorkState workState)
        {
            return bool.Parse(workState.PropertyValues[MyAttr.IsAllowEmpty.Name]!) == false;
        }

        /// <returns>改行チェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateNewLine(WorkState workState)
        {
            return bool.Parse(workState.PropertyValues[MyAttr.IsAllowNewLine.Name]!) == false;
        }

        /// <returns>正規表現による必須または安全パターチェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateAnyPattern(WorkState workState)
        {
            return IsValidatePattern(workState) || IsValidateSafetyPattern(workState);
        }

        /// <returns>正規表現によるパターチェックを行う場合<see langword="true"/></returns>
        private static bool IsValidatePattern(WorkState workState)
        {
            return workState.PropertyValues[MyAttr.Pattern.Name] is not null;
        }

        /// <returns>正規表現による安全パターチェックを行う場合<see langword="true"/></returns>
        private static bool IsValidateSafetyPattern(WorkState workState)
        {
            return workState.PropertyValues[MyAttr.SafetyPattern.Name] is not null;
        }

        /// <returns>範囲チェックを行う場合 <see langword="true"/></returns>
        private static bool IsValidateOutOfRange(WorkState workState)
            => IsValidateByteLength(workState) || IsValidateLength(workState);

        /// <returns>バイトサイズの範囲チェックを行う場合 <see langword="true"/></returns>
        private static bool IsValidateByteLength(WorkState workState)
        {
            var isDefaultByteMax = ToInt32FromIntString((string)MyAttr.ByteMaxLength.DefaultValue!).ToString().Equals(
                workState.PropertyValues[MyAttr.ByteMaxLength.Name]);
            var isDefaultByteMin = ((int)MyAttr.ByteMinLength.DefaultValue!).ToString()
                .Equals(workState.PropertyValues[MyAttr.ByteMinLength.Name]);
            return !isDefaultByteMax || !isDefaultByteMin;
        }

        /// <returns>文字列長の範囲チェックを行う場合 <see langword="true"/></returns>
        private static bool IsValidateLength(WorkState workState)
        {
            var isDefaultMaxLength = ToInt32FromIntString((string)MyAttr.MaxLength.DefaultValue!).ToString()
                .Equals(workState.PropertyValues[MyAttr.MaxLength.Name]);
            var isDefaultMinLength = ((int)MyAttr.MinLength.DefaultValue!).ToString()
                .Equals(workState.PropertyValues[MyAttr.MinLength.Name]);
            return !isDefaultMaxLength || !isDefaultMinLength;
        }

        /// <returns><see cref="ArgumentNullException"/> ドキュメントコメント文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsConstructorArgumentNullException()
        {
            return SourceTextFormatter.Format("", new SourceFormatTarget[]
            {
                ($@"/// <exception cref=""{typeof(ArgumentNullException).FullName}"">"),
                ($@"///     {Sentence.ErrorDesc.Null(Tag.ParamRef("value"))}"),
                ($@"/// </exception>")
            });
        }

        /// <returns><see cref="ArgumentOutOfRangeException"/> ドキュメントコメント文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsConstructorArgumentOutOfRangeException(
            WorkState workState)
        {
            return SourceTextFormatter.Format("",
                SourceTextFormatter.ReduceMany($"/// {__}", "、または",
                    SourceTextFormatter.If(IsValidateByteLength(workState), new SourceFormatTarget[]
                    {
                        ($@"{Sentence.ErrorDesc.OutOfRange($@"{Tag.ParamRef("value")} のデータサイズ", Tag.See.Cref(MyAttr.ByteMinLength.Name), Tag.See.Cref(MyAttr.ByteMaxLength.Name))}")
                    }),
                    SourceTextFormatter.If(IsValidateOutOfRange(workState), new SourceFormatTarget[]
                    {
                        ($@"{Sentence.ErrorDesc.OutOfRange($@"{Tag.ParamRef("value")} の文字列長", Tag.See.Cref(MyAttr.MinLength.Name), Tag.See.Cref(MyAttr.MaxLength.Name))}")
                    })
                )
            ).AppendPrefixAndSuffixIfNotEmpty(
                ($@"/// <exception cref=""{typeof(ArgumentOutOfRangeException).FullName}"">"),
                ($@"/// </exception>")
            );
        }

        /// <returns><see cref="ArgumentException"/> ドキュメントコメント文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsConstructorArgumentException(
            WorkState workState)
        {
            return SourceTextFormatter.Format("",
                SourceTextFormatter.ReduceMany($"/// {__}", "、または",
                    SourceTextFormatter.If(IsValidateEmpty(workState), new SourceFormatTarget[]
                    {
                        ($@"{Tag.ParamRef("value")} が 空文字 の場合")
                    }),
                    SourceTextFormatter.If(IsValidateNewLine(workState), new SourceFormatTarget[]
                    {
                        ($@"{Tag.ParamRef("value")} に改行コードが含まれる場合")
                    }),
                    SourceTextFormatter.If(IsValidatePattern(workState), new SourceFormatTarget[]
                    {
                        ($@"{Tag.ParamRef("value")} が {Tag.See.Cref(MyAttr.Pattern.Name)} を満たさない場合")
                    })
                )
            ).AppendPrefixAndSuffixIfNotEmpty(
                ($@"/// <exception cref=""{typeof(ArgumentException).FullName}"">"),
                ($@"/// </exception>")
            );
        }

        /// <returns>非 <see langword="null"/> チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateNotNull()
            => SourceTextFormatter.Format("", new SourceFormatTarget[]
            {
                ($@"{{   // Validate NotNull"),
                ($@"    if (value is null) throw new System.ArgumentNullException(nameof(value));"),
                ($@"}}")
            });

        /// <returns>非空文字チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateNotEmpty(WorkState workState)
            => SourceTextFormatter.If(IsValidateEmpty(workState), new SourceFormatTarget[]
            {
                ($@"{{   // Validate NotEmpty"),
                ($@"    if (value.Equals("""")) throw new System.ArgumentException($""{{nameof(value)}} cannot empty string."");"),
                ($@"}}")
            });

        /// <returns>文字列パターンチェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidatePattern(WorkState workState)
            => SourceTextFormatter.If(IsValidateAnyPattern(workState), new SourceFormatTarget[]
            {
                ($@"{{   // Validate for Pattern"),
                ($@"    var requireRegex = new System.Text.RegularExpressions.Regex({MyAttr.Pattern.Name}, (System.Text.RegularExpressions.RegexOptions) {MyAttr.PatternOption.Name});",
                    IsValidatePattern(workState)),
                ($@"    if (!requireRegex.IsMatch(value)) throw new System.ArgumentException(""No match pattern."", nameof(value));",
                    IsValidatePattern(workState)),
                ($@"    var safetyRegex = new System.Text.RegularExpressions.Regex({MyAttr.SafetyPattern.Name}, (System.Text.RegularExpressions.RegexOptions) {MyAttr.PatternOption.Name});",
                    IsValidateSafetyPattern(workState)),
                ($@"    if (!safetyRegex.IsMatch(value)) WodiLib.Sys.Cmn.WodiLibLogger.GetInstance().Warning(WodiLib.Sys.WarningMessage.NotMatchRegex(value, safetyRegex));",
                    IsValidateSafetyPattern(workState)),
                ($@"}}")
            });

        /// <returns>改行チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateNewLine(WorkState workState)
            => SourceTextFormatter.If(IsValidateNewLine(workState), new SourceFormatTarget[]
            {
                ($@"{{   // Validate NewLine"),
                ($@"    if ( value.Contains(""\n"") || value.Contains(""\r\n"") ) {{ throw new System.ArgumentException($""Cannot use NewLine for {{nameof(value)}}. (value: {{value}})""); }}"),
                ($@"}}")
            });

        /// <returns>バイトサイズチェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateByteLength(WorkState workState)
            => SourceTextFormatter.If(IsValidateByteLength(workState), new SourceFormatTarget[]
            {
                ($@"{{   // Validate for ByteLength"),
                ($@"    var encoding = System.Text.Encoding.GetEncoding(""{workState.PropertyValues[MyAttr.ByteLengthEncoding.Name]}"");"),
                ($@"    var byteLength = encoding.GetByteCount(value);"),
                ($@"    if (byteLength < {MyAttr.ByteMinLength.Name} || {MyAttr.ByteMaxLength.Name} < byteLength) throw new System.ArgumentOutOfRangeException(nameof(value), byteLength, $""byteLength between {MyAttr.ByteMinLength.Name} and {MyAttr.ByteMaxLength.Name}"");"),
                ($@"}}")
            });

        /// <returns>文字列長チェックソースコード文字列パート</returns>
        private static SourceFormatTargetBlock SourceFormatTargetsValidateLength(WorkState workState)
            => SourceTextFormatter.If(IsValidateLength(workState), new SourceFormatTarget[]
            {
                ($@"{{   // Validate for Length"),
                ($@"    var length = value.Length;"),
                ($@"    if (length < {MyAttr.MinLength.Name} || {MyAttr.MaxLength.Name} < length) throw new System.ArgumentOutOfRangeException(nameof(value), length, $""length between {MyAttr.MinLength.Name} and {MyAttr.MaxLength.Name}"");"),
                ($@"}}")
            });

        /// <summary>
        ///     int 文字列を int に変換する。
        /// </summary>
        /// <param name="src">変換対象</param>
        /// <returns>変換結果</returns>
        private static int ToInt32FromIntString(string src)
        {
            return src switch
            {
                "int.MaxValue" => int.MaxValue,
                "int.MinValue" => int.MinValue,
                _ => int.Parse(src)
            };
        }
    }
}
