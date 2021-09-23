// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : OperationOverloadCodeMaker.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Helper
{
    /// <summary>
    ///     演算子オーバーロード用のコード生成クラス
    /// </summary>
    internal class OperationOverloadCodeMaker
    {
        /// <summary>単項演算子の引数名</summary>
        private const string UnaryOperatorArgName = "src";

        /// <summary>二項演算子の左項引数名</summary>
        private const string BinaryOperatorLeftArgName = "left";

        /// <summary>二項演算子の右項引数名</summary>
        private const string BinaryOperatorRightArgName = "right";

        /// <summary>対象クラス名</summary>
        private string TargetClassFullName { get; }

        /// <summary>対象クラス名</summary>
        private string TargetClassName { get; }

        /// <summary>値を保持するプロパティ名</summary>
        private string RawValuePropertyName { get; }

        /// <summary>内部保持する値の型名</summary>
        private string RawValueType { get; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="targetClassName">対象クラス名（名前空間を含まない）</param>
        /// <param name="targetClassNameSpace">対象クラス名前空間</param>
        /// <param name="rawValuePropertyName">値を保持するプロパティ名</param>
        /// <param name="rawValueType">内部保持する値の型名</param>
        public OperationOverloadCodeMaker(string targetClassName, string targetClassNameSpace,
            string rawValuePropertyName, string rawValueType)
        {
            TargetClassName = targetClassName;
            TargetClassFullName = $"{targetClassNameSpace}.{targetClassName}";
            RawValuePropertyName = rawValuePropertyName;
            RawValueType = rawValueType;
        }

        /// <summary>
        ///     単項演算子：インクリメントのオーバーロードコードを生成する。
        /// </summary>
        /// <param name="canIncrease">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock UnaryOperatorIncrement(bool canIncrease)
            => canIncrease
                ? new[]
                {
                    $@"/// {Tag.Summary($"++ 演算子")}",
                    $@"/// {Tag.Param(UnaryOperatorArgName, "項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix()} ++{SingleArgParam()} => "
                    + $@"new {TargetClassName}(checked(({RawValueType})({UnaryOperatorArgName}.{RawValuePropertyName} + ({RawValueType})1)));",
                    SourceFormatTarget.Empty
                }
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     単項演算子：デクリメントのオーバーロードコードを生成する。
        /// </summary>
        /// <param name="canDecrease">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock UnaryOperatorDecrement(bool canDecrease)
            => canDecrease
                ? new[]
                {
                    $@"/// {Tag.Summary($"-- 演算子")}",
                    $@"/// {Tag.Param(UnaryOperatorArgName, "項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix()} --{SingleArgParam()} => "
                    + $@"new {TargetClassName}(checked(({RawValueType})({UnaryOperatorArgName}.{RawValuePropertyName} - ({RawValueType})1)));",
                    SourceFormatTarget.Empty
                }
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     単項演算子：反転のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock UnaryOperatorComplement(bool canOperate)
            => canOperate
                ? new[]
                {
                    $@"/// {Tag.Summary($"~ 演算子")}",
                    $@"/// {Tag.Param(UnaryOperatorArgName, "項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix()} ~{SingleArgParam()} => "
                    + $@"new {TargetClassName}(checked(({RawValueType})(~{UnaryOperatorArgName}.{RawValuePropertyName})));",
                    SourceFormatTarget.Empty
                }
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     対象型を返す二項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="ope">演算子</param>
        /// <param name="otherFullNames">他項の型リスト</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock BinaryOperatorNewInstance(string ope, IEnumerable<string> otherFullNames,
            bool canOperate)
            => canOperate
                ? SourceFormatTargetBlock.Merge(
                    otherFullNames.Select(name => name.Equals(TargetClassFullName)
                        ? BinaryOperatorNewInstanceBySameClass(ope, true)
                            .TrimLastEmptyLine()
                        : BinaryOperatorNewInstanceByOtherClass(ope, new[] { name }, true)
                            .TrimLastEmptyLine()
                    ).ToArray()
                ).AddNewLine()
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     対象型を返す二項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="ope">演算子</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock BinaryOperatorNewInstanceBySameClass(string ope, bool canOperate)
            => canOperate
                ? new[]
                {
                    $@"/// {Tag.Summary($"{ope} 演算子")}",
                    $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                    $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix()} {ope}{DoubleArgParam(TargetClassName)} => "
                    + $@"new {TargetClassName} (checked(({RawValueType})({BinaryOperatorLeftArgName}.{RawValuePropertyName} {ope} {BinaryOperatorRightArgName}.{RawValuePropertyName})));",
                    SourceFormatTarget.Empty
                }
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     対象型を返す二項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="ope">演算子</param>
        /// <param name="rightClassTypes">右項のタイプ</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock BinaryOperatorNewInstanceByOtherClass(string ope,
            IEnumerable<string> rightClassTypes, bool canOperate)
            => canOperate
                ? SourceFormatTargetBlock.Merge(
                    rightClassTypes.Select(rightClassType =>
                        new SourceFormatTargetBlock
                        (
                            $@"/// {Tag.Summary($"{ope} 演算子")}",
                            $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                            $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                            $@"/// {Tag.Returns("演算結果")}",
                            $@"{OperatorPrefix()} {ope}{DoubleArgParam(rightClassType)} => "
                            + $@"new {TargetClassName} (checked(({RawValueType})({BinaryOperatorLeftArgName}.{RawValuePropertyName} {ope} ({RawValueType}){BinaryOperatorRightArgName})));"
                        )
                    ).ToArray()
                ).AddNewLine()
                : Array.Empty<SourceFormatTarget>();


        /// <summary>
        ///     2つの対象クラスインスタンスから<see cref="bool"/>型の結果を返す二項演算子オーバーロードコードを生成する。
        /// </summary>
        /// <param name="ope">演算子</param>
        /// <param name="otherFullNames">右項型</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock BinaryOperatorBool(string ope, IEnumerable<string> otherFullNames,
            bool canOperate)
            => canOperate
                ? SourceFormatTargetBlock.Merge(
                    otherFullNames.Select(otherFullName =>
                        otherFullName.Equals(TargetClassFullName)
                            ? BinaryOperatorBoolBySameClass(ope, true)
                                .TrimLastEmptyLine()
                            : BinaryOperatorBoolByOtherClass(ope, otherFullName, true)
                                .TrimLastEmptyLine()
                    ).ToArray()
                ).AddNewLine()
                : Array.Empty<SourceFormatTarget>();


        /// <summary>
        ///     2つの対象クラスインスタンスから<see cref="bool"/>型の結果を返す二項演算子オーバーロードコードを生成する。
        /// </summary>
        /// <param name="ope">演算子</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock BinaryOperatorBoolBySameClass(string ope, bool canOperate)
            => canOperate
                ? new[]
                {
                    $@"/// {Tag.Summary($"{ope} 演算子")}",
                    $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                    $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix("bool")} {ope}{DoubleArgParam()} => "
                    + $@"{BinaryOperatorLeftArgName}.{RawValuePropertyName} {ope} {BinaryOperatorRightArgName}.{RawValuePropertyName};",
                    SourceFormatTarget.Empty
                }
                : Array.Empty<SourceFormatTarget>();


        /// <summary>
        ///     2つの対象クラスインスタンスから<see cref="bool"/>型の結果を返す二項演算子オーバーロードコードを生成する。
        /// </summary>
        /// <param name="ope">演算子</param>
        /// <param name="rightClassType">右項型</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列ブロック</returns>
        public SourceFormatTargetBlock BinaryOperatorBoolByOtherClass(string ope, string rightClassType,
            bool canOperate)
            => canOperate
                ? new[]
                {
                    $@"/// {Tag.Summary($"{ope} 演算子")}",
                    $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                    $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix("bool")} {ope}{DoubleArgParam(rightClassType)} => "
                    + $@"{BinaryOperatorLeftArgName}.{RawValuePropertyName} {ope} ({RawValueType}){BinaryOperatorRightArgName};",
                    $@"/// {Tag.Summary($"{ope} 演算子")}",
                    $@"/// {Tag.Param(BinaryOperatorLeftArgName, "左項")}",
                    $@"/// {Tag.Param(BinaryOperatorRightArgName, "右項")}",
                    $@"/// {Tag.Returns("演算結果")}",
                    $@"{OperatorPrefix("bool")} {ope}{DoubleArgParam(rightClassType, TargetClassName)} => "
                    + $@"({RawValueType}){BinaryOperatorLeftArgName} {ope} {BinaryOperatorRightArgName}.{RawValuePropertyName};",
                    SourceFormatTarget.Empty
                }
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     単項演算子のソースコードプレフィックスを生成する。
        /// </summary>
        /// <returns>ソースコード文字列パーツ</returns>
        private string OperatorPrefix()
            => OperatorPrefix(TargetClassName);

        /// <summary>
        ///     単項演算子のソースコードプレフィックスを生成する。
        /// </summary>
        /// <param name="returnType">演算子の返却型</param>
        /// <returns>ソースコード文字列パーツ</returns>
        private string OperatorPrefix(string returnType)
            => $@"public static {returnType} operator";

        /// <summary>
        ///     引数を一つもつラムダ式のパラメータ定義ソースコードを生成する。
        /// </summary>
        /// <returns>ソースコード文字列パーツ</returns>
        private string SingleArgParam()
            => $@"({TargetClassName} {UnaryOperatorArgName})";

        /// <summary>
        ///     引数として自身の型を二つもつラムダ式のパラメータ定義ソースコードを生成する。
        /// </summary>
        /// <returns></returns>
        private string DoubleArgParam()
            => DoubleArgParam(TargetClassName, TargetClassName);

        /// <summary>
        ///     引数を二つもつラムダ式のパラメータ定義ソースコードを生成する。
        /// </summary>
        /// <param name="otherType">もう一つの引数タイプ</param>
        /// <returns>ソースコード文字列パーツ</returns>
        private string DoubleArgParam(string otherType)
            => DoubleArgParam(TargetClassName, otherType);

        /// <summary>
        ///     引数を二つもつラムダ式のパラメータ定義ソースコードを生成する。
        /// </summary>
        /// <param name="leftType">左項の型</param>
        /// <param name="rightType">右項の型</param>
        /// <returns>ソースコード文字列パーツ</returns>
        private string DoubleArgParam(string leftType, string rightType)
            => $@"({leftType} {BinaryOperatorLeftArgName}, {rightType} {BinaryOperatorRightArgName})";
    }

    internal static class OperationOverloadCodeMakerExtension
    {
        /// <summary>
        ///     右辺に <see cref="int"/> 型を取り、対象型を返す二項演算子のオーバーロードコードを生成する。
        /// </summary>
        /// <param name="src"><see cref="OperationOverloadCodeMaker"/>インスタンス</param>
        /// <param name="ope">演算子</param>
        /// <param name="canOperate">コード生成可否</param>
        /// <returns>ソースコード文字列</returns>
        public static SourceFormatTargetBlock BinaryOperatorNewInstanceByInt(this OperationOverloadCodeMaker src,
            string ope, bool canOperate)
            => src.BinaryOperatorNewInstance(ope, new[] { "int" }, canOperate);
    }
}
