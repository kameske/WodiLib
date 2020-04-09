// ========================================
// Project Name : WodiLib
// File Name    : PartyGraphicRemove.cs
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
    /// イベントコマンド・パーティ画像（削除）
    /// </summary>
    [Serializable]
    public class PartyGraphicRemove : PartyGraphicBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat
            = "{0}人目をPTから削除";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x03;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int memberId;

        /// <summary>X人目</summary>
        public int MemberId
        {
            get => memberId;
            set
            {
                memberId = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>処理内容</summary>
        protected override byte ExecCode
        {
            get => EventCommandConstant.PartyGraphic.ExecCode.Remove;
            set { }
        }

        /// <summary>処理対象数値変数指定フラグ（対象文字列指定のときfalse）</summary>
        public override bool IsTargetingValue
        {
            get => false;
            set { }
        }

        /// <summary>操作対象序列</summary>
        protected override int TargetIndex
        {
            get => MemberId;
            set => MemberId = value;
        }

        /// <summary>対象指定変数または対象ファイル名</summary>
        protected override IntOrStr Target
        {
            get => 0;
            set { }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文字列の処理内容部分を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列の処理内容部分</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandExecSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var memberStr = resolver.GetNumericVariableAddressStringIfVariableAddress(MemberId, type, desc);

            return string.Format(EventCommandSentenceFormat, memberStr);
        }
    }
}