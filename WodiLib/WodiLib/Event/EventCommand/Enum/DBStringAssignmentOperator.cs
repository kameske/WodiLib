// ========================================
// Project Name : WodiLib
// File Name    : DBStringAssignmentOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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

        static DBStringAssignmentOperator()
        {
            Assign = new DBStringAssignmentOperator(nameof(Assign), 0x02);
            Addition = new DBStringAssignmentOperator(nameof(Addition), 0x03);
        }

        private DBStringAssignmentOperator(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DBStringAssignmentOperator FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}