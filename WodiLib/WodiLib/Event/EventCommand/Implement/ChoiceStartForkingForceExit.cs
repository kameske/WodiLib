// ========================================
// Project Name : WodiLib
// File Name    : ChoiceStartForkingForceExit.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・強制中断分岐
    /// </summary>
    public class ChoiceStartForkingForceExit : ChoiceStartForkingSpecialBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChoiceStartForkingEtc;

        /// <inheritdoc />
        /// <summary>選択肢コード</summary>
        protected override byte ChoiceCode => EventCommandConstant.ChoiceStartForkingEtc.ChoiceCode.ForceExit;
    }
}