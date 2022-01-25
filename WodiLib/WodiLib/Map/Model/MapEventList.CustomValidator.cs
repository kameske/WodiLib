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
using WodiLib.Sys.Collections;

namespace WodiLib.Map
{
    public partial class MapEventList
    {
        private class CustomValidator : WodiLibListValidatorTemplate<MapEvent>
        {
            protected override IWodiLibListValidator<MapEvent>? BaseValidator { get; }

            private new IRestrictedCapacityList<MapEvent> Target { get; }

            public CustomValidator(IRestrictedCapacityList<MapEvent> target) : base(target)
            {
                Target = target;
                BaseValidator = new RestrictedCapacityListValidator<MapEvent>(target);
            }

            public override void Constructor(NamedValue<IEnumerable<MapEvent>> initItems)
            {
                BaseValidator!.Constructor(initItems);
                DuplicateEventId(initItems.Value.ToArray());
            }

            public override void Set(NamedValue<int> index, NamedValue<MapEvent> item)
            {
                BaseValidator!.Set(index, item);
                DuplicateSetEventId(index.Value, item.Value);
            }

            public override void Set(NamedValue<int> index, NamedValue<IEnumerable<MapEvent>> item)
            {
                BaseValidator!.Set(index, item);
                DuplicateSetEventId(index.Value, item.Value.ToArray());
            }

            public override void Insert(NamedValue<int> index, NamedValue<MapEvent> items)
            {
                BaseValidator!.Insert(index, items);
                DuplicateAddEventId(Target, items.Value);
            }

            public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<MapEvent>> items)
            {
                BaseValidator!.Insert(index, items);
                DuplicateAddEventId(Target, items.Value.ToArray());
            }

            public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<MapEvent>> items)
            {
                BaseValidator!.Overwrite(index, items);
                DuplicateOverwriteEventId(index.Value, items.Value.ToArray());
            }

            public override void Reset(NamedValue<IEnumerable<MapEvent>> items)
            {
                BaseValidator!.Reset(items);
                DuplicateEventId(items.Value.ToArray());
            }

            /// <summary>
            /// イベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="mapEvents">マップイベント</param>
            /// <exception cref="ArgumentException">イベントIDが重複している場合</exception>
            private static void DuplicateEventId(params MapEvent[] mapEvents)
            {
                var eventIds = mapEvents.Select(x => x.MapEventId).ToList();
                eventIds.Sort();
                for (var i = 1; i < eventIds.Count; i++)
                {
                    if (eventIds[i] == eventIds[i - 1])
                    {
                        throw new ArgumentException(
                            $"イベントIDが重複しています。（マップイベントID：{eventIds[i]}"
                        );
                    }
                }
            }

            /// <summary>
            /// 要素更新した場合にイベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="index">更新する要素の先頭インデックス</param>
            /// <param name="newItems">更新後要素</param>
            /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
            private void DuplicateSetEventId(int index, params MapEvent[] newItems)
            {
                DuplicateEventId(newItems);

                /*
                 * “更新対象の要素を一旦除去 -> 更新要素を追加”
                 * とみなして検証する
                 */

                // 更新後に変化しない部分のイベントID抽出
                var changeLength = newItems.Length;
                var fixedEvents = Target.Take(index - 1).Concat(Target.Skip(index + changeLength));

                DuplicateAddEventId(fixedEvents, newItems);
            }

            /// <summary>
            /// 要素上書きした場合にイベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="index">上書きする要素の先頭インデックス</param>
            /// <param name="newItems">上書き要素</param>
            /// <exception cref="ArgumentException">イベントIDが重複する場合</exception>
            private void DuplicateOverwriteEventId(int index, params MapEvent[] newItems)
                => DuplicateSetEventId(index, newItems);

            /// <summary>
            /// 要素追加した際にイベントIDの重複がないことを検証する
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="items">追加要素</param>
            /// <exception cref="ArgumentException"></exception>
            private static void DuplicateAddEventId(IEnumerable<MapEvent> target, params MapEvent[] items)
            {
                var selfIds = target.Select(item => item.MapEventId);
                var errorMapEvent = items.FirstOrDefault(item => selfIds.Contains(item.MapEventId));
                if (errorMapEvent is not null)
                {
                    throw new ArgumentException(
                        $"マップイベントIDが重複するため追加できません。" + $"（マップイベントID: {errorMapEvent.MapEventId}）"
                    );
                }
            }
        }
    }
}
