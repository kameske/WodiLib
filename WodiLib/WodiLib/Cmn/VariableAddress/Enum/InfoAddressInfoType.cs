// ========================================
// Project Name : WodiLib
// File Name    : InfoAddressInfoType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

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
            PositionX = new InfoAddressInfoType(nameof(PositionX), 0);
            PositionY = new InfoAddressInfoType(nameof(PositionY), 1);
            PositionXPrecise = new InfoAddressInfoType(nameof(PositionXPrecise), 2);
            PositionYPrecise = new InfoAddressInfoType(nameof(PositionYPrecise), 3);
            Height = new InfoAddressInfoType(nameof(Height), 4);
            ShadowGraphicId = new InfoAddressInfoType(nameof(ShadowGraphicId), 5);
            Direction = new InfoAddressInfoType(nameof(Direction), 6);
            CharacterGraphicName = new InfoAddressInfoType(nameof(CharacterGraphicName), 9);

            Empty = new InfoAddressInfoType(nameof(Empty), EmptyCodeList[0]);
        }

        private InfoAddressInfoType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public int Code { get; }

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

            return _FindFirst(x => x.Code == code);
        }
    }
}