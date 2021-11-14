// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal interface ITwoDimensionalList<TIn, TOut> :
        IReadableTwoDimensionalList<TOut>,
        IWritableTwoDimensionalList<TIn, TOut>,
        ISizeChangeableTwoDimensionalList<TIn, TOut>,
        INotifiableTwoDimensionalListChangeInternal<TOut>,
        IEqualityComparable<ITwoDimensionalList<TIn, TOut>>,
        IDeepCloneableTwoDimensionalListInternal<ITwoDimensionalList<TIn, TOut>, TIn>
        where TOut : TIn
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalListProperty.ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public new TOut this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<IEnumerable<TIn>>? other,
            IEqualityComparer<TIn>? itemComparer = null);

        /// <summary>
        ///     自身の全要素を簡易コピーした二次元配列を返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         返す二次元配列の状態は <paramref name="isTranspose"/> によって変化する。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///         自身の要素をそのまま返す。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///         自身を転置した状態の要素を返す。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="isTranspose">転置フラグ</param>
        /// <returns>自身の全要素簡易コピー配列</returns>
        public TOut[][] ToTwoDimensionalArray(bool isTranspose = false);
    }
}
