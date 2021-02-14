// ========================================
// Project Name : WodiLib
// File Name    : NumberConditionalOperator.cs
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
    ///     条件（変数） 条件演算子
    /// </summary>
    public record NumberConditionalOperator : TypeSafeEnum<NumberConditionalOperator>
    {
        /// <summary>より大きい</summary>
        public static readonly NumberConditionalOperator Above;

        /// <summary>以上</summary>
        public static readonly NumberConditionalOperator Greater;

        /// <summary>同じ</summary>
        public static readonly NumberConditionalOperator Equal;

        /// <summary>以下</summary>
        public static readonly NumberConditionalOperator Less;

        /// <summary>未満</summary>
        public static readonly NumberConditionalOperator Below;

        /// <summary>以外</summary>
        public static readonly NumberConditionalOperator Not;

        /// <summary>ビットを満たす</summary>
        public static readonly NumberConditionalOperator BitAnd;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static NumberConditionalOperator()
        {
            Above = new NumberConditionalOperator("Above", 0x00,
                "超");
            Greater = new NumberConditionalOperator("Greater", 0x01,
                "以上");
            Equal = new NumberConditionalOperator("Equal", 0x02,
                "と同じ");
            Less = new NumberConditionalOperator("Less", 0x03,
                "以下");
            Below = new NumberConditionalOperator("Below", 0x04,
                "未満");
            Not = new NumberConditionalOperator("Not", 0x05,
                "以外");
            BitAnd = new NumberConditionalOperator("BitAnd", 0x06,
                "のﾋﾞｯﾄを満たす");
        }

        private NumberConditionalOperator(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="src">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberConditionalOperator FromByte(byte src)
        {
            return AllItems.First(x => x.Code == src);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
