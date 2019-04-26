// ========================================
// Project Name : WodiLib
// File Name    : AssignValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 動作指定：代入
    /// </summary>
    public class AssignValue : CharaMoveCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override CharaMoveCommandCode CommandCode => CharaMoveCommandCode.SubstituteValue;

        /// <inheritdoc />
        public override byte ValueLengthByte => 0x02;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private VariableAddress targetAddress = (NormalNumberVariableAddress) NormalNumberVariableAddress.MinValue;

        /// <summary>
        ///     [Convertible(<see cref="NormalNumberVariableAddress"/>)]
        ///     [Convertible(<see cref="CalledEventVariableAddress"/>)]
        ///     対象アドレス
        /// </summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲外の値をセットしたとき</exception>
        public VariableAddress TargetAddress
        {
            get
            {
                if (targetAddress is NormalNumberVariableAddress) return targetAddress;

                if (Owner == null) return targetAddress;

                return Owner.ConvertVariableAddress((int) targetAddress);
            }
            set
            {
                var hasError = false;
                try
                {
                    var _ = (NormalNumberVariableAddress) (int) value;
                }
                catch
                {
                    try
                    {
                        var _ = (CalledEventVariableAddress) (int) value;
                    }
                    catch
                    {
                        hasError = true;
                    }
                }

                if (hasError)
                {
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.Unsuitable(nameof(TargetAddress), $"値：{value}"));
                }

                targetAddress = value;
                SetNumberValue(0, value.ToInt());
            }
        }

        /// <summary>代入値</summary>
        public CharaMoveCommandValue Value
        {
            get => GetNumberValue(1);
            set => SetNumberValue(1, value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[Nullable] 所有イベント種別</summary>
        internal TargetAddressOwner Owner { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AssignValue()
        {
            // 引数0の初期値設定
            SetNumberValue(0, targetAddress.ToInt());
        }
    }
}