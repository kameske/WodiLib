// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusAssignmentOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 変数操作＋ 代入演算子
    /// </summary>
    public class NumberPlusAssignmentOperator : TypeSafeEnum<NumberPlusAssignmentOperator>
    {
        /// <summary>=</summary>
        public static readonly NumberPlusAssignmentOperator Assign;

        /// <summary>+=</summary>
        public static readonly NumberPlusAssignmentOperator Addition;

        /// <summary>-=</summary>
        public static readonly NumberPlusAssignmentOperator Subtraction;

        /// <summary>*=</summary>
        public static readonly NumberPlusAssignmentOperator Multiplication;

        /// <summary>/=</summary>
        public static readonly NumberPlusAssignmentOperator Division;

        /// <summary>%=</summary>
        public static readonly NumberPlusAssignmentOperator Modulo;

        /// <summary>引き上げ</summary>
        public static readonly NumberPlusAssignmentOperator LowerBound;

        /// <summary>引き下げ</summary>
        public static readonly NumberPlusAssignmentOperator UpperBound;

        /// <summary>絶対値</summary>
        public static readonly NumberPlusAssignmentOperator Absolute;

        /// <summary>値</summary>
        public byte Code { get; }

        static NumberPlusAssignmentOperator()
        {
            Assign = new NumberPlusAssignmentOperator(nameof(Assign), 0x00);
            Addition = new NumberPlusAssignmentOperator(nameof(Addition), 0x01);
            Subtraction = new NumberPlusAssignmentOperator(nameof(Subtraction), 0x02);
            Multiplication = new NumberPlusAssignmentOperator(nameof(Multiplication), 0x03);
            Division = new NumberPlusAssignmentOperator(nameof(Division), 0x04);
            Modulo = new NumberPlusAssignmentOperator(nameof(Modulo), 0x05);
            LowerBound = new NumberPlusAssignmentOperator(nameof(LowerBound), 0x06);
            UpperBound = new NumberPlusAssignmentOperator(nameof(UpperBound), 0x07);
            Absolute = new NumberPlusAssignmentOperator(nameof(Absolute), 0x08);
        }

        private NumberPlusAssignmentOperator(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusAssignmentOperator FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}