// ========================================
// Project Name : WodiLib
// File Name    : MapTreeOpenState.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップツリー開閉状態
    /// </summary>
    public record MapTreeOpenState : TypeSafeEnum<MapTreeOpenState>
    {
        /// <summary>開いている</summary>
        public static readonly MapTreeOpenState Open;

        /// <summary>閉じている・または子ノードなし</summary>
        public static readonly MapTreeOpenState Close;

        /// <summary>開閉状態コード値</summary>
        public byte Code { get; }

        static MapTreeOpenState()
        {
            Open = new MapTreeOpenState(nameof(Open), 0x01);
            Close = new MapTreeOpenState(nameof(Close), 0x00);
        }

        private MapTreeOpenState(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     コード値からインスタンスを返す。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static MapTreeOpenState FromCode(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
