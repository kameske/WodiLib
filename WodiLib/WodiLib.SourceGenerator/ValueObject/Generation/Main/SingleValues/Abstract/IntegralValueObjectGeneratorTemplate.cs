// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntegralValueObjectGeneratorTemplate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Enums;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Generation.Helper;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract.
    IntegralNumericValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    /// <summary>
    ///     数値単一値オブジェクトジェネレータのテンプレートクラス
    /// </summary>
    internal abstract class IntegralValueObjectGeneratorTemplate : SingleValueObjectGeneratorTemplate
    {
        /// <inheritdoc/>
        private protected override bool IsImplementEquatable(WorkState workState)
            => !workState.CurrentTypeDefinitionInfo.ObjectType.Equals(ObjectType.Record);

        /// <inheritdoc/>
        private protected sealed override bool IsImplementFormattable(WorkState workState)
            => bool.Parse(workState.PropertyValues[MyAttr.IsUseBasicFormattable.Name]!);

        private protected sealed override bool ParentIsImplementFormattable(WorkState workState)
            => bool.Parse(workState.GetOrDefaultParentPropertyValue(MyAttr.IsUseBasicFormattable.Name, "false"));

        /// <inheritdoc/>
        private protected sealed override bool ParentIsImplementComparable(WorkState workState)
            => IntegralNumericOperation.CanCompare(workState.GetParentPropertyValue(MyAttr.Operations.Name));

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsPublicStaticProperties(
            WorkState workState)
        {
            var workResult = workState.PropertyValues;

            var isValidateRange = IsValidateRange(workState);
            var isValidateSafetyRange = IsValidateSafetyRange(workState);

            if (!isValidateRange && !isValidateSafetyRange) return Array.Empty<SourceFormatTarget>();

            return SourceTextFormatter.Format("",
                SourceTextFormatter.If(isValidateRange,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MaxValue, workResult, workState, "int.MaxValue",
                        workState.IsAnyPropertyOverwritten(MyAttr.MaxValue.Name, MyAttr.MinValue.Name)),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.MinValue, workResult, workState, "int.MinValue",
                        workState.IsAnyPropertyOverwritten(MyAttr.MaxValue.Name, MyAttr.MinValue.Name))
                ), SourceTextFormatter.If(isValidateSafetyRange,
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.SafetyMaxValue, workResult, workState, "int.MaxValue",
                        workState.IsAnyPropertyOverwritten(MyAttr.SafetyMaxValue.Name, MyAttr.SafetyMinValue.Name)),
                    SourceFormatTargetHelper.SourceFormatTargetsClassConstant_Numeric(
                        MyAttr.SafetyMinValue, workResult, workState, "int.MinValue",
                        workState.IsAnyPropertyOverwritten(MyAttr.SafetyMaxValue.Name, MyAttr.SafetyMinValue.Name))
                )
            );
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorException(
            WorkState workState)
        {
            if (!IsValidateRange(workState)) return Array.Empty<SourceFormatTarget>();

            return SourceTextFormatter.Format("", new SourceFormatTarget[]
            {
                ($@"/// <exception cref=""{typeof(ArgumentOutOfRangeException).FullName}"">"),
                ($@"/// {__}<paramref name=""value""/> が <see cref=""{MyAttr.MinValue.Name}""/> 未満または <see cref=""{MyAttr.MaxValue.Name}""/> を超える場合。"),
                ($@"/// </exception>")
            });
        }

        /// <inheritdoc/>
        private protected override SourceFormatTargetBlock SourceFormatTargetsConstructorBody(
            WorkState workState)
            => SourceTextFormatter.Format("",
                SourceTextFormatter.If(IsValidateRange(workState), new SourceFormatTarget[]
                {
                    ($@"{{   // value range"),
                    ($@"{__}if (value < {MyAttr.MinValue.Name} || {MyAttr.MaxValue.Name} < value) throw new System.ArgumentOutOfRangeException(nameof(value), value, $""value between {{{MyAttr.MinValue.Name}}} and {{{MyAttr.MaxValue.Name}}}"");"),
                    ($@"}}")
                }),
                SourceTextFormatter.If(IsValidateSafetyRange(workState), new SourceFormatTarget[]
                {
                    ($@"{{   // safety value range"),
                    ($@"{__}if (value < {MyAttr.SafetyMinValue.Name} || {MyAttr.SafetyMaxValue.Name} < value) WodiLib.Sys.Cmn.WodiLibLogger.GetInstance().Warning(WodiLib.Sys.WarningMessage.OutOfRange(nameof(value), {MyAttr.SafetyMinValue.Name}, {MyAttr.SafetyMaxValue.Name}, value));"),
                    ($@"}}")
                }), new SourceFormatTarget[]
                {
                    // DoConstructorExpansion メソッドで RawValue を更新する可能性があるので RawValue の初期化を先に行う
                    ($@"{workState.PropertyValues[MyAttr.PropertyName.Name]} = value;"),
                    ($@"DoConstructorExpansion(value);")
                }
            );

        /// <inheritdoc/>
        private protected sealed override SourceFormatTargetBlock SourceFormatTargetsExtendBody(
            WorkState workState)
        {
            var workResult = workState.PropertyValues;

            var fullName = workState.FullName;

            var operations = workResult[MyAttr.Operations.Name];
            var canIncreaseAndDecrease = IntegralNumericOperation.CanIncreaseAndDecrease(operations);
            var addAndSubtractableTypes = AddAndSubtractableTypes(workResult, fullName).ToArray();
            var multipleAndDividableTypes = MultipleAndDividableTypes(workResult, fullName).ToArray();
            var canModulo = IntegralNumericOperation.CanModulo(operations);
            var canComplement = IntegralNumericOperation.CanComplement(operations);
            var canShift = IntegralNumericOperation.CanShift(operations);
            var canAnd = IntegralNumericOperation.CanAnd(operations);
            var canOr = IntegralNumericOperation.CanOr(operations);
            var canXor = IntegralNumericOperation.CanXor(operations);
            var comparableTypes = ComparableTypes(workResult, fullName).ToArray();

            var operationOverloadCodeMaker = new OperationOverloadCodeMaker(workResult.Name, workResult.Namespace,
                workResult[MyAttr.PropertyName.Name]!, WrapType.FullName!);

            return SourceTextFormatter.Format("",
                operationOverloadCodeMaker.UnaryOperatorIncrement(canIncreaseAndDecrease),
                operationOverloadCodeMaker.UnaryOperatorDecrement(canIncreaseAndDecrease),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("+", addAndSubtractableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("-", addAndSubtractableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("*", multipleAndDividableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("/", multipleAndDividableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorNewInstance("%", new[] { "int" }, canModulo),
                operationOverloadCodeMaker.UnaryOperatorComplement(canComplement),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceByInt("<<", canShift),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceByInt(">>", canShift),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceBySameClass("&", canAnd),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceBySameClass("|", canOr),
                operationOverloadCodeMaker.BinaryOperatorNewInstanceBySameClass("^", canXor),
                operationOverloadCodeMaker.BinaryOperatorBool("<", comparableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorBool("<=", comparableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorBool(">=", comparableTypes, true),
                operationOverloadCodeMaker.BinaryOperatorBool(">", comparableTypes, true),
                new SourceFormatTarget[]
                {
                    ($@"partial void DoConstructorExpansion({WrapType} value);")
                });
        }

        /// <returns>範囲チェックを行う場合 <see langword="true"/> </returns>
        private static bool IsValidateRange(WorkState workState)
            => workState.IsAnyPropertyInitialized(MyAttr.MaxValue.Name, MyAttr.MinValue.Name);

        /// <returns>安全範囲チェックを行う場合 <see langword="true"/> </returns>
        private static bool IsValidateSafetyRange(WorkState workState)
            => workState.IsAnyPropertyInitialized(MyAttr.SafetyMaxValue.Name, MyAttr.SafetyMinValue.Name);

        /// <returns>加減算可能な右項型の一覧</returns>
        private static IEnumerable<string> AddAndSubtractableTypes(PropertyValues workResult, string fullName)
            => OperationRightTypes(workResult, fullName, IntegralNumericOperation.CanAddAndSubtract,
                MyAttr.AddAndSubtractTypes.Name);

        /// <returns>加減算可能な右項型の一覧</returns>
        private static IEnumerable<string> MultipleAndDividableTypes(PropertyValues workResult, string fullName)
            => OperationRightTypes(workResult, fullName, IntegralNumericOperation.CanMultipleAndDivide,
                MyAttr.MultipleAndDivideOtherTypes.Name);

        /// <returns>比較可能な右項型の一覧</returns>
        private static IEnumerable<string> ComparableTypes(PropertyValues workResult, string fullName)
            => OperationRightTypes(workResult, fullName, IntegralNumericOperation.CanCompare,
                MyAttr.CompareOtherTypes.Name);

        /// <returns>演算可能な右項型の一覧</returns>
        private static IEnumerable<string> OperationRightTypes(PropertyValues workResult, string fullName,
            Func<string, bool> funcOperateSameType, string getTypesPropertyName)
        {
            var operateSameType = funcOperateSameType(workResult.GetOrDefault(MyAttr.Operations.Name, "0"));
            var isInitializedTypes = workResult[getTypesPropertyName] is not null;
            if (!operateSameType && !isInitializedTypes) return Array.Empty<string>();

            var result = new List<string>();
            if (isInitializedTypes)
            {
                result = new List<string>(GetTypeNameList(workResult, getTypesPropertyName));
            }

            if (operateSameType && !result.Contains(fullName))
            {
                result = new[] { fullName }.Concat(result).ToList();
            }

            return result;
        }

        private static IEnumerable<string> GetTypeNameList(PropertyValues workResult, string propertyName)
            => workResult[propertyName] == null
                ? Array.Empty<string>()
                : workResult[propertyName]!.Split(',').Select(s => s.Trim()).ToArray();
    }
}
