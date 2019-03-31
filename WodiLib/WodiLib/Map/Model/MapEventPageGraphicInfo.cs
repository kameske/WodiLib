// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageGraphicInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントページグラフィック情報クラス
    /// </summary>
    public class MapEventPageGraphicInfo
    {
        /// <summary>
        /// キャラ画像タイルセットフラグ
        /// </summary>
        public bool IsGraphicTileChip { get; set; }

        private MapEventTileId graphicTileId;

        /// <summary>[Range(0, 9999)] キャラ画像タイルセットID </summary>
        /// <summary>IsGraphicTileChip = false の場合、 -1 固定</summary>
        /// <exception cref="PropertyAccessException">IsGraphicTileChip = false のときにセットした場合</exception>
        public MapEventTileId GraphicTileId
        {
            get
            {
                if (IsGraphicTileChip) return graphicTileId;
                return MapEventTileId.NotUse;
            }
            set
            {
                if (!IsGraphicTileChip)
                    throw new PropertyAccessException(
                        $"{nameof(IsGraphicTileChip)}がtrueの場合のみセットできます。");
                graphicTileId = value;
            }
        }

        private CharaChipFileName charaChipFileName = "";

        /// <summary>[NotNull] キャラチップファイル名</summary>
        /// <exception cref="PropertyAccessException">IsGraphicTileChip = true のときにセットした場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CharaChipFileName CharaChipFileName
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

        private CharaChipDirection initDirection = CharaChipDirection.Down;

        /// <summary>
        /// [NotNull] キャラ画像初期向き
        /// </summary>
        /// <exception cref="PropertyNullException">nullセットした場合</exception>
        public CharaChipDirection InitDirection
        {
            get => initDirection;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(InitDirection)));
                initDirection = value;
            }
        }

        /// <summary>初期キャラアニメーションID</summary>
        public AnimationId InitAnimationId { get; set; } = (AnimationId) 1;

        /// <summary>キャラチップ透過度</summary>
        public MapEventOpacity CharaChipOpacity { get; set; } = (MapEventOpacity) 255;

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
        /// キャラ画像タイルIDをセットし、キャラ画像タイルセットフラグを更新する。
        /// </summary>
        /// <param name="tileId">キャラ画像タイルセットID </param>
        public void SetGraphicTileId(MapEventTileId tileId)
        {
            graphicTileId = tileId;

            if (tileId != MapEventTileId.NotUse)
            {
                IsGraphicTileChip = true;
                return;
            }

            IsGraphicTileChip = false;
            charaChipFileName = "";
        }

        /// <summary>
        /// キャラ画像ファイル名をセットする。
        /// （タイルID使用フラグをfalseに強制更新する）
        /// </summary>
        /// <param name="fileName">[NotNull] 画像ファイル名</param>
        /// <returns>キャラ画像ファイル名</returns>
        public void SetGraphicFileName(CharaChipFileName fileName)
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
            result.AddRange(CharaChipFileName.ToWoditorStringBytes());
            // 初期向き
            result.Add(InitDirection.Code);
            // 初期アニメーション番号
            result.Add(InitAnimationId.ToByte());
            // キャラチップ透過度
            result.Add(CharaChipOpacity.ToByte());
            // キャラチップ表示形式
            result.Add(CharaChipDrawType.Code);

            return result.ToArray();
        }
    }
}