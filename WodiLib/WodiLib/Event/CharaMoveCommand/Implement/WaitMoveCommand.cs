// ========================================
// Project Name : WodiLib
// File Name    : WaitMoveCommand.cs
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
    /// 動作指定：ウェイト
    /// </summary>
    [Serializable]
    public class WaitMoveCommand : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ウェイト {0}フレーム";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.Wait;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        /// <summary>待機時間</summary>
        public CharaMoveCommandValue Frame
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
            var frameStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                Frame, type, desc);

            return string.Format(EventCommandSentenceFormat, frameStr);
        }
    }
}