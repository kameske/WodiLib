// ========================================
// Project Name : WodiLib
// File Name    : ChangeHeight.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：高さ変更
    /// </summary>
    public class ChangeHeight : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.ChangeHeight;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>高さ</summary>
        public int Height
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}