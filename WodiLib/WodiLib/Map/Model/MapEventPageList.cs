// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <inheritdoc />
    /// <summary>
    /// マップイベントページリストクラス
    /// </summary>
    public class MapEventPageList : IWodiLibObject
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントページ最大数</summary>
        public static readonly int EventPageMax = 10;

        /// <summary>イベントページ最小数</summary>
        public static readonly int EventPageMin = 1;

        /// <summary>ページ数</summary>
        public int Count => pageList.Count;

        private List<MapEventPage> pageList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pages">[NotNull][LengthRange(1, 10)]  1ページ毎のマップイベント</param>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pagesの長さが1～10以外の場合</exception>
        public MapEventPageList(IEnumerable<MapEventPage> pages)
        {
            if (pages == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(pages)));
            var mapEventOnePages = pages.ToList();
            if (!ValidatePageListLength(mapEventOnePages))
                throw new ArgumentException(
                    ErrorMessage.LengthRange(nameof(pages), EventPageMin, EventPageMax, mapEventOnePages.Count));
            pageList = mapEventOnePages;
        }

        /// <summary>
        /// マップイベント１ページ情報クラスを追加する。
        /// </summary>
        /// <param name="page">１ページ情報</param>
        /// <exception cref="ArgumentNullException">mapEventがnullの場合</exception>
        /// <exception cref="InvalidOperationException">ページ数が10を超える場合</exception>
        public void Add(MapEventPage page)
        {
            if (page == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(page)));
            if (!ValidatePageListLength(pageList.Count + 1))
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMax}を超えるため、これ以上追加できません。");
            pageList.Add(page);
        }

        /// <summary>
        /// マップイベント1ページ情報クラスを追加する。
        /// </summary>
        /// <param name="pages">[NotNull] 1ページ情報リスト</param>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        /// <exception cref="InvalidOperationException">ページ数が10を超える場合</exception>
        public void AddRange(IEnumerable<MapEventPage> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(pages)));

            var argPages = pages.ToList();

            if (!ValidatePageListLength(pageList.Count + argPages.Count))
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMax}を超えるため、これ以上追加できません。");
            pageList.AddRange(argPages);
        }

        /// <summary>
        /// ページを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ページ数-1)] インデックス</param>
        /// <param name="item">[NotNull] 挿入する項目</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">ページ数が10を超える場合</exception>
        public void Insert(int index, MapEventPage item)
        {
            if (index < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"ページ数-1(={pageList.Count - 1})", index));
            if (item == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));
            if (!ValidatePageListLength(pageList.Count + 1))
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMax}を超えるため、これ以上追加できません。");
            pageList.Insert(index, item);
        }

        /// <summary>
        /// ページを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, ページ数-1)] インデックス</param>
        /// <param name="collection">ページリスト</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        /// <exception cref="InvalidOperationException">ページ数が10を超える場合</exception>
        public void InsertRange(int index, IEnumerable<MapEventPage> collection)
        {
            var argPages = collection.ToList();

            if (index < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"ページ数-1(={pageList.Count - 1})", index));
            if (collection == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(collection)));
            if (!ValidatePageListLength(pageList.Count + argPages.Count))
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMax}を超えるため、これ以上追加できません。");

            pageList.InsertRange(index, argPages);
        }

        /// <summary>
        /// リストからアイテムを除去する。
        /// </summary>
        /// <param name="item">[NotNull] 削除アイテム</param>
        /// <returns>除去成否</returns>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        public bool Remove(MapEventPage item)
        {
            if (item == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(item)));

            // 削除する要素が見つからない場合は何もしない
            var delItem = pageList.SingleOrDefault(x => x == item);
            if (delItem == default(MapEventPage)) return false;

            // 削除した結果最小行数を下回る場合はエラー
            if (pageList.Count - 1 < EventPageMin)
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMin}を下回るため、これ以上削除できません。");

            var removedList = pageList.ToList();
            var result = removedList.Remove(item);
            pageList = removedList;
            return result;
        }

        /// <summary>
        /// インデックスを指定してページ情報を削除する。
        /// </summary>
        /// <param name="index">[Range(0, ページ数-1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">index が指定範囲以外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果ページ数が0以下になる</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"ページ数-1(={pageList.Count - 1})", index));
            if (pageList.Count - 1 < EventPageMin)
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMin}を下回るため、これ以上削除できません。");

            pageList.RemoveAt(index);
        }

        /// <summary>
        /// ページ情報を範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, ページ数-1)] 開始index</param>
        /// <param name="count">[Range(0, ページ数-1)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, count が指定範囲以外</exception>
        /// <exception cref="InvalidOperationException">削除した結果ページ数が0以下になる</exception>
        public void RemoveRange(int index, int count)
        {
            if (index < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"ページ数-1(={pageList.Count - 1}", index));
            if (count < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), 0, $"ページ数-1(={pageList.Count - 1}", count));
            if (count <= 0) return;

            var removedItemCount = pageList.Count - count;
            if (!ValidatePageListLength(removedItemCount))
                throw new InvalidOperationException(
                    $"ページ数が{EventPageMin}を下回るため、これ以上削除できません。");
            pageList.RemoveRange(index, count);
        }

        /// <summary>
        /// ページ情報を取得する。
        /// </summary>
        /// <param name="index">[Range(0, ページ数-1)] インデックス</param>
        /// <returns>マップイベントページ情報</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public MapEventPage Get(int index)
        {
            if (index < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"ページ数-1(={pageList.Count - 1})", index));

            return pageList[index];
        }

        /// <summary>
        /// ページ情報を範囲取得する。
        /// </summary>
        /// <param name="index">[Range(0, ページ数-1)] インデックス</param>
        /// <param name="count">[Range(0, ページ数)] 取得数</param>
        /// <returns>マップイベントページ情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, countが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">取得範囲がページ数を超える場合</exception>
        public IEnumerable<MapEventPage> GetRange(int index, int count)
        {
            if (index < 0 || pageList.Count <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"ページ数-1(={pageList.Count - 1})", index));
            if (count < 0 || pageList.Count < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), 0, $"ページ数(={pageList.Count}", count));
            if (index + count > pageList.Count)
                throw new ArgumentException(
                    "index + count <= ページ数である必要があります。" +
                    $"（index: {index}, count:{count}, ページ数：{pageList.Count}");

            return pageList.GetRange(index, count);
        }

        /// <summary>
        /// すべてのページ情報を返す。
        /// </summary>
        /// <returns>マップイベントページ情報リスト</returns>
        public IEnumerable<MapEventPage> GetAll()
        {
            return pageList;
        }

        /// <summary>
        /// マップイベントページリストの長さが適切かどうか検証する。
        /// </summary>
        /// <param name="pages">[NotNull] マップイベントページリスト</param>
        /// <returns>長さが適切な場合true</returns>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        public static bool ValidatePageListLength(IEnumerable<MapEventPage> pages)
        {
            if (pages == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(pages)));
            var mapEventOnePages = pages as MapEventPage[] ?? pages.ToArray();
            return ValidatePageListLength(mapEventOnePages.Length);
        }

        private static bool ValidatePageListLength(int length)
        {
            return (EventPageMin <= length && length <= EventPageMax);
        }

        /// <inheritdoc />
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            foreach (var page in pageList)
            {
                result.AddRange(page.ToBinary());
            }

            return result.ToArray();
        }
    }
}