// ========================================
// Project Name : WodiLib
// File Name    : CommonEventList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベントリスト
    /// </summary>
    internal class CommonEventList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベント数最小値
        /// </summary>
        public static readonly int LengthMin = 1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベント数
        /// </summary>
        public int Count => List.Count;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        private List<CommonEvent> List { get; } = new List<CommonEvent>
        {
            new CommonEvent()
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期配列指定）
        /// </summary>
        /// <param name="list">[NotNull] コモンイベントリスト</param>
        /// <exception cref="ArgumentNullException">listがnullの場合</exception>
        /// <exception cref="ArgumentException">listの長さが0の場合</exception>
        public CommonEventList(List<CommonEvent> list)
        {
            if (list == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(list)));
            if (list.Count == 0)
                throw new ArgumentException(
                    "コモンイベントが1つ以上必要です。");

            List = list;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベントを追加する。
        /// </summary>
        /// <param name="commonEvent">[NotNull] コモンイベント</param>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void Add(CommonEvent commonEvent)
        {
            if (commonEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvent)));

            List.Add(commonEvent);
        }

        /// <summary>
        /// コモンイベントを追加する。
        /// </summary>
        /// <param name="commonEvents">[NotNull] コモンイベント</param>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void AddRange(IEnumerable<CommonEvent> commonEvents)
        {
            if (commonEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            List.AddRange(commonEvents);
        }

        /// <summary>
        /// コモンイベントを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数)] インデックス</param>
        /// <param name="commonEvent">[NotNull] 挿入する項目</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void Insert(int index, CommonEvent commonEvent)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (commonEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvent)));

            List.Insert(index, commonEvent);
        }

        /// <summary>
        /// コモンイベントを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数)] インデックス</param>
        /// <param name="commonEvents">[NotNull] 挿入する項目</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">commonEventsがnullの場合</exception>
        public void InsertRange(int index, IEnumerable<CommonEvent> commonEvents)
        {
            var max = Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (commonEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            List.InsertRange(index, commonEvents);
        }

        /// <summary>
        /// インデックスを指定してコモンイベントを削除する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果コモンイベント数が0以下になる</exception>
        public void RemoveAt(int index)
        {
            var cnt = Count;

            var max = cnt - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (cnt - 1 < LengthMin)
                throw new InvalidOperationException(
                    $"コモンイベント数が{LengthMin}を下回るため、これ以上削除できません。");

            List.RemoveAt(index);
        }

        /// <summary>
        /// コモンイベントを範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <param name="count">[Range(0, コモンイベント数-1)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, count が指定範囲以外</exception>
        /// <exception cref="InvalidOperationException">削除した結果コモンイベント数が0以下になる</exception>
        public void RemoveRange(int index, int count)
        {
            var cnt = Count;

            var max = cnt - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));
            if (count < min || max < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, max, count));

            if (cnt - count < LengthMin)
                throw new InvalidOperationException(
                    $"コモンイベント数が{LengthMin}を下回るため、これ以上削除できません。");

            List.RemoveRange(index, count);
        }

        /// <summary>
        /// コモンイベントを取得する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <returns>コモンイベント</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public CommonEvent Get(int index)
        {
            var max = Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            return List[index];
        }

        /// <summary>
        /// コモンイベント情報を範囲取得する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <param name="count">[Range(0, コモンイベント数)] 取得数</param>
        /// <returns>コモンイベント</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, count が指定範囲以外</exception>
        /// <exception cref="ArgumentException">取得範囲がコモンイベント数を超える場合</exception>
        public IEnumerable<CommonEvent> GetRange(int index, int count)
        {
            var cnt = Count;

            var indexMax = cnt - 1;
            const int min = 0;
            if (index < min || indexMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, indexMax, index));
            var countMax = cnt;
            if (count < min || countMax < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, countMax, count));
            if (index + count > cnt)
                throw new ArgumentException(
                    "index + count <= ページ数である必要があります。" +
                    $"（index: {index}, count:{count}, ページ数：{cnt}");

            return List.GetRange(index, count);
        }

        /// <summary>
        /// すべてのコモンイベントを返す。
        /// </summary>
        /// <returns>コモンイベント</returns>
        public IEnumerable<CommonEvent> GetAll()
        {
            return List;
        }

        /// <summary>
        /// コモンイベントを更新する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <param name="commonEvent">[NotNull] コモンイベント</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void Update(int index, CommonEvent commonEvent)
        {
            var max = Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (commonEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvent)));

            List[index] = commonEvent;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // コモンイベント数
            result.AddRange(Count.ToBytes(Endian.Woditor));

            // コモンイベントリスト
            foreach (var commonEvent in List)
            {
                result.AddRange(commonEvent.ToBinary());
            }

            return result.ToArray();
        }
    }
}