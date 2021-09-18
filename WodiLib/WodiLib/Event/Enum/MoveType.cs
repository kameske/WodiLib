// ========================================
// Project Name : WodiLib
// File Name    : MoveType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     移動タイプ
    /// </summary>
    public class MoveType : TypeSafeEnum<MoveType>
    {
        /// <summary>動かない</summary>
        public static readonly MoveType Not;

        /// <summary>カスタム</summary>
        public static readonly MoveType Custom;

        /// <summary>ランダム</summary>
        public static readonly MoveType Random;

        /// <summary>プレイヤー接近</summary>
        public static readonly MoveType Nearer;

        /// <summary>値</summary>
        public byte Code { get; }

        static MoveType()
        {
            Not = new MoveType("Not", 0x00);
            Custom = new MoveType("Custom", 0x01);
            Random = new MoveType("Random", 0x02);
            Nearer = new MoveType("Nearer", 0x03);
        }

        private MoveType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static MoveType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
