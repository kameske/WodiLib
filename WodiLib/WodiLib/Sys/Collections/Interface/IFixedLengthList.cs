// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WodiLib.Sys
{
    /// <summary>
    /// 長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete("不適切な名前のため Ver 2.6 で削除します。 IFixedLengthList<T> を使用してください。")]
    public interface IFixedLengthCollection<T> : IFixedLengthList<T>
    {
    }

    /// <summary>
    /// 長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IFixedLengthList<T> : IModelBase<IFixedLengthList<T>>,
        IReadOnlyFixedLengthList<T>
    {
        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException">nullをセットしようとした場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        new T this[int index] { get; set; }

        /// <summary>
        /// 要素変更前通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        event NotifyCollectionChangedEventHandler CollectionChanging;

        /// <summary>
        /// リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] 更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        void SetRange(int index, IEnumerable<T> items);

        /// <summary>
        /// 指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, Count - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex が指定範囲外の場合
        /// </exception>
        void Move(int oldIndex, int newIndex);

        /// <summary>
        /// 指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, Count - 1)] 移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, Count - count)] 移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, Count - oldIndex)] 移動させる要素数</param>
        /// <exception cref="InvalidOperationException">
        ///    要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldIndex, newIndex, count が指定範囲外の場合
        /// </exception>
        void MoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        /// すべての要素を初期化する。
        /// </summary>
        void Clear();

        /// <summary>
        /// 要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     initItemsがnullの場合、
        ///     またはinitItems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">initItems の要素数が不適切な場合</exception>
        void Reset(IEnumerable<T> initItems);
    }
}
