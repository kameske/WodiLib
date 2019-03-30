// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringStartForking.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・条件（文字列）・分岐始端
    /// </summary>
    public class ConditionStringStartForking : ForkingStartBase
    {
        /// <summary>分岐番号最大数</summary>
        public static int CaseNumberMax => 3;

        /// <summary>分岐番号最小数</summary>
        private const int CaseNumberMin = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ConditionStringStartForking;

        /// <inheritdoc />
        /// <summary>[Range(0, 3)] 選択肢番号</summary>
        /// <exception cref="T:WodiLib.Sys.PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public override int CaseNumber
        {
            get => ChoiceCodeRaw - 1;
            set
            {
                if (value < CaseNumberMin || CaseNumberMax < value)
                {
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseNumber), CaseNumberMin, CaseNumberMax, value));
                }

                ChoiceCodeRaw = (byte) (value + 1);
            }
        }
    }
}