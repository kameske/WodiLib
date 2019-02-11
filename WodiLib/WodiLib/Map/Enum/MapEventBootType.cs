// ========================================
// Project Name : WodiLib
// File Name    : MapEventBootType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント起動条件
    /// </summary>
    public class MapEventBootType : TypeSafeEnum<MapEventBootType>
    {
        /// <summary>決定キー</summary>
        public static readonly MapEventBootType PushOKKey;

        /// <summary>自動実行</summary>
        public static readonly MapEventBootType Auto;

        /// <summary>並列実行</summary>
        public static readonly MapEventBootType Parallel;

        /// <summary>プレイヤー接触</summary>
        public static readonly MapEventBootType HitPlayer;

        /// <summary>イベント接触</summary>
        public static readonly MapEventBootType HitMapEvent;

        static MapEventBootType()
        {
            PushOKKey = new MapEventBootType("PushOKKey", 0x00);
            Auto = new MapEventBootType("Auto", 0x01);
            Parallel = new MapEventBootType("Parallel", 0x02);
            HitPlayer = new MapEventBootType("HitPlayer", 0x03);
            HitMapEvent = new MapEventBootType("HitMapEvent", 0x04);
        }

        private MapEventBootType(string id) : base(id)
        {
        }

        private MapEventBootType(string id, byte code) : this(id)
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
        public static MapEventBootType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}