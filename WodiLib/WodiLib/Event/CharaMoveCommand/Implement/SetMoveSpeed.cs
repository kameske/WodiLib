// ========================================
// Project Name : WodiLib
// File Name    : SetMoveSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：移動速度を設定
    /// </summary>
    public class SetMoveSpeed : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.SetMoveSpeed;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>速度</summary>
        public int Value
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}