// ========================================
// Project Name : WodiLib
// File Name    : InvalidMoveAnimation.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：移動アニメOFF
    /// </summary>
    public class InvalidMoveAnimation : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/


        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.InvalidMoveAnimation;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x00;
    }
}