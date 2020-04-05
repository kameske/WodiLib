// ========================================
// Project Name : WodiLib
// File Name    : ApproachEvent.cs
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
    /// 動作指定：イベントに接近
    /// </summary>
    [Serializable]
    public class ApproachEvent : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "Ev{0}に接近";

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
            set
            {
                SetNumberValue(0, value);
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var evStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                EventId, type, desc);

            return string.Format(EventCommandSentenceFormat, evStr);
        }
    }
}