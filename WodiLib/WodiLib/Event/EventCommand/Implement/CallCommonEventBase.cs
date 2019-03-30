// ========================================
// Project Name : WodiLib
// File Name    : CallCommonEventBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// コモンイベント呼び出し共通処理
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class CallCommonEventBase : EventCommandBase
    {
        /// <summary>ページ最大数</summary>
        private const int PageMax = 10;

        /// <summary>ページ最小数</summary>
        private const int PageMin = 1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                byte result = 0x03;
                result += (byte) IntArgValue;
                result += (byte) StrArgValue;
                result += (byte) (IsGetReturnValue ? 1 : 0);
                return result;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount
        {
            get
            {
                for (var i = StrArgValue - 1; i >= 0; i--)
                {
                    if (strArgList.HasStringParam(i)) return (byte) (StrArgValue + 1);
                }

                return (byte) (IsOrderByString ? 0x01 : 0x00);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 3～12)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            if (index > NumberVariableCount || index < 0)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));

            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                    if (!IsOrderByString) return EventIdOrName.ToInt();
                    return 0;

                case 2:
                    if (!IsOrderByString &&
                        (EventIdOrName.ToInt().IsMapEventId()
                         || EventIdOrName.ToInt().IsVariableAddress()))
                    {
                        // マップイベントID指定呼び出しの場合
                        return Page - 1;
                    }

                    // コモンイベントID指定呼び出しの場合
                    byte[] bytes = new byte[4];
                    bytes[0] = (byte) ((StrArgValue << 4) + IntArgValue);
                    bytes[1] = strArgList.ReferenceFlg;
                    bytes[2] = 0;
                    bytes[3] = (byte) (IsGetReturnValue ? 1 : 0);
                    return bytes.ToInt32(Endian.Little);

                default:
                    // ここから可変長引数パターン

                    // 数値引数の設定値を指すかどうかを確認する。
                    var tmpIndex = index - 3;
                    if (tmpIndex < IntArgValue)
                    {
                        // 数値引数の設定値を返す
                        return intArgList.Get(tmpIndex);
                    }

                    // 文字列引数の設定値を指すかどうかを確認する。
                    tmpIndex -= IntArgValue;
                    if (tmpIndex < StrArgValue)
                    {
                        // 文字列引数の設定値を返す
                        //   ただし文字列直接指定ならば 0 を返す
                        var strArg = strArgList.Get(tmpIndex);
                        if (strArg.InstanceIntOrStrType != IntOrStrType.Int)
                        {
                            return 0;
                        }

                        return strArg.ToInt();
                    }

                    // 残るは戻り値格納アドレスのみ
                    tmpIndex -= StrArgValue;
                    if (tmpIndex != 0 || !IsGetReturnValue)
                    {
                        // おかしい（通常来ないはず）
                        throw new InvalidOperationException();
                    }

                    return ResultOutputAddress;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 3～12)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            if (index > NumberVariableCount
                || index < 1)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));

            switch (index)
            {
                case 1:
                    if (!IsOrderByString) EventIdOrName = value;
                    return;

                case 2:
                    if (!IsOrderByString &&
                        (EventIdOrName.ToInt().IsMapEventId()
                         || EventIdOrName.ToInt().IsVariableAddress()))
                    {
                        // マップイベントの場合、ページ
                        Page = value + 1;
                        return;
                    }

                    // コモンイベントの場合、各フラグ
                    var bytes = value.ToBytes(Endian.Environment);
                    StrArgValue = (byte) ((bytes[0] & 0xF0) >> 4);
                    IntArgValue = (byte) (bytes[0] & 0x0F);
                    strArgList.ReferenceFlg = bytes[1];
                    IsGetReturnValue = bytes[3] == 0x01;
                    return;

                default:
                    // 可変長引数

                    // 数値引数の設定値を指すかどうかを確認する。
                    var tmpIndex = index - 3;
                    if (tmpIndex < IntArgValue)
                    {
                        // 数値引数の設定値を更新
                        intArgList.Set(tmpIndex, value);
                        return;
                    }

                    // 文字列引数の設定値を指すかどうかを確認する。
                    tmpIndex -= IntArgValue;
                    if (tmpIndex < StrArgValue)
                    {
                        // 文字列引数の設定値を更新
                        //   ただし文字列直接指定ならば 何もしない
                        var strArg = strArgList.Get(tmpIndex);
                        if (strArg.InstanceIntOrStrType != IntOrStrType.Int)
                        {
                            return;
                        }

                        strArgList.Set(tmpIndex, value);
                        return;
                    }

                    // 残るは戻り値格納アドレスのみ
                    tmpIndex -= StrArgValue;
                    if (tmpIndex != 0 || !IsGetReturnValue)
                    {
                        // おかしい（通常来ないはず）
                        throw new InvalidOperationException();
                    }

                    ResultOutputAddress = value;
                    return;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, -1～5)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetStringVariable(int index)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount, index));
            if (index == 0)
            {
                // コモンイベント名
                return EventIdOrName.HasStr ? EventIdOrName.ToStr() : "";
            }

            var tmpIndex = index - 1;

            var val = strArgList.Get(tmpIndex);
            if (val.InstanceIntOrStrType == IntOrStrType.Int) return "";
            return val.ToStr();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -1～5)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetStringVariable(int index, string value)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount, index));
            if (value == null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));

            if (index == 0)
            {
                // コモンイベント名
                EventIdOrName.Merge(value);
                return;
            }

            var tmpIndex = index - 1;
            // 引数タイプが数値なら何もしない
            if (strArgList.Get(tmpIndex).InstanceIntOrStrType == IntOrStrType.Int) return;
            strArgList.Set(tmpIndex, value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private IntOrStr eventIdOrName = 0;

        /// <summary>[NotNull] イベントID、または名前</summary>
        /// <exception cref="ArgumentNullException">nullを指定した場合</exception>
        protected IntOrStr EventIdOrName
        {
            get => eventIdOrName;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(EventIdOrName)));
                eventIdOrName = value;
            }
        }

        private int page = 1;

        /// <summary>[Range(1, 10)] マップイベントページ</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲外の値を設定したとき</exception>
        public int Page
        {
            get => page;
            set
            {
                if (value < PageMin || PageMax < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(Page), PageMin, PageMax, value));
                page = value;
            }
        }

        /// <summary>戻り値を受け取る</summary>
        public bool IsGetReturnValue { get; set; }

        /// <summary>戻り値格納アドレス</summary>
        public int ResultOutputAddress { get; set; }

        /// <summary>[Range(0, 4)] 数値引数の数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外をセットした場合</exception>
        public int IntArgValue
        {
            get => intArgValue;
            set
            {
                if (value < 0 || 4 < value)
                {
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(IntArgValue), 0, 4, value));
                }

                intArgValue = value;
            }
        }

        private int intArgValue;

        /// <summary>[Range(0, 4)] 文字列引数の数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外をセットした場合</exception>
        public int StrArgValue
        {
            get => strArgValue;
            set
            {
                if (value < 0 || 4 < value)
                {
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(StrArgValue), 0, 4, value));
                }

                strArgValue = value;
            }
        }

        private int strArgValue;

        private readonly CommonEventIntArgList intArgList = new CommonEventIntArgList();

        /// <summary>数値引数1</summary>
        public int IntArg1
        {
            get => intArgList.Get(0);
            set => intArgList.Set(0, value);
        }

        /// <summary>数値引数2</summary>
        public int IntArg2
        {
            get => intArgList.Get(1);
            set => intArgList.Set(1, value);
        }

        /// <summary>数値引数3</summary>
        public int IntArg3
        {
            get => intArgList.Get(2);
            set => intArgList.Set(2, value);
        }

        /// <summary>数値引数4</summary>
        public int IntArg4
        {
            get => intArgList.Get(3);
            set => intArgList.Set(3, value);
        }

        private readonly CommonEventStrArgList strArgList = new CommonEventStrArgList();

        /// <summary>[NotNull] 文字列引数1</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg1
        {
            get => strArgList.Get(0);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg1)));
                strArgList.Set(0, value);
            }
        }

        /// <summary>[NotNull] 文字列引数2</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg2
        {
            get => strArgList.Get(1);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg2)));
                strArgList.Set(1, value);
            }
        }

        /// <summary>[NotNull] 文字列引数3</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg3
        {
            get => strArgList.Get(2);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg3)));
                strArgList.Set(2, value);
            }
        }

        /// <summary>[NotNull] 文字列引数4</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg4
        {
            get => strArgList.Get(3);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg4)));
                strArgList.Set(3, value);
            }
        }

        /// <summary>イベント文字列指定フラグ</summary>
        protected abstract bool IsOrderByString { get; }
    }
}