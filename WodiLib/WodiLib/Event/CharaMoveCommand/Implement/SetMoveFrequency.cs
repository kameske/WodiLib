// ========================================
// Project Name : WodiLib
// File Name    : SetMoveFrequency.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：移動頻度を設定
    /// </summary>
    public class SetMoveFrequency : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.SetMoveFrequency;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>頻度</summary>
        public int Value
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}