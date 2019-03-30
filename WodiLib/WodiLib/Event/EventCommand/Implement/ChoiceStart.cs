// ========================================
// Project Name : WodiLib
// File Name    : ChoiceStart.cs
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
    /// イベントコマンド・選択肢・開始
    /// </summary>
    public class ChoiceStart : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChoiceStart;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x02;

        /// <inheritdoc />
        public override byte StringVariableCount => (byte) CaseValue;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 1)] インデックス</param>
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
                        (byte) (CancelForkIndex.Code + CaseValue),
                        forkFlags.ToByte(),
                        0x00, 0x00
                    };
                    return bytes.ToInt32(Endian.Environment);

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));
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
            if (index == 1)
            {
                var bytes = value.ToBytes(Endian.Environment);
                var cancelFlgByte = (byte) (bytes[0] & 0xF0);
                CancelForkIndex = ChoiceCancelForkType.ForByte(cancelFlgByte);
                CaseValue = bytes[0] & 0x0F;
                forkFlags = new ChoiceForkFlags(bytes[1]);
                return;
            }

            throw new ArgumentOutOfRangeException(
                ErrorMessage.OutOfRange(nameof(index), 1, 1, index));
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 9)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetStringVariable(int index)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            return choiceCaseList.Get(index);
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 9)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetStringVariable(int index, string value)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            if (value == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            choiceCaseList.Set(index, value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ChoiceCancelForkType cancelForkIndex = ChoiceCancelForkType.Else;

        /// <summary>[NotNull] キャンセル時分岐番号</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ChoiceCancelForkType CancelForkIndex
        {
            get => cancelForkIndex;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CancelForkIndex)));
                cancelForkIndex = value;
            }
        }

        private readonly ChoiceCaseList choiceCaseList = new ChoiceCaseList();

        /// <summary>[Range(1, 10)] 選択肢数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public int CaseValue
        {
            get => choiceCaseList.CaseValue;
            set
            {
                if (value < 1 || 10 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 10, value));
                choiceCaseList.CaseValue = value;
            }
        }

        /// <summary>[NotNull] 選択肢その1</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case1
        {
            get => choiceCaseList.Get(0);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case1)));
                choiceCaseList.Set(0, value);
            }
        }

        /// <summary>[NotNull] 選択肢その2</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case2
        {
            get => choiceCaseList.Get(1);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case2)));
                choiceCaseList.Set(1, value);
            }
        }

        /// <summary>[NotNull] 選択肢その3</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case3
        {
            get => choiceCaseList.Get(2);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case3)));
                choiceCaseList.Set(2, value);
            }
        }

        /// <summary>[NotNull] 選択肢その4</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case4
        {
            get => choiceCaseList.Get(3);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case4)));
                choiceCaseList.Set(3, value);
            }
        }

        /// <summary>[NotNull] 選択肢その5</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case5
        {
            get => choiceCaseList.Get(4);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case5)));
                choiceCaseList.Set(4, value);
            }
        }

        /// <summary>[NotNull] 選択肢その6</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case6
        {
            get => choiceCaseList.Get(5);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case6)));
                choiceCaseList.Set(5, value);
            }
        }

        /// <summary>[NotNull] 選択肢その7</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case7
        {
            get => choiceCaseList.Get(6);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case7)));
                choiceCaseList.Set(6, value);
            }
        }

        /// <summary>[NotNull] 選択肢その8</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case8
        {
            get => choiceCaseList.Get(7);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case8)));
                choiceCaseList.Set(7, value);
            }
        }

        /// <summary>[NotNull] 選択肢その9</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case9
        {
            get => choiceCaseList.Get(8);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case9)));
                choiceCaseList.Set(8, value);
            }
        }

        /// <summary>[NotNull] 選択肢その10</summary>
        /// <exception cref="PropertyNullException">nullを指定した場合</exception>
        public string Case10
        {
            get => choiceCaseList.Get(9);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case10)));
                choiceCaseList.Set(9, value);
            }
        }

        private ChoiceForkFlags forkFlags = new ChoiceForkFlags();

        /// <summary>強制中断フラグ</summary>
        public bool IsStopForce
        {
            get => forkFlags.IsStopForce;
            set => forkFlags.IsStopForce = value;
        }

        /// <summary>左キー分岐</summary>
        public bool IsForkLeftKey
        {
            get => forkFlags.IsForkLeftKey;
            set => forkFlags.IsForkLeftKey = value;
        }

        /// <summary>右キー分岐</summary>
        public bool IsForkRightKey
        {
            get => forkFlags.IsForkRightKey;
            set => forkFlags.IsForkRightKey = value;
        }
    }
}