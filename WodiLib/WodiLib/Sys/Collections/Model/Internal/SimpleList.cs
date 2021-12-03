// ========================================
// Project Name : WodiLib
// File Name    : SimpleList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 内部で使用する基本リストクラス。
    ///     基本的なメソッドを定義しただけのクラス。イベント通知などは一切行わない。
    /// </summary>
    internal class SimpleList<T> : ModelBase<SimpleList<T>>, IEnumerable<T>,
        IDeepCloneableList<SimpleList<T>, T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public T this[int index]
        {
            get => Get(index, 1).First();
            set => Set(index, value);
        }

        /// <inheritdoc cref="IList{T}.Count"/>
        public int Count => Items.Count;

        public Func<int, int, IEnumerable<T>> FuncMakeItems { get; set; } = default!;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private List<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal SimpleList(IEnumerable<T>? initValues = null, bool isDeepClone = false)
        {
            Items = CreateImpl(initValues, isDeepClone);
        }

        public SimpleList(IEnumerable<T>? initValues = null) : this(initValues, false)
        {
        }

        private static List<T> CreateImpl(IEnumerable<T>? initValues, bool isDeepClone)
        {
            if (initValues is null) return new List<T>();

            if (!isDeepClone) return new List<T>(initValues);

            var initValueArray = initValues.ToArray();
            if (initValueArray.Length == 0) return new List<T>();

            var isDeepCloneable = initValueArray[0] is IDeepCloneable<T>;
            if (!isDeepCloneable) return new List<T>(initValueArray);

            var deepCloneArray = initValueArray.Cast<IDeepCloneable<T>>()
                .Select(item => item.DeepClone());
            return new List<T>(deepCloneArray);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     GetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        public IEnumerable<T> Get(int index, int count)
            => Items.GetRange(index, count);

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
            => Items.GetEnumerator();

        /// <inheritdoc cref="List{T}.IndexOf(T)"/>
        public int IndexOf(T? item)
            => item is null ? -1 : Items.IndexOf(item);

        /// <summary>
        ///     SetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        public void Set(int index, params T[] items)
            => items.ForEach((item, i) => Items[index + i] = item);

        /// <summary>
        ///     Add, AddRange メソッドの処理本体
        /// </summary>
        /// <param name="items">挿入要素</param>
        public void Add(params T[] items)
            => Items.AddRange(items);

        /// <summary>
        ///     Insert, InsertRange メソッドの処理本体
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        public void Insert(int index, params T[] items)
            => Items.InsertRange(index, items);

        /// <summary>
        ///     Overwrite メソッドの処理本体
        /// </summary>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="param">上書き情報</param>
        public void Overwrite(int index, OverwriteParam<T> param)
        {
            Set(index, param.ReplaceNewItems);
            Insert(param.InsertStartIndex, param.InsertItems);
        }

        /// <summary>
        ///     Move, MoveRange メソッドの処理本体
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        public void Move(int oldIndex, int newIndex, int count)
        {
            var movedItems = Items.GetRange(oldIndex, count);
            Items.RemoveRange(oldIndex, count);
            Items.InsertRange(newIndex, movedItems);
        }

        /// <summary>
        ///     Remove, Remove, RemoveRange メソッドの処理本体
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        public void Remove(int index, int count)
            => Items.RemoveRange(index, count);

        /// <summary>
        ///     AdjustLength メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        public void Adjust(int length)
        {
            if (Count == length) return;
            if (Count > length)
            {
                AdjustIfLong(length);
            }

            // Count < length
            AdjustIfShort(length);
        }

        /// <summary>
        ///     AdjustLengthIfLong メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        public void AdjustIfLong(int length)
        {
            if (Count <= length) return;
            Remove(length, Count - length);
        }

        /// <summary>
        ///     AdjustLengthIfShort メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        public void AdjustIfShort(int length)
        {
            if (Count >= length) return;

            var addItems = FuncMakeItems(Count, length - Count);
            Add(addItems.ToArray());
        }

        /// <summary>
        ///     Reset, Clear メソッドの処理本体
        /// </summary>
        /// <param name="items">初期化要素</param>
        public void Reset(params T[] items)
        {
            Items.Clear();
            Items.AddRange(items);
        }

        /// <inheritdoc/>
        public override bool ItemEquals(SimpleList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc cref="IReadOnlyExtendedList{TIn,TOut}.ItemEquals{TOther}"/>
        public bool ItemEquals<TOther>(IEnumerable<TOther>? other, IEqualityComparer<TOther>? itemComparer = null)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherList = other.ToList();

            if (Count != otherList.Count) return false;
            if (Count == 0) return true;

            return this.Zip(otherList).All(zip =>
            {
                var (x, y) = zip;

                return ItemEquals(x, y, itemComparer);
            });
        }

        /// <inheritdoc/>
        public override SimpleList<T> DeepClone()
        {
            var result = new SimpleList<T>(this, true)
            {
                FuncMakeItems = FuncMakeItems
            };
            return result;
        }

        public SimpleList<T> DeepCloneWith(int? length)
            => DeepCloneWith<T>(null, length);

        /// <inheritdoc/>
        public SimpleList<T> DeepCloneWith<TItem>(IReadOnlyDictionary<int, TItem>? values, int? length)
            where TItem : T
        {
            var result = DeepClone();

            if (length is not null)
            {
                result.Adjust(length.Value);
            }

            values?.ForEach(pair =>
            {
                if (0 <= pair.Key && pair.Key < result.Count) result[pair.Key] = pair.Value;
            });

            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 2つの要素について同値判定を行う。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="other"></param>
        /// <param name="itemComparer"></param>
        /// <typeparam name="TOther"></typeparam>
        /// <returns></returns>
        private static bool ItemEquals<TOther>(T? item, TOther? other, IEqualityComparer<TOther>? itemComparer)
        {
            if (ReferenceEquals(item, other))
            {
                return true;
            }

            if (item is null && other is null)
            {
                return true;
            }

            if (item is null | other is null)
            {
                return false;
            }

            if (itemComparer is not null && item is TOther otherX)
            {
                return itemComparer.Equals(otherX, other!);
            }

            if (item is IEqualityComparable comparable)
            {
                return comparable.ItemEquals(other);
            }

            return Equals(item, other);
        }
    }
}
