// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusPositionInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 変数操作＋（座標）・取得情報種別
    /// </summary>
    public class NumberPlusPositionInfoType : TypeSafeEnum<NumberPlusPositionInfoType>
    {
        /// <summary>イベントID</summary>
        public static readonly NumberPlusPositionInfoType EventId;

        /// <summary>通行可能（タイルのみ）</summary>
        public static readonly NumberPlusPositionInfoType PassableTile;

        /// <summary>通行可能（タイル、Ev）</summary>
        public static readonly NumberPlusPositionInfoType PassableTileAndEvent;

        /// <summary>最も上にあるタイル番号</summary>
        public static readonly NumberPlusPositionInfoType UppermostChipNumber;

        /// <summary>レイヤー1のチップ番号</summary>
        public static readonly NumberPlusPositionInfoType Layer1ChipNumber;

        /// <summary>レイヤー2のチップ番号</summary>
        public static readonly NumberPlusPositionInfoType Layer2ChipNumber;

        /// <summary>レイヤー3のチップ番号</summary>
        public static readonly NumberPlusPositionInfoType Layer3ChipNumber;

        /// <summary>最も上にあるタイルのタグ番号</summary>
        public static readonly NumberPlusPositionInfoType UppermostTileTag;

        /// <summary>レイヤー1のタグ番号</summary>
        public static readonly NumberPlusPositionInfoType Layer1TileTag;

        /// <summary>レイヤー2のタグ番号</summary>
        public static readonly NumberPlusPositionInfoType Layer2TileTag;

        /// <summary>レイヤー3のタグ番号</summary>
        public static readonly NumberPlusPositionInfoType Layer3TileTag;

        /// <summary>値</summary>
        public byte Code { get; }

        static NumberPlusPositionInfoType()
        {
            EventId = new NumberPlusPositionInfoType(nameof(EventId), 0x01);
            PassableTile = new NumberPlusPositionInfoType(nameof(PassableTile), 0x02);
            PassableTileAndEvent = new NumberPlusPositionInfoType(nameof(PassableTileAndEvent), 0x03);
            UppermostChipNumber = new NumberPlusPositionInfoType(nameof(UppermostChipNumber), 0x04);
            Layer1ChipNumber = new NumberPlusPositionInfoType(nameof(Layer1ChipNumber), 0x05);
            Layer2ChipNumber = new NumberPlusPositionInfoType(nameof(Layer2ChipNumber), 0x06);
            Layer3ChipNumber = new NumberPlusPositionInfoType(nameof(Layer3ChipNumber), 0x07);
            UppermostTileTag = new NumberPlusPositionInfoType(nameof(UppermostTileTag), 0x00);
            Layer1TileTag = new NumberPlusPositionInfoType(nameof(Layer1TileTag), 0x08);
            Layer2TileTag = new NumberPlusPositionInfoType(nameof(Layer2TileTag), 0x09);
            Layer3TileTag = new NumberPlusPositionInfoType(nameof(Layer3TileTag), 0x0A);
        }

        private NumberPlusPositionInfoType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusPositionInfoType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}