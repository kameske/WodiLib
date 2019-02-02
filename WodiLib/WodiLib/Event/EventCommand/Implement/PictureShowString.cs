// ========================================
// Project Name : WodiLib
// File Name    : PictureShowString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・ピクチャ（表示） ファイル読み込み（文字列をピクチャとして描画）
    /// </summary>
    public class PictureShowString : PictureShowBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private string printString = "";

        /// <summary>[NotNull] 表示文字列</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string PrintString
        {
            get => printString;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PrintString)));
                printString = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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
            set => _DivisionWidth = value;
        }

        /// <inheritdoc />
        /// <summary>分割数横</summary>
        protected override int _DivisionWidth { get; set; }

        /// <summary>分割数縦</summary>
        public int DivisionHeight
        {
            get => _DivisionHeight;
            set => _DivisionHeight = value;
        }

        /// <inheritdoc />
        /// <summary>分割数縦</summary>
        protected override int _DivisionHeight { get; set; }

        /// <inheritdoc />
        /// <summary>読み込みファイル名または表示文字列</summary>
        protected override string LoadFileNameOrDrawString
        {
            get => PrintString;
            set => PrintString = value;
        }

        /// <inheritdoc />
        /// <summary>表示タイプコード</summary>
        protected override byte DisplayTypeCode
        {
            get => EventCommandConstant.PictureShow.ShowTypeCode.String;
            set { }
        }

        /// <inheritdoc />
        /// <summary>文字列使用フラグ</summary>
        protected override bool IsUseString => true;

        /// <inheritdoc />
        /// <summary>文字列変数指定フラグフラグ</summary>
        protected override bool IsLoadForVariableAddress => false;
    }
}