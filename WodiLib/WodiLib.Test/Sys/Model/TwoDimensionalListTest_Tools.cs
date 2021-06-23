// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListTest_Tools.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Sys
{
    /// <summary>
    ///     <see cref="TwoDimensionalList{T}"/> のテスト用ツール定義クラス
    /// </summary>
    internal static class TwoDimensionalListTest_Tools
    {
        #region TestParams

        /// <summary>
        ///     初期インスタンス行数
        /// </summary>
        public const int InitRowLength = 4;

        /// <summary>
        ///     初期インスタンス列数
        /// </summary>
        public const int InitColumnLength = 6;

        /// <summary>
        ///     CopyToメソッドテスト時に引数に指定する一次元配列の余剰領域
        /// </summary>
        public const int OneArrayBuffer = 3;

        /// <summary>
        ///     CopyToメソッドテスト時に引数に指定する二次元配列の行方向余剰領域
        /// </summary>
        public const int TwoDimArrayRowBuffer = 7;

        /// <summary>
        ///     CopyToメソッドテスト時に引数に指定する二次元配列の列方向余剰領域
        /// </summary>
        public const int TwoDimArrayColumnBuffer = 2;

        #endregion

        #region TestRecord

        /// <summary>
        ///     テスト要素
        /// </summary>
        public record TestRecord : IEqualityComparable<TestRecord>, IDeepCloneable<TestRecord>
        {
            public string Value { get; }

            public TestRecord(string value)
            {
                Value = value;
            }

            public bool ItemEquals(TestRecord other)
            {
                return Value.Equals(other?.Value);
            }

            public bool ItemEquals(object other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (other is not TestRecord casted) return false;
                return ItemEquals(casted);
            }

            public TestRecord DeepClone()
                => new(Value);

            object IDeepCloneable.DeepClone()
                => DeepClone();

            public override string ToString()
            {
                return Value;
            }
        }

        public static TestRecord MakeInitItem(int row, int column)
            => MakeItem(row, column, "InitItem");

        public static TestRecord MakeInsertItem(int row, int column)
            => MakeItem(row, column, "InsertItem");

        public static TestRecord MakeListDefaultItem(int row, int column)
            => MakeItem(row, column, "ListDefault");

        public static TestRecord MakeItem(int row, int column, string prefix = "", string suffix = "")
        {
            return new($"{prefix}_{row:d2}_{column:d2}_{suffix}");
        }

        #endregion

        #region TestInstanceType_DoubleEnumerable

        /// <summary>
        ///     テスト用 IEnumerable{IEnumerable{TestRecord}} インスタンス種別
        /// </summary>
        public enum TestDoubleEnumerableInstanceType
        {
            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>
            /// </summary>
            NotNull_RowBasic_ColumnBasic,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/> - 1,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>
            /// </summary>
            NotNull_RowShort_ColumnBasic,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/> + 1,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>
            /// </summary>
            NotNull_RowLong_ColumnBasic,

            /// <summary>
            ///     行数 = 1,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>
            /// </summary>
            NotNull_RowOne_ColumnBasic,

            /// <summary>
            ///     行数 = 1,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> - 1
            /// </summary>
            NotNull_RowOne_ColumnShort,

            /// <summary>
            ///     行数 = 1,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> + 1
            /// </summary>
            NotNull_RowOne_ColumnLong,

            /// <summary>
            ///     行数 = 1,
            ///     列数 = 1
            /// </summary>
            NotNull_RowOne_ColumnOne,

            /// <summary>
            ///     行数 = 1,
            ///     列数 = 2
            /// </summary>
            NotNull_RowOne_ColumnTwo,

            /// <summary>
            ///     行数 = 2,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>
            /// </summary>
            NotNull_RowTwo_ColumnBasic,

            /// <summary>
            ///     行数 = 2,
            ///     列数 = 1
            /// </summary>
            NotNull_RowTwo_ColumnOne,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> - 1
            /// </summary>
            NotNull_RowBasic_ColumnShort,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> + 1
            /// </summary>
            NotNull_RowBasic_ColumnLong,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = 1
            /// </summary>
            NotNull_RowBasic_ColumnOne,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/> - 1,
            ///     列数 = 1
            /// </summary>
            NotNull_RowShort_ColumnOne,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/> + 1,
            ///     列数 = 1
            /// </summary>
            NotNull_RowLong_ColumnOne,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = 2
            /// </summary>
            NotNull_RowBasic_ColumnTwo,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>、
            ///     <see cref="NotNull_RowBasic_ColumnBasic"/> と異なる要素がある
            /// </summary>
            NotNull_HasDifferenceItemComparedBasic,

            /// <summary>
            ///     行数 = 0,
            ///     列数 = 0
            /// </summary>
            Empty,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = 0
            /// </summary>
            EmptyRows,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>,
            ///     <see langword="null"/> 行あり（奇数行）
            /// </summary>
            HasNullRow,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>,
            ///     <see langword="null"/> 列あり（奇数列）
            /// </summary>
            HasNullColumn,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> & 奇数列の場合 + 1,
            /// </summary>
            Jagged,

            /// <summary>
            ///     <see langword="null"/>
            /// </summary>
            Null
        }

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> の列挙子名および転置フラグから行数を取得する。
        /// </summary>
        public static int InitRowLengthFrom(string value, bool isTranspose)
            => InitRowLengthFrom(TestDoubleEnumerableInstanceTypeFrom(value), isTranspose);

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> および転置フラグから行数を取得する。
        /// </summary>
        public static int InitRowLengthFrom(TestDoubleEnumerableInstanceType type, bool isTranspose)
            => (type, isTranspose) switch
            {
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic, false) => InitRowLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic, false) => InitRowLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort, true) => InitColumnLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong, true) => InitColumnLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo, true) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, false) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, false) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort, true) => InitColumnLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong, true) => InitColumnLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne, false) => InitRowLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne, false) => InitRowLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo, true) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.Empty, _) => 0,
                (TestDoubleEnumerableInstanceType.EmptyRows, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.EmptyRows, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.HasNullRow, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.HasNullRow, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.HasNullColumn, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.HasNullColumn, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.Jagged, false) => InitRowLength,
                (TestDoubleEnumerableInstanceType.Jagged, true) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.Null, _) => -1,
                _ => throw new ArgumentOutOfRangeException()
            };

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> の列挙子名および転置フラグから列数を取得する。
        /// </summary>
        public static int InitColumnLengthFrom(string value, bool isTranspose)
            => InitColumnLengthFrom(TestDoubleEnumerableInstanceTypeFrom(value), isTranspose);

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> および転置フラグから列数を取得する。
        /// </summary>
        public static int InitColumnLengthFrom(TestDoubleEnumerableInstanceType type, bool isTranspose)
            => (type, isTranspose) switch
            {
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic, true) => InitRowLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic, true) => InitRowLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort, false) => InitColumnLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong, false) => InitColumnLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo, false) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo, true) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, true) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, true) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort, false) => InitColumnLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong, false) => InitColumnLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne, true) => InitRowLength - 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne, false) => 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne, true) => InitRowLength + 1,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo, false) => 2,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.Empty, _) => 0,
                (TestDoubleEnumerableInstanceType.EmptyRows, _) => 0,
                (TestDoubleEnumerableInstanceType.HasNullRow, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.HasNullRow, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.HasNullColumn, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.HasNullColumn, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.Jagged, false) => InitColumnLength,
                (TestDoubleEnumerableInstanceType.Jagged, true) => InitRowLength,
                (TestDoubleEnumerableInstanceType.Null, _) => -1,
                _ => throw new ArgumentOutOfRangeException()
            };

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> の列挙子名および転置フラグから空フラグを取得する。
        /// </summary>
        public static bool IsEmptyFrom(string value, bool isTranspose)
            => IsEmptyFrom(TestDoubleEnumerableInstanceTypeFrom(value), isTranspose);

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> および転置フラグから空フラグを取得する。
        /// </summary>
        public static bool IsEmptyFrom(TestDoubleEnumerableInstanceType type, bool isTranspose)
            => (type, isTranspose) switch
            {
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo, _) => false,
                (TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, _) => false,
                (TestDoubleEnumerableInstanceType.Empty, _) => true,
                (TestDoubleEnumerableInstanceType.EmptyRows, false) => false,
                (TestDoubleEnumerableInstanceType.EmptyRows, true) => true,
                (TestDoubleEnumerableInstanceType.HasNullRow, _) => false,
                (TestDoubleEnumerableInstanceType.HasNullColumn, _) => false,
                (TestDoubleEnumerableInstanceType.Jagged, _) => false,
                (TestDoubleEnumerableInstanceType.Null, _) => false,
                _ => throw new ArgumentOutOfRangeException()
            };

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> の列挙子名から<see cref="TestDoubleEnumerableInstanceType"/> インスタンスを取得する。
        /// </summary>
        private static TestDoubleEnumerableInstanceType TestDoubleEnumerableInstanceTypeFrom(string value)
            => value switch
            {
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne,
                nameof(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo) =>
                    TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                nameof(TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic) =>
                    TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic,
                nameof(TestDoubleEnumerableInstanceType.Empty) => TestDoubleEnumerableInstanceType.Empty,
                nameof(TestDoubleEnumerableInstanceType.EmptyRows) => TestDoubleEnumerableInstanceType.EmptyRows,
                nameof(TestDoubleEnumerableInstanceType.HasNullRow) => TestDoubleEnumerableInstanceType.HasNullRow,
                nameof(TestDoubleEnumerableInstanceType.HasNullColumn) =>
                    TestDoubleEnumerableInstanceType.HasNullColumn,
                nameof(TestDoubleEnumerableInstanceType.Jagged) => TestDoubleEnumerableInstanceType.Jagged,
                nameof(TestDoubleEnumerableInstanceType.Null) => TestDoubleEnumerableInstanceType.Null,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };

        /// <summary>
        ///     <see cref="TestDoubleEnumerableInstanceType"/> および転置フラグから<see cref="TestRecordListType"/> インスタンスを取得する。
        /// </summary>
        private static TestRecordListType TestRecordListTypeFrom(TestDoubleEnumerableInstanceType type,
            bool isTranspose)
            => (type, isTranspose) switch
            {
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnBasic, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnBasic, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnShort, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnLong, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnTwo, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnShort, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnLong, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowShort_ColumnOne, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowLong_ColumnOne, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, _) => TestRecordListType
                    .NotNull,
                (TestDoubleEnumerableInstanceType.Empty, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.EmptyRows, _) => TestRecordListType.NotNull,
                (TestDoubleEnumerableInstanceType.HasNullRow, false) => TestRecordListType.HasNullRow,
                (TestDoubleEnumerableInstanceType.HasNullRow, true) => TestRecordListType.HasNullColumn,
                (TestDoubleEnumerableInstanceType.HasNullColumn, false) => TestRecordListType.HasNullColumn,
                (TestDoubleEnumerableInstanceType.HasNullColumn, true) => TestRecordListType.HasNullRow,
                (TestDoubleEnumerableInstanceType.Jagged, _) => TestRecordListType.Jagged,
                (TestDoubleEnumerableInstanceType.Null, _) => TestRecordListType.NotNull,
                _ => throw new ArgumentException()
            };

        #endregion

        #region TestInstanceType_SingleEnumerable

        /// <summary>
        ///     テスト用 IEnumerable{TestRecord} インスタンス種別
        /// </summary>
        public enum TestSingleEnumerableInstanceType
        {
            /// <summary>
            ///     要素数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     または <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>
            /// </summary>
            NotNull_Basic,

            /// <summary>
            ///     要素数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/> - 1,
            ///     または <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> - 1
            /// </summary>
            NotNull_Short,

            /// <summary>
            ///     要素数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/> + 1,
            ///     または <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/> + 1
            /// </summary>
            NotNull_Long,

            /// <summary>
            ///     行数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     列数 = <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>,
            ///     <see cref="NotNull_Basic"/> と異なる要素がある
            /// </summary>
            NotNull_HasDifferenceItemComparedBasic,

            /// <summary>
            ///     要素数 = 0
            /// </summary>
            Empty,

            /// <summary>
            ///     要素数 = <see cref="TwoDimensionalListTest_Tools.InitRowLength"/>,
            ///     または <see cref="TwoDimensionalListTest_Tools.InitColumnLength"/>,
            ///     <see langword="null"/> 要素あり（奇数番目）
            /// </summary>
            HasNullItem,

            /// <summary>
            ///     <see langword="null"/>
            /// </summary>
            Null
        }

        /// <summary>
        ///     <see cref="TestSingleEnumerableInstanceType"/> の列挙子名および転置フラグから行数を取得する。
        /// </summary>
        public static int InitLengthFrom(string value, bool directionIsRow)
            => InitLengthFrom(TestSingleEnumerableInstanceTypeFrom(value), directionIsRow);

        /// <summary>
        ///     <see cref="TestSingleEnumerableInstanceType"/> および転置フラグから行数を取得する。
        /// </summary>
        public static int InitLengthFrom(TestSingleEnumerableInstanceType type, bool directionIsRow)
            => (type, directionIsRow) switch
            {
                (TestSingleEnumerableInstanceType.NotNull_Basic, true) => InitRowLength,
                (TestSingleEnumerableInstanceType.NotNull_Basic, false) => InitColumnLength,
                (TestSingleEnumerableInstanceType.NotNull_Short, true) => InitRowLength - 1,
                (TestSingleEnumerableInstanceType.NotNull_Short, false) => InitColumnLength - 1,
                (TestSingleEnumerableInstanceType.NotNull_Long, true) => InitRowLength + 1,
                (TestSingleEnumerableInstanceType.NotNull_Long, false) => InitColumnLength + 1,
                (TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, true) => InitRowLength,
                (TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, false) => InitColumnLength,
                (TestSingleEnumerableInstanceType.Empty, _) => 0,
                (TestSingleEnumerableInstanceType.HasNullItem, true) => InitRowLength,
                (TestSingleEnumerableInstanceType.HasNullItem, false) => InitColumnLength,
                (TestSingleEnumerableInstanceType.Null, _) => -1,
                _ => throw new ArgumentOutOfRangeException()
            };

        /// <summary>
        ///     <see cref="TestSingleEnumerableInstanceType"/> の列挙子名および転置フラグから空フラグを取得する。
        /// </summary>
        public static bool IsEmptyLineFrom(string value, bool isTranspose)
            => IsEmptyLineFrom(TestSingleEnumerableInstanceTypeFrom(value), isTranspose);

        /// <summary>
        ///     <see cref="TestSingleEnumerableInstanceType"/> および転置フラグから空フラグを取得する。
        /// </summary>
        public static bool IsEmptyLineFrom(TestSingleEnumerableInstanceType type, bool isTranspose)
            => (type, isTranspose) switch
            {
                (TestSingleEnumerableInstanceType.NotNull_Basic, _) => false,
                (TestSingleEnumerableInstanceType.NotNull_Short, _) => false,
                (TestSingleEnumerableInstanceType.NotNull_Long, _) => false,
                (TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic, _) => false,
                (TestSingleEnumerableInstanceType.Empty, _) => true,
                (TestSingleEnumerableInstanceType.HasNullItem, _) => false,
                (TestSingleEnumerableInstanceType.Null, _) => false,
                _ => throw new ArgumentOutOfRangeException()
            };

        /// <summary>
        ///     <see cref="TestSingleEnumerableInstanceType"/> の列挙子名から<see cref="TestSingleEnumerableInstanceType"/> インスタンスを取得する。
        /// </summary>
        private static TestSingleEnumerableInstanceType TestSingleEnumerableInstanceTypeFrom(string value)
            => value switch
            {
                nameof(TestSingleEnumerableInstanceType.NotNull_Basic) =>
                    TestSingleEnumerableInstanceType.NotNull_Basic,
                nameof(TestSingleEnumerableInstanceType.NotNull_Short) =>
                    TestSingleEnumerableInstanceType.NotNull_Short,
                nameof(TestSingleEnumerableInstanceType.NotNull_Long) => TestSingleEnumerableInstanceType.NotNull_Long,
                nameof(TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic) =>
                    TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic,
                nameof(TestSingleEnumerableInstanceType.Empty) => TestSingleEnumerableInstanceType.Empty,
                nameof(TestSingleEnumerableInstanceType.HasNullItem) => TestSingleEnumerableInstanceType.HasNullItem,
                nameof(TestSingleEnumerableInstanceType.Null) => TestSingleEnumerableInstanceType.Null,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };

        /// <summary>
        ///     <see cref="TestSingleEnumerableInstanceType"/> から<see cref="TestRecordsType"/> インスタンスを取得する。
        /// </summary>
        private static TestRecordsType TestRecordListTypeFrom(TestSingleEnumerableInstanceType type)
            => (type) switch
            {
                TestSingleEnumerableInstanceType.NotNull_Basic => TestRecordsType.NotNull,
                TestSingleEnumerableInstanceType.NotNull_Short => TestRecordsType.NotNull,
                TestSingleEnumerableInstanceType.NotNull_Long => TestRecordsType.NotNull,
                TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic => TestRecordsType.NotNull,
                TestSingleEnumerableInstanceType.Empty => TestRecordsType.NotNull,
                TestSingleEnumerableInstanceType.HasNullItem => TestRecordsType.HasNullItem,
                TestSingleEnumerableInstanceType.Null => TestRecordsType.NotNull,
                _ => throw new ArgumentException()
            };

        #endregion

        #region TwoDimensionalList

        /// <summary>
        ///     TwoDimensionalList{TestRecord} を作成する。イベント通知は一切行わず、
        ///     引数の検証処理も行わない。
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static TwoDimensionalList<TestRecord> MakeTwoDimensionalList(string type,
            Func<int, int, TestRecord> funcMakeItem)
            => MakeTwoDimensionalList(TestDoubleEnumerableInstanceTypeFrom(type), funcMakeItem);

        /// <summary>
        ///     TwoDimensionalList{TestRecord} を作成する。イベント通知は一切行わず、
        ///     引数の検証処理も行わない。
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static TwoDimensionalList<TestRecord> MakeTwoDimensionalList(TestDoubleEnumerableInstanceType type,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type == TestDoubleEnumerableInstanceType.Null) return null;

            // あえて行列を入れ替える場面はないはず
            var items = MakeTestRecordList(type, false, funcMakeItem);
            var result = new TwoDimensionalList<TestRecord>(items,
                target => new CommonTwoDimensionalListValidator<TestRecord>(target),
                funcMakeItem)
            {
                NotifyPropertyChangingEventType = NotifyPropertyChangeEventType.Disabled,
                NotifyPropertyChangedEventType = NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangingEventType = NotifyCollectionChangeEventType.None,
                NotifyCollectionChangedEventType = NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangingEventType = NotifyTwoDimensionalListChangeEventType.None,
                NotifyTwoDimensionalListChangedEventType = NotifyTwoDimensionalListChangeEventType.None
            };

            return result;
        }

        /// <summary>
        ///     TwoDimensionalList{TestRecord} を作成する。イベント通知は一切行わず、
        ///     引数の検証処理も行わない。
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="validator">検証処理実施インスタンス</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static TwoDimensionalList<TestRecord> MakeTwoDimensionalList(TestDoubleEnumerableInstanceType type,
            ITwoDimensionalListValidator<TestRecord> validator, Func<int, int, TestRecord> funcMakeItem)
        {
            // あえて行列を入れ替える場面はないはず
            var items = MakeTestRecordList(type, false, funcMakeItem);
            var result = new TwoDimensionalList<TestRecord>(items, _ => validator, funcMakeItem)
            {
                NotifyPropertyChangingEventType = NotifyPropertyChangeEventType.Disabled,
                NotifyPropertyChangedEventType = NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangingEventType = NotifyCollectionChangeEventType.None,
                NotifyCollectionChangedEventType = NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangingEventType = NotifyTwoDimensionalListChangeEventType.None,
                NotifyTwoDimensionalListChangedEventType = NotifyTwoDimensionalListChangeEventType.None
            };

            return result;
        }

        #endregion

        #region TestTwoDimEnumerable

        /// <summary>
        ///     テスト用の二次元列挙作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="isTranspose">行列入れ替えフラグ</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static IEnumerable<IEnumerable<TestRecord>> MakeTestRecordList(
            string type, bool isTranspose,
            Func<int, int, TestRecord> funcMakeItem)
            => MakeTestRecordList(TestDoubleEnumerableInstanceTypeFrom(type), isTranspose, funcMakeItem);

        /// <summary>
        ///     テスト用の二次元列挙作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="isTranspose">行列入れ替えフラグ</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static IEnumerable<IEnumerable<TestRecord>> MakeTestRecordList(
            TestDoubleEnumerableInstanceType type,
            bool isTranspose,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type == TestDoubleEnumerableInstanceType.Null) return null;

            var recordType = TestRecordListTypeFrom(type, isTranspose);
            var initRowLength = InitRowLengthFrom(type, isTranspose);
            var initColumnLength = InitColumnLengthFrom(type, isTranspose);

            var result = MakeTestRecordList(recordType, initRowLength, initColumnLength, funcMakeItem);
            return ProcessingTestRecordList(result, type, isTranspose, funcMakeItem);
        }

        /// <summary>
        ///     テスト用の二次元配列作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="isTranspose">行列入れ替えフラグ</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static TestRecord[][] MakeTestRecordArrays(string type, bool isTranspose,
            Func<int, int, TestRecord> funcMakeItem)
            => MakeTestRecordArrays(TestDoubleEnumerableInstanceTypeFrom(type), isTranspose, funcMakeItem);

        /// <summary>
        ///     テスト用の二次元配列作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="isTranspose">行列入れ替えフラグ</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static TestRecord[][] MakeTestRecordArrays(
            TestDoubleEnumerableInstanceType type, bool isTranspose,
            Func<int, int, TestRecord> funcMakeItem)
            => MakeTestRecordList(type, isTranspose, funcMakeItem)?.ToTwoDimensionalArray();

        /// <summary>
        ///     必要に応じて <paramref name="target"/> を加工する。
        /// </summary>
        private static IEnumerable<IEnumerable<TestRecord>> ProcessingTestRecordList(
            IEnumerable<IEnumerable<TestRecord>> target,
            TestDoubleEnumerableInstanceType type, bool isTranspose,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type != TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic) return target;

            {
                // type == TestDoubleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic
                // 要素の一つをデフォルトとは異なる値に置き換える
                var arr = target.ToTwoDimensionalArray();
                arr[0][0] = new TestRecord(funcMakeItem(0, 0).Value + "_Processed");
                return arr;
            }
        }

        /// <summary>
        ///     テスト用の二次元列挙種別
        /// </summary>
        private enum TestRecordListType
        {
            /// <summary>
            ///     <see langword="null"/> 要素のない四角行列
            /// </summary>
            NotNull,

            /// <summary>
            ///     <see langword="null"/> 行のある四角行列（行数2～）
            /// </summary>
            HasNullRow,

            /// <summary>
            ///     <see langword="null"/> 列のある四角行列（列数2～）
            /// </summary>
            HasNullColumn,

            /// <summary>
            ///     <see langword="null"/> ジャグ配列（行列数ともに2～）
            /// </summary>
            Jagged,

            /// <summary>
            ///     <see langword="null"/> （行列数無視）
            /// </summary>
            Null
        }

        /// <summary>
        ///     テスト用の二次元列挙作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="rowLength">行数</param>
        /// <param name="columnLength">列数</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static IEnumerable<IEnumerable<TestRecord>> MakeTestRecordList(
            TestRecordListType type,
            int rowLength, int columnLength,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type == TestRecordListType.Null) return null;

            var (hasNullRow, hasNullColumn) = type switch
            {
                TestRecordListType.NotNull => (false, false),
                TestRecordListType.HasNullRow => (true, false),
                TestRecordListType.HasNullColumn => (false, true),
                TestRecordListType.Jagged => (false, false),
                _ => throw new Exception(),
            };

            return Enumerable.Range(0, rowLength).Select(rowIdx =>
            {
                if (hasNullRow && rowIdx % 2 == 1) return null;

                var colLength = type == TestRecordListType.Jagged && rowIdx % 2 == 1
                    ? columnLength + 1
                    : columnLength;

                return Enumerable.Range(0, colLength).Select(colIdx =>
                {
                    if (hasNullColumn && colIdx % 2 == 1) return null;
                    return funcMakeItem(rowIdx, colIdx);
                });
            });
        }

        public static bool IsAllItemReferenceEquals(IEnumerable<IEnumerable<TestRecord>> left,
            IEnumerable<IEnumerable<TestRecord>> right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null ^ right is null) return false;

            var leftArrays = left.ToTwoDimensionalArray();
            var rightArrays = right.ToTwoDimensionalArray();

            if (leftArrays.Length != rightArrays.Length) return false;
            if (leftArrays.Length == 0) return true;
            if (leftArrays[0].Length != rightArrays[0].Length) return false;

            return LinqExtension.Zip(leftArrays, rightArrays)
                .All(zip => LinqExtension.Zip(zip.Item1, zip.Item2)
                    .All(innerZip => ReferenceEquals(innerZip.Item1, innerZip.Item2)));
        }

        public static bool IsAllItemReferenceEquals(IEnumerable<TestRecord> left,
            IEnumerable<TestRecord> right)
            => IsAllItemReferenceEquals(new[] {left}, new[] {right});

        public static bool IsAllItemReferenceEquals(TestRecord[,] left,
            TestRecord[,] right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null ^ right is null) return false;

            if (left.Length != right.Length) return false;
            if (left.Length == 0) return true;

            return LinqExtension.Zip(left.Cast<TestRecord>(), right.Cast<TestRecord>())
                .All(zip => ReferenceEquals(zip.Item1, zip.Item2));
        }

        public static bool IsAllItemEquals(IEnumerable<IEnumerable<TestRecord>> left,
            IEnumerable<IEnumerable<TestRecord>> right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null ^ right is null) return false;

            var leftArrays = left.ToTwoDimensionalArray();
            var rightArrays = right.ToTwoDimensionalArray();

            if (leftArrays.Length != rightArrays.Length) return false;
            if (leftArrays.Length == 0) return true;
            if (leftArrays[0].Length != rightArrays[0].Length) return false;

            return LinqExtension.Zip(leftArrays, rightArrays)
                .All(zip => LinqExtension.Zip(zip.Item1, zip.Item2)
                    .All(innerZip => innerZip.Item1.Equals(innerZip.Item2)));
        }

        #endregion

        #region TestEnumerable

        /// <summary>
        ///     テスト用の列挙作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="directionIsRow">行方向フラグ（要素数を行基準にするか列基準にするかを決める）</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static IEnumerable<TestRecord> MakeTestRecords(
            string type, bool directionIsRow,
            Func<int, int, TestRecord> funcMakeItem)
            => MakeTestRecords(TestSingleEnumerableInstanceTypeFrom(type), directionIsRow, funcMakeItem);

        /// <summary>
        ///     テスト用の列挙作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="directionIsRow">行方向フラグ（要素数を行基準にするか列基準にするかを決める）</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static IEnumerable<TestRecord> MakeTestRecords(
            TestSingleEnumerableInstanceType type,
            bool directionIsRow,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type == TestSingleEnumerableInstanceType.Null) return null;

            var recordType = TestRecordListTypeFrom(type);
            var initLength = InitLengthFrom(type, directionIsRow);

            var result = MakeTestRecords(recordType, initLength, funcMakeItem);
            return ProcessingTestRecords(result, type, directionIsRow, funcMakeItem);
        }

        /// <summary>
        ///     テスト用の列挙作成
        /// </summary>
        /// <param name="type">種別</param>
        /// <param name="directionIsRow">行方向フラグ（要素数を行基準にするか列基準にするかを決める）</param>
        /// <param name="funcMakeItem">要素生成関数</param>
        public static TestRecord[] MakeTestRecordArray(
            TestSingleEnumerableInstanceType type,
            bool directionIsRow,
            Func<int, int, TestRecord> funcMakeItem)
            => MakeTestRecords(type, directionIsRow, funcMakeItem)?.ToArray();

        /// <summary>
        ///     必要に応じて <paramref name="target"/> を加工する。
        /// </summary>
        private static IEnumerable<TestRecord> ProcessingTestRecords(
            IEnumerable<TestRecord> target,
            TestSingleEnumerableInstanceType type, bool directionIsRow,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type != TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic) return target;

            {
                // type == TestSingleEnumerableInstanceType.NotNull_HasDifferenceItemComparedBasic
                // 要素の一つをデフォルトとは異なる値に置き換える
                var arr = target.ToArray();
                arr[0] = new TestRecord(funcMakeItem(0, 0).Value + "_Processed");
                return arr;
            }
        }

        private enum TestRecordsType
        {
            /// <summary>
            ///     <see langword="null"/> 要素のない列挙
            /// </summary>
            NotNull,

            /// <summary>
            ///     <see langword="null"/> 行のある列挙（要素数2～）
            /// </summary>
            HasNullItem,

            /// <summary>
            ///     <see langword="null"/> （行列数無視）
            /// </summary>
            Null
        }

        private static IEnumerable<TestRecord> MakeTestRecords(TestRecordsType type,
            int length,
            Func<int, int, TestRecord> funcMakeItem)
        {
            if (type == TestRecordsType.Null) return null;

            var hasNullItem = type == TestRecordsType.HasNullItem;

            return Enumerable.Range(0, length)
                .Select(idx =>
                {
                    if (hasNullItem && idx % 2 == 1) return null;
                    return funcMakeItem(0, idx);
                });
        }

        #endregion

        #region TestArray

        public static TestRecord[,] MakeDoubleRegularArray(bool isNull)
        {
            if (isNull) return null;

            const int arrayRowLength = InitRowLength + TwoDimArrayRowBuffer;
            const int arrayColumnLength = InitColumnLength + TwoDimArrayColumnBuffer;

            return new TestRecord[arrayRowLength, arrayColumnLength];
        }

        public static TestRecord[][] MakeDoubleJaggedArray(bool isNull)
        {
            if (isNull) return null;

            const int arrayRowLength = InitRowLength + TwoDimArrayRowBuffer;
            const int arrayColumnLength = InitColumnLength + TwoDimArrayColumnBuffer;

            return Enumerable.Range(0, arrayRowLength)
                .Select(_ => new TestRecord[arrayColumnLength])
                .ToArray();
        }

        public static TestRecord[] MakeSingleArray(bool isNull)
        {
            if (isNull) return null;

            const int arrayLength = InitRowLength * InitColumnLength + OneArrayBuffer;
            return new TestRecord[arrayLength];
        }

        #endregion

        #region ItemListComparer

        public class ItemListComparer : IEqualityComparer<IEnumerable<TestRecord>>
        {
            private IEqualityComparer<TestRecord> ItemComparer { get; }

            public ItemListComparer(IEqualityComparer<TestRecord> itemComparer)
            {
                ItemComparer = itemComparer;
            }

            public bool Equals(IEnumerable<TestRecord> x, IEnumerable<TestRecord> y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null ^ y is null) return false;

                var xArray = x.ToArray();
                var yArray = y.ToArray();
                if (xArray.Length != yArray.Length) return false;

                return LinqExtension.Zip(xArray, yArray)
                    .All(zip => ItemComparer.Equals(zip.Item1, zip.Item2));
            }

            public int GetHashCode(IEnumerable<TestRecord> obj)
            {
                var result = 0L;
                obj.ForEach(item =>
                {
                    result += item.GetHashCode();
                    while (result < int.MinValue)
                    {
                        result -= int.MinValue;
                    }

                    while (result > int.MaxValue)
                    {
                        result -= int.MaxValue;
                    }
                });

                return (int) result;
            }
        }

        #endregion

        #region Other TestPatterns

        public enum ContainsItemType
        {
            /// <summary>
            ///     対象の <see cref="TwoDimensionalList{T}"/> に含まれる要素
            /// </summary>
            Included,

            /// <summary>
            ///     対象の <see cref="TwoDimensionalList{T}"/> に含まれない要素
            /// </summary>
            NotIncluded,

            /// <summary>
            ///     <see langword="null"/> 要素
            /// </summary>
            Null,
        }

        #endregion

        public static Direction TestDirectionFrom(string value)
            => value switch
            {
                null => null,
                nameof(Direction.Row) => Direction.Row,
                nameof(Direction.Column) => Direction.Column,
                nameof(Direction.None) => Direction.None,
                _ => throw new ArgumentException()
            };

        public static Direction NotifyDirectionFrom(
            NotifyTwoDimensionalListChangeEventGroupingType groupingType, Direction execDirection)
        {
            return groupingType.Id switch
            {
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.Row) => Direction.Row,
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.Column) => Direction.Column,
                _ => execDirection,
            };
        }

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}"/> の変更通知で必ず行方向の通知を行う通知種別
        /// </summary>
        private static readonly NotifyTwoDimensionalListChangeEventType[] RowDirectionTwoDimListNotifyTypes =
        {
            NotifyTwoDimensionalListChangeEventType.Multi_Row,
            NotifyTwoDimensionalListChangeEventType.Simple_Row,
        };

        /// <summary>
        ///     <see cref="TwoDimensionalList{T}"/> の変更通知で必ず列方向の通知を行う通知種別
        /// </summary>
        private static readonly NotifyTwoDimensionalListChangeEventType[] ColumnDirectionTwoDimListNotifyTypes =
        {
            NotifyTwoDimensionalListChangeEventType.Multi_Column,
            NotifyTwoDimensionalListChangeEventType.Simple_Column,
        };

        /// <summary>
        ///     <paramref name="execDirection"/> と <paramref name="notifyType"/> が示す方向（行 or 列）が
        ///     一致するかどうかを返す。
        /// </summary>
        public static bool IsEqualExecDirectionAndNotifyDirection(Direction execDirection,
            NotifyTwoDimensionalListChangeEventType notifyType)
        {
            if (execDirection != Direction.Column
                && ColumnDirectionTwoDimListNotifyTypes.Contains(notifyType))
            {
                return false;
            }

            if (execDirection == Direction.Column
                && RowDirectionTwoDimListNotifyTypes.Contains(notifyType))
            {
                return false;
            }

            return true;
        }
    }
}
