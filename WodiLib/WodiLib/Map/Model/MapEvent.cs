// ========================================
// Project Name : WodiLib
// File Name    : MapEvent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント
    /// </summary>
    [Serializable]
    public class MapEvent : ModelBase<MapEvent>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapEventId mapEventId;

        /// <summary>マップイベントID</summary>
        public MapEventId MapEventId
        {
            get => mapEventId;
            set
            {
                mapEventId = value;
                NotifyPropertyChanged();
            }
        }

        private MapEventName eventName = "";

        /// <summary>マップイベント名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEventName EventName
        {
            get => eventName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventName)));
                eventName = value;
                NotifyPropertyChanged();
            }
        }

        private Position position;

        /// <summary>座標</summary>
        public Position Position
        {
            get => position;
            set
            {
                position = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>ページ数</summary>
        public int PageValue => MapEventPageList.Count;

        private MapEventPageList mapEventPageList = new MapEventPageList(new[]
        {
            new MapEventPage()
        });

        /// <summary>1ページ毎のマップイベント</summary>
        /// <exception cref="ArgumentNullException">pagesがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pagesの長さが1～10以外の場合</exception>
        public MapEventPageList MapEventPageList
        {
            get => mapEventPageList;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(MapEventPageList)));

                mapEventPageList.PropertyChanged -= OnMapEventPageListPropertyChanged;
                mapEventPageList = value;
                mapEventPageList.PropertyChanged += OnMapEventPageListPropertyChanged;

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(PageValue));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップイベントリストプロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnMapEventPageListPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(MapEventPageList.Count):
                    NotifyPropertyChanged(nameof(PageValue));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapEvent()
        {
            mapEventPageList.PropertyChanged += OnMapEventPageListPropertyChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文字列情報リストを取得する。
        /// </summary>
        /// <param name="resolver">名前解決クラスインスタンス</param>
        /// <param name="desc">付加情報</param>
        /// <param name="page">[Range(0, PageValue-1)] ページインデックス</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">pageが指定された範囲外の場合</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyList<EventCommandSentenceInfo> MakeEventCommandSentenceInfoList(
            EventCommandSentenceResolver resolver, EventCommandSentenceResolveDesc? desc,
            int page)
        {
            var targetPage = MapEventPageList[page];

            return targetPage.MakeEventCommandSentenceInfoList(resolver, desc);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool ItemEquals(MapEvent? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MapEventId == other.MapEventId
                   && eventName.Equals(other.eventName)
                   && Position == other.Position
                   && mapEventPageList.Equals(other.mapEventPageList);
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
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Header = {0x6F, 0x39, 0x30, 0x00, 0x00};

        /// <summary>フッタバイト</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Footer = {0x70};
    }
}
