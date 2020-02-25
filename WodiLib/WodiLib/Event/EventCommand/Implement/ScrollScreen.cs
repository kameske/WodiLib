// ========================================
// Project Name : WodiLib
// File Name    : ScrollScreen.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・画面スクロール
    /// </summary>
    [Serializable]
    public class ScrollScreen : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormatMove = "■画面スクロール：{0} {1} {2}{3}";
        private const string EventCommandSentenceFormatLock = "■画面スクロール：{0}";

        private const string EventCommandSentenceFormatMoveValue = "右方向:{0} 下方向:{1}";
        private const string EventCommandSentenceFormatNotMoveValue = "";

        private const string EventCommandSentenceWait = "[ｳｪｲﾄ]";
        private const string EventCommandSentenceNoWait = "";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ScrollScreen;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x04;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 3)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                {
                    var byte0 = (byte) (Speed.Code + ScrollType.Code);
                    byte byte1 = 0x00;
                    {
                        // byte1
                        if (IsWaitForComplete) byte1 += FlgWaitForComplete;
                        if (IsPixel) byte1 += FlgPixel;
                    }
                    return new byte[] {byte0, byte1, 0x00, 0x00}.ToInt32(Endian.Environment);
                }

                case 2:
                    return X;

                case 3:
                    return Y;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 3)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    Speed = ScrollSpeed.FromByte((byte) (bytes[0] & 0xF0));
                    ScrollType = ScrollScreenType.FromByte((byte) (bytes[0] & 0x0F));
                    IsWaitForComplete = (bytes[1] & FlgWaitForComplete) != 0;
                    IsPixel = (bytes[1] & FlgPixel) != 0;
                    return;
                }

                case 2:
                    X = value;
                    return;

                case 3:
                    Y = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 3, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetSafetyStringVariable(int index)
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
        public override void SetSafetyStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (ScrollType.IsLockType)
            {
                return string.Format(EventCommandSentenceFormatLock,
                    ScrollType.EventCommandSentence);
            }

            string moveStr;
            string waitStr;
            if (ScrollType.IsMoveType)
            {
                var moveXStr = resolver.GetNumericVariableAddressStringIfVariableAddress(X, type, desc);
                var moveYStr = resolver.GetNumericVariableAddressStringIfVariableAddress(Y, type, desc);
                moveStr = string.Format(EventCommandSentenceFormatMoveValue,
                    moveXStr, moveYStr);
                // ウェイトする場合でも「ピクセル単位」がONだとウェイト文字列が表示されない
                waitStr = IsWaitForComplete && !IsPixel
                    ? EventCommandSentenceWait
                    : EventCommandSentenceNoWait;
            }
            else
            {
                moveStr = EventCommandSentenceFormatNotMoveValue;
                waitStr = EventCommandSentenceNoWait;
            }

            return string.Format(EventCommandSentenceFormatMove,
                ScrollType.EventCommandSentence, Speed.EventCommandSentence, moveStr, waitStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ScrollScreenType scrollType = ScrollScreenType.LockScroll;

        /// <summary>[NotNull] 画面スクロール種別</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ScrollScreenType ScrollType
        {
            get => scrollType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ScrollType)));
                scrollType = value;
            }
        }

        private ScrollSpeed speed = ScrollSpeed.Instant;

        /// <summary>[NotNull] 速度</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ScrollSpeed Speed
        {
            get => speed;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Speed)));
                speed = value;
            }
        }

        /// <summary>X</summary>
        public int X { get; set; }

        /// <summary>Y</summary>
        public int Y { get; set; }

        /// <summary>完了まで待機</summary>
        public bool IsWaitForComplete { get; set; }

        /// <summary>ピクセル単位</summary>
        public bool IsPixel { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgWaitForComplete = 0x01;
        private const byte FlgPixel = 0x02;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     VersionCheck
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public override void OutputVersionWarningLogIfNeed()
        {
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_20))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_20();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.20未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_20()
        {
            if (Speed == ScrollSpeed.Speed32)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting($"{nameof(ScrollScreen)}.{nameof(Speed)}",
                    $"{ScrollSpeed.Speed32}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_20));
            }

            if (Speed == ScrollSpeed.Speed64)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting($"{nameof(ScrollScreen)}.{nameof(Speed)}",
                    $"{ScrollSpeed.Speed64}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_20));
            }
        }
    }
}