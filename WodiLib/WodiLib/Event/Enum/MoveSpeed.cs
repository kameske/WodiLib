// ========================================
// Project Name : WodiLib
// File Name    : MoveSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    /// 移動速度
    /// </summary>
    public class MoveSpeed : TypeSafeEnum<MoveSpeed>
    {
        /// <summary>6:最遅</summary>
        public static readonly MoveSpeed Slowest;

        /// <summary>5:遅い</summary>
        public static readonly MoveSpeed Slower;

        /// <summary>4:少し遅い</summary>
        public static readonly MoveSpeed Slow;

        /// <summary>3:標準</summary>
        public static readonly MoveSpeed Normal;

        /// <summary>2:少し速い</summary>
        public static readonly MoveSpeed Fast;

        /// <summary>1:速い</summary>
        public static readonly MoveSpeed Faster;

        /// <summary>0:最速</summary>
        public static readonly MoveSpeed Fastest;

        /// <summary>値</summary>
        public byte Code { get; }

        static MoveSpeed()
        {
            Slowest = new MoveSpeed("Slowest", 0x06);
            Slower = new MoveSpeed("Slower", 0x05);
            Slow = new MoveSpeed("Slow", 0x04);
            Normal = new MoveSpeed("Normal", 0x03);
            Fast = new MoveSpeed("Fast", 0x02);
            Faster = new MoveSpeed("Faster", 0x01);
            Fastest = new MoveSpeed("Fastest", 0x00);
        }

        private MoveSpeed(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static MoveSpeed FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}