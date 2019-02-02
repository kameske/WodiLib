// ========================================
// Project Name : WodiLib
// File Name    : ScrollScreenType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 画面スクロール種別
    /// </summary>
    public class ScrollScreenType : TypeSafeEnum<ScrollScreenType>
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

        static ScrollScreenType()
        {
            MoveScreen = new ScrollScreenType(nameof(MoveScreen), 0x00);
            BackToHero = new ScrollScreenType(nameof(BackToHero), 0x01);
            LockScroll = new ScrollScreenType(nameof(LockScroll), 0x02);
            UnlockScroll = new ScrollScreenType(nameof(UnlockScroll), 0x03);
        }

        private ScrollScreenType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static ScrollScreenType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}