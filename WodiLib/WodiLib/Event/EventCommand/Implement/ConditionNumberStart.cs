// ========================================
// Project Name : WodiLib
// File Name    : ConditionNumberStart.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・条件（変数）・始端
    /// </summary>
    public class ConditionNumberStart : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ConditionNumberStart;

        /// <inheritdoc />
        public override byte NumberVariableCount => (byte) (2 + CaseValue * 3);

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 10)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                    byte[] bytes =
                    {
                        (byte) CaseValue, 0x00, 0x00, 0x00
                    };
                    if (IsElseCase) bytes[0] += 0x10;
                    return bytes.ToInt32(Endian.Environment);

                case 2:
                    return conditionList.Get(0).LeftSide;

                case 3:
                    return conditionList.Get(0).RightSide;

                case 4:
                    return conditionList.Get(0).ToConditionFlag();

                case 5:
                    return conditionList.Get(1).LeftSide;

                case 6:
                    return conditionList.Get(1).RightSide;

                case 7:
                    return conditionList.Get(1).ToConditionFlag();

                case 8:
                    return conditionList.Get(2).LeftSide;

                case 9:
                    return conditionList.Get(2).RightSide;

                case 10:
                    return conditionList.Get(2).ToConditionFlag();

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 10, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 1)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    IsElseCase = (bytes[0] & 0xF0) != 0;
                    CaseValue = bytes[0] & 0x0F;
                    return;
                }

                case 2:
                    Case1.LeftSide = value;
                    return;

                case 3:
                    Case1.RightSide = value;
                    return;

                case 4:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    Case1.IsNotReferX = (bytes[0] & 0xF0) != 0;
                    Case1.Condition = NumberConditionalOperator.FromByte((byte) (bytes[0] & 0x0F));
                    return;
                }

                case 5:
                    Case2.LeftSide = value;
                    return;

                case 6:
                    Case2.RightSide = value;
                    return;

                case 7:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    Case2.IsNotReferX = (bytes[0] & 0xF0) != 0;
                    Case2.Condition = NumberConditionalOperator.FromByte((byte) (bytes[0] & 0x0F));
                    return;
                }

                case 8:
                    Case3.LeftSide = value;
                    return;

                case 9:
                    Case3.RightSide = value;
                    return;

                case 10:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    Case3.IsNotReferX = (bytes[0] & 0xF0) != 0;
                    Case3.Condition = NumberConditionalOperator.FromByte((byte) (bytes[0] & 0x0F));
                    return;
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 10, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetStringVariable(int index)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void SetStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>「上記以外」分岐フラグ</summary>
        public bool IsElseCase { get; set; }

        private readonly ConditionNumberList conditionList = new ConditionNumberList();

        /// <summary>[Range(1, 3)] 分岐数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public int CaseValue
        {
            get => conditionList.ConditionValue;
            set
            {
                if (value < 1 || 3 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 3, value));
                conditionList.ConditionValue = value;
            }
        }

        /// <summary>[NotNull] 条件1</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionNumberDesc Case1
        {
            get => conditionList.Get(0);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case1)));
                conditionList.Set(0, value);
            }
        }

        /// <summary>[NotNull] 条件2</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionNumberDesc Case2
        {
            get => conditionList.Get(1);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case2)));
                conditionList.Set(1, value);
            }
        }

        /// <summary>[NotNull] 条件3</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionNumberDesc Case3
        {
            get => conditionList.Get(2);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case3)));
                conditionList.Set(2, value);
            }
        }
    }
}