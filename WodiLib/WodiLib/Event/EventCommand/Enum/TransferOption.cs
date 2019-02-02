// ========================================
// Project Name : WodiLib
// File Name    : TransferOption.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// トランジションオプション
    /// </summary>
    public class TransferOption : TypeSafeEnum<TransferOption>
    {
        /// <summary>トランジションなし</summary>
        public static readonly TransferOption NoTransition;

        /// <summary>トランジションを行う（暗転なし）</summary>
        public static readonly TransferOption TransitionNoFade;

        /// <summary>トランジションを行う（暗転あり）</summary>
        public static readonly TransferOption NoTransitionFade;

        /// <summary>値</summary>
        public byte Code { get; }

        static TransferOption()
        {
            NoTransition = new TransferOption(nameof(NoTransition), 0x00);
            TransitionNoFade = new TransferOption(nameof(TransitionNoFade), 0x10);
            NoTransitionFade = new TransferOption(nameof(NoTransitionFade), 0x20);
        }

        private TransferOption(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static TransferOption FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}