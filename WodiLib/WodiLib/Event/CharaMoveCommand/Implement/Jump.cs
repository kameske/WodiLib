// ========================================
// Project Name : WodiLib
// File Name    : Jump.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：ジャンプ
    /// </summary>
    public class Jump : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.Jump;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x02;

        /// <summary>右</summary>
        public int RightPoint
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }

        /// <summary>下</summary>
        public int DownPoint
        {
            get => GetNumberValue(1);
            set => SetNumberValue(1, value);
        }
    }
}