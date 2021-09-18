// ========================================
// Project Name : WodiLib
// File Name    : NumberPlusEtcInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     変数操作＋（その他）・取得情報種別
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

        static NumberPlusEtcInfoType()
        {
            CurrentMapId = new NumberPlusEtcInfoType(nameof(CurrentMapId), 0,
                "現在のマップID");
            PlayingBgmNumber = new NumberPlusEtcInfoType(nameof(PlayingBgmNumber), 1,
                "再生中のBGM番号");
            PlayingBgsNumber = new NumberPlusEtcInfoType(nameof(PlayingBgsNumber), 2,
                "再生中のBGS番号");
            BgmPosition = new NumberPlusEtcInfoType(nameof(BgmPosition), 3,
                "BGM現在再生位置(ﾐﾘ秒･1周目のみ/MIDIならTick値)");
            BgsPosition = new NumberPlusEtcInfoType(nameof(BgsPosition), 4,
                "BGS現在再生位置(ﾐﾘ秒･1周目のみ)");
            CurrentBgmLength = new NumberPlusEtcInfoType(nameof(CurrentBgmLength), 5,
                "BGMの曲の長さ(ﾐﾘ秒/MIDIならTick値)");
            CurrentBgsLength = new NumberPlusEtcInfoType(nameof(CurrentBgsLength), 6,
                "BGSの曲の長さ(ﾐﾘ秒)");
            MouseX = new NumberPlusEtcInfoType(nameof(MouseX), 20,
                "マウスX座標");
            MouseY = new NumberPlusEtcInfoType(nameof(MouseY), 21,
                "マウスY座標");
            MouseLeftClick = new NumberPlusEtcInfoType(nameof(MouseLeftClick), 7,
                "ﾏｳｽ左ｸﾘｯｸ状態");
            MouseRightClick = new NumberPlusEtcInfoType(nameof(MouseRightClick), 8,
                "ﾏｳｽ右ｸﾘｯｸ状態");
            MouseCenterClick = new NumberPlusEtcInfoType(nameof(MouseCenterClick), 9,
                "ﾏｳｽ中ｸﾘｯｸ状態");
            MouseWheelDelta = new NumberPlusEtcInfoType(nameof(MouseWheelDelta), 10,
                "ﾏｳｽﾎｲｰﾙ変化");
            MouseXDelta = new NumberPlusEtcInfoType(nameof(MouseXDelta), 11,
                "ﾏｳｽX座標の変化");
            MouseYDelta = new NumberPlusEtcInfoType(nameof(MouseYDelta), 12,
                "ﾏｳｽY座標の変化");
            EventIdAtMousePosition = new NumberPlusEtcInfoType(nameof(EventIdAtMousePosition), 13,
                "ﾏｳｽ座標にあるEvID");
            MapWidth = new NumberPlusEtcInfoType(nameof(MapWidth), 15,
                "マップサイズ[横]");
            MapHeight = new NumberPlusEtcInfoType(nameof(MapHeight), 16,
                "マップサイズ[縦]");
            ThisMapEventId = new NumberPlusEtcInfoType(nameof(ThisMapEventId), 14,
                "ﾏｯﾌﾟｲﾍﾞﾝﾄID(ｺﾓﾝなら呼び出し元ID)");
            ThisCommonEventId = new NumberPlusEtcInfoType(nameof(ThisCommonEventId), 17,
                "このｺﾓﾝｲﾍﾞﾝﾄID(ｺﾓﾝでなければ-1)");
            ActiveEventId = new NumberPlusEtcInfoType(nameof(ActiveEventId), 18,
                "処理中の自動/接触/決定ｷｰEv番号[ｺﾓﾝなら+500000]");
            ActiveEventLine = new NumberPlusEtcInfoType(nameof(ActiveEventLine), 19,
                "処理中のEv行数 [自動/接触/決定キー起動Ev]");
        }

        private NumberPlusEtcInfoType(string id, int code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>値</summary>
        public int Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static NumberPlusEtcInfoType FromValue(int code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
