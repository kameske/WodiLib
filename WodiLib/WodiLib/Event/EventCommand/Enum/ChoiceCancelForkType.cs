// ========================================
// Project Name : WodiLib
// File Name    : ChoiceCancelForkType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Commons;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 選択肢キャンセル分岐先
    /// </summary>
    public class ChoiceCancelForkType : TypeSafeEnum<ChoiceCancelForkType>
    {
        /// <summary>選択肢1</summary>
        public static readonly ChoiceCancelForkType Case1;

        /// <summary>選択肢2</summary>
        public static readonly ChoiceCancelForkType Case2;

        /// <summary>選択肢3</summary>
        public static readonly ChoiceCancelForkType Case3;

        /// <summary>選択肢4</summary>
        public static readonly ChoiceCancelForkType Case4;

        /// <summary>選択肢5</summary>
        public static readonly ChoiceCancelForkType Case5;

        /// <summary>選択肢6</summary>
        public static readonly ChoiceCancelForkType Case6;

        /// <summary>選択肢7</summary>
        public static readonly ChoiceCancelForkType Case7;

        /// <summary>選択肢8</summary>
        public static readonly ChoiceCancelForkType Case8;

        /// <summary>選択肢9</summary>
        public static readonly ChoiceCancelForkType Case9;

        /// <summary>選択肢10</summary>
        public static readonly ChoiceCancelForkType Case10;

        /// <summary>別分岐</summary>
        public static readonly ChoiceCancelForkType Else;

        /// <summary>キャンセル不能</summary>
        public static readonly ChoiceCancelForkType Cannot;

        static ChoiceCancelForkType()
        {
            Case1 = new ChoiceCancelForkType(nameof(Case1), 0x20);
            Case2 = new ChoiceCancelForkType(nameof(Case2), 0x30);
            Case3 = new ChoiceCancelForkType(nameof(Case3), 0x40);
            Case4 = new ChoiceCancelForkType(nameof(Case4), 0x50);
            Case5 = new ChoiceCancelForkType(nameof(Case5), 0x60);
            Case6 = new ChoiceCancelForkType(nameof(Case6), 0x70);
            Case7 = new ChoiceCancelForkType(nameof(Case7), 0x80);
            Case8 = new ChoiceCancelForkType(nameof(Case8), 0x90);
            Case9 = new ChoiceCancelForkType(nameof(Case9), 0xA0);
            Case10 = new ChoiceCancelForkType(nameof(Case10), 0xB0);
            Else = new ChoiceCancelForkType(nameof(Else), 0x00);
            Cannot = new ChoiceCancelForkType(nameof(Cannot), 0x10);
        }

        /// <summary>値</summary>
        public byte Code { get; }

        private ChoiceCancelForkType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static ChoiceCancelForkType ForByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}