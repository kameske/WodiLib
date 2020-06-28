// ========================================
// Project Name : WodiLib
// File Name    : MoveSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    /// 移動速度
    /// </summary>
    public class MoveSpeed : TypeSafeEnum<MoveSpeed>
    {
        /// <summary>0:最遅</summary>
        public static readonly MoveSpeed Slowest;

        /// <summary>1:遅い</summary>
        public static readonly MoveSpeed Slower;

        /// <summary>2:少し遅い</summary>
        public static readonly MoveSpeed Slow;

        /// <summary>3:標準</summary>
        public static readonly MoveSpeed Normal;

        /// <summary>4:少し速い</summary>
        public static readonly MoveSpeed Fast;

        /// <summary>5:速い</summary>
        public static readonly MoveSpeed Faster;

        /// <summary>6:最速</summary>
        public static readonly MoveSpeed Fastest;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>マップイベント移動ルートに対する使用可能フラグ</summary>
        public bool CanSetForMapEventMoveRoute { get; }

        static MoveSpeed()
        {
            Slowest = new MoveSpeed("Slowest", 0x00, true);
            Slower = new MoveSpeed("Slower", 0x01, true);
            Slow = new MoveSpeed("Slow", 0x02, true);
            Normal = new MoveSpeed("Normal", 0x03, true);
            Fast = new MoveSpeed("Fast", 0x04, true);
            Faster = new MoveSpeed("Faster", 0x05, true);
            Fastest = new MoveSpeed("Fastest", 0x06, false);
        }

        private MoveSpeed(string id, byte code, bool canSetForMapEventMoveRoute) : base(id)
        {
            Code = code;
            CanSetForMapEventMoveRoute = canSetForMapEventMoveRoute;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static MoveSpeed FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
