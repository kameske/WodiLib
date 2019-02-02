// ========================================
// Project Name : WodiLib
// File Name    : ChoiceStartForkingRightKey.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・右キー分岐
    /// </summary>
    public class ChoiceStartForkingRightKey : ChoiceStartForkingSpecialBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.ChoiceStartForkingEtc;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override byte ChoiceCode
        {
            get => EventCommandConstant.ChoiceStartForkingEtc.ChoiceCode.RightKey;
            set { }
        }
    }
}