// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusPictureInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 変数操作＋：取得画像情報種別
    /// </summary>
    public class NumberPlusPictureInfoType : TypeSafeEnum<NumberPlusPictureInfoType>
    {
        /// <summary>X座標</summary>
        public static readonly NumberPlusPictureInfoType PositionX;

        /// <summary>Y座標</summary>
        public static readonly NumberPlusPictureInfoType PositionY;

        /// <summary>画像サイズ（横）</summary>
        public static readonly NumberPlusPictureInfoType ImageSizeWidth;

        /// <summary>画像サイズ（縦）</summary>
        public static readonly NumberPlusPictureInfoType ImageSizeHeight;

        /// <summary>パターン番号</summary>
        public static readonly NumberPlusPictureInfoType PatternNumber;

        /// <summary>不透明度</summary>
        public static readonly NumberPlusPictureInfoType Opacity;

        /// <summary>角度</summary>
        public static readonly NumberPlusPictureInfoType Angle;

        /// <summary>拡大率（横）</summary>
        public static readonly NumberPlusPictureInfoType ZoomWidth;

        /// <summary>拡大率（縦）</summary>
        public static readonly NumberPlusPictureInfoType ZoomHeight;

        /// <summary>マウス重なり？</summary>
        public static readonly NumberPlusPictureInfoType IsMouseCursorHover;

        /// <summary>使用中？</summary>
        public static readonly NumberPlusPictureInfoType IsUsePicture;

        /// <summary>表示完了？</summary>
        public static readonly NumberPlusPictureInfoType IsDoneStringDisplaying;

        /// <summary>自由変形左上X座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeLeftUpX;

        /// <summary>自由変形左上Y座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeLeftUpY;

        /// <summary>自由変形右上X座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeRightUpX;

        /// <summary>自由変形右上Y座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeRightUpY;

        /// <summary>自由変形左下X座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeLeftDownX;

        /// <summary>自由変形左下Y座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeLeftDownY;

        /// <summary>自由変形右下X座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeRightDownX;

        /// <summary>自由変形右下Y座標</summary>
        public static readonly NumberPlusPictureInfoType FreeModeRightDownY;

        /// <summary>値</summary>
        public int Code { get; }

        static NumberPlusPictureInfoType()
        {
            PositionX = new NumberPlusPictureInfoType(nameof(PositionX), 0);
            PositionY = new NumberPlusPictureInfoType(nameof(PositionY), 1);
            ImageSizeWidth = new NumberPlusPictureInfoType(nameof(ImageSizeWidth), 2);
            ImageSizeHeight = new NumberPlusPictureInfoType(nameof(ImageSizeHeight), 3);
            PatternNumber = new NumberPlusPictureInfoType(nameof(PatternNumber), 4);
            Opacity = new NumberPlusPictureInfoType(nameof(Opacity), 5);
            Angle = new NumberPlusPictureInfoType(nameof(Angle), 6);
            ZoomWidth = new NumberPlusPictureInfoType(nameof(ZoomWidth), 11);
            ZoomHeight = new NumberPlusPictureInfoType(nameof(ZoomHeight), 12);
            IsMouseCursorHover = new NumberPlusPictureInfoType(nameof(IsMouseCursorHover), 8);
            IsUsePicture = new NumberPlusPictureInfoType(nameof(IsUsePicture), 9);
            IsDoneStringDisplaying = new NumberPlusPictureInfoType(nameof(IsDoneStringDisplaying), 10);
            FreeModeLeftUpX = new NumberPlusPictureInfoType(nameof(FreeModeLeftUpX), 13);
            FreeModeLeftUpY = new NumberPlusPictureInfoType(nameof(FreeModeLeftUpY), 14);
            FreeModeRightUpX = new NumberPlusPictureInfoType(nameof(FreeModeRightUpX), 15);
            FreeModeRightUpY = new NumberPlusPictureInfoType(nameof(FreeModeRightUpY), 16);
            FreeModeLeftDownX = new NumberPlusPictureInfoType(nameof(FreeModeLeftDownX), 17);
            FreeModeLeftDownY = new NumberPlusPictureInfoType(nameof(FreeModeLeftDownY), 18);
            FreeModeRightDownX = new NumberPlusPictureInfoType(nameof(FreeModeRightDownX), 19);
            FreeModeRightDownY = new NumberPlusPictureInfoType(nameof(FreeModeRightDownY), 20);
        }

        private NumberPlusPictureInfoType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusPictureInfoType FromValue(int code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}