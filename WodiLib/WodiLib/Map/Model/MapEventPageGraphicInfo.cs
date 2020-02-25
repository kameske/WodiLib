// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageGraphicInfo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップイベントページグラフィック情報クラス
    /// </summary>
    [Serializable]
    public class MapEventPageGraphicInfo : IEquatable<MapEventPageGraphicInfo>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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

        private CharaChipFilePath charaChipFilePath = "";

        /// <summary>[NotNull] キャラチップファイル名</summary>
        /// <exception cref="PropertyAccessException">IsGraphicTileChip = true のときにセットした場合</exception>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CharaChipFilePath CharaChipFilePath
        {
            get => charaChipFilePath;
            set
            {
                if (IsGraphicTileChip)
                    throw new PropertyAccessException(
                        $"{nameof(IsGraphicTileChip)}がfalseの場合のみセットできます。");
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CharaChipFilePath)));
                charaChipFilePath = value;
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
                if (value is null)
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
                if (value is null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(CharaChipDrawType)));
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
            charaChipFilePath = "";
        }

        /// <summary>
        /// キャラ画像ファイル名をセットする。
        /// （タイルID使用フラグをfalseに強制更新する）
        /// </summary>
        /// <param name="filePath">[NotNull] 画像ファイル名</param>
        /// <returns>キャラ画像ファイル名</returns>
        public void SetGraphicFileName(CharaChipFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            IsGraphicTileChip = false;
            CharaChipFilePath = filePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapEventPageGraphicInfo()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(MapEventPageGraphicInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return graphicTileId == other.graphicTileId
                   && CharaChipOpacity == other.CharaChipOpacity
                   && InitAnimationId == other.InitAnimationId
                   && charaChipFilePath.Equals(other.charaChipFilePath)
                   && initDirection.Equals(other.initDirection)
                   && charaChipDrawType.Equals(other.charaChipDrawType)
                   && IsGraphicTileChip == other.IsGraphicTileChip;
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
            result.AddRange(CharaChipFilePath.ToWoditorStringBytes());
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(IsGraphicTileChip), IsGraphicTileChip);
            info.AddValue(nameof(graphicTileId), graphicTileId);
            info.AddValue(nameof(charaChipFilePath), charaChipFilePath);
            info.AddValue(nameof(initDirection), initDirection.Code);
            info.AddValue(nameof(InitAnimationId), InitAnimationId);
            info.AddValue(nameof(CharaChipOpacity), CharaChipOpacity);
            info.AddValue(nameof(charaChipDrawType), charaChipDrawType.Code);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MapEventPageGraphicInfo(SerializationInfo info, StreamingContext context)
        {
            IsGraphicTileChip = info.GetBoolean(nameof(IsGraphicTileChip));
            graphicTileId = info.GetInt32(nameof(graphicTileId));
            charaChipFilePath = info.GetValue<CharaChipFilePath>(nameof(charaChipFilePath));
            initDirection = CharaChipDirection.FromByte(info.GetByte(nameof(initDirection)));
            InitAnimationId = info.GetByte(nameof(InitAnimationId));
            CharaChipOpacity = info.GetByte(nameof(CharaChipOpacity));
            charaChipDrawType = PictureDrawType.FromByte(info.GetByte(nameof(charaChipDrawType)));
        }
    }
}