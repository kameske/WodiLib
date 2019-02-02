// ========================================
// Project Name : WodiLib
// File Name    : EventBootType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     イベント起動条件
    /// </summary>
    public class EventBootType : TypeSafeEnum<EventBootType>
    {
        /// <summary>決定キー</summary>
        public static readonly EventBootType PushOKKey;

        /// <summary>自動実行</summary>
        public static readonly EventBootType Auto;

        /// <summary>並列実行</summary>
        public static readonly EventBootType Parallel;

        /// <summary>プレイヤー接触</summary>
        public static readonly EventBootType HitPlayer;

        /// <summary>イベント接触</summary>
        public static readonly EventBootType HitEvent;

        static EventBootType()
        {
            PushOKKey = new EventBootType("PushOKKey", 0x00);
            Auto = new EventBootType("Auto", 0x01);
            Parallel = new EventBootType("Parallel", 0x02);
            HitPlayer = new EventBootType("HitPlayer", 0x03);
            HitEvent = new EventBootType("HitEvent", 0x04);
        }

        private EventBootType(string id) : base(id)
        {
        }

        private EventBootType(string id, byte code) : this(id)
        {
            Code = code;
        }

        /// <summary>起動条件コード</summary>
        public byte Code { get; }

        /// <summary>
        /// コード値からインスタンスを返す。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static EventBootType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}