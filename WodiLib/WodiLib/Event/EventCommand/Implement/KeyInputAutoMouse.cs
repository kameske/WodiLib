// ========================================
// Project Name : WodiLib
// File Name    : KeyInputAutoMouse.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Text;
using WodiLib.Cmn;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・自動キー入力（マウス）
    /// </summary>
    [Serializable]
    public class KeyInputAutoMouse : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■自動キー入力: マウス  {0}";

        private class InputTypeString
        {
            public const string LeftClick = "左クリック  ";
            public const string RightClick = "右クリック  ";
            public const string CenterClick = "中クリック  ";

            private const string MouseWheelSentence = "[ マウスホイール:  +{0} ]";
            private const string MouseMoveSentence = "[ ﾏｳｽX:  {0} / Y:  {1} ]";

            public static string MouseWheel(int value,
                EventCommandSentenceResolver resolver, EventCommandSentenceType type,
                EventCommandSentenceResolveDesc desc)
            {
                var varName = value.IsVariableAddressSimpleCheck()
                    ? resolver.GetNumericVariableAddressStringIfVariableAddress(value, type, desc)
                    : value.ToString();

                return string.Format(MouseWheelSentence, varName);
            }

            public static string MouseMove(int x, int y,
                EventCommandSentenceResolver resolver, EventCommandSentenceType type,
                EventCommandSentenceResolveDesc desc)
            {
                var xName = x.IsVariableAddressSimpleCheck()
                    ? resolver.GetNumericVariableAddressStringIfVariableAddress(x, type, desc)
                    : x.ToString();
                var yName = y.IsVariableAddressSimpleCheck()
                    ? resolver.GetNumericVariableAddressStringIfVariableAddress(y, type, desc)
                    : y.ToString();

                return string.Format(MouseMoveSentence, xName, yName);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.KeyInputAuto;

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                if (IsInputWheel) return 0x04;
                if (IsInputPosition) return 0x04;
                return 0x02;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x02;

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
                    var byte3 = EventCommandConstant.KeyInputAuto.Type.Mouse;
                    return new byte[] {inputFlag.ToByte(), 0x00, 0x00, byte3}.ToInt32(Endian.Environment);

                case 2:
                    if (IsInputPosition) return PositionX;
                    if (IsInputWheel) return MouseWheel;
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));

                case 3:
                    if (IsInputPosition) return PositionY;
                    if (IsInputWheel) return 0;
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 2, index));

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
                    inputFlag = new InputFlag(bytes[0]);
                    return;
                }

                case 2:
                    if (IsInputPosition) PositionX = value;
                    else if (IsInputWheel) MouseWheel = value;
                    return;

                case 3:
                    if (IsInputPosition) PositionY = value;
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
            var builder = new StringBuilder();

            if (IsInputLeftClick) builder.Append(InputTypeString.LeftClick);
            if (IsInputRightClick) builder.Append(InputTypeString.RightClick);
            if (IsInputCenterClick) builder.Append(InputTypeString.CenterClick);
            if (IsInputWheel) builder.Append(InputTypeString.MouseWheel(MouseWheel, resolver, type, desc));
            if (IsInputPosition) builder.Append(InputTypeString.MouseMove(PositionX, PositionY, resolver, type, desc));

            return string.Format(EventCommandSentenceFormat, builder);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private InputFlag inputFlag = new InputFlag();

        /// <summary>左クリック</summary>
        public bool IsInputLeftClick
        {
            get => inputFlag.IsInputLeftClick;
            set
            {
                inputFlag.IsInputLeftClick = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>右クリック</summary>
        public bool IsInputRightClick
        {
            get => inputFlag.IsInputRightClick;
            set
            {
                inputFlag.IsInputRightClick = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>中クリック</summary>
        public bool IsInputCenterClick
        {
            get => inputFlag.IsInputCenterClick;
            set
            {
                inputFlag.IsInputCenterClick = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>ホイール入力</summary>
        public bool IsInputWheel
        {
            get => inputFlag.IsInputWheel;
            set
            {
                inputFlag.IsInputWheel = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        /// <summary>左クリック</summary>
        public int MouseWheel { get; set; }

        /// <summary>位置</summary>
        public bool IsInputPosition
        {
            get => inputFlag.IsInputPosition;
            set
            {
                inputFlag.IsInputPosition = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private int positionX;

        /// <summary>X座標</summary>
        public int PositionX
        {
            get => positionX;
            set
            {
                positionX = value;
                NotifyPropertyChanged();
            }
        }

        private int positionY;

        /// <summary>Y座標</summary>
        public int PositionY
        {
            get => positionY;
            set
            {
                positionY = value;
                NotifyPropertyChanged();
            }
        }

        private class InputFlag
        {
            // ホイール入力と位置指定は同時には不可能

            /// <summary>左クリック</summary>
            public bool IsInputLeftClick { get; set; }

            /// <summary>右クリック</summary>
            public bool IsInputRightClick { get; set; }

            /// <summary>中クリック</summary>
            public bool IsInputCenterClick { get; set; }

            private bool isInputWheel;

            /// <summary>ホイール入力</summary>
            public bool IsInputWheel
            {
                get => isInputWheel;
                set
                {
                    isInputWheel = value;
                    if (value) isInputPosition = false;
                }
            }

            private bool isInputPosition;

            /// <summary>位置</summary>
            public bool IsInputPosition
            {
                get => isInputPosition;
                set
                {
                    isInputPosition = value;
                    if (value) isInputWheel = false;
                }
            }

            public InputFlag(byte flag = 0x00)
            {
                IsInputLeftClick = (flag & FlgLeftClick) != 0;
                IsInputRightClick = (flag & FlgRightClick) != 0;
                IsInputCenterClick = (flag & FlgCenterClick) != 0;
                IsInputWheel = (flag & FlgMouseWheel) != 0;
                IsInputPosition = (flag & FlgPosition) != 0;
            }

            private const byte FlgLeftClick = 0x01;
            private const byte FlgRightClick = 0x02;
            private const byte FlgCenterClick = 0x04;
            private const byte FlgPosition = 0x08;
            private const byte FlgMouseWheel = 0x10;

            public byte ToByte()
            {
                byte result = 0x00;
                if (IsInputLeftClick) result += FlgLeftClick;
                if (IsInputRightClick) result += FlgRightClick;
                if (IsInputCenterClick) result += FlgCenterClick;
                if (IsInputWheel) result += FlgMouseWheel;
                if (IsInputPosition) result += FlgPosition;
                return result;
            }
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
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(KeyInputAutoMouse)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_00));
        }
    }
}