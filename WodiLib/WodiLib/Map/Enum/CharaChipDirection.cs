// ========================================
// Project Name : WodiLib
// File Name    : CharaChipDirection.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Commons;

namespace WodiLib.Map
{
    /// <summary>
    /// キャラチップ向き
    /// </summary>
    public class CharaChipDirection : TypeSafeEnum<CharaChipDirection>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>上方向</summary>
        public static readonly CharaChipDirection Up;

        /// <summary>下方向</summary>
        public static readonly CharaChipDirection Down;

        /// <summary>左方向</summary>
        public static readonly CharaChipDirection Left;

        /// <summary>右方向</summary>
        public static readonly CharaChipDirection Right;

        /// <summary>左上方向</summary>
        public static readonly CharaChipDirection LeftUp;

        /// <summary>左下方向</summary>
        public static readonly CharaChipDirection LeftDown;

        /// <summary>右上方向</summary>
        public static readonly CharaChipDirection RightUp;

        /// <summary>右下方向</summary>
        public static readonly CharaChipDirection RightDown;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>コード値</summary>
        public byte Code { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コード値からインスタンスを返す。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static CharaChipDirection FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static CharaChipDirection()
        {
            Up = new CharaChipDirection(nameof(Up), 0x08);
            Down = new CharaChipDirection(nameof(Down), 0x02);
            Left = new CharaChipDirection(nameof(Left), 0x04);
            Right = new CharaChipDirection(nameof(Right), 0x06);
            LeftUp = new CharaChipDirection(nameof(LeftUp), 0x07);
            LeftDown = new CharaChipDirection(nameof(LeftDown), 0x01);
            RightUp = new CharaChipDirection(nameof(RightUp), 0x09);
            RightDown = new CharaChipDirection(nameof(RightDown), 0x03);
        }

        private CharaChipDirection(string id, byte code) : base(id)
        {
            Code = code;
        }
    }
}