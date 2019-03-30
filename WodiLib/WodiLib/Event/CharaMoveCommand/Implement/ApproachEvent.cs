// ========================================
// Project Name : WodiLib
// File Name    : ApproachEvent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：イベントに接近
    /// </summary>
    public class ApproachEvent : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.ApproachEvent;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>
        /// イベントID
        /// </summary>
        public CharaMoveCommandValue EventId
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}