// ========================================
// Project Name : WodiLib
// File Name    : MoveFrequency.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     移動頻度
    /// </summary>
    public class MoveFrequency : TypeSafeEnum<MoveFrequency>
    {
        /// <summary>0:毎フレーム</summary>
        public static readonly MoveFrequency Frame;

        /// <summary>1:超短間隔</summary>
        public static readonly MoveFrequency Shortest;

        /// <summary>2:短間隔</summary>
        public static readonly MoveFrequency Short;

        /// <summary>3:中間隔く</summary>
        public static readonly MoveFrequency Middle;

        /// <summary>4:大間隔</summary>
        public static readonly MoveFrequency Long;

        /// <summary>5:超大間隔</summary>
        public static readonly MoveFrequency Longer;

        /// <summary>6:頻度遅</summary>
        public static readonly MoveFrequency Longest;

        /// <summary>値</summary>
        public byte Code { get; }

        static MoveFrequency()
        {
            Frame = new MoveFrequency("Frame", 0x00);
            Shortest = new MoveFrequency("Shortest", 0x01);
            Short = new MoveFrequency("Short", 0x02);
            Middle = new MoveFrequency("Middle", 0x03);
            Long = new MoveFrequency("Long", 0x04);
            Longer = new MoveFrequency("Longer", 0x05);
            Longest = new MoveFrequency("Longest", 0x06);
        }

        private MoveFrequency(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static MoveFrequency FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
