// ========================================
// Project Name : WodiLib
// File Name    : DeviceInputControlType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     キー入力・入力制御タイプ
    /// </summary>
    public record DeviceInputControlType : TypeSafeEnum<DeviceInputControlType>
    {
        /// <summary>キーボード指定キー</summary>
        public static readonly DeviceInputControlType KeyboardKey;

        /// <summary>マウス全部</summary>
        public static readonly DeviceInputControlType AllMouseInput;

        /// <summary>パッド全部</summary>
        public static readonly DeviceInputControlType AllPadInput;

        /// <summary>すべてのデバイス</summary>
        public static readonly DeviceInputControlType AllDevices;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static DeviceInputControlType()
        {
            KeyboardKey = new DeviceInputControlType(nameof(KeyboardKey), 0x00,
                "キーボード入力");
            AllMouseInput = new DeviceInputControlType(nameof(AllMouseInput), 0x01,
                "マウス入力全て");
            AllPadInput = new DeviceInputControlType(nameof(AllPadInput), 0x02,
                "パッド入力全て");
            AllDevices = new DeviceInputControlType(nameof(AllDevices), 0x03,
                "全てのデバイス");
        }

        private DeviceInputControlType(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DeviceInputControlType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
