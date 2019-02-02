// ========================================
// Project Name : WodiLib
// File Name    : PlaySE.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：効果音再生
    /// </summary>
    public class PlaySE : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte CommandCode => CharaMoveCommandCode.PlaySE;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>効果音ID</summary>
        public int SoundId
        {
            get => GetNumberValue(0);
            set => SetNumberValue(0, value);
        }
    }
}