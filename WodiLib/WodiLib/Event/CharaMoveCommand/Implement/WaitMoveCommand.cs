// ========================================
// Project Name : WodiLib
// File Name    : WaitMoveCommand.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：ウェイト
    /// </summary>
    public class WaitMoveCommand : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.Wait;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>待機時間</summary>
        public CharaMoveCommandValue Frame
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}