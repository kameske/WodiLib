// ========================================
// Project Name : WodiLib
// File Name    : DeviceKeyInputControlType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// キー入力コントロール
    /// </summary>
    public class DeviceKeyInputControlType : TypeSafeEnum<DeviceKeyInputControlType>
    {
        /// <summary>許可する</summary>
        public static readonly DeviceKeyInputControlType Allow;

        /// <summary>禁止する</summary>
        public static readonly DeviceKeyInputControlType Deny;

        /// <summary>値</summary>
        public byte Code { get; }

        static DeviceKeyInputControlType()
        {
            Allow = new DeviceKeyInputControlType(nameof(Allow), 0x00);
            Deny = new DeviceKeyInputControlType(nameof(Deny), 0x01);
        }

        private DeviceKeyInputControlType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DeviceKeyInputControlType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}