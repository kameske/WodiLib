// ========================================
// Project Name : WodiLib
// File Name    : KeyInputKeyboard.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・キー入力（キーボード）
    /// </summary>
    [Serializable]
    public class KeyInputKeyboard : KeyInputBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>キー入力種別フラグ値</summary>
        private readonly byte FlgKeyInputType = EventCommandConstant.KeyInput.Type.Keyboard;

        private const string EventCommandSentenceFormat
            = "{1}ｷｰﾎﾞｰﾄﾞ(100～){0}";

        private const string EventCommandSentenceOnlyTarget = " [ｷｰｺｰﾄﾞ[{0}]のみ判定]{1}";
        private const string EventCommandSentenceAllTarget = "";

        private const string EventCommandSentenceWait = "[入力待ち] ";
        private const string EventCommandSentenceNonWait = "";

        private const string EventCommandSentenceTargetKeyName = " ( {0}ｷｰ ) ";
        private const string EventCommandSentenceNonTargetKeyName = "";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                if (VersionConfig.IsGreaterVersion(WoditorVersion.Ver2_00)) return 0x04;

                // ver 2.00 より前
                return IsOnlySpecificKey ? (byte) 0x04 : (byte) 0x03;
            }
        }

        /// <inheritdoc />
        public override byte NumberVariableCountMin => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

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
                    return LeftSide;

                case 2:
                {
                    var byte0 = (byte) (IsWaitForInput ? FlgWaitForInput : 0x00);
                    byte0 += targetCode;
                    var byte1 = (byte) (IsOnlySpecificKey ? FlgOnlySpecificKey : 0x00);
                    byte1 += FlgKeyInputType;
                    return new byte[] {byte0, byte1, 0x00, 0x00}.ToInt32(Endian.Environment);
                }

                case 3:
                    return SpecificKeyCode;

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
                    LeftSide = value;
                    return;

                case 2:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    IsWaitForInput = (bytes[0] & FlgWaitForInput) != 0;
                    targetCode = (byte) (bytes[0] - (byte) (IsWaitForInput ? FlgWaitForInput : 0));
                    IsOnlySpecificKey = (byte) (bytes[1] - FlgKeyInputType) != 0;
                    return;
                }

                case 3:
                    SpecificKeyCode = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 3, index));
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            string targetStr;
            if (IsOnlySpecificKey)
            {
                var keyCodeStr = resolver.GetNumericVariableAddressStringIfVariableAddress(SpecificKeyCode, type, desc);

                var keyName = SpecificKeyCode.IsVariableAddressSimpleCheck()
                    ? EventCommandSentenceNonTargetKeyName
                    : KeyboardCode.IsKeyCode(SpecificKeyCode)
                        ? string.Format(EventCommandSentenceTargetKeyName,
                            KeyboardCode.GetKeyName(SpecificKeyCode))
                        : EventCommandSentenceNonTargetKeyName;

                targetStr = string.Format(EventCommandSentenceOnlyTarget,
                    keyCodeStr, keyName);
            }
            else
            {
                targetStr = EventCommandSentenceAllTarget;
            }

            var waitStr = IsWaitForInput
                ? EventCommandSentenceWait
                : EventCommandSentenceNonWait;

            return string.Format(EventCommandSentenceFormat,
                targetStr, waitStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool isOnlySpecificKey;

        /// <summary>特定キーのみ判定フラグ</summary>
        public bool IsOnlySpecificKey
        {
            get => isOnlySpecificKey;
            set
            {
                isOnlySpecificKey = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private int specificKeyCode;

        /// <summary>特定キーコード</summary>
        public int SpecificKeyCode
        {
            get => specificKeyCode;
            set
            {
                specificKeyCode = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>取得対象（内部保持用）</summary>
        private byte targetCode;
    }
}