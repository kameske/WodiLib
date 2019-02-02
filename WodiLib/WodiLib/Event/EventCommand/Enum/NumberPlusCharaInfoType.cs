// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusCharaInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 変数操作＋・取得項目
    /// </summary>
    public class NumberPlusCharaInfoType : TypeSafeEnum<NumberPlusCharaInfoType>
    {
        /// <summary>X座標（標準）</summary>
        public static readonly NumberPlusCharaInfoType XPositionStandard;

        /// <summary>Y座標（標準）</summary>
        public static readonly NumberPlusCharaInfoType YPositionStandard;

        /// <summary>X座標（精密）</summary>
        public static readonly NumberPlusCharaInfoType XPositionPrecise;

        /// <summary>Y座標（精密）</summary>
        public static readonly NumberPlusCharaInfoType YPositionPrecise;

        /// <summary>地面からの高さ</summary>
        public static readonly NumberPlusCharaInfoType Elevation;

        /// <summary>向き</summary>
        public static readonly NumberPlusCharaInfoType Direction;

        /// <summary>画面X座標</summary>
        public static readonly NumberPlusCharaInfoType ScreenX;

        /// <summary>画面Y座標</summary>
        public static readonly NumberPlusCharaInfoType ScreenY;

        /// <summary>影グラフィック番号</summary>
        public static readonly NumberPlusCharaInfoType ShadowGraphic;

        /// <summary>現在地店のタグタイル番号</summary>
        public static readonly NumberPlusCharaInfoType CurrentLocationTileTag;

        /// <summary>イベントID</summary>
        public static readonly NumberPlusCharaInfoType EventId;

        /// <summary>画面内にいる？</summary>
        public static readonly NumberPlusCharaInfoType IsOnScreen;

        /// <summary>起動中ページ</summary>
        public static readonly NumberPlusCharaInfoType ActivePage;

        /// <summary>起動条件</summary>
        public static readonly NumberPlusCharaInfoType RunCondition;

        /// <summary>接触拡張範囲X</summary>
        public static readonly NumberPlusCharaInfoType RangeExtendX;

        /// <summary>接触拡張範囲Y</summary>
        public static readonly NumberPlusCharaInfoType RangeExtendY;

        /// <summary>アニメパターン</summary>
        public static readonly NumberPlusCharaInfoType AnimationPattern;

        /// <summary>移動中？</summary>
        public static readonly NumberPlusCharaInfoType IsMoving;

        /// <summary>値</summary>
        public int Code { get; }

        static NumberPlusCharaInfoType()
        {
            XPositionStandard = new NumberPlusCharaInfoType(nameof(XPositionStandard), 0);
            YPositionStandard = new NumberPlusCharaInfoType(nameof(YPositionStandard), 1);
            XPositionPrecise = new NumberPlusCharaInfoType(nameof(XPositionPrecise), 2);
            YPositionPrecise = new NumberPlusCharaInfoType(nameof(YPositionPrecise), 3);
            Elevation = new NumberPlusCharaInfoType(nameof(Elevation), 4);
            Direction = new NumberPlusCharaInfoType(nameof(Direction), 5);
            ScreenX = new NumberPlusCharaInfoType(nameof(ScreenX), 6);
            ScreenY = new NumberPlusCharaInfoType(nameof(ScreenY), 7);
            ShadowGraphic = new NumberPlusCharaInfoType(nameof(ShadowGraphic), 8);
            CurrentLocationTileTag = new NumberPlusCharaInfoType(nameof(CurrentLocationTileTag), 9);
            EventId = new NumberPlusCharaInfoType(nameof(EventId), 10);
            IsOnScreen = new NumberPlusCharaInfoType(nameof(IsOnScreen), 11);
            ActivePage = new NumberPlusCharaInfoType(nameof(ActivePage), 12);
            RunCondition = new NumberPlusCharaInfoType(nameof(RunCondition), 13);
            RangeExtendX = new NumberPlusCharaInfoType(nameof(RangeExtendX), 14);
            RangeExtendY = new NumberPlusCharaInfoType(nameof(RangeExtendY), 15);
            AnimationPattern = new NumberPlusCharaInfoType(nameof(AnimationPattern), 16);
            IsMoving = new NumberPlusCharaInfoType(nameof(IsMoving), 17);
        }

        private NumberPlusCharaInfoType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusCharaInfoType FromValue(int code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}