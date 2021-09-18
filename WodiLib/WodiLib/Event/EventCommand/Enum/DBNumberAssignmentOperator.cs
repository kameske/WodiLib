// ========================================
// Project Name : WodiLib
// File Name    : DBNumberAssignmentOperator.cs
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
    ///     DB操作（数値）代入演算子
    /// </summary>
    public class DBNumberAssignmentOperator : TypeSafeEnum<DBNumberAssignmentOperator>
    {
        /// <summary>=</summary>
        public static readonly DBNumberAssignmentOperator Assign;

        /// <summary>+=</summary>
        public static readonly DBNumberAssignmentOperator Addition;

        /// <summary>-=</summary>
        public static readonly DBNumberAssignmentOperator Subtraction;

        /// <summary>*=</summary>
        public static readonly DBNumberAssignmentOperator Multiplication;

        /// <summary>/=</summary>
        public static readonly DBNumberAssignmentOperator Division;

        /// <summary>%=</summary>
        public static readonly DBNumberAssignmentOperator Modulo;

        /// <summary>引き上げ</summary>
        public static readonly DBNumberAssignmentOperator LowerBound;

        /// <summary>引き下げ</summary>
        public static readonly DBNumberAssignmentOperator UpperBound;

        static DBNumberAssignmentOperator()
        {
            Assign = new DBNumberAssignmentOperator(nameof(Assign), 0x00,
                "=");
            Addition = new DBNumberAssignmentOperator(nameof(Addition), 0x10,
                "+=");
            Subtraction = new DBNumberAssignmentOperator(nameof(Subtraction), 0x20,
                "-=");
            Multiplication = new DBNumberAssignmentOperator(nameof(Multiplication),
                0x30, "*=");
            Division = new DBNumberAssignmentOperator(nameof(Division), 0x40,
                "/=");
            Modulo = new DBNumberAssignmentOperator(nameof(Modulo), 0x50,
                "%=");
            LowerBound = new DBNumberAssignmentOperator(nameof(LowerBound),
                0x60, "下限");
            UpperBound = new DBNumberAssignmentOperator(nameof(UpperBound),
                0x70, "上限");
        }

        private DBNumberAssignmentOperator(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static DBNumberAssignmentOperator FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
