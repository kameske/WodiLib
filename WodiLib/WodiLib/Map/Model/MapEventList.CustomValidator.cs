// ========================================
// Project Name : WodiLib
// File Name    : MapEventList.CustomValidator.cs
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
    public partial class MapEventList
    {
        private class CustomValidator : IExtendedListValidator<MapEvent>
        {
            private IReadOnlyRestrictedCapacityList<MapEvent> Target { get; }

            private RestrictedCapacityListValidator<MapEvent> PreConditionValidator { get; }

            public CustomValidator(IReadOnlyRestrictedCapacityList<MapEvent> target)
            {
                Target = target;
                PreConditionValidator = new RestrictedCapacityListValidator<MapEvent>(target);
            }

            public void Constructor(params MapEvent[] initItems)
            {
                PreConditionValidator.Constructor(initItems);
                DuplicateEventId(initItems);
            }

            public void Get(int index, int count)
            {
                PreConditionValidator.Get(index, count);
            }

            public void Set(int index, params MapEvent[] items)
            {
                PreConditionValidator.Set(index, items);
                DuplicateSetEventId(index, items);
            }

            public void Insert(int index, params MapEvent[] items)
            {
                PreConditionValidator.Insert(index, items);
                DuplicateAddEventId(Target, items);
            }

            public void Overwrite(int index, params MapEvent[] items)
            {
                PreConditionValidator.Overwrite(index, items);
                DuplicateOverwriteEventId(index, items);
            }

            public void Move(int oldIndex, int newIndex, int count)
            {
                PreConditionValidator.Move(oldIndex, newIndex, count);
            }

            public void Remove(MapEvent? item)
            {
                PreConditionValidator.Remove(item);
            }

            public void Remove(int index, int count)
            {
                PreConditionValidator.Remove(index, count);
            }

            public void AdjustLength(int length)
            {
                PreConditionValidator.AdjustLength(length);
            }

            public void AdjustLengthIfShort(int length)
            {
                PreConditionValidator.AdjustLengthIfShort(length);
            }

            public void AdjustLengthIfLong(int length)
            {
                PreConditionValidator.AdjustLengthIfLong(length);
            }

            public void Reset(params MapEvent[] items)
            {
                PreConditionValidator.Reset(items);
                DuplicateEventId(items);
            }

            /// <summary>
            /// イベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="mapEvents">マップイベント</param>
            /// <exception cref="ArgumentException">イベントIDが重複している場合</exception>
            private static void DuplicateEventId(IEnumerable<MapEvent> mapEvents)
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
            /// <param name="index">更新する要素の先頭インデックス</param>
            /// <param name="newItems">更新後要素</param>
            /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
            private void DuplicateSetEventId(int index,
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
                var fixedEvents = Target.Take(index - 1).Concat(Target.Skip(index + changeLength));

                DuplicateAddEventId(fixedEvents, newItemList);
            }

            /// <summary>
            /// 要素上書きした場合にイベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="index">上書きする要素の先頭インデックス</param>
            /// <param name="newItems">上書き要素</param>
            /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
            private void DuplicateOverwriteEventId(int index,
                IEnumerable<MapEvent> newItems)
                => DuplicateSetEventId(index, newItems);

            /// <summary>
            /// 要素追加した際にイベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="items">追加要素</param>
            /// <exception cref="ArgumentException"></exception>
            private static void DuplicateAddEventId(IEnumerable<MapEvent> target, IEnumerable<MapEvent> items)
            {
                var selfIds = target.Select(item => item.MapEventId);
                var errorMapEvent = items.FirstOrDefault(item => selfIds.Contains(item.MapEventId));
                if (!(errorMapEvent is null))
                    throw new ArgumentException(
                        $"マップイベントIDが重複するため追加できません。" +
                        $"（マップイベントID: {errorMapEvent.MapEventId}）");
            }
        }
    }
}
