// ========================================
// Project Name : WodiLib
// File Name    : DeviceInputControlType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// キー入力・入力制御タイプ
    /// </summary>
    public class DeviceInputControlType : TypeSafeEnum<DeviceInputControlType>
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

        static DeviceInputControlType()
        {
            KeyboardKey = new DeviceInputControlType(nameof(KeyboardKey), 0x00);
            AllMouseInput = new DeviceInputControlType(nameof(AllMouseInput), 0x01);
            AllPadInput = new DeviceInputControlType(nameof(AllPadInput), 0x02);
            AllDevices = new DeviceInputControlType(nameof(AllDevices), 0x03);
        }

        private DeviceInputControlType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DeviceInputControlType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}