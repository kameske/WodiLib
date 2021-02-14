// ========================================
// Project Name : WodiLib
// File Name    : ScrollScreenType.cs
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
    ///     画面スクロール種別
    /// </summary>
    public record ScrollScreenType : TypeSafeEnum<ScrollScreenType>
    {
        /// <summary>画面移動</summary>
        public static readonly ScrollScreenType MoveScreen;

        /// <summary>主人公に戻す</summary>
        public static readonly ScrollScreenType BackToHero;

        /// <summary>スクロールロック</summary>
        public static readonly ScrollScreenType LockScroll;

        /// <summary>スクロールロック解除</summary>
        public static readonly ScrollScreenType UnlockScroll;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        /// <summary>スクロール固定タイプフラグ</summary>
        public bool IsLockType { get; }

        /// <summary>スクロール移動タイプフラグ</summary>
        public bool IsMoveType { get; }

        static ScrollScreenType()
        {
            MoveScreen = new ScrollScreenType(nameof(MoveScreen), 0x00,
                "画面移動", false, true);
            BackToHero = new ScrollScreenType(nameof(BackToHero), 0x01,
                "主人公に戻す", false, false);
            LockScroll = new ScrollScreenType(nameof(LockScroll), 0x02,
                "スクロールロック", true, false);
            UnlockScroll = new ScrollScreenType(nameof(UnlockScroll), 0x03,
                "スクロールロック解除", true, false);
        }

        private ScrollScreenType(string id, byte code,
            string eventCommandSentence, bool isLockType, bool isMoveType) : base(id)
        {
            Code = code;
            IsLockType = isLockType;
            EventCommandSentence = eventCommandSentence;
            IsMoveType = isMoveType;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static ScrollScreenType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
