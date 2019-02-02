// ========================================
// Project Name : WodiLib
// File Name    : ChangePenetration.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：不透明度設定
    /// </summary>
    public class ChangePenetration : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.ChangePenetration;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>透過度</summary>
        public int Opacity
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}