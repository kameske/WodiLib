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
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントリストクラス
    /// </summary>
    public class MapEventList : IWodiLibObject
    {
        /// <summary>イベントID最大値</summary>
        public static readonly int EventIdMax = 9999;

        /// <summary>イベントID最小値</summary>
        public static readonly int EventIdMin = 0;

        /// <summary>ページ数</summary>
        public int Count => eventList.Count;

        private readonly List<MapEvent> eventList;

        /// <inheritdoc />
        public MapEventList() : this(new MapEvent[0])
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="events">[NotNull] 1ページ毎のマップイベント</param>
        /// <exception cref="ArgumentException">events内のイベントIDが重複している場合</exception>
        /// <exception cref="ArgumentNullException">eventsがnullの場合</exception>
        public MapEventList(IEnumerable<MapEvent> events)
        {
            if (events == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(events)));

            var mapEvents = events.ToList();

            // イベントID重複チェック
            if (!ValidateDuplicateEventId(mapEvents))
                throw new ArgumentException(
                    $"イベントIDが重複しています。");

            eventList = mapEvents;
        }

        /// <summary>
        /// マップイベントクラスを追加する。
        /// </summary>
        /// <param name="mapEvent">[NotNull] マップイベント</param>
        /// <exception cref="ArgumentNullException">mapEventがnullの場合</exception>
        /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
        public void Add(MapEvent mapEvent)
        {
            if (mapEvent == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapEvent)));
            if (ContainsEventId(mapEvent.MapEventId))
                throw new ArgumentException(
                    $"マップイベントIDが重複するため追加できません。" +
                    $"（マップイベントID: {mapEvent.MapEventId}）");

            eventList.Add(mapEvent);
        }

        /// <summary>
        /// マップイベントクラスを追加する。
        /// </summary>
        /// <param name="mapEvents">[NotNull] マップイベントリスト</param>
        /// <exception cref="ArgumentNullException">mapEventがnullの場合</exception>
        /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
        public void AddRange(IEnumerable<MapEvent> mapEvents)
        {
            if (mapEvents == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapEvents)));

            var selfPages = eventList.ToList();

            var mapEventArray = mapEvents as MapEvent[] ?? mapEvents.ToArray();
            if (!ValidateDuplicateEventId(mapEventArray))
                throw new ArgumentException(
                    $"マップイベントIDが重複するため追加できません。");

            selfPages.AddRange(mapEventArray);
        }

        /// <summary>
        /// マップイベントを除去する。
        /// </summary>
        /// <param name="mapEventId">[Range(0, 9999)] 削除マップイベントID</param>
        /// <exception cref="ArgumentOutOfRangeException">mapEventIdが指定範囲以外の場合</exception>
        /// <exception cref="ArgumentException">指定されたマップイベントIDが存在しない場合</exception>
        public void Remove(int mapEventId)
        {
            if (mapEventId < EventIdMin || EventIdMax < mapEventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(mapEventId), EventIdMin, EventIdMax, mapEventId));

            var removeItem = eventList.FirstOrDefault(x => x.MapEventId == mapEventId);

            if (removeItem == null)
                throw new ArgumentException(
                    $"指定されたマップイベントIDが存在しません。" +
                    $"({nameof(mapEventId)}: {mapEventId}");

            eventList.Remove(removeItem);
        }

        /// <summary>
        /// マップイベントを取得する。
        /// </summary>
        /// <param name="mapEventId">[Range(0, 9999)] インデックス</param>
        /// <returns>[Nullable] マップイベント</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外の場合</exception>
        public MapEvent Get(int mapEventId)
        {
            if (mapEventId < EventIdMin || EventIdMax <= mapEventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(mapEventId), EventIdMin, EventIdMax, mapEventId));

            return eventList.FirstOrDefault(x => x.MapEventId == mapEventId);
        }

        /// <summary>
        /// 指定したイベントIDのインスタンスを保持しているかどうかを返す。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>イベント保持フラグ</returns>
        public bool ContainsEventId(int mapEventId)
        {
            if (mapEventId < EventIdMin || EventIdMax < mapEventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(mapEventId), EventIdMin, EventIdMax, mapEventId));
            var searchEvent = eventList.FirstOrDefault(x => x.MapEventId == mapEventId);
            return searchEvent != null;
        }

        /// <summary>
        /// マップイベントIDの重複をチェックする。
        /// </summary>
        /// <param name="mapEvents"></param>
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

        /// <inheritdoc />
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            foreach (var mapEvent in eventList)
            {
                result.AddRange(mapEvent.ToBinary());
            }

            return result.ToArray();
        }
    }
}