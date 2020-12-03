// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// 【読み取り専用】二次元リスト
    /// </summary>
    /// <remarks>
    /// 外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    /// すべての行について列数は常に同じ値を取り続ける。<br/>
    /// 行数、列数のうち片方だけが 0 となる状態は存在しない。<br/>
    /// <br/>
    /// リスト操作用各種メソッドについて、「行に対して操作するメソッド」と「列に対して操作するメソッド」があることに注意。<br/>
    /// これらの操作にて入力/出力する二重シーケンスは、どちらのメソッドを使用するかによって内容のフォーマットが変わる。
    /// 具体的には“行に対する操作の場合は外側のシーケンス＝行データ、内側のシーケンス＝列データ”
    /// ”列に対する操作の場合は外側のシーケンス＝列データ、内側のシーケンス＝行データ”となる。<br/>
    /// コードサンプルは <see cref="GetRowRange"/>, <see cref="GetColumnRange"/> を参照。<br/>
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyTwoDimensionalList<T> : IModelBase<IReadOnlyTwoDimensionalList<T>>,
        IEquatable<IEnumerable<IEnumerable<T>>>, IEnumerable<IEnumerable<T>>,
        ISerializable
    {
        /// <summary>
        /// 要素変更後通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>> TwoDimensionListChanged;

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 行番号</param>
        /// <param name="column">[Range(0, ColumnCount - 1)] 列番号</param>
        /// <exception cref="ArgumentOutOfRangeException">row, column が指定範囲外の場合</exception>
        T this[int row, int column] { get; }

        /// <summary>
        /// 行数
        /// </summary>
        int RowCount { get; }

        /// <summary>
        /// 列数
        /// </summary>
        int ColumnCount { get; }

        /// <summary>
        /// 二次元リストが空かどうか<br/>
        /// 行数・列数が 0 の場合 true, それ以外の場合false。
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 指定した行の要素を順次取得する。
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 行番号</param>
        /// <returns>指定行の要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">row が指定範囲外の場合</exception>
        IEnumerable<T> GetRow(int row);

        /// <summary>
        /// 指定した列の要素を順次取得する。
        /// </summary>
        /// <param name="column">[Range(0, ColumnCount - 1)] 列番号</param>
        /// <returns>指定列の要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">column が指定範囲外の場合</exception>
        IEnumerable<T> GetColumn(int column);

        /// <summary>
        /// 指定範囲の行データを簡易コピーした二次元リストを取得する。
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 開始行番号</param>
        /// <param name="rowCount">[Range(0, RowCount)] 行数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     row, rowCount, が指定範囲外の場合、
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        /// <example>
        /// [ [ 0,  1,  2],<br/>
        ///   [10, 11, 12],<br/>
        ///   [20, 21, 22] ]<br/>
        /// のデータに対して<br/>
        ///     GetRowRange(1, 2)<br/>
        /// を実行した場合の実行結果は<br/>
        /// [ [10, 11, 12],<br/>
        ///   [20, 21, 22] ]<br/>
        /// となる。<br/>
        /// </example>
        IEnumerable<IEnumerable<T>> GetRowRange(int row, int rowCount);

        /// <summary>
        /// 指定範囲の列データを簡易コピーした二次元リストを取得する。
        /// </summary>
        /// <remarks>
        /// 取得結果は行と列が入れ替わっていることに留意。GetColumn で取得したデータがシーケンシャルに返却される。
        /// </remarks>
        /// <param name="column">[Range(0, ColumnCount - 1)] 開始列番号</param>
        /// <param name="columnCount">[Range(0, ColumnCount)] 列数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     column, columnCount, が指定範囲外の場合、
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        /// <example>
        /// [ [ 0,  1,  2],<br/>
        ///   [10, 11, 12],<br/>
        ///   [20, 21, 22] ]<br/>
        /// のデータに対して<br/>
        ///     GetColumnRange(1, 2)<br/>
        /// を実行した場合の実行結果は<br/>
        /// [ [ 1, 11, 21],<br/>
        ///   [ 2, 21, 22] ]<br/>
        /// となる。<br/>
        /// </example>
        IEnumerable<IEnumerable<T>> GetColumnRange(int column, int columnCount);

        /// <summary>
        /// 指定範囲の要素を簡易コピーした二次元リストを取得する。
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 開始行番号</param>
        /// <param name="rowCount">[Range(0, RowCount)] 行数</param>
        /// <param name="column">[Range(0, ColumnCount - 1)] 開始列番号</param>
        /// <param name="columnCount">[Range(0, ColumnCount)] 列数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     row, rowCount, column, columnCount が指定範囲外の場合、
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        IEnumerable<IEnumerable<T>> GetRange(
            int row, int rowCount,
            int column, int columnCount);
    }
}
