// ========================================
// Project Name : WodiLib
// File Name    : SoundBase.cs
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
    /// イベントコマンド・サウンドベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SoundBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                if (Specification == AudioSpecification.FileName)
                {
                    if (AudioType == AudioType.Bgm || AudioType == AudioType.Bgs) return 0x08;
                    return 0x07;
                }

                if (Specification == AudioSpecification.SdbDirect)
                {
                    if (ExecCode == ExecType.PlayBack) return 0x05;
                    return 0x02;
                }

                if (ExecCode == ExecType.PlayBack) return 0x05;
                return 0x04;
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount
            => (byte) (Specification == AudioSpecification.FileName ? 0x01 : 0x00);

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 4～7)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
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
                    var byte0 = (byte) (AudioType.Code + ExecCode);
                    var idBytes = SoundId.ToBytes(Endian.Little);
                    var byte1 = idBytes[0];
                    var byte2 = idBytes[1];
                    var byte3 = Specification.Code;
                    return new[] {byte0, byte1, byte2, byte3}.ToInt32(Endian.Little);
                }
                case 2:
                    return FadeTime;

                case 3:
                    return Specification == AudioSpecification.SdbRefer
                        ? NumberVariable
                        : 0;
                case 4:
                    return StartTime;

                case 5:
                    return Volume;

                case 6:
                    return Frequency;

                case 7:
                    return LoopPoint;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 4～7)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Little);
                    AudioType = AudioType.FromByte((byte) (bytes[0] & 0xF0));
                    // 2,3バイト目がともに 0xFF の場合、データID=-1（停止）
                    if (bytes[1] == 0xFF && bytes[2] == 0xFF)
                    {
                        SoundId = -1;
                    }
                    else
                    {
                        SoundId = new byte[] {bytes[1], bytes[2], 0x00, 0x00}.ToInt32(Endian.Little);
                    }

                    Specification = AudioSpecification.FromByte(bytes[3]);
                    return;
                }
                case 2:
                    FadeTime = value;
                    return;

                case 3:
                    NumberVariable = value;
                    return;

                case 4:
                    StartTime = value;
                    return;

                case 5:
                    Volume = value;
                    return;

                case 6:
                    Frequency = value;
                    return;

                case 7:
                    LoopPoint = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 0)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetStringVariable(int index)
        {
            if (index == 0) return AudioFileName;
            throw new ArgumentOutOfRangeException(
                ErrorMessage.OutOfRange(nameof(index), 0, 0, index));
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 0)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetStringVariable(int index, string value)
        {
            if (value == null) throw new ArgumentNullException(ErrorMessage.NotNull(value));
            switch (index)
            {
                case 0:
                    AudioFileName = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 0, index));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private AudioType audioType = AudioType.Bgm;

        /// <summary>[NotNull] 処理対象</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public AudioType AudioType
        {
            get => audioType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AudioType)));
                audioType = value;
            }
        }

        /// <summary>BGM/BGS/SEデータID</summary>
        public int SoundId { get; set; }

        private AudioSpecification specification = AudioSpecification.FileName;

        /// <summary>[NotNull] 指定方法</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public AudioSpecification Specification
        {
            get => specification;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Specification)));
                specification = value;
            }
        }

        /// <summary>フェード時間</summary>
        public int FadeTime { get; set; }

        /// <summary>参照変数</summary>
        public int NumberVariable { get; set; }

        /// <summary>再生箇所</summary>
        public int StartTime { get; set; }

        /// <summary>音量</summary>
        public int Volume { get; set; } = DefaultVolume;

        /// <summary>周波数</summary>
        public int Frequency { get; set; } = DefaultFrequency;

        /// <summary>ループ位置（MIDIの場合、キー）</summary>
        public int LoopPoint { get; set; } = DefaultLoopPoint;

        private string audioFileName = "";

        /// <summary>[NotNull] 音声ファイル名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string AudioFileName
        {
            get => audioFileName;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AudioFileName)));
                audioFileName = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>処理内容コード値</summary>
        protected abstract ExecType ExecCode { get; }

        /// <summary>
        /// 処理種別
        /// </summary>
        protected enum ExecType
        {
            /// <summary>通常再生</summary>
            PlayBack = 0,

            /// <summary>先読み</summary>
            Preload = 1,

            /// <summary>手動開放</summary>
            ReleaseManual = 2,

            /// <summary>全開放</summary>
            ReleaseAll = 3
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const int DefaultLoopPoint = 0;
        private const int DefaultVolume = 100;
        private const int DefaultFrequency = 100;
    }
}