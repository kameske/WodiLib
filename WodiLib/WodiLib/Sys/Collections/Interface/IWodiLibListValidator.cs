// ========================================
// Project Name : WodiLib
// File Name    : IWodiLibListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自実装リスト検証処理インタフェース
    /// </summary>
    /// <remarks>
    ///     <see cref="IRestrictedCapacityList{TIn, TOut}"/>,
    ///     <see cref="IFixedLengthList{TIn, TOut}"/> で用いられる。
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public interface IWodiLibListValidator<T>
    {
        /// <summary>
        ///     コンストラクタの検証処理
        /// </summary>
        /// <param name="initItems">初期要素</param>
        void Constructor(NamedValue<IEnumerable<T>> initItems);

        /// <summary>
        ///     インデクサによる要素取得、GetRange メソッドの検証処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        void Get(NamedValue<int> index, NamedValue<int> count);

        /// <summary>
        ///     インデクサによる要素更新の検証処理
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="item">更新要素</param>
        void Set(NamedValue<int> index, NamedValue<T> item);

        /// <summary>
        ///     SetRange メソッドの検証処理
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        void Set(NamedValue<int> index, NamedValue<IEnumerable<T>> items);

        /// <summary>
        ///     Add, Insert メソッドの検証処理
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="item">挿入要素</param>
        void Insert(NamedValue<int> index, NamedValue<T> item);

        /// <summary>
        ///     AddRange, InsertRange メソッドの検証処理
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items);

        /// <summary>
        ///     Overwrite メソッドの検証処理
        /// </summary>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="items">上書き要素</param>
        void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items);

        /// <summary>
        ///     Move, MoveRange メソッドの検証処理
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count);

        /// <summary>
        ///     Remove メソッドの検証処理
        /// </summary>
        /// <param name="item">除去対象</param>
        void Remove(NamedValue<T?> item);

        /// <summary>
        ///     Remove, RemoveRange メソッドの検証処理
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        void Remove(NamedValue<int> index, NamedValue<int> count);

        /// <summary>
        ///     AdjustLength メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustLength(NamedValue<int> length);

        /// <summary>
        ///     AdjustLengthIfShort メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustLengthIfShort(NamedValue<int> length);

        /// <summary>
        ///     AdjustLengthIfLong メソッドの検証処理
        /// </summary>
        /// <param name="length">調整要素数</param>
        void AdjustLengthIfLong(NamedValue<int> length);

        /// <summary>
        ///     Reset, Clear メソッドの検証処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        void Reset(NamedValue<IEnumerable<T>> items);
    }
}
