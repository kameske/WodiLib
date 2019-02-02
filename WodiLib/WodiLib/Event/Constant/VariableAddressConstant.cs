// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressConstant.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event
{
    /// <summary>
    /// 変数アドレス値に関する定数クラス
    /// </summary>
    public static class VariableAddressConstant
    {
        /// <summary>マップイベントのセルフ変数最小値</summary>
        public static readonly int MapEventVariableAddressMin = 1000000;

        /// <summary>マップイベントのセルフ変数最大値</summary>
        public static readonly int MapEventVariableAddressMax = 1099999;

        /// <summary>このマップイベントのセルフ変数最小値</summary>
        public static readonly int ThisMapEventVariableAddressMin = 1100000;

        /// <summary>このマップイベントのセルフ変数最大値</summary>
        public static readonly int ThisMapEventVariableAddressMax = 1100009;

        /// <summary>コモンイベントのセルフ変数最小値</summary>
        public static readonly int CommonEventVariableAddressMin = 15000000;

        /// <summary>コモンイベントのセルフ変数最大値</summary>
        public static readonly int CommonEventVariableAddressMax = 15999999;

        /// <summary>このコモンイベントのセルフ変数最小値</summary>
        public static readonly int ThisCommonEventVariableAddressMin = 16000000;

        /// <summary>このコモンイベントのセルフ変数最大値</summary>
        public static readonly int ThisCommonEventVariableAddressMax = 16000099;

        /// <summary>通常変数最小値</summary>
        public static readonly int NormalNumberVariableAddressMin = 2000000;

        /// <summary>通常変数最大値</summary>
        public static readonly int NormalNumberVariableAddressMax = 2099999;

        /// <summary>予備変数最小値</summary>
        public static readonly int SpareNumberVariableAddressMin = 2100000;

        /// <summary>予備変数最大値</summary>
        public static readonly int SpareNumberVariableAddressMax = 2999999;

        /// <summary>文字列変数最小値</summary>
        public static readonly int StringVariableAddressMin = 3000000;

        /// <summary>文字列変数最大値</summary>
        public static readonly int StringVariableAddressMax = 3999999;

        /// <summary>ランダム最小値</summary>
        public static readonly int RandomVariableAddressMin = 8000000;

        /// <summary>ランダム最大値</summary>
        public static readonly int RandomVariableAddressMax = 8999999;

        /// <summary>システム変数最小値</summary>
        public static readonly int SystemVariableAddressMin = 9000000;

        /// <summary>システム変数最大値</summary>
        public static readonly int SystemVariableAddressMax = 9099999;

        /// <summary>イベント座標最小値</summary>
        public static readonly int EventPositionAddressMin = 9100000;

        /// <summary>イベント座標最大値</summary>
        public static readonly int EventPositionAddressMax = 9179999;

        /// <summary>主人公座標最小値</summary>
        public static readonly int HeroPositionAddressMin = 9180000;

        /// <summary>主人公座標最大値</summary>
        public static readonly int HeroPositionAddressMax = 9180009;

        /// <summary>仲間座標最小値</summary>
        public static readonly int MemberPositionAddressMin = 9180010;

        /// <summary>仲間座標最大値</summary>
        public static readonly int MemberPositionAddressMax = 9180059;

        /// <summary>システム文字列最小値</summary>
        public static readonly int SystemStringVariableAddressMin = 9900000;

        /// <summary>システム文字列最大値</summary>
        public static readonly int SystemStringVariableAddressMax = 9999999;

        /// <summary>ユーザDB変数最小値</summary>
        public static readonly int UserDatabaseVariableAddressMin = 1000000000;

        /// <summary>ユーザDB変数最大値</summary>
        public static readonly int UserDatabaseVariableAddressMax = 1099999999;

        /// <summary>可変DB変数最小値</summary>
        public static readonly int ChangeableDatabaseVariableAddressMin = 1100000000;

        /// <summary>可変DB変数最大値</summary>
        public static readonly int ChangeableDatabaseVariableAddressMax = 1199999999;

        /// <summary>システムDB変数最小値</summary>
        public static readonly int SystemDatabaseVariableAddressMin = 1300000000;

        /// <summary>システムDB変数最大値</summary>
        public static readonly int SystemDatabaseVariableAddressMax = 1399999999;

        /// <summary>変数アドレス値最小値</summary>
        public static int VariableAddressMin => MapEventVariableAddressMin;

        /// <summary>変数アドレス値最大値</summary>
        public static int VariableAddressMax => SystemDatabaseVariableAddressMax;
    }
}