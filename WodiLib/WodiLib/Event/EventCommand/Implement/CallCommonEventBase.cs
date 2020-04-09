// ========================================
// Project Name : WodiLib
// File Name    : CallCommonEventBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using WodiLib.Cmn;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// コモンイベント呼び出し共通処理
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public abstract class CallCommonEventBase : EventCommandBase
    {
        /// <summary>ページ最大数</summary>
        private const int PageMax = 10;

        /// <summary>ページ最小数</summary>
        private const int PageMin = 1;

        /// <summary>数値引数の数最大数</summary>
        private const int IntArgValueMax = 5;

        /// <summary>数値引数の数最小数</summary>
        private const int IntArgValueMin = 0;

        /// <summary>文字列引数の数最大数</summary>
        private const int StrArgValueMax = 5;

        /// <summary>文字列引数の数最小数</summary>
        private const int StrArgValueMin = 0;

        /// <summary>イベントコマンド文字列・戻り値のフォーマット</summary>
        private const string EventCommandSentenceReturnVariableFormat
            = "{0} = ";

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.DarkViolet;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイプ設定プロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnEventIdOrNamePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            NotifyPropertyChanged(nameof(EventIdOrName));
            NotifyPropertyChanged(nameof(IsCallMapEventId));
        }

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
                    if (StrArgList.HasStringParam(i))
                        return (byte) (StrArgValue + 1);

                return (byte) (IsOrderByString ? 0x01 : 0x00);
            }
        }

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x03;

        /// <inheritdoc />
        /// <summary>文字列変数最小個数</summary>
        public override byte StringVariableCountMin => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 3～12)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
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
                        // マップイベントID指定呼び出しの場合
                        return Page - 1;

                    // コモンイベントID指定呼び出しの場合
                    var bytes = new byte[4];
                    bytes[0] = (byte) ((StrArgValue << 4) + IntArgValue);
                    bytes[1] = StrArgList.ReferenceFlg;
                    bytes[2] = 0;
                    bytes[3] = (byte) (IsGetReturnValue ? 1 : 0);
                    return bytes.ToInt32(Endian.Little);

                default:
                    // ここから可変長引数パターン

                    // 数値引数の設定値を指すかどうかを確認する。
                    var tmpIndex = index - 3;
                    if (tmpIndex < IntArgValue)
                        // 数値引数の設定値を返す
                        return IntArgList[tmpIndex];

                    // 文字列引数の設定値を指すかどうかを確認する。
                    tmpIndex -= IntArgValue;
                    if (tmpIndex < StrArgValue)
                    {
                        // 文字列引数の設定値を返す
                        //   ただし文字列直接指定ならば 0 を返す
                        var strArg = StrArgList[tmpIndex];
                        if (strArg.InstanceIntOrStrType != IntOrStrType.Int) return 0;

                        return strArg.ToInt();
                    }

                    // 残るは戻り値格納アドレスのみ
                    tmpIndex -= StrArgValue;
                    if (tmpIndex != 0 || !IsGetReturnValue)
                        // おかしい（通常来ないはず）
                        throw new InvalidOperationException();

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
        public override void SetSafetyNumberVariable(int index, int value)
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
                    if (IsCallMapEventId)
                    {
                        // マップイベントの場合、ページ
                        Page = value + 1;
                        return;
                    }

                    // コモンイベントの場合、各フラグ
                    var bytes = value.ToBytes(Endian.Environment);
                    StrArgValue = (byte) ((bytes[0] & 0xF0) >> 4);
                    IntArgValue = (byte) (bytes[0] & 0x0F);
                    StrArgList.ReferenceFlg = bytes[1];
                    IsGetReturnValue = bytes[3] == 0x01;
                    return;

                default:
                    // 可変長引数

                    // 数値引数の設定値を指すかどうかを確認する。
                    var tmpIndex = index - 3;
                    if (tmpIndex < IntArgValue)
                    {
                        // 数値引数の設定値を更新
                        IntArgList[tmpIndex] = value;
                        return;
                    }

                    // 文字列引数の設定値を指すかどうかを確認する。
                    tmpIndex -= IntArgValue;
                    if (tmpIndex < StrArgValue)
                    {
                        // 文字列引数の設定値を更新
                        //   ただし文字列直接指定ならば 何もしない
                        var strArg = StrArgList[tmpIndex];
                        if (strArg.InstanceIntOrStrType != IntOrStrType.Int) return;

                        StrArgList[tmpIndex] = value;
                        return;
                    }

                    // 残るは戻り値格納アドレスのみ
                    tmpIndex -= StrArgValue;
                    if (tmpIndex != 0 || !IsGetReturnValue)
                        // おかしい（通常来ないはず）
                        throw new InvalidOperationException();

                    ResultOutputAddress = value;
                    return;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, -1～5)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount, index));
            if (index == 0)
                // コモンイベント名
                return EventIdOrName.HasStr ? EventIdOrName.ToStr() : "";

            var tmpIndex = index - 1;

            var val = StrArgList[tmpIndex];
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
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (index < 0 || StringVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount, index));
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));

            if (index == 0)
            {
                // コモンイベント名
                EventIdOrName.Merge(value);
                return;
            }

            var tmpIndex = index - 1;
            // 引数タイプが数値なら何もしない
            if (StrArgList[tmpIndex].InstanceIntOrStrType == IntOrStrType.Int) return;
            StrArgList[tmpIndex] = value;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            var argsCommandString = MakeEventCommandSentenceCallCommonEventArgs(
                resolver, type, desc);
            var returnVarString = MakeEventCommandSentenceReturnVariableStr(
                resolver, type, desc);

            return MakeEventCommandMainSentenceInner(resolver, type, desc,
                argsCommandString, returnVarString);
        }

        /// <summary>
        /// CallCommonEventのイベントコマンドを取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <param name="argsCommandString">引数イベントコマンド文字列</param>
        /// <param name="returnVarString">返戻先イベントコマンド文字列</param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeEventCommandMainSentenceInner(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc,
            string argsCommandString, string returnVarString);


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
                if (value is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(EventIdOrName)));
                eventIdOrName.PropertyChanged -= OnEventIdOrNamePropertyChanged;
                eventIdOrName = value;
                eventIdOrName.PropertyChanged += OnEventIdOrNamePropertyChanged;
                OnEventIdOrNamePropertyChanged(eventIdOrName, new PropertyChangedEventArgs("this"));
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
                NotifyPropertyChanged();
            }
        }

        private bool isGetReturnValue;

        /// <summary>戻り値を受け取る</summary>
        public bool IsGetReturnValue
        {
            get => isGetReturnValue;
            set
            {
                isGetReturnValue = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private int resultOutputAddress;

        /// <summary>戻り値格納アドレス</summary>
        public int ResultOutputAddress
        {
            get => resultOutputAddress;
            set
            {
                resultOutputAddress = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>[Range(0, 3)] 数値引数の数</summary>
        /// <remarks>
        ///    隠し機能として、数値引数の数は 0～4 の範囲で指定可能。
        /// </remarks>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外をセットした場合</exception>
        public int IntArgValue
        {
            get => intArgValue;
            set
            {
                if (value < IntArgValueMin || IntArgValueMax < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(IntArgValue), IntArgValueMin, IntArgValueMax, value));

                intArgValue = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private int intArgValue;

        /// <summary>[Range(0, 3)] 文字列引数の数</summary>
        /// <remarks>
        ///    隠し機能として、文字列引数の数は 0～4 の範囲で指定可能。
        /// </remarks>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外をセットした場合</exception>
        public int StrArgValue
        {
            get => strArgValue;
            set
            {
                if (value < StrArgValueMin || StrArgValueMax < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(StrArgValue), StrArgValueMin, StrArgValueMax, value));

                strArgValue = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
                NotifyPropertyChanged(nameof(StringVariableCount));
            }
        }

        private int strArgValue;

        /// <summary>
        /// 数値引数リスト
        /// </summary>
        public CommonEventIntArgList IntArgList { get; }
            = new CommonEventIntArgList();

        /// <summary>数値引数1</summary>
        public int IntArg1
        {
            get => IntArgList[0];
            set
            {
                IntArgList[0] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>数値引数2</summary>
        public int IntArg2
        {
            get => IntArgList[1];
            set
            {
                IntArgList[1] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>数値引数3</summary>
        public int IntArg3
        {
            get => IntArgList[2];
            set
            {
                IntArgList[2] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>数値引数4</summary>
        public int IntArg4
        {
            get => IntArgList[3];
            set
            {
                IntArgList[3] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>数値引数5</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int IntArg5
        {
            get => IntArgList[4];
            set
            {
                IntArgList[4] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 文字列引数リスト
        /// </summary>
        public CommonEventStrArgList StrArgList { get; }
            = new CommonEventStrArgList();

        /// <summary>[NotNull] 文字列引数1</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg1
        {
            get => StrArgList[0];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg1)));
                StrArgList[0] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>[NotNull] 文字列引数2</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg2
        {
            get => StrArgList[1];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg2)));
                StrArgList[1] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>[NotNull] 文字列引数3</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg3
        {
            get => StrArgList[2];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg3)));
                StrArgList[2] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>[NotNull] 文字列引数4</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IntOrStr StringArg4
        {
            get => StrArgList[3];
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StringArg4)));
                StrArgList[3] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>文字列引数5（変数アドレス値）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int StringArg5
        {
            get => StrArgList[4].ToInt();
            set
            {
                StrArgList[4] = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>イベント文字列指定フラグ</summary>
        protected abstract bool IsOrderByString { get; }

        /// <summary>マップイベント呼び出しフラグ</summary>
        protected bool IsCallMapEventId
            => !IsOrderByString &&
               (EventIdOrName.ToInt().IsMapEventId()
                || EventIdOrName.ToInt().IsVariableAddress());

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コモンイベント呼び出し時の引数文字列
        /// </summary>
        private string MakeEventCommandSentenceCallCommonEventArgs(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var intArgsStrList = IntArgList.Where((_, idx) => idx < IntArgValue)
                .Select((x, idx) =>
                {
                    // 三項演算子のみを用いた場合、コード分析が "eventId == null is always false" と認識する。
                    // クロージャ化することで警告表示を回避。
                    var eventId = ((Func<int?>) (() =>
                            IsOrderByString
                                ? (int?) resolver.GetCommonEventId(EventIdOrName.ToStr())
                                : EventIdOrName.ToInt()
                        ))();

                    if (eventId is null)
                        return resolver.GetNumericVariableAddressStringIfVariableAddress(x, type, desc);

                    var id = eventId.Value;
                    var correctId = resolver.GetCorrectEventIdByRelativeId(id, desc.CommonEventId, type);
                    return resolver.GetCommonEventIntArgSentence(correctId, idx, x, type, desc);
                });

            var strArgsStrList = StrArgList.Where((_, idx) => idx < StrArgValue)
                .Select(x => x.HasInt
                    ? resolver.GetStringVariableAddressString(x.ToInt(), type, desc)
                    : x.ToStr());

            var argStrList = intArgsStrList.Concat(strArgsStrList).ToList();

            return argStrList.Count != 0
                ? $" / {string.Join(" / ", argStrList)}"
                : string.Empty;
        }

        /// <summary>
        /// コモンイベント呼び出し時の返戻先
        /// </summary>
        private string MakeEventCommandSentenceReturnVariableStr(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (!IsGetReturnValue) return "";

            var varAddressCmdStr = resolver.GetNumericVariableAddressString(
                ResultOutputAddress, type, desc);

            return string.Format(EventCommandSentenceReturnVariableFormat,
                varAddressCmdStr);
        }
    }
}