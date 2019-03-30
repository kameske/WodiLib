// ========================================
// Project Name : WodiLib
// File Name    : SetMoveSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：移動速度を設定
    /// </summary>
    public class SetMoveSpeed : CharaMoveCommandBase
    {
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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Value)));

                moveSpeed = value;
                var val = (int) moveSpeed.Code;
                SetNumberValue(0, (CharaMoveCommandValue) val);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SetMoveSpeed()
        {
            // 引数0の初期値設定
            var val = (int) moveSpeed.Code;
            SetNumberValue(0, (CharaMoveCommandValue) val);
        }
    }
}