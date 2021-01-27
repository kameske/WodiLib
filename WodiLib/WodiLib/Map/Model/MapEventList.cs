// ========================================
// Project Name : WodiLib
// File Name    : MapEventList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントリストクラス
    /// </summary>
    public partial class MapEventList : RestrictedCapacityList<MapEvent>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static int MaxCapacity => 9999;

        /// <summary>リスト最小数</summary>
        public static int MinCapacity => 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapEventList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="events">1ページ毎のマップイベント</param>
        /// <exception cref="ArgumentException">events内のイベントIDが重複している場合</exception>
        /// <exception cref="ArgumentNullException">eventsがnullの場合</exception>
        public MapEventList(IEnumerable<MapEvent> events) : base(events)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したマップイベントIDのマップイベントを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベント（存在しない場合null）</returns>
        public MapEvent? GetMapEvent(MapEventId mapEventId)
            => this.FirstOrDefault(x => x.MapEventId == mapEventId);

        /// <summary>
        /// 指定したマップイベントIDのマップイベントページリストを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベントページリスト</returns>
        /// <exception cref="ArgumentException">マップイベントIDで指定したマップイベントが存在しない場合</exception>
        public MapEventPageList GetEventPageList(MapEventId mapEventId)
        {
            var targetEvent = GetMapEvent(mapEventId);
            if (targetEvent is null)
                throw new ArgumentException(
                    ErrorMessage.NotFound($"ID={mapEventId}のマップイベント"));

            return targetEvent.MapEventPageList;
        }

        /// <summary>
        /// 指定したマップイベントID、ページインデックスのマップイベントページ情報を取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="pageIndex">[Range(1, {対象イベントのページ数})] マップイベントページインデックス</param>
        /// <returns>マップイベントページ情報</returns>
        /// <exception cref="ArgumentException">マップイベントIDで指定したマップイベントが存在しない場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pageIndex が指定範囲外の場合</exception>
        public MapEventPage GetMapEventPage(MapEventId mapEventId, MapEventPageIndex pageIndex)
        {
            var targetEvent = GetMapEvent(mapEventId);
            if (targetEvent is null)
                throw new ArgumentException(
                    ErrorMessage.NotFound($"ID={mapEventId}のマップイベント"));

            return targetEvent.MapEventPageList[pageIndex];
        }

        /// <summary>
        /// マップイベントIDからマップイベントを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベント（取得できない場合null）</returns>
        [Obsolete("GetMapEvent(MapEventId) メソッドと重複するメソッドのため、 Ver 2.6 にて削除します。")]
        public MapEvent? GetForMapEventId(MapEventId mapEventId)
        {
            return this.FirstOrDefault(x => x.MapEventId == mapEventId);
        }

        /// <summary>
        /// 指定したイベントIDのインスタンスを保持しているかどうかを返す。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>イベント保持フラグ</returns>
        public bool ContainsEventId(MapEventId mapEventId)
        {
            var searchEvent = this.FirstOrDefault(x => x.MapEventId == mapEventId);
            return !(searchEvent is null);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override IWodiLibListValidator<MapEvent> MakeValidator()
        {
            return new CustomValidator(this);
        }

        /// <inheritdoc />
        protected override MapEvent MakeDefaultItem(int index) => new MapEvent();

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

            foreach (var mapEvent in this)
            {
                result.AddRange(mapEvent.ToBinary());
            }

            return result.ToArray();
        }
    }
}
