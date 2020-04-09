// ========================================
// Project Name : WodiLib
// File Name    : Jump.cs
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
    /// 動作指定：ジャンプ
    /// </summary>
    [Serializable]
    public class Jump : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "ジャンプ-右{0}下{1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.Jump;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x02;

        /// <summary>右</summary>
        public CharaMoveCommandValue RightPoint
        {
            get => GetNumberValue(0);
            set
            {
                SetNumberValue(0, value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>下</summary>
        public CharaMoveCommandValue DownPoint
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
            var rightStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                RightPoint, type, desc);
            var downStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                DownPoint, type, desc);

            return string.Format(EventCommandSentenceFormat,
                rightStr, downStr);
        }
    }
}