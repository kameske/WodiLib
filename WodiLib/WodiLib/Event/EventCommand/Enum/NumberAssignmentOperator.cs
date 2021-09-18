// ========================================
// Project Name : WodiLib
// File Name    : NumberAssignmentOperator.cs
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
    ///     変数操作・代入演算子
    /// </summary>
    public class NumberAssignmentOperator : TypeSafeEnum<NumberAssignmentOperator>
    {
        /// <summary>=</summary>
        public static readonly NumberAssignmentOperator Assign;

        /// <summary>+=</summary>
        public static readonly NumberAssignmentOperator Addition;

        /// <summary>-=</summary>
        public static readonly NumberAssignmentOperator Subtraction;

        /// <summary>*=</summary>
        public static readonly NumberAssignmentOperator Multiplication;

        /// <summary>/=</summary>
        public static readonly NumberAssignmentOperator Division;

        /// <summary>%=</summary>
        public static readonly NumberAssignmentOperator Modulo;

        /// <summary>引き上げ</summary>
        public static readonly NumberAssignmentOperator LowerBound;

        /// <summary>引き下げ</summary>
        public static readonly NumberAssignmentOperator UpperBound;

        /// <summary>絶対値</summary>
        public static readonly NumberAssignmentOperator Absolute;

        /// <summary>角度</summary>
        public static readonly NumberAssignmentOperator Angle;

        /// <summary>Sin</summary>
        public static readonly NumberAssignmentOperator Sin;

        /// <summary>Cos</summary>
        public static readonly NumberAssignmentOperator Cos;

        static NumberAssignmentOperator()
        {
            Assign = new NumberAssignmentOperator(nameof(Assign), 0x00,
                "=");
            Addition = new NumberAssignmentOperator(nameof(Addition), 0x01,
                "+=");
            Subtraction = new NumberAssignmentOperator(nameof(Subtraction), 0x02,
                "-=");
            Multiplication = new NumberAssignmentOperator(nameof(Multiplication), 0x03,
                "*=");
            Division = new NumberAssignmentOperator(nameof(Division), 0x04,
                "/=");
            Modulo = new NumberAssignmentOperator(nameof(Modulo), 0x05,
                "%=");
            LowerBound = new NumberAssignmentOperator(nameof(LowerBound), 0x06,
                "下限=");
            UpperBound = new NumberAssignmentOperator(nameof(UpperBound), 0x07,
                "上限=");
            Absolute = new NumberAssignmentOperator(nameof(Absolute), 0x08,
                "絶対値=");
            Angle = new NumberAssignmentOperator(nameof(Angle), 0x09,
                "= 角度[x10]←傾き");
            Sin = new NumberAssignmentOperator(nameof(Sin), 0x0A,
                "= sin[x1000]←角度[x10] =");
            Cos = new NumberAssignmentOperator(nameof(Cos), 0x0B,
                "= cos[x1000]←角度[x10] =");
        }

        private NumberAssignmentOperator(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object EventCommandSentence { get; }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberAssignmentOperator FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
