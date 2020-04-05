// ========================================
// Project Name : WodiLib
// File Name    : SyntheticVoice.cs
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
    /// イベントコマンド・合成音声
    /// </summary>
    [Serializable]
    public class SyntheticVoice : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■合成音声：[{0}] 速度[{1}]％/音量[{2}]％/声の高さ[{3}]％/遅延[{4}]ﾌﾚｰﾑ";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.SyntheticVoice;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x05;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x01;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.BrightGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 4)] インデックス</param>
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
                    return PlaybackSpeed;

                case 2:
                    return Volume;

                case 3:
                    return VoiceTone;

                case 4:
                    return Delay;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 4, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 4)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    PlaybackSpeed = value;
                    return;

                case 2:
                    Volume = value;
                    return;

                case 3:
                    VoiceTone = value;
                    return;

                case 4:
                    Delay = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 4, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 0)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
        {
            if (index == 0) return PlaybackText;
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
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            if (index == 0)
            {
                PlaybackText = value;
                return;
            }

            throw new ArgumentOutOfRangeException(
                ErrorMessage.OutOfRange(nameof(index), 0, 0, index));
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var speedStr = resolver.GetNumericVariableAddressStringIfVariableAddress(PlaybackSpeed, type, desc);
            var volStr = resolver.GetNumericVariableAddressStringIfVariableAddress(Volume, type, desc);
            var toneStr = resolver.GetNumericVariableAddressStringIfVariableAddress(VoiceTone, type, desc);
            var delayStr = resolver.GetNumericVariableAddressStringIfVariableAddress(Delay, type, desc);

            return string.Format(EventCommandSentenceFormat,
                PlaybackText, speedStr, volStr, toneStr, delayStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int playbackSpeed;

        /// <summary>読む速さ</summary>
        public int PlaybackSpeed
        {
            get => playbackSpeed;
            set
            {
                playbackSpeed = value;
                NotifyPropertyChanged();
            }
        }

        private int volume;

        /// <summary>音量</summary>
        public int Volume
        {
            get => volume;
            set
            {
                volume = value;
                NotifyPropertyChanged();
            }
        }

        private int voiceTone;

        /// <summary>声の高さ</summary>
        public int VoiceTone
        {
            get => voiceTone;
            set
            {
                voiceTone = value;
                NotifyPropertyChanged();
            }
        }

        private int delay;

        /// <summary>再生遅延</summary>
        public int Delay
        {
            get => delay;
            set
            {
                delay = value;
                NotifyPropertyChanged();
            }
        }

        private string playbackText = "";

        /// <summary>[NotNull] 再生文章</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string PlaybackText
        {
            get => playbackText;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PlaybackText)));
                playbackText = value;
                NotifyPropertyChanged();
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
            if (VersionConfig.IsGreaterVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_GreaterVer2_00();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.00以上 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_GreaterVer2_00()
        {
            Logger.Warning(VersionWarningMessage.NotGreaterInCommand($"{nameof(SyntheticVoice)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_00));
        }
    }
}