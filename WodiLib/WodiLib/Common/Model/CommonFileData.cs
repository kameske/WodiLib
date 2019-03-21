// ========================================
// Project Name : WodiLib
// File Name    : CommonFileData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンファイルデータクラス
    /// </summary>
    public class CommonFileData
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダ</summary>
        public static readonly byte[] Header =
        {
            0x6C, 0x38, 0x0C, 0x03
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
        public void SetCommonEventList(IEnumerable<CommonEvent> commonEvents)
        {
            var eventList = commonEvents.ToList();

            if (commonEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(commonEvents)));

            if (eventList.Count == 0)
                throw new ArgumentException(
                    $"{nameof(commonEvents)}の要素数は1以上である必要があります。");

            CommonEventList = new CommonEventList(eventList);
        }

        /// <summary>
        /// すべてのコモンイベントを返す。
        /// </summary>
        /// <returns>コモンイベント</returns>
        public IEnumerable<CommonEvent> GetAllCommonEvent()
        {
            return CommonEventList.GetAll();
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

            return result.ToArray();
        }
    }
}