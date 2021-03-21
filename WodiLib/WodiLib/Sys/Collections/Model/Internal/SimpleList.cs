// ========================================
// Project Name : WodiLib
// File Name    : SimpleList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 内部で使用する基本リストクラス。
    ///     基本的なメソッドを定義しただけのクラス。イベント通知などは一切行わない。
    /// </summary>
    internal class SimpleList<T> : ModelBase<SimpleList<T>>, IEnumerable<T>
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private List<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public SimpleList(IEnumerable<T>? initValues = null)
        {
            Items = initValues is null
                ? new List<T>()
                : new List<T>(initValues);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // ______________________ Get ______________________

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

        // ______________________ -Of ______________________

        /// <inheritdoc cref="List{T}.IndexOf(T)"/>
        public int IndexOf([AllowNull] T item)
            => item is null ? -1 : Items.IndexOf(item);

        // ______________________ -To ______________________

        /// <inheritdoc cref="List{T}.CopyTo(T[], int)"/>
        public void CopyTo(T[] array, int arrayIndex)
            => Items.CopyTo(array, arrayIndex);

        // ______________________ Set ______________________

        /// <summary>
        ///     SetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        public void Set(int index, params T[] items)
            => items.ForEach((item, i) => Items[index + i] = item);

        // ______________________ Add ______________________

        /// <summary>
        ///     Add, AddRange メソッドの処理本体
        /// </summary>
        /// <param name="items">挿入要素</param>
        public void Add(params T[] items)
            => Items.AddRange(items);

        // ______________________ Insert ______________________

        /// <summary>
        ///     Insert, InsertRange メソッドの処理本体
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        public void Insert(int index, params T[] items)
            => Items.InsertRange(index, items);

        // ______________________ Move ______________________

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

        // ______________________ Remove ______________________

        /// <summary>
        ///     Remove, Remove, RemoveRange メソッドの処理本体
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        public void Remove(int index, int count)
            => Items.RemoveRange(index, count);

        // ______________________ Reset ______________________

        /// <summary>
        /// Reset, Clear メソッドの処理本体
        /// </summary>
        /// <param name="items">初期化要素</param>
        public void Reset(params T[] items)
        {
            Items.Clear();
            Items.AddRange(items);
        }

        // ______________________ ItemEquals ______________________

        /// <inheritdoc/>
        public override bool ItemEquals(SimpleList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        // ______________________ DeepClone ______________________

        /// <inheritdoc/>
        public override SimpleList<T> DeepClone() => new(this);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
