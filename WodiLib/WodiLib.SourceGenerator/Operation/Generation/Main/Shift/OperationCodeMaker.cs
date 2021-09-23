// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : OperationCodeMaker.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Shift
{
    /// <summary>
    ///     演算子オーバーロード用のコード生成クラス
    /// </summary>
    internal class OperationCodeMaker
    {
        /// <summary>二項演算子の左項引数名</summary>
        private const string BinaryOperatorLeftArgName = "left";

        /// <summary>二項演算子の右項引数名</summary>
        private const string BinaryOperatorRightArgName = "right";

        /// <summary>対象クラス名</summary>
        private string TargetClassName { get; }

        /// <summary>演算時キャスト型</summary>
        private string InnerCastType { get; }

        /// <summary>演算結果返却コード種別コード値</summary>
        private int ReturnTypeCode { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="targetClassName">対象クラス名（名前空間を含まない）</param>
        /// <param name="innerCastType">演算時キャスト型</param>
        /// <param name="returnTypeCode">演算結果返却コード種別コード値</param>
        public OperationCodeMaker(string targetClassName, string innerCastType, int returnTypeCode)
        {
            TargetClassName = targetClassName;
            InnerCastType = innerCastType;
            ReturnTypeCode = returnTypeCode;
        }

        /// <summary>
        ///     対象型を返すシフト演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="operation">演算子</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock MakeSourceFormatTargetShiftOperator(string operation)
        {
            var returnItemCode = ReturnTypeCode switch
            {
                OperationResultReturnCodeType.Code_New =>
                    $@"new {TargetClassName} (checked((({InnerCastType}) {BinaryOperatorLeftArgName}) {operation} {BinaryOperatorRightArgName}));",
                OperationResultReturnCodeType.Code_ExplicitCast =>
                    $@"({TargetClassName}) (checked((({InnerCastType}) {BinaryOperatorLeftArgName}) {operation} {BinaryOperatorRightArgName}));",
                OperationResultReturnCodeType.Code_ImplicitCast =>
                    $@"checked((({InnerCastType}) {BinaryOperatorLeftArgName}) {operation} {BinaryOperatorRightArgName});",
                _ => throw new ArgumentOutOfRangeException(nameof(ReturnTypeCode))
            };
            return new SourceFormatTargetBlock(
                $@"/// {Tag.Summary($"{operation} 演算子")}",
                $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                $@"/// {Tag.Returns("演算結果")}",
                $@"public static {TargetClassName} operator {operation}({TargetClassName} {BinaryOperatorLeftArgName}, int {BinaryOperatorRightArgName}) => "
                + returnItemCode,
                SourceFormatTarget.Empty
            );
        }
    }
}
