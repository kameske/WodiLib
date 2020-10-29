// ========================================
// Project Name : WodiLib
// File Name    : IExtendedList.cs
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
    /// WodiLib 独自リストインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IExtendedList<T> : IList<T>, IReadOnlyExtendedList<T>
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
        /// 要素数
        /// </summary>
        new int Count { get; }

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
        /// リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitemsにnull要素が含まれる場合
        /// </exception>
        void InsertRange(int index, IEnumerable<T> items);

        /// <summary>
        /// 指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, Count)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="ArgumentException">items中にnull要素が含まれる場合</exception>
        /// <exception cref="InvalidOperationException">追加操作によって要素数がMaxCapacityを超える場合</exception>
        /// <example>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10, 11, 12 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 11, 12 }
        ///     </code>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 3 }
        ///     </code>
        /// </example>
        void Overwrite(int index, IEnumerable<T> items);

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
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合</exception>
        void MoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        /// 要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果要素数がMinCapacity未満になる場合</exception>
        void RemoveRange(int index, int count);

        /// <summary>
        /// 要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustLength(int length);

        /// <summary>
        /// 要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustLengthIfShort(int length);

        /// <summary>
        /// 要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する要素数</param>
        void AdjustLengthIfLong(int length);

        /// <summary>
        /// 要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        void Reset(IEnumerable<T> initItems);
    }
}
