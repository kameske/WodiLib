// ========================================
// Project Name : WodiLib
// File Name    : ChangeGraphic.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：グラフィック変更
    /// </summary>
    public class ChangeGraphic : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.ChangeGraphic;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>画像ID</summary>
        public CharaMoveCommandValue GraphicId
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}