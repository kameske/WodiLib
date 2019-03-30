// ========================================
// Project Name : WodiLib
// File Name    : ChoiceStartForkingCancel.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・キャンセル始端
    /// </summary>
    public class ChoiceStartForkingCancel : ChoiceStartForkingSpecialBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChoiceStartForkingCancel;

        /// <inheritdoc />
        protected override byte ChoiceCode => EventCommandConstant.ChoiceStartForkingEtc.ChoiceCode.Cancel;
    }
}