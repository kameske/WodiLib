// ========================================
// Project Name : WodiLib
// File Name    : ISimpleList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    internal interface ISimpleList<T> :
        IModelBase<ISimpleList<T>>,
        INotifyCollectionChanged,
        IList<T>
    {
        DelegateMakeListDefaultItem<T> MakeDefaultItem { get; }

        /// <summary>
        ///     GetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        IEnumerable<T> Get(int index, int count);

        /// <summary>
        ///     SetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        void Set(int index, params T[] items);

        /// <summary>
        ///     AddRange メソッドの処理本体
        /// </summary>
        /// <param name="items">挿入要素</param>
        void Add(params T[] items);

        /// <summary>
        ///     InsertRange メソッドの処理本体
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        void Insert(int index, params T[] items);

        /// <summary>
        ///     Overwrite メソッドの処理本体
        /// </summary>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="items">上書き要素</param>
        void Overwrite(int index, params T[] items);

        /// <summary>
        ///     MoveRange メソッドの処理本体
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        void Move(int oldIndex, int newIndex);

        /// <summary>
        ///     MoveRange メソッドの処理本体
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        void Move(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     RemoveRange メソッドの処理本体
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        void Remove(int index, int count);

        /// <summary>
        ///     AdjustLength メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        void Adjust(int length);

        /// <summary>
        ///     AdjustLengthIfLong メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        void AdjustIfLong(int length);

        /// <summary>
        ///     AdjustLengthIfShort メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        void AdjustIfShort(int length);

        /// <summary>
        ///     Reset メソッドの処理本体
        /// </summary>
        /// <param name="items">初期化要素</param>
        void Reset(params T[] items);
    }
}
