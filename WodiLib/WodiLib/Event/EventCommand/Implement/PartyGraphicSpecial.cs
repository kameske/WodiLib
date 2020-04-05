// ========================================
// Project Name : WodiLib
// File Name    : PartyGraphicSpecial.cs
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
    /// イベントコマンド・パーティ画像（特殊処理）
    /// </summary>
    [Serializable]
    public class PartyGraphicSpecial : PartyGraphicBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x02;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private PartyGraphicSpecialType subject = PartyGraphicSpecialType.PushCharactersFront;

        /// <summary>[NotNull] 特殊処理内容</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public PartyGraphicSpecialType Subject
        {
            get => subject;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Subject)));
                subject = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>処理内容</summary>
        protected override byte ExecCode
        {
            get => (byte) (Subject.Code + EventCommandConstant.PartyGraphic.ExecCode.Special);
            set => Subject = PartyGraphicSpecialType.FromByte((byte) (value & 0xF0));
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
            get => 0;
            set { }
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
            return Subject.EventCommandSentence;
        }
    }
}