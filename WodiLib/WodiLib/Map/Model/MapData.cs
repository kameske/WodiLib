// ========================================
// Project Name : WodiLib
// File Name    : MapData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Event;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     MapDataクラス
    /// </summary>
    [Serializable]
    public class MapData : ModelBase<MapData>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapDataMemo memo = "";

        /// <summary>[NotNull] メモ</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public MapDataMemo Memo
        {
            get => memo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Memo)));
                memo = value;
                NotifyPropertyChanged();
            }
        }

        private TileSetId tileSetId = 0;

        /// <summary>タイルセットID</summary>
        public TileSetId TileSetId
        {
            get => tileSetId;
            set
            {
                tileSetId = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>マップサイズ横</summary>
        public MapSizeWidth MapSizeWidth => Layer1.Width;

        /// <summary>マップサイズ縦</summary>
        public MapSizeHeight MapSizeHeight => Layer1.Height;

        private Layer layer1 = new Layer();

        /// <summary>[NotNull] レイヤー1</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public Layer Layer1
        {
            get => layer1;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Layer1)));

                layer1.PropertyChanged -= OnLayer1PropertyChanged;

                layer1 = value;

                Layer2.UpdateWidth(value.Width);
                Layer2.UpdateHeight(value.Height);
                Layer3.UpdateWidth(value.Width);
                Layer3.UpdateHeight(value.Height);

                Layer1.PropertyChanged += OnLayer1PropertyChanged;

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(MapSizeWidth));
                NotifyPropertyChanged(nameof(MapSizeHeight));
            }
        }

        private Layer layer2 = new Layer();

        /// <summary>[NotNull] レイヤー2</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        /// <exception cref="PropertyException">Layer1と異なるマップサイズのインスタンスをセットしようとした場合</exception>
        public Layer Layer2
        {
            get => layer2;
            set
            {
                if (value is null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(Layer2)));
                if (value.Width != Layer1.Width || value.Height != Layer1.Height)
                    throw new PropertyException(
                        $"{nameof(Layer2)}のマップサイズは{nameof(Layer1)}のマップサイズと同じサイズである必要があります。");
                layer2 = value;
                NotifyPropertyChanged();
            }
        }

        private Layer layer3 = new Layer();

        /// <summary>[NotNull] レイヤー3</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        /// <exception cref="PropertyException">Layer1と異なるマップサイズのインスタンスをセットしようとした場合</exception>
        public Layer Layer3
        {
            get => layer3;
            set
            {
                if (value is null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(Layer3)));
                if (value.Width != Layer1.Width || value.Height != Layer1.Height)
                    throw new PropertyException(
                        $"{nameof(Layer3)}のマップサイズは{nameof(Layer1)}のマップサイズと同じサイズである必要があります。");
                layer3 = value;
                NotifyPropertyChanged();
            }
        }

        private MapEventList mapEvents = new MapEventList();

        /// <summary>[NotNull] マップイベントリスト</summary>
        public MapEventList MapEvents
        {
            get => mapEvents;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MapEvents)));

                mapEvents = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// レイヤー情報を返す。
        /// </summary>
        /// <param name="index">[Range(0, 2)] インデックス</param>
        /// <returns>レイヤー情報</returns>
        /// <exception cref="ArgumentOutOfRangeException">インデックスが 0～2以外の場合</exception>
        public Layer GetLayer(int index)
        {
            switch (index)
            {
                case 0:
                    return Layer1;
                case 1:
                    return Layer2;
                case 2:
                    return Layer3;
                default:
                    var message = $"indexは 0～2の範囲で指定する必要があります。（設定値：{index}";
                    throw new ArgumentOutOfRangeException(message);
            }
        }

        /// <summary>
        /// レイヤー情報をセットする。
        /// </summary>
        /// <param name="index">[Range(0, 2)] インデックス</param>
        /// <param name="layer">[NotNull] セットするレイヤー情報</param>
        /// <exception cref="ArgumentNullException">レイヤー情報が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">インデックスが 0～2 以外の場合</exception>
        public void SetLayer(int index, Layer layer)
        {
            if (layer is null)
            {
                var message = $"Layerに null をセットすることはできません";
                throw new ArgumentNullException(message);
            }

            switch (index)
            {
                case 0:
                    Layer1 = layer;
                    return;
                case 1:
                    Layer2 = layer;
                    return;
                case 2:
                    Layer3 = layer;
                    return;
                default:
                    var message = $"indexは 0～2の範囲で指定する必要があります。（設定値：{index}";
                    throw new ArgumentOutOfRangeException(message);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダ</summary>
        public static readonly byte[] HeaderBytes =
        {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x57, 0x4F, 0x4C, 0x46, 0x4D,
            0x00, 0x00,
            0x00, 0x00, 0x00, 0x64,
            0x00, 0x00, 0x00, 0x65
        };

        /// <summary>フッタ</summary>
        public static readonly byte[] Footer = {0x66};

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// レイヤ1プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnLayer1PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Layer.Width):
                    NotifyPropertyChanged(nameof(MapSizeWidth));
                    break;
                case nameof(Layer.Height):
                    NotifyPropertyChanged(nameof(MapSizeHeight));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapData()
        {
            layer1.PropertyChanged += OnLayer1PropertyChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したマップイベントIDのマップイベントを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベント（存在しない場合null）</returns>
        public MapEvent GetMapEvent(MapEventId mapEventId)
            => MapEvents.GetMapEvent(mapEventId);

        /// <summary>
        /// 指定したマップイベントIDのマップイベントページリストを取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>マップイベントページリスト</returns>
        /// <exception cref="ArgumentException">マップイベントIDで指定したマップイベントが存在しない場合</exception>
        public MapEventPageList GetEventPageList(MapEventId mapEventId)
            => MapEvents.GetEventPageList(mapEventId);

        /// <summary>
        /// 指定したマップイベントID、ページインデックスのマップイベントページ情報を取得する。
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="pageIndex">[Range(1, {対象イベントのページ数})] マップイベントページインデックス</param>
        /// <returns>マップイベントページ情報</returns>
        /// <exception cref="ArgumentException">マップイベントIDで指定したマップイベントが存在しない場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pageIndex が指定範囲外の場合</exception>
        public MapEventPage GetMapEventPage(MapEventId mapEventId, MapEventPageIndex pageIndex)
            => MapEvents.GetMapEventPage(mapEventId, pageIndex);

        /// <summary>
        /// マップサイズ横を更新する。
        /// レイヤ情報もともに更新される。
        /// </summary>
        /// <param name="width">マップサイズ横</param>
        public void UpdateMapSizeWidth(MapSizeWidth width)
        {
            Layer2.UpdateWidth(width);
            Layer3.UpdateWidth(width);
            Layer1.UpdateWidth(width);
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// レイヤ情報もともに更新される。
        /// </summary>
        /// <param name="height">マップサイズ縦</param>
        public void UpdateMapSizeHeight(MapSizeHeight height)
        {
            Layer2.UpdateHeight(height);
            Layer3.UpdateHeight(height);
            Layer1.UpdateHeight(height);
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// </summary>
        /// <param name="width">サイズ横</param>
        /// <param name="height">マップサイズ縦</param>
        public void UpdateMapSize(MapSizeWidth width, MapSizeHeight height)
        {
            UpdateMapSizeWidth(width);
            UpdateMapSizeHeight(height);
        }

        /// <summary>
        /// イベントコマンド文字列情報リストを取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <param name="eventId">[Range(0, MapEvents.Count - 1)] マップイベントID</param>
        /// <param name="page">[Range(0, PageValue-1)] ページインデックス</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">eventIdがThisMapEventの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">eventId, pageが指定された範囲外の場合</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyList<EventCommandSentenceInfo> MakeEventCommandSentenceInfoList(
            EventCommandSentenceResolver resolver, EventCommandSentenceResolveDesc desc,
            MapEventId eventId, int page)
        {
            if (eventId == MapEventId.ThisMapEvent)
                throw new ArgumentException(
                    ErrorMessage.Deny(nameof(eventId), $"{nameof(MapEventId)}.{nameof(MapEventId.ThisMapEvent)}"));

            const int idMin = 0;
            var idMax = MapEvents.Count - 1;
            if (eventId < idMin || idMax < eventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(eventId), idMin, idMax, eventId));

            return MapEvents[eventId].MakeEventCommandSentenceInfoList(resolver, desc, page);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(MapData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TileSetId == other.TileSetId
                   && memo == other.memo
                   && layer1.Equals(other.layer1)
                   && layer2.Equals(other.layer2)
                   && layer3.Equals(other.layer3)
                   && mapEvents.Equals(other.mapEvents);
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
            CheckMakeBinary();

            var result = new List<byte>();

            // ヘッダ
            result.AddRange(HeaderBytes);
            // メモ
            result.AddRange(Memo.ToWoditorStringBytes());
            // タイルセットID
            result.AddRange(TileSetId.ToBytes(Endian.Woditor));
            // マップサイズ横
            result.AddRange(MapSizeWidth.ToBytes(Endian.Woditor));
            // マップサイズ縦
            result.AddRange(MapSizeHeight.ToBytes(Endian.Woditor));
            // イベント数
            result.AddRange(MapEvents.Count.ToBytes(Endian.Woditor));
            // レイヤー
            result.AddRange(Layer1.ToBinary());
            result.AddRange(Layer2.ToBinary());
            result.AddRange(Layer3.ToBinary());
            // マップイベント
            result.AddRange(MapEvents.ToBinary());
            // フッタ
            result.AddRange(Footer);

            return result.ToArray();
        }

        private void CheckMakeBinary()
        {
            if (MapSizeWidth != Layer1.Width) throw new InvalidOperationException(LayerSizeErrorMessage(1, "横"));
            if (MapSizeHeight != Layer1.Height) throw new InvalidOperationException(LayerSizeErrorMessage(1, "縦"));
            if (MapSizeWidth != Layer2.Width) throw new InvalidOperationException(LayerSizeErrorMessage(2, "横"));
            if (MapSizeHeight != Layer2.Height) throw new InvalidOperationException(LayerSizeErrorMessage(2, "縦"));
            if (MapSizeWidth != Layer3.Width) throw new InvalidOperationException(LayerSizeErrorMessage(3, "横"));
            if (MapSizeHeight != Layer3.Height) throw new InvalidOperationException(LayerSizeErrorMessage(3, "縦"));
        }

        private static string LayerSizeErrorMessage(int layerNum, string direction)
        {
            return $"レイヤー{layerNum}のマップサイズ（{direction}）がマップデータのマップサイズ（{direction}）と異なります。";
        }
    }
}