// ========================================
// Project Name : WodiLib
// File Name    : MapEventList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            StartObserveListEvent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="events">1ページ毎のマップイベント</param>
        /// <exception cref="ArgumentException">events内のイベントIDが重複している場合</exception>
        /// <exception cref="ArgumentNullException">eventsがnullの場合</exception>
        public MapEventList(IReadOnlyList<MapEvent> events) : base(events)
        {
            if (events is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(events)));

            var mapEvents = events.ToList();

            // イベントID重複チェック
            MapEventListValidationHelper.DuplicateEventId(mapEvents);

            StartObserveListEvent();
        }

        /// <summary>
        /// 独自リストのイベント購読を開始する。コンストラクタ用。
        /// </summary>
        private void StartObserveListEvent()
        {
            CollectionChanging += OnCollectionChanging;
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

        /// <summary>
        /// マップイベントIDからマップイベントを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベント（取得できない場合null）</returns>
        public MapEvent? GetForMapEventId(MapEventId mapEventId)
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override MapEvent MakeDefaultItem(int index) => new MapEvent();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Event Handler
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CollectionChanging

        private void OnCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ExecuteByAction<MapEvent>(
                replaceAction: PreSetItem,
                addAction: PreInsertItem
            );
        }

        /// <summary>
        /// 要素更新前に呼び出される処理
        /// </summary>
        /// <param name="index">更新する要素の先頭インデックス</param>
        /// <param name="oldItems">更新前要素</param>
        /// <param name="newItems">更新後要素</param>
        private void PreSetItem(int index, IEnumerable<MapEvent> oldItems, IEnumerable<MapEvent> newItems)
        {
            MapEventListValidationHelper.DuplicateSetEventId(this, index, newItems);
        }

        /// <summary>
        /// 要素追加前に呼び出される処理
        /// </summary>
        /// <param name="index">追加するインデックス</param>
        /// <param name="items">追加要素</param>
        private void PreInsertItem(int index, IEnumerable<MapEvent> items)
        {
            MapEventListValidationHelper.DuplicateAddEventId(this, items);
        }

        #endregion

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
