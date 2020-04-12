// ========================================
// Project Name : WodiLib
// File Name    : DirectionKeyType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using Commons;

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

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static DirectionKeyType()
        {
            NotAccept = new DirectionKeyType(nameof(NotAccept), 0x00,
                "");
            FourDirections = new DirectionKeyType(nameof(FourDirections), 0x01,
                "4方向");
            EightDirections = new DirectionKeyType(nameof(EightDirections), 0x02,
                "8方向");
            Up = new DirectionKeyType(nameof(Up), 0x03,
                "上(8)のみ");
            Down = new DirectionKeyType(nameof(Down), 0x04,
                "下(2)のみ");
            Left = new DirectionKeyType(nameof(Left), 0x05,
                "左(4)のみ");
            Right = new DirectionKeyType(nameof(Right), 0x06,
                "右(6)のみ");
            UpDown = new DirectionKeyType(nameof(UpDown), 0x07,
                "上下(8,2)");
            LeftRight = new DirectionKeyType(nameof(LeftRight), 0x08,
                "左右(4,6)");
        }

        private DirectionKeyType(string id, byte code,
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
        public static DirectionKeyType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}