// ========================================
// Project Name : WodiLib
// File Name    : PictureShowSeWindowDirect.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・ピクチャ（表示） お手軽ウィンドウ（直接指定）
    /// </summary>
    public class PictureShowSimpleWindowDirect : PictureShowBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private string loadFileName = "";

        /// <summary>[NotNull] 読み込みファイル名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public string LoadFileName
        {
            get => loadFileName;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(LoadFileName)));
                loadFileName = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
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
            get => LoadFileName;
            set => LoadFileName = value;
        }

        /// <inheritdoc />
        /// <summary>表示タイプコード</summary>
        protected override byte DisplayTypeCode
        {
            get => EventCommandConstant.PictureShow.ShowTypeCode.SimpleWindowDirect;
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