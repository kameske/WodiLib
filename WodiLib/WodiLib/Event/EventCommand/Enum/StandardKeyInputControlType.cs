// ========================================
// Project Name : WodiLib
// File Name    : StandardKeyInputControlType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 基本キー入力制御タイプ
    /// </summary>
    public class StandardKeyInputControlType : TypeSafeEnum<StandardKeyInputControlType>
    {
        /// <summary>移動○ / キー入力○</summary>
        public static readonly StandardKeyInputControlType OkMoveOkInput;

        /// <summary>移動× / キー入力○</summary>
        public static readonly StandardKeyInputControlType NgMoveOkInput;

        /// <summary>移動× / キー入力×</summary>
        public static readonly StandardKeyInputControlType NgMoveNgInput;

        /// <summary>値</summary>
        public byte Code { get; }

        static StandardKeyInputControlType()
        {
            OkMoveOkInput = new StandardKeyInputControlType(nameof(OkMoveOkInput), 0x00);
            NgMoveOkInput = new StandardKeyInputControlType(nameof(NgMoveOkInput), 0x01);
            NgMoveNgInput = new StandardKeyInputControlType(nameof(NgMoveNgInput), 0x02);
        }

        private StandardKeyInputControlType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="src">バイト値</param>
        /// <returns>インスタンス</returns>
        public static StandardKeyInputControlType ForByte(byte src)
        {
            return _FindFirst(x => x.Code == src);
        }
    }
}