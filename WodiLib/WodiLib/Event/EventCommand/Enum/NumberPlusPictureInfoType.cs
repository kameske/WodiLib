// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusPictureInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
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

        /// <summary>拡大率（Ver2.00より前）</summary>
        public static readonly NumberPlusPictureInfoType Zoom;

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

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static NumberPlusPictureInfoType()
        {
            PositionX = new NumberPlusPictureInfoType(nameof(PositionX), 0,
                "X座標");
            PositionY = new NumberPlusPictureInfoType(nameof(PositionY), 1,
                "Y座標");
            ImageSizeWidth = new NumberPlusPictureInfoType(nameof(ImageSizeWidth), 2,
                "画像サイズ(横)");
            ImageSizeHeight = new NumberPlusPictureInfoType(nameof(ImageSizeHeight), 3,
                "画像サイズ(縦)");
            PatternNumber = new NumberPlusPictureInfoType(nameof(PatternNumber), 4,
                "パターン番号");
            Opacity = new NumberPlusPictureInfoType(nameof(Opacity), 5,
                "不透明度");
            Angle = new NumberPlusPictureInfoType(nameof(Angle), 6,
                "角度");
            Zoom = new NumberPlusPictureInfoType(nameof(Zoom), 7,
                "拡大率");
            ZoomWidth = new NumberPlusPictureInfoType(nameof(ZoomWidth), 11,
                "拡大率(横)");
            ZoomHeight = new NumberPlusPictureInfoType(nameof(ZoomHeight), 12,
                "拡大率(縦)");
            IsMouseCursorHover = new NumberPlusPictureInfoType(nameof(IsMouseCursorHover), 8,
                "マウス重なってる？(1=YES)");
            IsUsePicture = new NumberPlusPictureInfoType(nameof(IsUsePicture), 9,
                "ピクチャが使用中？(1=YES)");
            IsDoneStringDisplaying = new NumberPlusPictureInfoType(nameof(IsDoneStringDisplaying), 10,
                "文字列、表示完了？(1=YES)");
            FreeModeLeftUpX = new NumberPlusPictureInfoType(nameof(FreeModeLeftUpX), 13,
                "左上X座標 [自由変形時以外なら-1]");
            FreeModeLeftUpY = new NumberPlusPictureInfoType(nameof(FreeModeLeftUpY), 14,
                "左上Y座標 [自由変形時以外なら-1]");
            FreeModeRightUpX = new NumberPlusPictureInfoType(nameof(FreeModeRightUpX), 15,
                "右上X座標 [自由変形時以外なら-1]");
            FreeModeRightUpY = new NumberPlusPictureInfoType(nameof(FreeModeRightUpY), 16,
                "右上Y座標 [自由変形時以外なら-1]");
            FreeModeLeftDownX = new NumberPlusPictureInfoType(nameof(FreeModeLeftDownX), 17,
                "左下X座標 [自由変形時以外なら-1]");
            FreeModeLeftDownY = new NumberPlusPictureInfoType(nameof(FreeModeLeftDownY), 18,
                "左下Y座標 [自由変形時以外なら-1]");
            FreeModeRightDownX = new NumberPlusPictureInfoType(nameof(FreeModeRightDownX), 19,
                "右下X座標 [自由変形時以外なら-1]");
            FreeModeRightDownY = new NumberPlusPictureInfoType(nameof(FreeModeRightDownY), 20,
                "右下Y座標 [自由変形時以外なら-1]");
        }

        private NumberPlusPictureInfoType(string id, int code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
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