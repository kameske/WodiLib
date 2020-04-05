// ========================================
// Project Name : WodiLib
// File Name    : KeyInputAutoBasic.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Text;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// 自動キー入力（基本入力）実装
    /// </summary>
    [Serializable]
    public class KeyInputAutoBasic : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■自動キー入力:   {0}";

        private static class InputKeyString
        {
            public const string Ok = "決定  ";
            public const string Cancel = "ｷｬﾝｾﾙ  ";
            public const string Sub = "サブ  ";
            public const string Down = "↓ｷｰ  ";
            public const string Left = "←ｷｰ  ";
            public const string Right = "→ｷｰ  ";
            public const string Up = "↑ｷｰ  ";
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.KeyInputAuto;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x02;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。\r\na
        /// </summary>
        /// <param name="index">[Range(0, 1)] インデックス</param>
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
                    var byte3 = EventCommandConstant.KeyInputAuto.Type.Basic;
                    return new byte[] {inputFlag.ToByte(), 0x00, 0x00, byte3}.ToInt32(Endian.Environment);

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
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    inputFlag = new InputFlag(bytes[0]);
                    return;
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 1, index));
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private InputFlag inputFlag = new InputFlag();

        /// <summary>自動キー入力（決定）</summary>
        public bool IsInputOk
        {
            get => inputFlag.IsInputOk;
            set
            {
                inputFlag.IsInputOk = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自動キー入力（キャンセル）</summary>
        public bool IsInputCancel
        {
            get => inputFlag.IsInputCancel;
            set
            {
                inputFlag.IsInputCancel = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自動キー入力（サブ）</summary>
        public bool IsInputSub
        {
            get => inputFlag.IsInputSub;
            set
            {
                inputFlag.IsInputSub = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自動キー入力（上）</summary>
        public bool IsInputUp
        {
            get => inputFlag.IsInputUp;
            set
            {
                inputFlag.IsInputUp = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自動キー入力（左）</summary>
        public bool IsInputLeft
        {
            get => inputFlag.IsInputLeft;
            set
            {
                inputFlag.IsInputLeft = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自動キー入力（右）</summary>
        public bool IsInputRight
        {
            get => inputFlag.IsInputRight;
            set
            {
                inputFlag.IsInputRight = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自動キー入力（下）</summary>
        public bool IsInputDown
        {
            get => inputFlag.IsInputDown;
            set
            {
                inputFlag.IsInputDown = value;
                NotifyPropertyChanged();
            }
        }

        private class InputFlag
        {
            /// <summary>自動キー入力（決定）</summary>
            public bool IsInputOk { get; set; }

            /// <summary>自動キー入力（キャンセル）</summary>
            public bool IsInputCancel { get; set; }

            /// <summary>自動キー入力（サブ）</summary>
            public bool IsInputSub { get; set; }

            /// <summary>自動キー入力（上）</summary>
            public bool IsInputUp { get; set; }

            /// <summary>自動キー入力（左）</summary>
            public bool IsInputLeft { get; set; }

            /// <summary>自動キー入力（右）</summary>
            public bool IsInputRight { get; set; }

            /// <summary>自動キー入力（下）</summary>
            public bool IsInputDown { get; set; }

            public InputFlag(byte flg = 0x00)
            {
                IsInputOk = (flg & FlgOk) != 0;
                IsInputCancel = (flg & FlgCancel) != 0;
                IsInputSub = (flg & FlgSub) != 0;
                IsInputUp = (flg & FlgUp) != 0;
                IsInputLeft = (flg & FlgLeft) != 0;
                IsInputRight = (flg & FlgRight) != 0;
                IsInputDown = (flg & FlgDown) != 0;
            }

            private const int FlgOk = 0x01;
            private const int FlgCancel = 0x02;
            private const int FlgSub = 0x04;
            private const int FlgDown = 0x10;
            private const int FlgLeft = 0x20;
            private const int FlgRight = 0x40;
            private const int FlgUp = 0x80;

            public byte ToByte()
            {
                byte result = 0x00;
                if (IsInputOk) result += FlgOk;
                if (IsInputCancel) result += FlgCancel;
                if (IsInputSub) result += FlgSub;
                if (IsInputUp) result += FlgUp;
                if (IsInputLeft) result += FlgLeft;
                if (IsInputRight) result += FlgRight;
                if (IsInputDown) result += FlgDown;
                return result;
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var builder = new StringBuilder();
            if (IsInputOk) builder.Append(InputKeyString.Ok);
            if (IsInputCancel) builder.Append(InputKeyString.Cancel);
            if (IsInputSub) builder.Append(InputKeyString.Sub);
            if (IsInputDown) builder.Append(InputKeyString.Down);
            if (IsInputLeft) builder.Append(InputKeyString.Left);
            if (IsInputRight) builder.Append(InputKeyString.Right);
            if (IsInputUp) builder.Append(InputKeyString.Up);
            return string.Format(EventCommandSentenceFormat,
                builder);
        }

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
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_00();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.00未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_00()
        {
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(KeyInputAutoBasic)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_00));
        }
    }
}