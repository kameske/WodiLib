// ========================================
// Project Name : WodiLib
// File Name    : SetAnimateSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：アニメ頻度を設定
    /// </summary>
    [Serializable]
    public class SetAnimateSpeed : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "アニメ頻度を設定 => {0}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.SetAnimateSpeed;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        private AnimateSpeed animateSpeed = AnimateSpeed.Frame;

        /// <summary>[NotNull] 頻度</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public AnimateSpeed Value
        {
            get => animateSpeed;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Value)));

                animateSpeed = value;
                var val = animateSpeed.Code;
                SetNumberValue(0, val);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SetAnimateSpeed()
        {
            // 引数0の初期値設定
            var val = animateSpeed.Code;
            SetNumberValue(0, val);
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetEventCommandSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            return string.Format(EventCommandSentenceFormat, Value.Code);
        }
    }
}