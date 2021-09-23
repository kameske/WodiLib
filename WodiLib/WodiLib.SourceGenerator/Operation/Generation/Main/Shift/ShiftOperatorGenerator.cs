// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ShiftOperatorGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using System.Text;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;
using MyAttr =
    WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Attributes.ShiftOperateAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Shift
{
    internal class ShiftOperatorGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        /// <inheritDoc/>
        private protected override string HintName(WorkState workState)
        {
            var thisType = workState.FullName;

            var operationCode = workState.PropertyValues[MyAttr.Operation.Name]!;

            var operationCodeHex = $"{int.Parse(operationCode):X}";

            return $"{thisType.CompressNameSpace()}.ShiftOperation0x{operationCodeHex}";
        }

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var thisType = workState.PropertyValues.TargetSymbol?.Name ?? "";
            var defInfo = workState.CurrentTypeDefinitionInfo;

            var propValues = workState.PropertyValues;
            var operation = propValues[MyAttr.Operation.Name]!;
            var innerCastType = propValues[MyAttr.InnerCastType.Name]!;
            var returnTypeCode = int.Parse(propValues[MyAttr.ReturnCodeType.Name]!);

            var codeMaker = new OperationCodeMaker(thisType, innerCastType, returnTypeCode);

            return SourceTextFormatter.Format("", new SourceFormatTarget[]
                {
                    ($@"{DefinitionSource(defInfo)} {thisType}"),
                    ($@"{{")
                },
                SourceTextFormatter.Format(IndentSpace, OperationBlock(codeMaker, operation)),
                new SourceFormatTarget[]
                {
                    ($@"}}")
                });
        }

        /// <summary>
        ///     定義宣言部のソースを生成する。
        /// </summary>
        /// <param name="typeDefinitionInfo">型定義情報</param>
        /// <returns>ソースコード文字列</returns>
        private static string DefinitionSource(TypeDefinitionInfo typeDefinitionInfo)
        {
            var resultBuilder = new StringBuilder();

            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);
            resultBuilder.Append(accessibility);
            resultBuilder.Append(" partial ");
            if (typeDefinitionInfo.IsClass)
            {
                resultBuilder.Append(" class ");
            }
            else if (typeDefinitionInfo.IsRecord)
            {
                resultBuilder.Append(" record ");
            }
            else
            {
                resultBuilder.Append(" struct ");
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        ///     演算子オーバーロードソースコードブロックを生成する。
        /// </summary>
        /// <param name="codeMaker">演算子オーバーロードコード生成処理</param>
        /// <param name="operationCode">オーバーロードする演算子フラグ文字列</param>
        /// <returns></returns>
        private static SourceFormatTargetBlock OperationBlock(OperationCodeMaker codeMaker, string operationCode)
            => SourceFormatTargetBlock.Merge(
                new (string ope, Func<string, bool> determineMake)[]
                    {
                        ("<<", ShiftOperationType.CanLeftShift),
                        (">>", ShiftOperationType.CanRightShift)
                    }.Where(param => param.determineMake(operationCode))
                    .Select(param => codeMaker.MakeSourceFormatTargetShiftOperator(param.ope))
                    .ToArray()
            ).TrimLastEmptyLine();

        private ShiftOperatorGenerator()
        {
        }

        public static ShiftOperatorGenerator Instance { get; } = new();
    }
}
