// ========================================
// Project Name : WodiLib
// File Name    : PictureDelayReset.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・ピクチャ（ディレイリセット）
    /// </summary>
    [Serializable]
    public class PictureDelayReset : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■ﾋﾟｸﾁｬ[ﾃﾞｨﾚｲ内容ﾘｾｯﾄ]：{0} {1}";

        private const string EventCommandSentenceSingleTarget = "";
        private const string EventCommandSentenceMultiTarget = "～ {0} ";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Picture;

        /// <inheritdoc />
        public override byte NumberVariableCount => (byte) (IsMultiTarget ? 0x04 : 0x03);

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x03;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.BrightGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 2～3)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                {
                    var byte0 = (byte) (ExecCode + intValue1_0UpperCode);
                    var byte1 = intValue1_1Code;
                    var byte2 = intValue1_2Code;
                    var byte3 = (byte) ((byte) (IsMultiTarget ? 0x01 : 0x00) + intValue1_3EtcCode);
                    return new[] {byte0, byte1, byte2, byte3}.ToInt32(Endian.Environment);
                }

                case 2:
                    return PictureNumber;

                case 3:
                    return SequenceValue;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 2～3)] インデックス</param>
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
                    intValue1_0UpperCode = (byte) (bytes[0] & 0xF0);
                    intValue1_1Code = bytes[1];
                    intValue1_2Code = bytes[2];
                    IsMultiTarget = (byte) (bytes[3] & FlagMultiTarget) != 0;
                    intValue1_3EtcCode = (byte) (bytes[3] - (byte) (IsMultiTarget ? FlagMultiTarget : 0));
                    return;
                }

                case 2:
                    PictureNumber = value;
                    return;

                case 3:
                    SequenceValue = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
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
            var pictureNumberStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PictureNumber, type, desc);
            string pictureEndStr;
            if (IsMultiTarget)
            {
                var pictureEndVarName =
                    resolver.GetNumericVariableAddressStringIfVariableAddress(SequenceValue, type, desc);
                pictureEndStr = string.Format(EventCommandSentenceMultiTarget, pictureEndVarName);
            }
            else
            {
                pictureEndStr = EventCommandSentenceSingleTarget;
            }

            return string.Format(EventCommandSentenceFormat,
                pictureNumberStr, pictureEndStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int pictureNumber;

        /// <summary>ピクチャ番号</summary>
        public int PictureNumber
        {
            get => pictureNumber;
            set
            {
                pictureNumber = value;
                NotifyPropertyChanged();
            }
        }

        private bool isMultiTarget;

        /// <summary>連続ピクチャ操作フラグ</summary>
        public bool IsMultiTarget
        {
            get => isMultiTarget;
            set
            {
                isMultiTarget = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(NumberVariableCount));
            }
        }

        private int sequenceValue;

        /// <summary>連続ピクチャ数</summary>
        public int SequenceValue
        {
            get => sequenceValue;
            set
            {
                sequenceValue = value;
                NotifyPropertyChanged();
            }
        }

        private const byte ExecCode = 0x03;

        private const byte FlagMultiTarget = 0x01;

        // 以下値記憶用変数
        private byte intValue1_0UpperCode;
        private byte intValue1_1Code;
        private byte intValue1_2Code;
        private byte intValue1_3EtcCode;
    }
}