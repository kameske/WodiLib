// ========================================
// Project Name : WodiLib
// File Name    : MapEvent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント
    /// </summary>
    public class MapEvent
    {
        /// <summary>マップイベントID</summary>
        public MapEventId MapEventId { get; set; }

        /// <summary>マップイベント名</summary>
        public MapEventName EventName { get; set; } = (MapEventName) "";

        /// <summary>X標</summary>
        public Position Position { get; set; }

        /// <summary>ページ数</summary>
        public int PageValue => MapEventPageList.Count;

        private MapEventPageList mapEventPageList = new MapEventPageList(new[]
        {
            new MapEventPage()
        });

        /// <summary>[NotNull] 1ページ毎のマップイベント</summary>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pagesの長さが1～10以外の場合</exception>
        public MapEventPageList MapEventPageList
        {
            get => mapEventPageList;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(MapEventPageList)));
                mapEventPageList = value;
            }
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
            // マップイベントID
            result.AddRange(MapEventId.ToBytes(Endian.Woditor));
            // イベント名
            result.AddRange(EventName.ToWoditorStringBytes());
            // X座標
            result.AddRange(Position.X.ToBytes(Endian.Woditor));
            // Y座標
            result.AddRange(Position.Y.ToBytes(Endian.Woditor));
            // イベントページ数
            result.AddRange(PageValue.ToBytes(Endian.Woditor));
            // 0パディング（4バイト）
            result.AddRange(0.ToBytes(Endian.Woditor));
            // マップイベントページ
            result.AddRange(MapEventPageList.ToBinary());
            // フッタ
            result.AddRange(Footer);

            return result.ToArray();
        }

        /// <summary>ヘッダバイト</summary>
        public static readonly byte[] Header = {0x6F, 0x39, 0x30, 0x00, 0x00};

        /// <summary>フッタバイト</summary>
        public static readonly byte[] Footer = {0x70};
    }
}