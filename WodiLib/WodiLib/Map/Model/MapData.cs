// ========================================
// Project Name : WodiLib
// File Name    : MapData.cs
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
    ///     MapDataクラス
    /// </summary>
    public class MapData
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
            set => memo = value ?? throw new PropertyNullException(ErrorMessage.NotNull(nameof(Memo)));
        }

        /// <summary>タイルセットID</summary>
        public TileSetId TileSetId { get; set; } = 0;

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
                layer1 = value ?? throw new PropertyNullException(ErrorMessage.NotNull(nameof(Layer1)));

                Layer2.UpdateWidth(value.Width);
                Layer2.UpdateHeight(value.Height);
                Layer3.UpdateWidth(value.Width);
                Layer3.UpdateHeight(value.Height);
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
                if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(Layer2)));
                if (value.Width != Layer1.Width || value.Height != Layer1.Height)
                    throw new PropertyException(
                        $"{nameof(Layer2)}のマップサイズは{nameof(Layer1)}のマップサイズと同じサイズである必要があります。");
                layer2 = value;
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
                if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(Layer3)));
                if (value.Width != Layer1.Width || value.Height != Layer1.Height)
                    throw new PropertyException(
                        $"{nameof(Layer3)}のマップサイズは{nameof(Layer1)}のマップサイズと同じサイズである必要があります。");
                layer3 = value;
            }
        }

        private MapEventList mapEvents = new MapEventList();

        /// <summary>[NotNull] マップイベントリスト</summary>
        public MapEventList MapEvents
        {
            get => mapEvents;
            set => mapEvents = value ?? throw new PropertyNullException(ErrorMessage.NotNull(nameof(MapEvents)));
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
            if (layer == null)
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
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// マップサイズ横を更新する。
        /// レイヤ情報もともに更新される。
        /// </summary>
        /// <param name="width">マップサイズ横</param>
        public void UpdateMapSizeWidth(MapSizeWidth width)
        {
            Layer1.UpdateWidth(width);
            Layer2.UpdateWidth(width);
            Layer3.UpdateWidth(width);
        }

        /// <summary>
        /// マップサイズ縦を更新する。
        /// レイヤ情報もともに更新される。
        /// </summary>
        /// <param name="height">マップサイズ縦</param>
        public void UpdateMapSizeHeight(MapSizeHeight height)
        {
            Layer1.UpdateHeight(height);
            Layer2.UpdateHeight(height);
            Layer3.UpdateHeight(height);
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