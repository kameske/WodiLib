// ========================================
// Project Name : WodiLib
// File Name    : ValidReadinessAnimation.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：待機時アニメON
    /// </summary>
    public class ValidReadinessAnimation : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.ValidReadinessAnimation;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x00;
    }
}