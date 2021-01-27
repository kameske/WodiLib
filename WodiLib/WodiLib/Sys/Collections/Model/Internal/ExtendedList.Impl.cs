// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.Impl.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    internal partial class ExtendedList<T>
    {
        /// <summary>
        /// <see cref="ExtendedList{T}"/> 処理転送先クラス
        /// </summary>
        private class Impl : ModelBase<Impl>,
            IEnumerable<T>
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <inheritdoc cref="IList{T}.Count"/>
            public int Count => Items.Count;

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            private List<T> Items { get; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public Impl(IEnumerable<T>? initValues = null)
            {
                Items = initValues is null
                    ? new List<T>()
                    : new List<T>(initValues);
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <inheritdoc />
            public IEnumerator<T> GetEnumerator()
                => Items.GetEnumerator();

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            /// <summary>
            /// インデクサによる要素取得、GetRange メソッドの処理本体
            /// </summary>
            /// <param name="index">インデックス</param>
            /// <param name="count">要素数</param>
            public IEnumerable<T> Get(int index, int count)
                => Items.GetRange(index, count);

            /// <inheritdoc cref="List{T}.IndexOf(T)"/>
            public int IndexOf(T item)
                => Items.IndexOf(item);

            /// <summary>
            /// インデクサによる要素更新、SetRange メソッドの処理本体
            /// </summary>
            /// <param name="index">更新開始インデックス</param>
            /// <param name="items">更新要素</param>
            public void Set(int index, params T[] items)
                => items.ForEach((item, i) => Items[index + i] = item);

            /// <summary>
            /// Add, AddRange, Insert, InsertRange メソッドの処理本体
            /// </summary>
            /// <param name="index">挿入先インデックス</param>
            /// <param name="items">挿入要素</param>
            public void Insert(int index, params T[] items)
                => Items.InsertRange(index, items);

            /// <summary>
            /// Move, MoveRange メソッドの処理本体
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
            /// Remove, Remove, RemoveRange メソッドの処理本体
            /// </summary>
            /// <param name="index">除去開始インデックス</param>
            /// <param name="count">除去する要素数</param>
            public void Remove(int index, int count)
                => Items.RemoveRange(index, count);

            /// <inheritdoc cref="List{T}.CopyTo(T[], int)" />
            public void CopyTo(T[] array, int arrayIndex)
                => Items.CopyTo(array, arrayIndex);

            /// <inheritdoc />
            public override bool ItemEquals(Impl? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                return this.SequenceEqual(other);
            }
        }
    }
}
