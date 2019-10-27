// ========================================
// Project Name : WodiLib
// File Name    : PartyGraphicSpecialType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// パーティ画像（特殊処理）種類
    /// </summary>
    public class PartyGraphicSpecialType : TypeSafeEnum<PartyGraphicSpecialType>
    {
        /// <summary>キャラクターを前に詰める</summary>
        public static readonly PartyGraphicSpecialType PushCharactersFront;

        /// <summary>キャラクター画像を全消去する</summary>
        public static readonly PartyGraphicSpecialType EraseAllCharacters;

        /// <summary>仲間全員を主人公の位置にワープ</summary>
        public static readonly PartyGraphicSpecialType WrapPartyToHero;

        /// <summary>仲間と主人公の動きのシンクロ開始</summary>
        public static readonly PartyGraphicSpecialType StartHeroPartySynchro;

        /// <summary>仲間と主人公の動きのシンクロ解除</summary>
        public static readonly PartyGraphicSpecialType CancelHeroPartySynchro;

        /// <summary>パーティ全員を透明にする</summary>
        public static readonly PartyGraphicSpecialType MakePartyTransparent;

        /// <summary>パーティ全員の透明を解除する</summary>
        public static readonly PartyGraphicSpecialType CancelPartyTransparent;

        /// <summary>パーティ全員の画像を記憶</summary>
        public static readonly PartyGraphicSpecialType SavePartyMembers;

        /// <summary>記憶したパーティ全員の画像をロード</summary>
        public static readonly PartyGraphicSpecialType LoadPartyMembers;

        /// <summary>パーティの隊列をオンにする</summary>
        public static readonly PartyGraphicSpecialType TurnPartyFollowingOn;

        /// <summary>パーティの隊列を解除する</summary>
        public static readonly PartyGraphicSpecialType TurnPartyFollowingOff;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static PartyGraphicSpecialType()
        {
            PushCharactersFront = new PartyGraphicSpecialType(nameof(PushCharactersFront),
                0x00, "ｷｬﾗ画像の隊列を前に詰める");
            EraseAllCharacters = new PartyGraphicSpecialType(nameof(EraseAllCharacters),
                0x10, "パーティ全員の画像を消去");
            WrapPartyToHero = new PartyGraphicSpecialType(nameof(WrapPartyToHero),
                0x20, "仲間全員を主人公の位置にワープ");
            StartHeroPartySynchro = new PartyGraphicSpecialType(nameof(StartHeroPartySynchro),
                0x30, "仲間と主人公の動きをシンクロ開始");
            CancelHeroPartySynchro = new PartyGraphicSpecialType(nameof(CancelHeroPartySynchro),
                0x40, "仲間と主人公のシンクロ解除");
            MakePartyTransparent = new PartyGraphicSpecialType(nameof(MakePartyTransparent),
                0x50, "パーティ全員を透明にする");
            CancelPartyTransparent = new PartyGraphicSpecialType(nameof(CancelPartyTransparent),
                0x60, "パーティ全員の透明を解除する");
            SavePartyMembers = new PartyGraphicSpecialType(nameof(SavePartyMembers),
                0x70, "パーティ全員の画像を記憶する");
            LoadPartyMembers = new PartyGraphicSpecialType(nameof(LoadPartyMembers),
                0x80, "記憶したパーティ全員の画像をロード");
            TurnPartyFollowingOn = new PartyGraphicSpecialType(nameof(TurnPartyFollowingOn),
                0x90, "パーティの隊列をオンにする");
            TurnPartyFollowingOff = new PartyGraphicSpecialType(nameof(TurnPartyFollowingOff),
                0xA0, "パーティの隊列を解除する");
        }

        private PartyGraphicSpecialType(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static PartyGraphicSpecialType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}