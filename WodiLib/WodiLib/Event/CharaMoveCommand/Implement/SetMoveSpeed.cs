// ========================================
// Project Name : WodiLib
// File Name    : SetMoveSpeed.cs
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
    /// 動作指定：移動速度を設定
    /// </summary>
    [Serializable]
    public class SetMoveSpeed : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "移動速度を設定 => {0}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.SetMoveSpeed;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        private MoveSpeed moveSpeed = MoveSpeed.Slowest;

        /// <summary>[NotNull] 速度</summary>
        public MoveSpeed Value
        {
            get => moveSpeed;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Value)));

                moveSpeed = value;
                var val = moveSpeed.Code;
                SetNumberValue(0, val);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SetMoveSpeed()
        {
            // 引数0の初期値設定
            var val = moveSpeed.Code;
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