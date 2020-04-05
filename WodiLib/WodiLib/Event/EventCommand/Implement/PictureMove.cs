// ========================================
// Project Name : WodiLib
// File Name    : PictureMove.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Project;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・ピクチャ（移動）
    /// </summary>
    [Serializable]
    public class PictureMove : PictureDrawBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Picture;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override string DrawTypeStr => "移動";

        /// <inheritdoc />
        /// <summary>読み込みファイル指定文字列変数</summary>
        protected override int _LoadFireStringVar
        {
            get => 0;
            set { }
        }

        /// <summary>分割数横</summary>
        public int DivisionWidth
        {
            get => _DivisionWidth;
            set
            {
                _DivisionWidth = value;
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>分割数横</summary>
        protected override int _DivisionWidth { get; set; }

        /// <summary>分割数縦</summary>
        public int DivisionHeight
        {
            get => _DivisionHeight;
            set
            {
                _DivisionHeight = value;
                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        /// <summary>分割数縦</summary>
        protected override int _DivisionHeight { get; set; }

        /// <summary>スクロールとリンク</summary>
        public bool IsLinkScroll
        {
            get => _IsLinkScroll;
            set
            {
                _IsLinkScroll = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>スクロールとリンク</summary>
        protected override bool _IsLinkScroll { get; set; }

        /// <inheritdoc />
        /// <summary>読み込みファイル名または表示文字列</summary>
        protected override string LoadFileNameOrDrawString
        {
            get => "";
            set { }
        }

        /// <inheritdoc />
        /// <summary>表示タイプコード</summary>
        protected override byte DisplayTypeCode { get; set; }

        /// <inheritdoc />
        /// <summary>処理内容コード</summary>
        protected override byte ExecCode
        {
            get => EventCommandConstant.PictureShow.ExecCode.Move;
            set { }
        }

        /// <inheritdoc />
        /// <summary>表示位置コード</summary>
        protected override byte AnchorPositionCode { get; set; }

        /// <inheritdoc />
        /// <summary>表示形式コード</summary>
        protected override byte DrawTypeCode { get; set; }

        /// <inheritdoc />
        /// <summary>文字列使用フラグ</summary>
        protected override bool IsUseString => false;

        /// <inheritdoc />
        /// <summary>文字列変数指定フラグフラグ</summary>
        protected override bool IsLoadForVariableAddress => false;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override string MakeEventCommandAnchorSentence()
            => "";

        /// <inheritdoc />
        protected override string MakeEventCommandDrawItemSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
            => "";
    }
}