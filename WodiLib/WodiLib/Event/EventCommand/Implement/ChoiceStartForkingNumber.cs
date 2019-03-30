// ========================================
// Project Name : WodiLib
// File Name    : ChoiceStartForkingNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・選択肢始端
    /// </summary>
    public class ChoiceStartForkingNumber : ForkingStartBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const int CaseNumberMax = 9;
        private const int CaseNumberMin = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChoiceStartForkingNumber;

        /// <inheritdoc />
        /// <summary>[Range(0, 9)] 選択肢番号</summary>
        /// <exception cref="T:WodiLib.Sys.PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public override int CaseNumber
        {
            get => ChoiceCodeRaw - 2;
            set
            {
                if (value < CaseNumberMin
                    || CaseNumberMax < value)
                {
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseNumber), 0, 9, value));
                }

                ChoiceCodeRaw = (byte) (value + 2);
            }
        }
    }
}