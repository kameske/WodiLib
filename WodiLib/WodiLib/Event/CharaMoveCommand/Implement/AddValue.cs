// ========================================
// Project Name : WodiLib
// File Name    : AddValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：加算
    /// </summary>
    public class AddValue : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.AddValue;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x02;


        private VariableAddress targetAddress = (NormalNumberVariableAddress) NormalNumberVariableAddress.MinValue;

        /// <summary>
        ///     [Convertible(<see cref="NormalNumberVariableAddress"/>)]
        ///     [Convertible(<see cref="CalledEventVariableAddress"/>)]
        ///     対象アドレス
        /// </summary>
        public VariableAddress TargetAddress
        {
            get => targetAddress;
            set
            {
                if (value is NormalNumberVariableAddress
                    || value is CalledEventVariableAddress)
                {
                    targetAddress = value;
                }

                SetNumberValue(0, value.ToInt());
            }
        }

        /// <summary>
        /// 加算値
        /// </summary>
        public CharaMoveCommandValue Value
        {
            get => GetNumberValue(1);
            set => SetNumberValue(1, value);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddValue()
        {
            // 引数0の初期値設定
            SetNumberValue(0, targetAddress.ToInt());
        }
    }
}