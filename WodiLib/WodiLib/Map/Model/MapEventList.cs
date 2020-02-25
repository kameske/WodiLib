// ========================================
// Project Name : WodiLib
// File Name    : MapEventList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントリストクラス
    /// </summary>
    [Serializable]
    public class MapEventList : RestrictedCapacityCollection<MapEvent>
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
        /// <param name="events">[NotNull] 1ページ毎のマップイベント</param>
        /// <exception cref="ArgumentException">events内のイベントIDが重複している場合</exception>
        /// <exception cref="ArgumentNullException">eventsがnullの場合</exception>
        public MapEventList(IReadOnlyList<MapEvent> events) : base(events)
        {
            if (events is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(events)));

            var mapEvents = events.ToList();

            // イベントID重複チェック
            if (!ValidateDuplicateEventId(mapEvents))
                throw new ArgumentException(
                    $"イベントIDが重複しています。");
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
        public MapEvent GetMapEvent(MapEventId mapEventId)
            => Items.FirstOrDefault(x => x.MapEventId == mapEventId);

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 指定したインデックス位置にある要素を置き換える。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void SetItem(int index, MapEvent item)
        {
            var baseMapId = Items[index].MapEventId;
            if (baseMapId == item.MapEventId)
            {
                // マップイベントIDが同じならチェック無しで上書き
                base.SetItem(index, item);
                return;
            }

            // マップIDが重複する場合エラー
            if (ContainsEventId(item.MapEventId))
                throw new ArgumentException(
                    $"マップイベントIDが重複するため追加できません。" +
                    $"（マップイベントID: {item.MapEventId}）");

            base.SetItem(index, item);
        }

        /// <inheritdoc />
        /// <summary>
        /// 指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void InsertItem(int index, MapEvent item)
        {
            // マップIDが重複する場合エラー
            if (ContainsEventId(item.MapEventId))
                throw new ArgumentException(
                    $"マップイベントIDが重複するため追加できません。" +
                    $"（マップイベントID: {item.MapEventId}）");

            base.InsertItem(index, item);
        }

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override MapEvent MakeDefaultItem(int index) => new MapEvent();

        /// <summary>
        /// マップイベントIDからマップイベントを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベント（取得できない場合null）</returns>
        public MapEvent GetForMapEventId(MapEventId mapEventId)
        {
            return Items.FirstOrDefault(x => x.MapEventId == mapEventId);
        }

        /// <summary>
        /// 指定したイベントIDのインスタンスを保持しているかどうかを返す。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>イベント保持フラグ</returns>
        public bool ContainsEventId(MapEventId mapEventId)
        {
            var searchEvent = Items.FirstOrDefault(x => x.MapEventId == mapEventId);
            return !(searchEvent is null);
        }

        /// <summary>
        /// マップイベントIDの重複をチェックする。
        /// </summary>
        /// <param name="mapEvents">マップイベントリスト</param>
        /// <returns>重複がない場合true</returns>
        private static bool ValidateDuplicateEventId(IEnumerable<MapEvent> mapEvents)
        {
            var eventIds = mapEvents.Select(x => x.MapEventId).ToList();
            eventIds.Sort();
            for (var i = 1; i < eventIds.Count; i++)
            {
                if (eventIds[i] == eventIds[i - 1]) return false;
            }

            return true;
        }

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            foreach (var mapEvent in Items)
            {
                result.AddRange(mapEvent.ToBinary());
            }

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MapEventList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}