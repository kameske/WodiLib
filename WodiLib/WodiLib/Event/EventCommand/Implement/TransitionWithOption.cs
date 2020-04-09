// ========================================
// Project Name : WodiLib
// File Name    : TransitionWithOption.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・オプション指定トランジション
    /// </summary>
    [Serializable]
    public class TransitionWithOption : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "■ﾄﾗﾝｼﾞｼｮﾝﾀｲﾌﾟ指定： [{0}] {1}ﾌﾚｰﾑ{2}";

        private const string EventCommandSentenceFormatInstant = "瞬間表示";
        private const string EventCommandSentenceFormatEtc = "{0}：{1}";

        private const string EventCommandSentenceHasWait = "（ｳｪｲﾄ有り）";
        private const string EventCommandSentenceNonWait = "";

        private const int TransitionInstantId = -1;

        private static readonly TypeId TransitionTypeId = 11;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.TransitionWithOption;

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

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
                    return TransitionId;

                case 2:
                {
                    var bytes = FadeTime.ToBytes(Endian.Environment);
                    if (IsWaitForComplete) bytes[2] = FlgWait;
                    return bytes.ToInt32(Endian.Little);
                }

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
                    TransitionId = value;
                    return;

                case 2:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    IsWaitForComplete = (bytes[2] & FlgWait) != 0;
                    bytes[2] = bytes[3] = 0;
                    FadeTime = bytes.ToInt32(Endian.Little);
                    return;
                }

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
            var targetStr = MakeEventCommandTargetSentence(resolver);
            var waitStr = IsWaitForComplete
                ? EventCommandSentenceHasWait
                : EventCommandSentenceNonWait;

            return string.Format(EventCommandSentenceFormat,
                targetStr, FadeTime.ToString(), waitStr);
        }

        private string MakeEventCommandTargetSentence(
            EventCommandSentenceResolver resolver)
        {
            if (TransitionId == TransitionInstantId) return EventCommandSentenceFormatInstant;

            var (isFound, name) = resolver.GetDatabaseDataName(DBKind.System, TransitionTypeId, TransitionId);

            var nameStr = isFound && !TransitionId.IsVariableAddressSimpleCheck()
                ? name
                : EventCommandSentenceResolver_Database_Basic.DatabaseDataNotFound;

            return string.Format(EventCommandSentenceFormatEtc,
                TransitionId.ToString(), nameStr);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int transitionId;

        /// <summary>トランジション</summary>
        public int TransitionId
        {
            get => transitionId;
            set
            {
                transitionId = value;
                NotifyPropertyChanged();
            }
        }

        private int fadeTime;

        /// <summary>フェード時間</summary>
        public int FadeTime
        {
            get => fadeTime;
            set
            {
                fadeTime = value;
                NotifyPropertyChanged();
            }
        }

        private bool isWaitForComplete;

        /// <summary>完了までウェイト</summary>
        public bool IsWaitForComplete
        {
            get => isWaitForComplete;
            set
            {
                isWaitForComplete = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgWait = 0x01;
    }
}