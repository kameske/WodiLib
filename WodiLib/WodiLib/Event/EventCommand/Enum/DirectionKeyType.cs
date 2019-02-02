// ========================================
// Project Name : WodiLib
// File Name    : DirectionKeyType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// キー入力（基本）方向キー種別
    /// </summary>
    public class DirectionKeyType : TypeSafeEnum<DirectionKeyType>
    {
        /// <summary>受け付けない</summary>
        public static readonly DirectionKeyType NotAccept;

        /// <summary>4方向</summary>
        public static readonly DirectionKeyType FourDirections;

        /// <summary>8方向</summary>
        public static readonly DirectionKeyType EightDirections;

        /// <summary>上下</summary>
        public static readonly DirectionKeyType UpDown;

        /// <summary>左右</summary>
        public static readonly DirectionKeyType LeftRight;

        /// <summary>上のみ</summary>
        public static readonly DirectionKeyType Up;

        /// <summary>下のみ</summary>
        public static readonly DirectionKeyType Down;

        /// <summary>左のみ</summary>
        public static readonly DirectionKeyType Left;

        /// <summary>右のみ</summary>
        public static readonly DirectionKeyType Right;

        /// <summary>値</summary>
        public byte Code { get; }

        static DirectionKeyType()
        {
            NotAccept = new DirectionKeyType(nameof(NotAccept), 0x00);
            FourDirections = new DirectionKeyType(nameof(FourDirections), 0x01);
            EightDirections = new DirectionKeyType(nameof(EightDirections), 0x02);
            Up = new DirectionKeyType(nameof(Up), 0x03);
            Down = new DirectionKeyType(nameof(Down), 0x04);
            Left = new DirectionKeyType(nameof(Left), 0x05);
            Right = new DirectionKeyType(nameof(Right), 0x06);
            UpDown = new DirectionKeyType(nameof(UpDown), 0x07);
            LeftRight = new DirectionKeyType(nameof(LeftRight), 0x08);
        }

        private DirectionKeyType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DirectionKeyType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}