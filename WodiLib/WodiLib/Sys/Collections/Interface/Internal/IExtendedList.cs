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
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <typeparamref name="T"/> が変更通知を行うクラスだった場合、
    ///     通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    internal interface IExtendedList<T> :
        IEnumerable<T>,
        IModelBase<IExtendedList<T>>,
        INotifyCollectionChanged
    {
        #region Properties

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public T this[int index] { get; set; }

        /// <summary>要素数</summary>
        public int Count { get; }

        /// <summary>要素初期化関数</summary>
        public Func<int, T> MakeDefaultItem { get; }

        /// <summary>検証処理実装</summary>
        public IWodiLibListValidator<T>? Validator { get; }

        #endregion

        /// <inheritdoc cref="IEqualityComparable{T}.ItemEquals(T?)"/>
        public bool ItemEquals(IEnumerable<T>? other);
        
        #region CRUD core

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.GetRange{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.GetRange{T}" path="param"/>
        public IEnumerable<T> GetRangeCore(int index, int count);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.SetRange{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.SetRange{T}" path="param"/>
        public void SetRangeCore(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.InsertRange{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.InsertRange{T}" path="param"/>
        public void InsertRangeCore(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Overwrite{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Overwrite{T}" path="param"/>
        public void OverwriteCore(int index, IEnumerable<T> items);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.MoveRange{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.MoveRange{T}" path="param"/>
        public void MoveRangeCore(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.RemoveRange{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.RemoveRange{T}" path="param"/>
        public void RemoveRangeCore(int index, int count);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.AdjustLength{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLength{T}" path="param"/>
        public void AdjustLengthCore(int length);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Reset{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Reset{T}" path="param"/>
        public void ResetCore(IEnumerable<T> items);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Clear{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Clear{T}" path="param"/>
        public void ClearCore();

        #endregion
    }

    internal static class ExtendedListInterfaceExtension
    {
        #region CRUD

        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="IExtendedList{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IExtendedList{T}.Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        public static IEnumerable<T> GetRange<T>(this IExtendedList<T> list, int index, int count)
        {
            list.ValidateGetRange(index, count);
            return list.GetRangeCore(index, count);
        }

        /// <summary>
        ///     リストの連続した要素を更新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="IExtendedList{T}.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public static void SetRange<T>(this IExtendedList<T> list, int index, IEnumerable<T> items)
        {
            list.ValidateSetRange(index, items);
            list.SetRangeCore(index, items);
        }

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void Add<T>(this IExtendedList<T> list, T item)
        {
            list.ValidateAdd(item);
            list.AddCore(item);
        }

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public static void AddRange<T>(this IExtendedList<T> list, IEnumerable<T> items)
        {
            list.ValidateAddRange(items);
            list.AddRangeCore(items);
        }

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void Insert<T>(this IExtendedList<T> list, int index, T item)
        {
            list.ValidateInsert(index, item);
            list.InsertCore(index, item);
        }

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public static void InsertRange<T>(this IExtendedList<T> list, int index, IEnumerable<T> items)
        {
            list.ValidateInsertRange(index, items);
            list.InsertRangeCore(index, items);
        }

        /// <summary>
        ///     指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/>)] インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
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
        public static void Overwrite<T>(this IExtendedList<T> list, int index, IEnumerable<T> items)
        {
            list.ValidateOverwrite(index, items);
            list.OverwriteCore(index, items);
        }

        /// <summary>
        ///     指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="oldIndex">[Range(0, <see cref="IExtendedList{T}.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="IExtendedList{T}.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。
        /// </exception>
        public static void Move<T>(this IExtendedList<T> list, int oldIndex, int newIndex)
        {
            list.ValidateMove(oldIndex, newIndex);
            list.MoveCore(oldIndex, newIndex);
        }

        /// <summary>
        ///     指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="oldIndex">
        ///     [Range(0, <see cref="IExtendedList{T}.Count"/> - 1)]
        ///     移動する項目のインデックス開始位置
        /// </param>
        /// <param name="newIndex">
        ///     [Range(0, <see cref="IExtendedList{T}.Count"/> - 1)]
        ///     移動先のインデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IExtendedList{T}.Count"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public static void MoveRange<T>(this IExtendedList<T> list, int oldIndex, int newIndex, int count)
        {
            list.ValidateMoveRange(oldIndex, newIndex, count);
            list.MoveRangeCore(oldIndex, newIndex, count);
        }

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        public static void Remove<T>(this IExtendedList<T> list, int index)
        {
            list.ValidateRemove(index);
            list.RemoveCore(index);
        }

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index">[Range(0, <see cref="ICollection{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ICollection{T}.Count"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        public static void RemoveRange<T>(this IExtendedList<T> list, int index, int count)
        {
            list.ValidateRemoveRange(index, count);
            list.RemoveRangeCore(index, count);
        }

        /// <summary>
        ///     要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="length">
        ///     [Range(0, -)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustLength<T>(this IExtendedList<T> list, int length)
        {
            list.ValidateAdjustLength(length);
            list.AdjustLengthCore(length);
        }

        /// <summary>
        ///     要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="length">
        ///     [Range(0, -)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustLengthIfShort<T>(this IExtendedList<T> list, int length)
        {
            list.ValidateAdjustLength(length);
            list.AdjustLengthIfShortCore(length);
        }

        /// <summary>
        ///     要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="length">
        ///     [Range(0, -)]
        ///     調整する要素数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustLengthIfLong<T>(this IExtendedList<T> list, int length)
        {
            list.ValidateAdjustLength(length);
            list.AdjustLengthIfLongCore(length);
        }

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="items">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public static void Reset<T>(this IExtendedList<T> list, IEnumerable<T> items)
        {
            list.ValidateReset(items);
            list.ResetCore(items);
        }

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public static void Clear<T>(this IExtendedList<T> list)
        {
            list.ValidateClear();
            list.ClearCore();
        }

        #endregion

        #region Validate

        /// <summary>
        ///     インデクサによる取得の検証処理。
        /// </summary>
        /// <inheritdoc cref="IExtendedList{T}.this[int]" path="param|exception[cref='ArgumentOutOfRangeException']"/>
        public static void ValidateGet<T>(this IExtendedList<T> list, int index)
            => list.ValidateGetRange(index, 1);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.GetRange{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.GetRange{T}" path="param|exception"/>
        public static void ValidateGetRange<T>(this IExtendedList<T> list, int index, int count)
        {
            list.Validator?.Get((nameof(index), index), (nameof(count), count));
        }

        /// <summary>
        ///     インデクサによる更新の検証処理。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index"><inheritdoc cref="IExtendedList{T}.this[int]" path="param[name='index']"/></param>
        /// <param name="item">編集要素</param>
        /// <inheritdoc cref="IExtendedList{T}.this[int]" path="exception"/>
        public static void ValidateSet<T>(this IExtendedList<T> list, int index, T item)
            => list.ValidateSetRange(index, new[] { item });

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.SetRange{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.SetRange{T}" path="param|exception"/>
        public static void ValidateSetRange<T>(this IExtendedList<T> list, int index, IEnumerable<T> items)
        {
            list.Validator?.Set((nameof(index), index), (nameof(items), items));
        }

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Add{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Add{T}" path="param|exception"/>
        public static void ValidateAdd<T>(this IExtendedList<T> list, T item)
            => list.ValidateAddRange(new[] { item });

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.AddRange{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AddRange{T}" path="param|exception"/>
        public static void ValidateAddRange<T>(this IExtendedList<T> list, IEnumerable<T> items)
            => list.ValidateInsertRange(list.Count, items);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Insert{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Insert{T}" path="param|exception"/>
        public static void ValidateInsert<T>(this IExtendedList<T> list, int index, T item)
            => list.ValidateInsertRange(index, new[] { item });

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.InsertRange{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.InsertRange{T}" path="param|exception"/>
        public static void ValidateInsertRange<T>(this IExtendedList<T> list, int index, IEnumerable<T> items)
        {
            list.Validator?.Insert((nameof(index), index), (nameof(items), items));
        }

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Overwrite{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Overwrite{T}" path="param|exception"/>
        public static void ValidateOverwrite<T>(this IExtendedList<T> list, int index, IEnumerable<T> items)
        {
            list.Validator?.Overwrite((nameof(index), index), (nameof(items), items));
        }

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Move{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Move{T}" path="param|exception"/>
        public static void ValidateMove<T>(this IExtendedList<T> list, int oldIndex, int newIndex)
            => list.ValidateMoveRange(oldIndex, newIndex, 1);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.MoveRange{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.MoveRange{T}" path="param|exception"/>
        public static void ValidateMoveRange<T>(this IExtendedList<T> list, int oldIndex, int newIndex, int count)
        {
            list.Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex), (nameof(count), count));
        }

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Remove{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Remove{T}" path="param|exception"/>
        public static void ValidateRemove<T>(this IExtendedList<T> list, int index)
            => list.ValidateRemoveRange(index, 1);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.RemoveRange{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.RemoveRange{T}" path="param|exception"/>
        public static void ValidateRemoveRange<T>(this IExtendedList<T> list, int index, int count)
            => list.Validator?.Remove((nameof(index), index), (nameof(count), count));

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.AdjustLength{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLength{T}" path="param|exception"/>
        public static void ValidateAdjustLength<T>(this IExtendedList<T> list, int length)
            => list.Validator?.AdjustLength((nameof(length), length));

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Reset{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Reset{T}" path="param|exception"/>
        public static void ValidateReset<T>(this IExtendedList<T> list, IEnumerable<T> items)
            => list.Validator?.Reset((nameof(items), items));

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Clear{T}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Clear{T}" path="param|exception"/>
        public static void ValidateClear<T>(this IExtendedList<T> list)
            => list.Validator?.Clear();

        #endregion

        #region CRUD core

        /// <summary>
        ///     インデクサによる取得処理中核。
        /// </summary>
        /// <inheritdoc cref="IExtendedList{T}.this[int]" path="param"/>
        public static T GetCore<T>(this IExtendedList<T> list, int index)
            => list.GetRangeCore(index, 1).First();

        /// <summary>
        ///     インデクサによる更新処理中核。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="index"><inheritdoc cref="IExtendedList{T}.this[int]" path="param[name='index']"/></param>
        /// <param name="item">編集要素</param>
        /// <inheritdoc cref="IExtendedList{T}.this[int]" path="exception"/>
        public static void SetCore<T>(this IExtendedList<T> list, int index, T item)
            => list.SetRangeCore(index, new[] { item });

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Add{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Add{T}" path="param"/>
        public static void AddCore<T>(this IExtendedList<T> list, T item)
            => list.AddRangeCore(new[] { item });

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.AddRange{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AddRange{T}" path="param"/>
        public static void AddRangeCore<T>(this IExtendedList<T> list, IEnumerable<T> items)
            => list.InsertRangeCore(list.Count, items);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Insert{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Insert{T}" path="param"/>
        public static void InsertCore<T>(this IExtendedList<T> list, int index, T item)
            => list.InsertRangeCore(index, new[] { item });

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Move{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Move{T}" path="param"/>
        public static void MoveCore<T>(this IExtendedList<T> list, int oldIndex, int newIndex)
            => list.MoveRangeCore(oldIndex, newIndex, 1);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.Remove{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.Remove{T}" path="param"/>
        public static void RemoveCore<T>(this IExtendedList<T> list, int index)
            => list.RemoveRangeCore(index, 1);

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.AdjustLengthIfShort{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLengthIfShort{T}" path="param"/>
        public static void AdjustLengthIfShortCore<T>(this IExtendedList<T> list, int length)
            => list.AdjustLengthCore(Math.Min(list.Count, length));

        /// <summary>
        ///     <see cref="ExtendedListInterfaceExtension.AdjustLengthIfLong{T}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ExtendedListInterfaceExtension.AdjustLengthIfLong{T}" path="param"/>
        public static void AdjustLengthIfLongCore<T>(this IExtendedList<T> list, int length)
            => list.AdjustLengthCore(Math.Max(list.Count, length));

        #endregion
    }
}
