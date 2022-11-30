// ========================================
// Project Name : WodiLib.Test
// File Name    : EnumerableExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IEnumerable{T}"/> 拡張クラス
    /// </summary>
    internal static class EnumerableExtension
    {
        /// <summary>
        /// 要素を編集した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="item">編集する要素</param>
        /// <param name="index">編集インデックス</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を追加した新たなリストインスタンス</returns>
        public static IList<T> Setted<T>(this IEnumerable<T> target, int index, T item)
            => target.Setted(index, new[] { item });

        /// <summary>
        /// 要素を編集した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="items">編集する要素</param>
        /// <param name="index">編集開始インデックス</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を追加した新たなリストインスタンス</returns>
        public static IList<T> Setted<T>(this IEnumerable<T> target, int index, IEnumerable<T> items)
        {
            var result = new List<T>(target);
            items.ForEach((item, offset) => result[index + offset] = item);
            return result;
        }

        /// <summary>
        /// 要素を追加した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="item">追加する要素</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を追加した新たなリストインスタンス</returns>
        public static IList<T> Added<T>(this IEnumerable<T> target, T item)
            => target.Added(new[] { item });

        /// <summary>
        /// 要素を追加した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="items">追加する要素</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を追加した新たなリストインスタンス</returns>
        public static IList<T> Added<T>(this IEnumerable<T> target, IEnumerable<T> items)
        {
            var result = new List<T>(target);
            result.AddRange(items);
            return result;
        }

        /// <summary>
        /// 要素を挿入した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="index">挿入インデックス</param>
        /// <param name="item">挿入する要素</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を挿入した新たなリストインスタンス</returns>
        public static IList<T> Inserted<T>(this IEnumerable<T> target, int index, T item)
            => target.Inserted(index, new[] { item });

        /// <summary>
        /// 要素を挿入した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="index">挿入インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を挿入した新たなリストインスタンス</returns>
        public static IList<T> Inserted<T>(this IEnumerable<T> target, int index, IEnumerable<T> items)
        {
            var result = new List<T>(target);
            result.InsertRange(index, items);
            return result;
        }

        /// <summary>
        /// 要素を上書きした新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="items">上書きする要素</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を上書きした新たなリストインスタンス</returns>
        public static IList<T> Overwritten<T>(this IEnumerable<T> target, int index, IEnumerable<T> items)
        {
            var customList = new SimpleList<T>(default!, target);
            customList.Overwrite(index, items.ToArray());
            return customList.ToList();
        }
        
        /// <summary>
        /// 要素を移動した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="oldIndex">移動前インデックス</param>
        /// <param name="newIndex">移動先インデックス</param>
        /// <param name="count">移動する要素数</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を移動した新たなリストインスタンス</returns>
        public static IList<T> Moved<T>(this IEnumerable<T> target, int oldIndex, int newIndex, int count = 1)
        {
            var result = new SimpleList<T>(default!, target);
            result.Move(oldIndex, newIndex, count);
            return result.ToList();
        }

        /// <summary>
        /// 要素を除去した新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去要素数</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素を除去した新たなリストインスタンス</returns>
        public static IList<T> Removed<T>(this IEnumerable<T> target, int index, int count = 1)
        {
            var result = new List<T>(target);
            result.RemoveRange(index, count);
            return result;
        }

        /// <summary>
        /// 要素を指定した個数に合わせた新たなリストを生成する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="length">調整要素数</param>
        /// <param name="factory">要素追加時の追加要素生成処理</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素数を調整した新たなリストインスタンス</returns>
        public static IList<T> Adjusted<T>(this IEnumerable<T> target, int length, Func<int, T> factory)
            => target.AdjustedIfShort(length, factory)
                .AdjustedIfLong(length);

        /// <summary>
        /// 要素を指定した個数に合わせた新たなリストを生成する。
        /// 元リストの要素数が規定数に足りない場合のみ要素を充足する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="length">調整要素数</param>
        /// <param name="factory">要素追加時の追加要素生成処理</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素数を調整した新たなリストインスタンス</returns>
        public static IList<T> AdjustedIfShort<T>(this IEnumerable<T> target, int length, Func<int, T> factory)
        {
            var result = new List<T>(target);
            var addLength = length - result.Count;

            if (addLength <= 0)
            {
                return result;
            }

            var startIndex = result.Count;

            var addItems = addLength.Range().Select(i => factory(startIndex + i));
            result.AddRange(addItems);

            return result;
        }

        /// <summary>
        /// 要素を指定した個数に合わせた新たなリストを生成する。
        /// 元リストの要素数が規定数を超える場合のみ要素を除去する。
        /// 元のリストには変更を加えない。
        /// </summary>
        /// <param name="target">ソースとなるリスト</param>
        /// <param name="length">調整要素数</param>
        /// <typeparam name="T">リスト要素型</typeparam>
        /// <returns>要素数を調整した新たなリストインスタンス</returns>
        public static IList<T> AdjustedIfLong<T>(this IEnumerable<T> target, int length)
        {
            var result = new List<T>(target);
            var removeLength = result.Count - length;

            if (removeLength <= 0)
            {
                return result;
            }

            var removeIndex = length;

            result.RemoveRange(removeIndex, removeLength);

            return result;
        }

        /// <summary>
        /// <see cref="Enumerable.All{TSource}"/> 要素インデックス付与バージョン.
        /// </summary>
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return !source.Where((t, i) => !predicate(t, i)).Any();
        }
    }
}
