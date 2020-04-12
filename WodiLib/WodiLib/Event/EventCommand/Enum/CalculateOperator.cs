// ========================================
// Project Name : WodiLib
// File Name    : CalculateOperator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using Commons;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 計算演算子
    /// </summary>
    public class CalculateOperator : TypeSafeEnum<CalculateOperator>
    {
        /// <summary>+</summary>
        public static readonly CalculateOperator Addition;

        /// <summary>-</summary>
        public static readonly CalculateOperator Subtraction;

        /// <summary>*</summary>
        public static readonly CalculateOperator Multiplication;

        /// <summary>/</summary>
        public static readonly CalculateOperator Division;

        /// <summary>%</summary>
        public static readonly CalculateOperator Modulo;

        /// <summary>ビット積</summary>
        public static readonly CalculateOperator BitAnd;

        /// <summary>～</summary>
        public static readonly CalculateOperator Between;

        /// <summary>角度</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly CalculateOperator Angle;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static CalculateOperator()
        {
            Addition = new CalculateOperator(nameof(Addition), 0x00,
                "+");
            Subtraction = new CalculateOperator(nameof(Subtraction), 0x10,
                "-");
            Multiplication = new CalculateOperator(nameof(Multiplication), 0x20,
                "*");
            Division = new CalculateOperator(nameof(Division), 0x30,
                "/");
            Modulo = new CalculateOperator(nameof(Modulo), 0x40,
                "%");
            BitAnd = new CalculateOperator(nameof(BitAnd), 0x50,
                "論理積");
            Between = new CalculateOperator(nameof(Between), 0x60,
                "～");
            Angle = new CalculateOperator(nameof(Angle), 0xF0,
                "");
        }

        private CalculateOperator(string id, byte code,
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
        public static CalculateOperator FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}