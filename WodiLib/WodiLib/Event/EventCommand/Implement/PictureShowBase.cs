// ========================================
// Project Name : WodiLib
// File Name    : PictureShowBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・ピクチャ（表示）ベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class PictureShowBase : PictureDrawBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFreePosition = "[自由変形]";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.Picture;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private PictureAnchorPosition anchorPosition = PictureAnchorPosition.LeftUp;

        /// <summary>[NotNull] ピクチャ表示位置</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public PictureAnchorPosition AnchorPosition
        {
            get => anchorPosition;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(anchorPosition)));
                anchorPosition = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>自由変形フラグ</summary>
        public bool IsFreePosition
        {
            get => _IsFreePosition;
            set
            {
                _IsFreePosition = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override string DrawTypeStr => "表示";

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

        /// <inheritdoc />
        /// <summary>スクロールとリンク</summary>
        protected override bool _IsLinkScroll { get; set; }

        /// <inheritdoc />
        /// <summary>処理内容コード</summary>
        protected override byte ExecCode
        {
            get => EventCommandConstant.PictureShow.ExecCode.Show;
            set { }
        }

        /// <inheritdoc />
        /// <summary>表示位置コード</summary>
        protected override byte AnchorPositionCode
        {
            get => AnchorPosition.Code;
            set => AnchorPosition = PictureAnchorPosition.FromByte(value);
        }

        /// <inheritdoc />
        /// <summary>表示形式コード</summary>
        protected override byte DrawTypeCode
        {
            get => IsSamePrintType
                ? FlgSameDrawType
                : PrintType.Code;
            set
            {
                if (value == FlgSameDrawType)
                {
                    IsSamePrintType = true;
                    PrintType = PictureDrawType.Normal;
                    return;
                }

                PrintType = PictureDrawType.FromByte(value);
            }
        }

        /// <summary>表示形式同値コード</summary>
        private readonly byte FlgSameDrawType = 0x0F;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override string MakeEventCommandAnchorSentence()
        {
            if (IsFreePosition) return EventCommandSentenceFreePosition;
            return $"[{AnchorPosition.EventCommandSentence}]";
        }
    }
}