// ========================================
// Project Name : WodiLib
// File Name    : TransferOption.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     トランジションオプション
    /// </summary>
    public record TransferOption : TypeSafeEnum<TransferOption>
    {
        /// <summary>トランジションなし</summary>
        public static readonly TransferOption NoTransition;

        /// <summary>トランジションを行う（暗転なし）</summary>
        public static readonly TransferOption TransitionNoFade;

        /// <summary>トランジションを行う（暗転あり）</summary>
        public static readonly TransferOption NoTransitionFade;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        internal string EventCommandSentence { get; }

        static TransferOption()
        {
            NoTransition = new TransferOption(nameof(NoTransition),
                0x00, "ﾄﾗﾝｼﾞｼｮﾝなし");
            TransitionNoFade = new TransferOption(nameof(TransitionNoFade),
                0x10, "ﾄﾗﾝｼﾞｼｮﾝ + 暗転なし");
            NoTransitionFade = new TransferOption(nameof(NoTransitionFade),
                0x20, "ﾄﾗﾝｼﾞｼｮﾝ + 暗転有り");
        }

        private TransferOption(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static TransferOption FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
