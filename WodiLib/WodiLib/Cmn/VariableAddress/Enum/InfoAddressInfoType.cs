// ========================================
// Project Name : WodiLib
// File Name    : InfoAddressInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Commons;
using WodiLib.Map;

namespace WodiLib.Cmn
{
    /// <summary>
    /// 各情報アドレス情報種別
    /// </summary>
    public class InfoAddressInfoType : TypeSafeEnum<InfoAddressInfoType>
    {
        /// <summary>X座標</summary>
        public static readonly InfoAddressInfoType PositionX;

        /// <summary>Y座標</summary>
        public static readonly InfoAddressInfoType PositionY;

        /// <summary>X座標（精密）</summary>
        public static readonly InfoAddressInfoType PositionXPrecise;

        /// <summary>Y座標（精密）</summary>
        public static readonly InfoAddressInfoType PositionYPrecise;

        /// <summary>高さ</summary>
        public static readonly InfoAddressInfoType Height;

        /// <summary>影番号</summary>
        public static readonly InfoAddressInfoType ShadowGraphicId;

        /// <summary>方向</summary>
        public static readonly InfoAddressInfoType Direction;

        /// <summary>キャラチップ画像</summary>
        public static readonly InfoAddressInfoType CharacterGraphicName;

        /// <summary>空</summary>
        public static readonly InfoAddressInfoType Empty;

        static InfoAddressInfoType()
        {
            PositionX = new InfoAddressInfoType(nameof(PositionX), 0,
                "{0}のX座標(ﾏｯﾌﾟ)");
            PositionY = new InfoAddressInfoType(nameof(PositionY), 1,
                "{0}のY座標(ﾏｯﾌﾟ)");
            PositionXPrecise = new InfoAddressInfoType(nameof(PositionXPrecise), 2,
                "{0}のX座標(精密)");
            PositionYPrecise = new InfoAddressInfoType(nameof(PositionYPrecise), 3,
                "{0}のY座標(精密)");
            Height = new InfoAddressInfoType(nameof(Height), 4,
                "{0}の高さ（ピクセル）");
            ShadowGraphicId = new InfoAddressInfoType(nameof(ShadowGraphicId), 5,
                "{0}の影番号");
            Direction = new InfoAddressInfoType(nameof(Direction), 6,
                "{0}の方向");
            CharacterGraphicName = new InfoAddressInfoType(nameof(CharacterGraphicName), 9,
                "{0}のキャラチップ画像");

            Empty = new InfoAddressInfoType(nameof(Empty), EmptyCodeList[0],
                ""); // この文字列はWodiLib内で使用しない
        }

        private InfoAddressInfoType(string id, int code, string eventCommandStringFormat) : base(id)
        {
            Code = code;
            EventCommandStringFormat = eventCommandStringFormat;
        }

        /// <summary>コード値</summary>
        public int Code { get; }

        /// <summary>イベントコマンド文フォーマット</summary>
        private string EventCommandStringFormat { get; }

        /// <summary>
        /// イベントコマンド文用文字列を生成する。（マップイベント）
        /// </summary>
        /// <param name="mapEventId">マップイベントID</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MakeEventCommandSentenceForMapEvent(MapEventId mapEventId)
        {
            return string.Format(EventCommandStringFormat, $"Ev{mapEventId}");
        }

        /// <summary>
        /// イベントコマンド文用文字列を生成する。（主人公情報）
        /// </summary>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MakeEventCommandSentenceForHero()
        {
            return string.Format(EventCommandStringFormat, "主人公");
        }

        /// <summary>
        /// イベントコマンド文用文字列を生成する。（仲間情報）
        /// </summary>
        /// <param name="memberId">仲間ID</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MakeEventCommandSentenceForMember(MemberId memberId)
        {
            return string.Format(EventCommandStringFormat, $"仲間{memberId}");
        }

        /// <summary>
        /// イベントコマンド文用文字列を生成する。（このマップイベント情報）
        /// </summary>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MakeEventCommandSentenceForThisMapEvent()
        {
            return string.Format(EventCommandStringFormat, "このEv");
        }

        private static readonly List<int> EmptyCodeList = new List<int>
        {
            7, 8
        };

        /// <summary>
        /// コード値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static InfoAddressInfoType FromCode(int code)
        {
            // Empty判定
            if (EmptyCodeList.Contains(code)) return Empty;

            return AllItems.First(x => x.Code == code);
        }
    }
}