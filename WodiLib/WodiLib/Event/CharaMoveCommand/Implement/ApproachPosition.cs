// ========================================
// Project Name : WodiLib
// File Name    : ApproachPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：座標に接近
    /// </summary>
    public class ApproachPosition : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.ApproachPosition;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x02;

        /// <summary>X座標</summary>
        public CharaMoveCommandValue PositionX
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }

        /// <summary>Y座標</summary>
        public CharaMoveCommandValue PositionY
        {
            get => GetNumberValue(1);
            set => SetNumberValue(1, value);
        }
    }
}