// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusCharaInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using Commons;

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

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static NumberPlusCharaInfoType()
        {
            XPositionStandard = new NumberPlusCharaInfoType(nameof(XPositionStandard), 0,
                "X座標(標準)");
            YPositionStandard = new NumberPlusCharaInfoType(nameof(YPositionStandard), 1,
                "Y座標(標準)");
            XPositionPrecise = new NumberPlusCharaInfoType(nameof(XPositionPrecise), 2,
                "X座標(精密)");
            YPositionPrecise = new NumberPlusCharaInfoType(nameof(YPositionPrecise), 3,
                "Y座標(精密)");
            Elevation = new NumberPlusCharaInfoType(nameof(Elevation), 4,
                "高さ(ﾋﾟｸｾﾙ数)");
            Direction = new NumberPlusCharaInfoType(nameof(Direction), 5,
                "向き(1～9)");
            ScreenX = new NumberPlusCharaInfoType(nameof(ScreenX), 6,
                "画面X座標");
            ScreenY = new NumberPlusCharaInfoType(nameof(ScreenY), 7,
                "画面Y座標");
            ShadowGraphic = new NumberPlusCharaInfoType(nameof(ShadowGraphic), 8,
                "影ｸﾞﾗﾌｨｯｸ番号");
            CurrentLocationTileTag = new NumberPlusCharaInfoType(nameof(CurrentLocationTileTag), 9,
                "現在地点ﾀｲﾙのﾀｸﾞ番号");
            EventId = new NumberPlusCharaInfoType(nameof(EventId), 10,
                "イベントID");
            IsOnScreen = new NumberPlusCharaInfoType(nameof(IsOnScreen), 11,
                "画面内にいる？(1=YES 0=NO)");
            ActivePage = new NumberPlusCharaInfoType(nameof(ActivePage), 12,
                "現在の起動ﾍﾟｰｼﾞ 0=ﾅｼ 1～=起動ﾍﾟｰｼﾞ");
            RunCondition = new NumberPlusCharaInfoType(nameof(RunCondition), 13,
                "起動条件(0:決定ｷｰ ～ 4:Ev接触)");
            RangeExtendX = new NumberPlusCharaInfoType(nameof(RangeExtendX), 14,
                "接触範囲拡張Ｘ");
            RangeExtendY = new NumberPlusCharaInfoType(nameof(RangeExtendY), 15,
                "接触範囲拡張Ｙ");
            AnimationPattern = new NumberPlusCharaInfoType(nameof(AnimationPattern), 16,
                "アニメパターン[0-4]");
            IsMoving = new NumberPlusCharaInfoType(nameof(IsMoving), 17,
                "移動中？[1=YES 0=NO / ※主人公のみ1ﾌﾚ以上停止時のみ0]");
        }

        private NumberPlusCharaInfoType(string id, int code,
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
        public static NumberPlusCharaInfoType FromValue(int code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}