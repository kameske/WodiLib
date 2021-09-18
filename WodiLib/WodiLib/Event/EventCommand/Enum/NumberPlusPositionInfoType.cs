// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusPositionInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     変数操作＋（座標）・取得情報種別
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

        static NumberPlusPositionInfoType()
        {
            EventId = new NumberPlusPositionInfoType(nameof(EventId), 0x01,
                "ｲﾍﾞﾝﾄID");
            PassableTile = new NumberPlusPositionInfoType(nameof(PassableTile), 0x02,
                "通行判定:タイル(○=0,×=1)");
            PassableTileAndEvent = new NumberPlusPositionInfoType(nameof(PassableTileAndEvent), 0x03,
                "通行判定:タイル＆イベント(○=0,×=1)");
            UppermostChipNumber = new NumberPlusPositionInfoType(nameof(UppermostChipNumber), 0x04,
                "最も上にあるチップ番号(1-15:ｵｰﾄ,16-:etc…)");
            Layer1ChipNumber = new NumberPlusPositionInfoType(nameof(Layer1ChipNumber), 0x05,
                "レイヤー1のチップ番号(0:透明,1-15:ｵｰﾄ,16-:etc)");
            Layer2ChipNumber = new NumberPlusPositionInfoType(nameof(Layer2ChipNumber), 0x06,
                "レイヤー2のチップ番号(0:透明,1-15:ｵｰﾄ,16-:etc)");
            Layer3ChipNumber = new NumberPlusPositionInfoType(nameof(Layer3ChipNumber), 0x07,
                "レイヤー3のチップ番号(0:透明,1-15:ｵｰﾄ,16-:etc)");
            UppermostTileTag = new NumberPlusPositionInfoType(nameof(UppermostTileTag), 0x00,
                "最も上にあるﾀｲﾙのﾀｸﾞ番号");
            Layer1TileTag = new NumberPlusPositionInfoType(nameof(Layer1TileTag), 0x08,
                "レイヤー1のタグ番号");
            Layer2TileTag = new NumberPlusPositionInfoType(nameof(Layer2TileTag), 0x09,
                "レイヤー2のタグ番号");
            Layer3TileTag = new NumberPlusPositionInfoType(nameof(Layer3TileTag), 0x0A,
                "レイヤー3のタグ番号");
        }

        private NumberPlusPositionInfoType(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusPositionInfoType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
