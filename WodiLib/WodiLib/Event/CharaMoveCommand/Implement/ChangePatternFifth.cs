// ========================================
// Project Name : WodiLib
// File Name    : ChangePatternFifth.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：パターン5に変更
    /// </summary>
    public class ChangePatternFifth : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.ChangePatternFifth;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x00;
    }
}