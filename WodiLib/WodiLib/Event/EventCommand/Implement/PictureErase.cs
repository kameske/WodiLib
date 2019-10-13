// ========================================
// Project Name : WodiLib
// File Name    : PictureErase.cs
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
    /// イベントコマンド・ピクチャ（消去）
    /// </summary>
    public class PictureErase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Picture;

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                if (IsMultiTarget) return 0x07;
                return 0x05;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x04;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 6)] インデックス</param>
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
                    var byte0 = (byte) (FlgExecCode + intValue1_0UpperCode);
                    var byte1 = intValue1_1Code;
                    var byte2 = intValue1_2Code;
                    var byte3 = (byte) ((byte) (IsMultiTarget ? 0x01 : 0x00) + intValue1_3EtcCode);
                    return new[] {byte0, byte1, byte2, byte3}.ToInt32(Endian.Environment);
                }

                case 2:
                    return PictureNumber;

                case 3:
                    return ProcessTime;

                case 4:
                    return Delay;

                case 5:
                    return 0;

                case 6:
                    return SequenceValue;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 6, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 6)] インデックス</param>
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
                    ProcessTime = value;
                    return;

                case 4:
                    Delay = value;
                    return;

                case 5:
                    return;

                case 6:
                    SequenceValue = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 6, index));
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

        /// <summary>ピクチャ番号</summary>
        public int PictureNumber { get; set; }

        /// <summary>連続ピクチャ操作フラグ</summary>
        public bool IsMultiTarget { get; set; }

        /// <summary>連続ピクチャ数</summary>
        public int SequenceValue { get; set; }

        /// <summary>処理時間</summary>
        public int ProcessTime { get; set; }

        /// <summary>発動ディレイ</summary>
        public int Delay { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlagMultiTarget = 0x01;
        private readonly byte FlgExecCode = EventCommandConstant.PictureShow.ExecCode.Erase;

        // 以下値記憶用変数
        private byte intValue1_0UpperCode;
        private byte intValue1_1Code;
        private byte intValue1_2Code;
        private byte intValue1_3EtcCode;
    }
}