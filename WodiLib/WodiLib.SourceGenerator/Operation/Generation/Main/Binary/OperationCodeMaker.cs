// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : OperationCodeMaker.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Operation.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.Operation.Generation.Main.Binary
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

        /// <summary>演算相手クラス型名リスト</summary>
        private string[] OtherTypes { get; }

        /// <summary>延算時キャストクラス</summary>
        private string InnerCastType { get; }

        /// <summary>返戻型</summary>
        private string ReturnType { get; }

        /// <summary>対象クラスを左項に配置するかどうか</summary>
        private bool TargetClassIsLeft { get; }

        /// <summary>演算結果返却コード種別コード値</summary>
        private int ReturnTypeCode { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="targetClassName">対象クラス名（名前空間を含まない）</param>
        /// <param name="otherTypes">演算相手クラス型名リスト</param>
        /// <param name="innerCastType">演算時にキャストする型</param>
        /// <param name="returnType">返戻型</param>
        /// <param name="targetClassIsLeft">対象クラスを左項に配置するかどうか</param>
        /// <param name="returnTypeCode">演算結果返却コード種別コード値</param>
        public OperationCodeMaker(string targetClassName, string[] otherTypes, string innerCastType, string returnType,
            bool targetClassIsLeft, int returnTypeCode)
        {
            TargetClassName = targetClassName;
            OtherTypes = otherTypes;
            InnerCastType = innerCastType;
            ReturnType = returnType;
            TargetClassIsLeft = targetClassIsLeft;
            ReturnTypeCode = returnTypeCode;
        }

        /// <summary>
        ///     対象型を返す二項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="operation">演算子</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock MakeSourceFormatTargetBinaryOperator(string operation)
            => SourceFormatTargetBlock.Merge(
                OtherTypes.Select(otherType => MakeSourceFormatTargetBinaryOperatorOneType(operation, otherType))
                    .ToArray()
            );

        /// <summary>
        ///     対象型を返す二項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="operation">演算子</param>
        /// <param name="otherType">演算相手の型</param>
        /// <returns>ソースコード文字列ブロック</returns>
        private SourceFormatTargetBlock MakeSourceFormatTargetBinaryOperatorOneType(string operation, string otherType)
        {
            var caster = new CastItemCodeMaker(TargetClassName, otherType, InnerCastType, TargetClassIsLeft);
            var returnItemCode = ReturnTypeCode switch
            {
                OperationResultReturnCodeType.Code_New =>
                    $@"new {ReturnType} (checked(({InnerCastType}) ({caster.LeftItemCode} {operation} {caster.RightItemCode})));",
                OperationResultReturnCodeType.Code_ExplicitCast =>
                    $@"({ReturnType}) (checked(({InnerCastType}) ({caster.LeftItemCode} {operation} {caster.RightItemCode})));",
                OperationResultReturnCodeType.Code_ImplicitCast =>
                    $@"checked(({InnerCastType}) ({caster.LeftItemCode} {operation} {caster.RightItemCode}));",
                _ => throw new ArgumentOutOfRangeException(nameof(ReturnTypeCode))
            };

            return new SourceFormatTargetBlock
            (
                $@"/// {Tag.Summary($"{operation} 演算子")}",
                $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                $@"/// {Tag.Returns("演算結果")}",
                $@"public static {ReturnType} operator {operation}({ArgsDefinition(otherType)}) => {returnItemCode}",
                SourceFormatTarget.Empty
            );
        }

        private string ArgsDefinition(string otherType)
        {
            var (leftClass, rightClass) = TargetClassIsLeft
                ? (TargetClassName, otherType)
                : (otherType, TargetClassName);
            return $@"{leftClass} {BinaryOperatorLeftArgName}, {rightClass} {BinaryOperatorRightArgName}";
        }

        private class CastItemCodeMaker
        {
            public string TargetClassType { get; }
            public string OtherType { get; }
            public string InnerCastType { get; }
            public bool TargetClassIsLeft { get; }

            public CastItemCodeMaker(string targetClassType, string otherType, string innerCastType,
                bool targetClassIsLeft)
            {
                TargetClassType = targetClassType;
                OtherType = otherType;
                InnerCastType = innerCastType;
                TargetClassIsLeft = targetClassIsLeft;
            }

            public string LeftItemCode
                => CastItemCode(BinaryOperatorLeftArgName, true);

            public string RightItemCode
                => CastItemCode(BinaryOperatorRightArgName, false);

            public string CastItemCode(string itemName, bool isLeft)
                => isLeft == TargetClassIsLeft
                    ? CastItemCode(itemName, TargetClassType)
                    : CastItemCode(itemName, OtherType);

            private string CastItemCode(string itemName, string itemType)
                => itemType.Equals(InnerCastType)
                    ? $"{itemName}"
                    : $"(({InnerCastType}) {itemName})";
        }
    }
}
