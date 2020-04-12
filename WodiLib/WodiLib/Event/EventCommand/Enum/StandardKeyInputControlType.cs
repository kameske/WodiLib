// ========================================
// Project Name : WodiLib
// File Name    : StandardKeyInputControlType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Commons;

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

        /// <summary>イベントコマンド文字列</summary>
        internal string EventCommandSentence { get; }

        static StandardKeyInputControlType()
        {
            OkMoveOkInput = new StandardKeyInputControlType(nameof(OkMoveOkInput),
                0x00, "[ 移動時○  キー入力○ ]");
            NgMoveOkInput = new StandardKeyInputControlType(nameof(NgMoveOkInput),
                0x01, "[ 移動時×  キー入力○ ]");
            NgMoveNgInput = new StandardKeyInputControlType(nameof(NgMoveNgInput),
                0x02, "[ 移動時×  キー入力× ]");
        }

        private StandardKeyInputControlType(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="src">バイト値</param>
        /// <returns>インスタンス</returns>
        public static StandardKeyInputControlType ForByte(byte src)
        {
            return AllItems.First(x => x.Code == src);
        }
    }
}