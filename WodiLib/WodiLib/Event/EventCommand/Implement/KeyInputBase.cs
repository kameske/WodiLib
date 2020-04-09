// ========================================
// Project Name : WodiLib
// File Name    : KeyInputBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Cmn;
using WodiLib.Project;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・キー入力ベースクラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class KeyInputBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>キー入力を待つフラグ値</summary>
        protected const byte FlgWaitForInput = 0x80;

        /// <summary>特定キーのみ受付フラグ値</summary>
        protected const byte FlgOnlySpecificKey = 0x10;

        private const string EventCommandSentenceFormat = "■キー入力：{0} / {1}";

        private const int CorrectAddressIfNotNumericVariableAddressLeftSide = 2000000;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public sealed override EventCommandCode EventCommandCode => EventCommandCode.KeyInput;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.DeepRed;

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数をセットする。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed override string GetSafetyStringVariable(int index)
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
        public sealed override void SetSafetyStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            // 左辺が「このイベントのセルフ変数」「このコモンイベントのセルフ変数」「通常変数」いずれかである場合しか考慮しない。
            //   上記以外の変数アドレスが指定されている場合、イベントコマンド文の表示が乱れる。
            var leftSideValue = LeftSide.IsNumericVariableAddressSimpleCheck()
                ? LeftSide
                : CorrectAddressIfNotNumericVariableAddressLeftSide;
            var leftSideName = resolver.GetNumericVariableAddressString(leftSideValue, type, desc);
            if (NormalNumberVariableAddress.MinValue <= leftSideValue &&
                leftSideValue <= NormalNumberVariableAddress.MaxValue)
            {
                leftSideName = $"変数{leftSideName}";
            }

            var rightSide = MakeEventCommandRightSideSentence(resolver, type, desc);

            return string.Format(EventCommandSentenceFormat,
                leftSideName, rightSide);
        }

        /// <summary>
        /// イベントコマンド文字列の右辺部分を取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int leftSide;

        /// <summary>左辺（代入先）</summary>
        public int LeftSide
        {
            get => leftSide;
            set
            {
                leftSide = value;
                NotifyPropertyChanged();
            }
        }

        private bool isWaitForInput;

        /// <summary>キー入力を待つ</summary>
        public bool IsWaitForInput
        {
            get => isWaitForInput;
            set
            {
                isWaitForInput = value;
                NotifyPropertyChanged();
            }
        }
    }
}