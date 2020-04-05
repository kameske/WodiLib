// ========================================
// Project Name : WodiLib
// File Name    : ApproachPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：座標に接近
    /// </summary>
    [Serializable]
    public class ApproachPosition : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "X:{0},Y:{1} に接近";

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
            set
            {
                SetNumberValue(0, value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>Y座標</summary>
        public CharaMoveCommandValue PositionY
        {
            get => GetNumberValue(1);
            set
            {
                SetNumberValue(1, value);
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var xStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                PositionX, type, desc);
            var yStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                PositionY, type, desc);

            return string.Format(EventCommandSentenceFormat, xStr, yStr);
        }
    }
}