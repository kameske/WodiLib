// ========================================
// Project Name : WodiLib
// File Name    : DBStringAssignmentOperator.cs
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
    /// DB文字列代入演算子
    /// </summary>
    public class DBStringAssignmentOperator : TypeSafeEnum<DBStringAssignmentOperator>
    {
        /// <summary>=</summary>
        public static readonly DBStringAssignmentOperator Assign;

        /// <summary>+=</summary>
        public static readonly DBStringAssignmentOperator Addition;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static DBStringAssignmentOperator()
        {
            Assign = new DBStringAssignmentOperator(nameof(Assign), 0x02,
                "=");
            Addition = new DBStringAssignmentOperator(nameof(Addition), 0x12,
                "+=");
        }

        private DBStringAssignmentOperator(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DBStringAssignmentOperator FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
