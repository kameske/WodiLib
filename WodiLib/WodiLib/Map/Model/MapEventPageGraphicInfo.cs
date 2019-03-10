// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageGraphicInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントページグラフィック情報クラス
    /// </summary>
    public class MapEventPageGraphicInfo
    {
        /// <summary>キャラグラフィックにタイル画像を使用しない場合の値</summary>
        public static readonly int GraphicNotUseTileId = -1;

        /// <summary>
        /// キャラ画像タイルセットフラグ
        /// </summary>
        public bool IsGraphicTileChip { get; set; }

        private int graphicTileId;

        /// <summary>[Range(0, 99999)] キャラ画像タイルセットID </summary>
        /// <summary>IsGraphicTileChip = false の場合、 -1 固定</summary>
        /// <exception cref="PropertyAccessException">IsGraphicTileChip = false の場合</exception>
        /// <exception cref="PropertyOutOfRangeException">範囲外の値を指定した場合</exception>
        public int GraphicTileId
        {
            get
            {
                if (IsGraphicTileChip) return graphicTileId;
                return GraphicNotUseTileId;
            }
            set
            {
                if (!IsGraphicTileChip)
                    throw new PropertyAccessException(
                        $"{nameof(IsGraphicTileChip)}がtrueの場合のみセットできます。");
                if (value < MapChip.StandardTileMin || MapChip.StandardTileMax < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(GraphicTileId), 0, 99999, value));
                graphicTileId = value;
            }
        }

        private string charaChipFileName = "";

        /// <summary>[NotNull] キャラチップファイル名</summary>
        /// <exception cref="PropertyAccessException">IsGraphicTileChip = true の場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string CharaChipFileName
        {
            get => charaChipFileName;
            set
            {
                if (IsGraphicTileChip)
                    throw new PropertyAccessException(
                        $"{nameof(IsGraphicTileChip)}がfalseの場合のみセットできます。");
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CharaChipFileName)));
                charaChipFileName = value;
            }
        }

        private byte initDirection = 2;

        /// <summary>
        /// [Range(1, 9)] キャラ画像初期向き
        /// </summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲外の値をセットした場合</exception>
        public byte InitDirection
        {
            get => initDirection;
            set
            {
                if (value < 1 || 9 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(InitDirection), 1, 9, value));
                initDirection = value;
            }
        }

        /// <summary>初期キャラアニメーションID</summary>
        public byte InitAnimationId { get; set; } = 1;

        /// <summary>キャラチップ透過度</summary>
        public byte CharaChipOpacity { get; set; }

        private PictureDrawType charaChipDrawType = PictureDrawType.Normal;

        /// <summary>[NotNull] キャラチップ表示タイプ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public PictureDrawType CharaChipDrawType
        {
            get => charaChipDrawType;
            set
            {
                if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(CharaChipDrawType)));
                charaChipDrawType = value;
            }
        }

        /// <summary>
        /// キャラ画像タイルIDをセットする。
        /// （タイルID使用フラグをtrueに強制更新する）
        /// </summary>
        /// <param name="tileId">[Range(0, 99999)] キャラ画像タイルセットID </param>
        /// <exception cref="PropertyOutOfRangeException">範囲外の値を指定した場合</exception>
        public void SetGraphicTileId(int tileId)
        {
            if (tileId < MapChip.StandardTileMin || MapChip.StandardTileMax < tileId)
                throw new PropertyOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(GraphicTileId), MapChip.StandardTileMin,
                        MapChip.StandardTileMax, tileId));
            // CharaChipFileName は IsGraphicTileChip = false の場合のみ書き換え可能
            IsGraphicTileChip = false;
            CharaChipFileName = "";
            IsGraphicTileChip = true;
            GraphicTileId = tileId;
        }

        /// <summary>
        /// キャラ画像ファイル名をセットする。
        /// （タイルID使用フラグをfalseに強制更新する）
        /// </summary>
        /// <param name="fileName">[NotNull] 画像ファイル名</param>
        /// <returns>キャラ画像ファイル名</returns>
        public void SetGraphicFileName(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));

            IsGraphicTileChip = false;
            CharaChipFileName = fileName;
        }

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // キャラ画像タイルセットID
            result.AddRange(GraphicTileId.ToBytes(Endian.Woditor));
            // キャラチップ名
            result.AddRange(new WoditorString(CharaChipFileName).StringByte);
            // 初期向き
            result.Add(InitDirection);
            // 初期アニメーション番号
            result.Add(InitAnimationId);
            // キャラチップ透過度
            result.Add(CharaChipOpacity);
            // キャラチップ表示形式
            result.Add(CharaChipDrawType.Code);

            return result.ToArray();
        }
    }
}