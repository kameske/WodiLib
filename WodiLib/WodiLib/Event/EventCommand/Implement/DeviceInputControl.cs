// ========================================
// Project Name : WodiLib
// File Name    : DeviceInputControl.cs
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
    /// イベントコマンド・キー入力禁止（デバイス入力）
    /// </summary>
    [Serializable]
    public class DeviceInputControl : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormatKeyboard
            = "■キー入力禁止/許可: {0}  コード[ {2} ] [ {1} ]";

        private const string EventCommandSentenceFormatEtc
            = "■キー入力禁止/許可: {0}   [ {1} ]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.DeviceInputControl;

        /// <inheritdoc />
        public override byte NumberVariableCount =>
            (byte) (keyType == DeviceInputControlType.KeyboardKey ? 0x03 : 0x02);

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
        /// <param name="index">[Range(0, 1～2)] インデックス</param>
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
                    var byte3 = EventCommandConstant.KeyInputControl.TargetCode.Device;
                    return new byte[] {KeyType.Code, ControlType.Code, 0x00, byte3}.ToInt32(Endian.Environment);
                case 2:
                    if (keyType != DeviceInputControlType.KeyboardKey)
                        throw new ArgumentOutOfRangeException(
                            ErrorMessage.OutOfRange(nameof(index), 1, 1, index));
                    return KeyCode;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 1～2)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            if (index < 1 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    KeyType = DeviceInputControlType.FromByte(bytes[0]);
                    ControlType = DeviceKeyInputControlType.FromByte(bytes[1]);
                    return;
                }

                case 2:
                {
                    if (keyType != DeviceInputControlType.KeyboardKey)
                        throw new ArgumentOutOfRangeException(
                            ErrorMessage.OutOfRange(nameof(index), 1, 1, index));
                    KeyCode = value;
                    return;
                }

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
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
            if (KeyType == DeviceInputControlType.KeyboardKey)
            {
                return string.Format(EventCommandSentenceFormatKeyboard,
                    KeyType.EventCommandSentence, ControlType.EventCommandSentence,
                    KeyCode);
            }

            return string.Format(EventCommandSentenceFormatEtc,
                KeyType.EventCommandSentence, ControlType.EventCommandSentence);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DeviceInputControlType keyType = DeviceInputControlType.AllDevices;

        /// <summary>[NotNull] 入力制御タイプ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DeviceInputControlType KeyType
        {
            get => keyType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(KeyType)));
                keyType = value;
            }
        }

        /// <summary>キーコード</summary>
        public int KeyCode { get; set; }

        private DeviceKeyInputControlType controlType = DeviceKeyInputControlType.Allow;

        /// <summary>[NotNull] コントロール</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DeviceKeyInputControlType ControlType
        {
            get => controlType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ControlType)));
                controlType = value;
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
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(DeviceInputControl)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_00));
        }
    }
}