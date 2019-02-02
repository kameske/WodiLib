// ========================================
// Project Name : WodiLib
// File Name    : PictureAnchorPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// ピクチャ表示位置
    /// </summary>
    public class PictureAnchorPosition : TypeSafeEnum<PictureAnchorPosition>
    {
        /// <summary>左上</summary>
        public static readonly PictureAnchorPosition LeftUp;

        /// <summary>中心</summary>
        public static readonly PictureAnchorPosition Center;

        /// <summary>左下</summary>
        public static readonly PictureAnchorPosition LeftDown;

        /// <summary>右上</summary>
        public static readonly PictureAnchorPosition RightUp;

        /// <summary>右下</summary>
        public static readonly PictureAnchorPosition RightDown;

        /// <summary>値</summary>
        public byte Code { get; }

        static PictureAnchorPosition()
        {
            LeftUp = new PictureAnchorPosition(nameof(LeftUp), 0x00);
            Center = new PictureAnchorPosition(nameof(Center), 0x10);
            LeftDown = new PictureAnchorPosition(nameof(LeftDown), 0x20);
            RightUp = new PictureAnchorPosition(nameof(RightUp), 0x30);
            RightDown = new PictureAnchorPosition(nameof(RightDown), 0x40);
        }

        private PictureAnchorPosition(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static PictureAnchorPosition FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}