// ========================================
// Project Name : WodiLib
// File Name    : StandardKeyInputControl.cs
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
    /// イベントコマンド・キー入力禁止（基本入力）
    /// </summary>
    [Serializable]
    public class StandardKeyInputControl : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■キー入力禁止/許可:   {0} {1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.StandardKeyInputControl;

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
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
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
                {
                    var bytes = new byte[4];
                    bytes[0] = controlKeySet.ToByte();
                    bytes[1] = KeyType.Code;
                    bytes[2] = 0x00;
                    bytes[3] = EventCommandConstant.KeyInputControl.TargetCode.Basic;
                    return bytes.ToInt32(Endian.Little);
                }

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
                    var bytes = value.ToBytes(Endian.Little);
                    controlKeySet = new ControlStandardKeySet(bytes[0]);
                    KeyType = StandardKeyInputControlType.ForByte(bytes[1]);
                    break;
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

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var targetKeyStr = controlKeySet.MakeEventCommandTargetKeySentence();

            return string.Format(EventCommandSentenceFormat,
                targetKeyStr, KeyType.EventCommandSentence);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private StandardKeyInputControlType keyType = StandardKeyInputControlType.OkMoveOkInput;

        /// <summary>[NotNull] 入力制御タイプ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public StandardKeyInputControlType KeyType
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

        /// <summary>制御（決定）</summary>
        public bool IsControlOk
        {
            get => controlKeySet.Ok;
            set => controlKeySet.Ok = value;
        }

        /// <summary>制御（キャンセル）</summary>
        public bool IsControlCancel
        {
            get => controlKeySet.Cancel;
            set => controlKeySet.Cancel = value;
        }

        /// <summary>制御（サブ）</summary>
        public bool IsControlSub
        {
            get => controlKeySet.Sub;
            set => controlKeySet.Sub = value;
        }

        /// <summary>制御（上）</summary>
        public bool IsControlUp
        {
            get => controlKeySet.Up;
            set => controlKeySet.Up = value;
        }

        /// <summary>制御（左）</summary>
        public bool IsControlLeft
        {
            get => controlKeySet.Left;
            set => controlKeySet.Left = value;
        }

        /// <summary>制御（右）</summary>
        public bool IsControlRight
        {
            get => controlKeySet.Right;
            set => controlKeySet.Right = value;
        }

        /// <summary>制御（下）</summary>
        public bool IsControlDown
        {
            get => controlKeySet.Down;
            set => controlKeySet.Down = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ControlStandardKeySet controlKeySet = new ControlStandardKeySet();

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
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(StandardKeyInputControl)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_00));
        }
    }
}