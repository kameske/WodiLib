// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusEtcInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 変数操作＋（その他）・取得情報種別
    /// </summary>
    public class NumberPlusEtcInfoType : TypeSafeEnum<NumberPlusEtcInfoType>
    {
        /// <summary>現在のマップID</summary>
        public static readonly NumberPlusEtcInfoType CurrentMapId;

        /// <summary>再生中のBGM番号</summary>
        public static readonly NumberPlusEtcInfoType PlayingBgmNumber;

        /// <summary>再生中のBGS番号</summary>
        public static readonly NumberPlusEtcInfoType PlayingBgsNumber;

        /// <summary>BGMの現在再生位置</summary>
        public static readonly NumberPlusEtcInfoType BgmPosition;

        /// <summary>BGSの現在再生位置</summary>
        public static readonly NumberPlusEtcInfoType BgsPosition;

        /// <summary>現在BGMの曲の長さ</summary>
        public static readonly NumberPlusEtcInfoType CurrentBgmLength;

        /// <summary>現在BGSの曲の長さ</summary>
        public static readonly NumberPlusEtcInfoType CurrentBgsLength;

        /// <summary>マウスX座標</summary>
        public static readonly NumberPlusEtcInfoType MouseX;

        /// <summary>マウスY座標</summary>
        public static readonly NumberPlusEtcInfoType MouseY;

        /// <summary>マウス左クリック状態</summary>
        public static readonly NumberPlusEtcInfoType MouseLeftClick;

        /// <summary>マウス右クリック状態</summary>
        public static readonly NumberPlusEtcInfoType MouseRightClick;

        /// <summary>マウス中クリック状態</summary>
        public static readonly NumberPlusEtcInfoType MouseCenterClick;

        /// <summary>マウスホイール変化</summary>
        public static readonly NumberPlusEtcInfoType MouseWheelDelta;

        /// <summary>マウスX座標の変化</summary>
        public static readonly NumberPlusEtcInfoType MouseXDelta;

        /// <summary>マウスY座標の変化</summary>
        public static readonly NumberPlusEtcInfoType MouseYDelta;

        /// <summary>マウス座標にあるイベントID</summary>
        public static readonly NumberPlusEtcInfoType EventIdAtMousePosition;

        /// <summary>マップのサイズ（横）</summary>
        public static readonly NumberPlusEtcInfoType MapWidth;

        /// <summary>マップのサイズ（縦）</summary>
        public static readonly NumberPlusEtcInfoType MapHeight;

        /// <summary>このマップイベントID</summary>
        public static readonly NumberPlusEtcInfoType ThisMapEventId;

        /// <summary>このコモンイベントID</summary>
        public static readonly NumberPlusEtcInfoType ThisCommonEventId;

        /// <summary>処理中の自動/接触/決定キーEv番号</summary>
        public static readonly NumberPlusEtcInfoType ActiveEventId;

        /// <summary>処理中のEv行数</summary>
        public static readonly NumberPlusEtcInfoType ActiveEventLine;

        /// <summary>値</summary>
        public int Code { get; }

        static NumberPlusEtcInfoType()
        {
            CurrentMapId = new NumberPlusEtcInfoType(nameof(CurrentMapId), 0);
            PlayingBgmNumber = new NumberPlusEtcInfoType(nameof(PlayingBgmNumber), 1);
            PlayingBgsNumber = new NumberPlusEtcInfoType(nameof(PlayingBgsNumber), 2);
            BgmPosition = new NumberPlusEtcInfoType(nameof(BgmPosition), 3);
            BgsPosition = new NumberPlusEtcInfoType(nameof(BgsPosition), 4);
            CurrentBgmLength = new NumberPlusEtcInfoType(nameof(CurrentBgmLength), 5);
            CurrentBgsLength = new NumberPlusEtcInfoType(nameof(CurrentBgsLength), 6);
            MouseX = new NumberPlusEtcInfoType(nameof(MouseX), 20);
            MouseY = new NumberPlusEtcInfoType(nameof(MouseY), 21);
            MouseLeftClick = new NumberPlusEtcInfoType(nameof(MouseLeftClick), 7);
            MouseRightClick = new NumberPlusEtcInfoType(nameof(MouseRightClick), 8);
            MouseCenterClick = new NumberPlusEtcInfoType(nameof(MouseCenterClick), 9);
            MouseWheelDelta = new NumberPlusEtcInfoType(nameof(MouseWheelDelta), 10);
            MouseXDelta = new NumberPlusEtcInfoType(nameof(MouseXDelta), 11);
            MouseYDelta = new NumberPlusEtcInfoType(nameof(MouseYDelta), 12);
            EventIdAtMousePosition = new NumberPlusEtcInfoType(nameof(EventIdAtMousePosition), 13);
            MapWidth = new NumberPlusEtcInfoType(nameof(MapWidth), 15);
            MapHeight = new NumberPlusEtcInfoType(nameof(MapHeight), 16);
            ThisMapEventId = new NumberPlusEtcInfoType(nameof(ThisMapEventId), 14);
            ThisCommonEventId = new NumberPlusEtcInfoType(nameof(ThisCommonEventId), 17);
            ActiveEventId = new NumberPlusEtcInfoType(nameof(ActiveEventId), 18);
            ActiveEventLine = new NumberPlusEtcInfoType(nameof(ActiveEventLine), 19);
        }

        private NumberPlusEtcInfoType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusEtcInfoType FromValue(int code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}