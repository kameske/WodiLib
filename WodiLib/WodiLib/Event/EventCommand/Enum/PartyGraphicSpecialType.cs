// ========================================
// Project Name : WodiLib
// File Name    : PartyGraphicSpecialType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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

        static PartyGraphicSpecialType()
        {
            PushCharactersFront = new PartyGraphicSpecialType(nameof(PushCharactersFront), 0x00);
            EraseAllCharacters = new PartyGraphicSpecialType(nameof(EraseAllCharacters), 0x10);
            WrapPartyToHero = new PartyGraphicSpecialType(nameof(WrapPartyToHero), 0x20);
            StartHeroPartySynchro = new PartyGraphicSpecialType(nameof(StartHeroPartySynchro), 0x30);
            CancelHeroPartySynchro = new PartyGraphicSpecialType(nameof(CancelHeroPartySynchro), 0x40);
            MakePartyTransparent = new PartyGraphicSpecialType(nameof(MakePartyTransparent), 0x50);
            CancelPartyTransparent = new PartyGraphicSpecialType(nameof(CancelPartyTransparent), 0x60);
            SavePartyMembers = new PartyGraphicSpecialType(nameof(SavePartyMembers), 0x70);
            LoadPartyMembers = new PartyGraphicSpecialType(nameof(LoadPartyMembers), 0x80);
            TurnPartyFollowingOn = new PartyGraphicSpecialType(nameof(TurnPartyFollowingOn), 0x90);
            TurnPartyFollowingOff = new PartyGraphicSpecialType(nameof(TurnPartyFollowingOff), 0xA0);
        }

        private PartyGraphicSpecialType(string id, byte code) : base(id)
        {
            Code = code;
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