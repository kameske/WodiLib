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
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Unary
{
    /// <summary>
    ///     演算子オーバーロード用のコード生成クラス
    /// </summary>
    internal class OperationCodeMaker
    {
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
        ///     対象型を返す単項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="operation">演算子</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock MakeSourceFormatTargetUnaryOperator(string operation)
        {
            var returnItemCode = ReturnTypeCode switch
            {
                OperationResultReturnCodeType.Code_New =>
                    $@"new {TargetClassName} (checked({operation}tmp));",
                OperationResultReturnCodeType.Code_ExplicitCast =>
                    $@"({TargetClassName}) (checked({operation}tmp));",
                OperationResultReturnCodeType.Code_ImplicitCast =>
                    $@"checked({operation}tmp);",
                _ => throw new ArgumentOutOfRangeException(nameof(ReturnTypeCode))
            };

            return new SourceFormatTargetBlock(
                $@"/// {Tag.Summary($"{operation} 演算子")}",
                $@"/// {Tag.Param("src", "対象")}",
                $@"/// {Tag.Returns("演算結果")}",
                $@"public static {TargetClassName} operator {operation}({TargetClassName} src)",
                $@"{{",
                $@"{__}var tmp = ({InnerCastType})src;",
                $@"{__}return {returnItemCode}",
                $@"}}",
                SourceFormatTarget.Empty
            );
        }
    }
}
