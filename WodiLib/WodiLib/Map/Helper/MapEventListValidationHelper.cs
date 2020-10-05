// ========================================
// Project Name : WodiLib
// File Name    : MapEventListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Map
{
    /// <summary>
    /// MpaEventList 検証ヘルパークラス
    /// </summary>
    internal static class MapEventListValidationHelper
    {
        /// <summary>
        /// イベントIDの重複がないことを検証する
        /// </summary>
        /// <param name="mapEvents">マップイベント</param>
        /// <exception cref="ArgumentException">イベントIDが重複している場合</exception>
        public static void DuplicateEventId(IEnumerable<MapEvent> mapEvents)
        {
            var eventIds = mapEvents.Select(x => x.MapEventId).ToList();
            eventIds.Sort();
            for (var i = 1; i < eventIds.Count; i++)
            {
                if (eventIds[i] == eventIds[i - 1])
                    throw new ArgumentException(
                        $"イベントIDが重複しています。（マップイベントID：{eventIds[i]}");
            }
        }

        /// <summary>
        /// 要素更新した場合にイベントIDの重複がないことを検証する
        /// </summary>
        /// <param name="self">検証対象</param>
        /// <param name="index">更新する要素の先頭インデックス</param>
        /// <param name="newItems">更新後要素</param>
        /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
        public static void DuplicateSetEventId(MapEventList self, int index,
            IEnumerable<MapEvent> newItems)
        {
            var newItemList = newItems.ToList();

            DuplicateEventId(newItemList);

            /*
             * “更新対象の要素を一旦除去 -> 更新要素を追加”
             * とみなして検証する
             */

            // 更新後に変化しない部分のイベントID抽出
            var changeLength = newItemList.Count;
            var fixedEvents = self.Take(index - 1).Concat(self.Skip(index + changeLength));

            DuplicateAddEventId(fixedEvents, newItemList);
        }

        /// <summary>
        /// 要素追加した際にイベントIDの重複がないことを検証する
        /// </summary>
        /// <param name="self">検証対象</param>
        /// <param name="items">追加要素</param>
        /// <exception cref="ArgumentException"></exception>
        public static void DuplicateAddEventId(IEnumerable<MapEvent> self, IEnumerable<MapEvent> items)
        {
            var selfIds = self.Select(item => item.MapEventId);
            var errorMapEvent = items.FirstOrDefault(item => selfIds.Contains(item.MapEventId));
            if (!(errorMapEvent is null))
                throw new ArgumentException(
                    $"マップイベントIDが重複するため追加できません。" +
                    $"（マップイベントID: {errorMapEvent.MapEventId}）");
        }
    }
}
