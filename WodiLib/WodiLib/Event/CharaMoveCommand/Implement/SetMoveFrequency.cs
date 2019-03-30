// ========================================
// Project Name : WodiLib
// File Name    : SetMoveFrequency.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：移動頻度を設定
    /// </summary>
    public class SetMoveFrequency : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.SetMoveFrequency;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x01;

        private MoveFrequency moveFrequency = MoveFrequency.Frame;

        /// <summary>[NotNull] 頻度</summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public MoveFrequency Value
        {
            get => moveFrequency;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Value)));

                moveFrequency = value;
                var val = (int) moveFrequency.Code;
                SetNumberValue(0, (CharaMoveCommandValue) val);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SetMoveFrequency()
        {
            // 引数0の初期値設定
            var val = (int) moveFrequency.Code;
            SetNumberValue(0, (CharaMoveCommandValue) val);
        }
    }
}