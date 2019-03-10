// ========================================
// Project Name : WodiLib
// File Name    : CommonFileData.cs
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
    /// コモンイベントデータ
    /// </summary>
    public class CommonEventData
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダ</summary>
        public static readonly byte[] Header =
        {
            0x00, 0x57, 0x00, 0x00, 0x4F, 0x4C, 0x00, 0x46, 0x43, 0x00, 0x8F
        };

        /// <summary>フッタ</summary>
        public static readonly byte[] Footer =
        {
            0x8F
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventList CommonEventList { get; set; } = new CommonEventList();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベントリストをセットする。
        /// </summary>
        /// <param name="commonEvents">[NotNull] コモンイベントリスト</param>
        /// <exception cref="ArgumentNullException">commonEventsがnullの場合</exception>
        /// <exception cref="ArgumentException">commonEventsの要素数が0の場合</exception>
        public void SetCommonEventList(List<CommonEvent> commonEvents)
        {
            if (commonEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            if (commonEvents.Count == 0)
                throw new ArgumentException(
                    $"{nameof(commonEvents)}の要素数は1以上である必要があります。");

            CommonEventList = new CommonEventList(commonEvents);
        }

        /// <summary>
        /// コモンイベントを追加する。
        /// </summary>
        /// <param name="commonEvent">[NotNull] コモンイベント</param>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void AddCommonEvent(CommonEvent commonEvent)
        {
            if (commonEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvent)));

            CommonEventList.Add(commonEvent);
        }

        /// <summary>
        /// コモンイベントを追加する。
        /// </summary>
        /// <param name="commonEvents">[NotNull] コモンイベント</param>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void AddCommonEventRange(IEnumerable<CommonEvent> commonEvents)
        {
            if (commonEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            CommonEventList.AddRange(commonEvents);
        }

        /// <summary>
        /// コモンイベントを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数)] インデックス</param>
        /// <param name="commonEvent">[NotNull] 挿入する項目</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">commonEventがnullの場合</exception>
        public void InsertCommonEvent(int index, CommonEvent commonEvent)
        {
            var max = CommonEventList.Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (commonEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvent)));

            CommonEventList.Insert(index, commonEvent);
        }

        /// <summary>
        /// コモンイベントを挿入する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数)] インデックス</param>
        /// <param name="commonEvents">[NotNull] 挿入する項目</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentNullException">commonEventsがnullの場合</exception>
        public void InsertCommonEventRange(int index, IEnumerable<CommonEvent> commonEvents)
        {
            var max = CommonEventList.Count;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (commonEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            CommonEventList.InsertRange(index, commonEvents);
        }

        /// <summary>
        /// インデックスを指定してコモンイベントを削除する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        /// <exception cref="InvalidOperationException">削除した結果コモンイベント数が0以下になる</exception>
        public void RemoveCommonEventAt(int index)
        {
            var cnt = CommonEventList.Count;

            var max = cnt - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (cnt - 1 < CommonEventList.LengthMin)
                throw new InvalidOperationException(
                    $"コモンイベント数が{CommonEventList.LengthMin}を下回るため、これ以上削除できません。");

            CommonEventList.RemoveAt(index);
        }

        /// <summary>
        /// コモンイベントを範囲削除する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <param name="count">[Range(0, コモンイベント数-1)] 削除数</param>
        /// <exception cref="ArgumentOutOfRangeException">index, count が指定範囲以外</exception>
        /// <exception cref="InvalidOperationException">削除した結果コモンイベント数が0以下になる</exception>
        public void RemoveCommonEventRange(int index, int count)
        {
            var cnt = CommonEventList.Count;

            var max = cnt - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));
            if (count < min || max < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(count), min, max, count));

            if (cnt - count < CommonEventList.LengthMin)
                throw new InvalidOperationException(
                    $"コモンイベント数が{CommonEventList.LengthMin}を下回るため、これ以上削除できません。");

            CommonEventList.RemoveRange(index, count);
        }

        /// <summary>
        /// コモンイベントを取得する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <returns>コモンイベント</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public CommonEvent GetCommonEvent(int index)
        {
            var max = CommonEventList.Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            return CommonEventList.Get(index);
        }

        /// <summary>
        /// コモンイベント情報を範囲取得する。
        /// </summary>
        /// <param name="index">[Range(0, コモンイベント数-1)] インデックス</param>
        /// <param name="count">[Range(0, コモンイベント数)] 取得数</param>
        /// <returns>コモンイベント</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, count が指定範囲以外</exception>
        /// <exception cref="ArgumentException">取得範囲がコモンイベント数を超える場合</exception>
        public IEnumerable<CommonEvent> GetCommonEventRange(int index, int count)
        {
            var cnt = CommonEventList.Count;

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

            return CommonEventList.GetRange(index, count);
        }

        /// <summary>
        /// すべてのコモンイベントを返す。
        /// </summary>
        /// <returns>コモンイベント</returns>
        public IEnumerable<CommonEvent> GetAllCommonEvent()
        {
            return CommonEventList.GetAll();
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
            var max = CommonEventList.Count - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), min, max, index));

            if (commonEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvent)));

            CommonEventList.Update(index, commonEvent);
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

            // ヘッダ
            result.AddRange(Header);

            // コモンイベント
            result.AddRange(CommonEventList.ToBinary());

            // フッタ
            result.AddRange(Footer);

            return result.ToArray();
        }
    }
}