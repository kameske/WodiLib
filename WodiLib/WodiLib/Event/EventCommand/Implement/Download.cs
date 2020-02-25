// ========================================
// Project Name : WodiLib
// File Name    : Download.cs
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
    /// イベントコマンド・ダウンロード
    /// </summary>
    [Serializable]
    public class Download : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■ダウンロード：URL[ {0} ]{1} {2}";

        private const string EventCommandStringHasWait = "<ｳｪｲﾄ>";
        private const string EventCommandStringNoWait = "";

        private const string EventCommandStringResponseOutputFile
            = " 保存先[ {0} ] ";

        private const string EventCommandStringResponseIfInputVariableIsZero
            = "";

        private const string EventCommandStringResponseInputVariable
            = " > [ {0} ]に格納";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Download;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x02;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 2)] インデックス</param>
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
                    var result = 0;
                    if (!IsOutputFile) result += 1;
                    if (IsWaitForComplete) result += 2;
                    return result;
                }

                case 2:
                    return IsStoreInVariable ? PathStringVar : 0;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 2, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 2)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    IsOutputFile = (value & 0x01) == 0;
                    IsWaitForComplete = (value & 0x02) != 0;
                    return;

                case 2:
                    PathStringVar = value;
                    IsStoreInVariable = value != 0;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 2, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 1)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return DownloadUrl;

                case 1:
                    return IsOutputFile ? StorageFolder : "";

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 1)] インデックス</param>
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
                    DownloadUrl = value;
                    return;

                case 1:
                    StorageFolder = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 1, index));
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var waitStr = IsWaitForComplete
                ? EventCommandStringHasWait
                : EventCommandStringNoWait;

            var responseStr = IsOutputFile
                ? string.Format(EventCommandStringResponseOutputFile, StorageFolder)
                : PathStringVar == 0
                    ? EventCommandStringResponseIfInputVariableIsZero
                    : string.Format(EventCommandStringResponseInputVariable,
                        resolver.GetStringVariableAddressString(PathStringVar, type, desc));

            return string.Format(EventCommandSentenceFormat,
                DownloadUrl, waitStr, responseStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private string downloadUrl = "";

        /// <summary>[NotNull] URL</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string DownloadUrl
        {
            get => downloadUrl;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DownloadUrl)));
                downloadUrl = value;
            }
        }

        /// <summary>保存フラグ</summary>
        public bool IsOutputFile { get; set; }

        private string storageFolder = "";

        /// <summary>[NotNull] 保存フォルダ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string StorageFolder
        {
            get => storageFolder;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(StorageFolder)));
                storageFolder = value;
            }
        }

        /// <summary>変数に格納する。ラグ</summary>
        public bool IsStoreInVariable { get; set; }

        /// <summary>格納先の文字列変数</summary>
        public int PathStringVar { get; set; }

        /// <summary>ダウンロード完了までウェイト</summary>
        public bool IsWaitForComplete { get; set; }


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
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_10))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_10();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.10未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_10()
        {
            Logger.Warning(VersionWarningMessage.NotUnderInCommand($"{nameof(Download)}",
                VersionConfig.GetConfigWoditorVersion(),
                WoditorVersion.Ver2_10));
        }
    }
}