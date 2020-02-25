// ========================================
// Project Name : WodiLib
// File Name    : SoundBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Database;
using WodiLib.Project;
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
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■サウンド：{0}";

        private const string EventCommandSentenceFormatExecBaseDirectFile
            = "{0} ﾌｧｲﾙ[{1}] 音 {2}％ 周 {3}％ {4}{5}";

        private const string EventCommandSentenceFormatExecBaseStop
            = "{0} 停止 {1}";

        private const string EventCommandSentenceFormatStopDuration
            = "  /  {0}：{1}ﾌﾚｰﾑ";

        private const string EventCommandSentenceFormatExecBaseId
            = "{0}{1}「{2}」{3}";

        private const string EventCommandSentenceFormatExecBaseVariableAddress
            = "{0} 変数[{1}] {2}";

        private const string EventCommandSentenceFormatLoop = "ﾙ {0}ms ";
        private const string EventCommandSentenceFormatNotLoop = "";

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
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x02;

        /// <inheritdoc />
        /// <summary>文字列変数最小個数</summary>
        public override byte StringVariableCountMin => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.BrightGreen;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 4～7)] インデックス</param>
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
        public override void SetSafetyNumberVariable(int index, int value)
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

                    // Ver2.24以降、"停止"の場合は4バイト目も 0xFF が格納される。
                    // この場合、指定方法はDB直接指定のみ
                    Specification = bytes[3] != 0xFF
                        ? AudioSpecification.FromByte(bytes[3])
                        : AudioSpecification.SdbDirect;

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
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 0)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
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
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
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

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (!UseDetailStringByEventCommandSentence) return MakeEventCommandSentenceSimple(resolver, type, desc);

            var execMainStr = MakeEventCommandExecMainSentence(resolver, type, desc);
            string execStr;
            if (Specification == AudioSpecification.FileName)
            {
                execStr = MakeEventCommandExecSentenceForFileName(resolver, type, desc, execMainStr);
            }
            else if (Specification == AudioSpecification.SdbDirect)
            {
                execStr = MakeEventCommandExecSentenceForSdbDirect(resolver, type, desc, execMainStr);
            }
            else
            {
                execStr = MakeEventCommandExecSentenceForSdbRefer(resolver, type, desc, execMainStr);
            }

            return string.Format(EventCommandSentenceFormat, execStr);
        }

        private string MakeEventCommandSentenceSimple(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var mainSentence = MakeEventCommandExecMainSentence(resolver, type, desc);
            return string.Format(EventCommandSentenceFormat, mainSentence);
        }

        /// <summary>
        /// イベントコマンド文字列の実行内容部分を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列の実行内容部分</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeEventCommandExecMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);

        private string MakeEventCommandExecSentenceForFileName(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc, string execMainStr)
        {
            var volStr = resolver.GetNumericVariableAddressStringIfVariableAddress(Volume, type, desc);
            var frequencyStr = resolver.GetNumericVariableAddressStringIfVariableAddress(Frequency, type, desc);
            var loopStr = MakeEventCommandLoopSentence(resolver, type, desc);

            return string.Format(EventCommandSentenceFormatExecBaseDirectFile,
                AudioType.EventCommandSentence, AudioFileName, volStr, frequencyStr,
                loopStr, execMainStr);
        }

        private string MakeEventCommandLoopSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (Specification != AudioSpecification.FileName)
            {
                return EventCommandSentenceFormatNotLoop;
            }

            if (AudioType == AudioType.Se)
            {
                return EventCommandSentenceFormatNotLoop;
            }

            var loopVarStr = resolver.GetNumericVariableAddressStringIfVariableAddress(LoopPoint, type, desc);
            return string.Format(EventCommandSentenceFormatLoop, loopVarStr);
        }

        private string MakeEventCommandExecSentenceForSdbDirect(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc, string execMainStr)
        {
            if (SoundId == -1)
            {
                var fadeStr = resolver.GetNumericVariableAddressStringIfVariableAddress(FadeTime, type, desc);
                string durationStr;
                if (UseDurationStrIfStop)
                {
                    durationStr = string.Format(EventCommandSentenceFormatStopDuration,
                        AudioType.EventCommandTimeSentence, fadeStr);
                }
                else
                {
                    durationStr = MakeEventCommandExecMainSentence(resolver, type, desc);
                }

                return string.Format(EventCommandSentenceFormatExecBaseStop,
                    AudioType.EventCommandSentence, durationStr);
            }

            TypeId soundTypeId;
            if (AudioType == AudioType.Bgm)
            {
                soundTypeId = 1;
            }
            else if (AudioType == AudioType.Bgs)
            {
                soundTypeId = 2;
            }
            else
            {
                soundTypeId = 3;
            }

            var soundIdStr = resolver.GetNumericVariableAddressStringIfVariableAddress(SoundId, type, desc);
            var dataName = resolver.GetDatabaseDataName(DBKind.System, soundTypeId, SoundId).Item2;

            return string.Format(EventCommandSentenceFormatExecBaseId,
                AudioType.EventCommandSentence, soundIdStr, dataName, execMainStr);
        }

        private string MakeEventCommandExecSentenceForSdbRefer(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc, string execMainStr)
        {
            var varAddressName = resolver.GetNumericVariableAddressStringIfVariableAddress(NumberVariable, type, desc);
            return string.Format(EventCommandSentenceFormatExecBaseVariableAddress,
                AudioType.EventCommandSentence, varAddressName, execMainStr);
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
                if (value is null)
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
                if (value is null)
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
                if (value is null)
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
        //     Internal Virtual Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>イベントコマンド文字列・詳細表示フラグ</summary>
        internal virtual bool UseDetailStringByEventCommandSentence => true;

        /// <summary>イベントコマンド文字列・停止時処理時間表示フラグ</summary>
        internal virtual bool UseDurationStrIfStop => true;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const int DefaultLoopPoint = 0;
        private const int DefaultVolume = 100;
        private const int DefaultFrequency = 100;
    }
}