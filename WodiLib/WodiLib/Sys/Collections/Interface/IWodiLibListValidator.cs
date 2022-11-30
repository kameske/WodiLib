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
    ///     各種検証において不正な引数の場合例外を発生させる。
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
        ///     GetRange メソッドの検証処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        void Get(NamedValue<int> index, NamedValue<int> count);

        /// <summary>
        ///     SetRange メソッドの検証処理
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        void Set(NamedValue<int> index, NamedValue<IEnumerable<T>> items);

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
        ///     MoveRange メソッドの検証処理
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count);

        /// <summary>
        ///     RemoveRange メソッドの検証処理
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
        ///     Reset メソッドの検証処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        void Reset(NamedValue<IEnumerable<T>> items);

        /// <summary>
        ///     Clear メソッドの検証処理
        /// </summary>
        void Clear();
    }

    /// <summary>
    ///     WodiLib 独自実装リスト検証処理インタフェースデフォルト実装用拡張クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class WodiLibListValidatorInterfaceExtension
    {
        /// <summary>
        ///     インデクサによる要素取得 メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="index">インデックス</param>
        public static void Get<T>(
            this IWodiLibListValidator<T> validator,
            NamedValue<int> index
        )
            => validator.Get(index, ("count", 1));

        /// <summary>
        ///     インデクサによる要素更新の検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="item">更新要素</param>
        public static void Set<T>(
            this IWodiLibListValidator<T> validator,
            NamedValue<int> index,
            NamedValue<T> item
        )
            => validator.Set(index, (item.Name, new[] { item.Value }));

        /// <summary>
        ///     Add, Insert メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="item">挿入要素</param>
        public static void Insert<T>(
            this IWodiLibListValidator<T> validator,
            NamedValue<int> index,
            NamedValue<T> item
        )
            => validator.Insert(index, (item.Name, new[] { item.Value }));

        /// <summary>
        ///     Move メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        public static void Move<T>(
            this IWodiLibListValidator<T> validator,
            NamedValue<int> oldIndex,
            NamedValue<int> newIndex
        )
            => validator.Move(oldIndex, newIndex, ("count", 1));

        /// <summary>
        ///     Remove メソッドの検証処理
        /// </summary>
        /// <param name="validator">validator</param>
        /// <param name="index">除去開始インデックス</param>
        public static void Remove<T>(
            this IWodiLibListValidator<T> validator,
            NamedValue<int> index
        )
            => validator.Remove(index, ("count", 1));
    }
}
