// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event
{
    /// <summary>
    /// 数値変数ヘルパークラス
    /// （廃止予定）
    /// </summary>
    [Obsolete]
    public static class VariableAddressHelper
    {
        /// <summary>ロガー</summary>
        private static readonly WodiLibLogger Logger = WodiLibLogger.GetInstance();

        /// <summary>
        /// 数値変数値かどうかを判定する。厳密な判定ではないことに注意（1000000～1399999999はすべてtrueとなる）
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが1000000～1399999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsVariableAddress(int number)
        {
            if (number < VariableAddressConstant.VariableAddressMin) return false;
            if (number > VariableAddressConstant.VariableAddressMax) return false;
            return true;
        }

        /// <summary>
        ///     変数アドレス値かどうかを厳密に判定する。
        ///     すべてのチェックメソッドを順番に実行するため処理コストに注意。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     変数アドレス値の場合、true。
        ///     変数アドレス値ではない場合、false。
        /// </returns>
        public static bool IsVariableAddressStrict(int number)
        {
            if (IsMapEventVariableAddress(number)) return true;
            if (IsThisMapEventVariableAddress(number)) return true;
            if (IsCommonEventVariableAddress(number)) return true;
            if (IsThisCommonEventVariableAddress(number)) return true;
            if (IsNormalNumberVariableAddress(number)) return true;
            if (IsSpareNumberVariableAddress(number)) return true;
            if (IsStringVariableAddress(number)) return true;
            if (IsRandomVariableAddress(number)) return true;
            if (IsSystemVariableAddress(number)) return true;
            if (IsEventPositionAddress(number)) return true;
            if (IsHeroPositionAddress(number)) return true;
            if (IsMemberPositionAddress(number)) return true;
            if (IsSystemStringVariableAddress(number)) return true;
            if (IsUserDatabaseVariableAddress(number)) return true;
            if (IsChangeableDatabaseVariableAddress(number)) return true;
            if (IsSystemDatabaseVariableAddress(number)) return true;
            return false;
        }

        /// <summary>
        /// "マップイベントのセルフ変数"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが1000000～1099999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsMapEventVariableAddress(int number)
        {
            if (number < VariableAddressConstant.MapEventVariableAddressMin) return false;
            if (number > VariableAddressConstant.MapEventVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "このマップイベントのセルフ変数"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが1100000～1100009の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsThisMapEventVariableAddress(int number)
        {
            if (number < VariableAddressConstant.ThisMapEventVariableAddressMin) return false;
            if (number > VariableAddressConstant.ThisMapEventVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "コモンイベントのセルフ変数"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが15000000～15999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsCommonEventVariableAddress(int number)
        {
            if (number < VariableAddressConstant.CommonEventVariableAddressMin) return false;
            if (number > VariableAddressConstant.CommonEventVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "このコモンイベントのセルフ変数"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが16000000～16000099の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsThisCommonEventVariableAddress(int number)
        {
            if (number < VariableAddressConstant.ThisCommonEventVariableAddressMin) return false;
            if (number > VariableAddressConstant.ThisCommonEventVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "通常変数X番"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが2000000～2099999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsNormalNumberVariableAddress(int number)
        {
            if (number < VariableAddressConstant.NormalNumberVariableAddressMin) return false;
            if (number > VariableAddressConstant.NormalNumberVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "予備変数YのX番"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが2100000～2999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsSpareNumberVariableAddress(int number)
        {
            if (number < VariableAddressConstant.SpareNumberVariableAddressMin) return false;
            if (number > VariableAddressConstant.SpareNumberVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "文字列変数X番"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが3000000～3999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsStringVariableAddress(int number)
        {
            if (number < VariableAddressConstant.StringVariableAddressMin) return false;
            if (number > VariableAddressConstant.StringVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "0からXまでのランダム値"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが8000000～8999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsRandomVariableAddress(int number)
        {
            if (number < VariableAddressConstant.RandomVariableAddressMin) return false;
            if (number > VariableAddressConstant.RandomVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "システム変数X番"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが9000000～9099999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsSystemVariableAddress(int number)
        {
            if (number < VariableAddressConstant.SystemVariableAddressMin) return false;
            if (number > VariableAddressConstant.SystemVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "イベントの座標"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが9100000～9179999の場合、true。
        ///     それ以外の場合、false。
        ///     ※位置の位が7,8の場合、範囲内の値であってもfalse。
        /// </returns>
        public static bool IsEventPositionAddress(int number)
        {
            if (number < VariableAddressConstant.EventPositionAddressMin) return false;
            if (number > VariableAddressConstant.EventPositionAddressMax) return false;

            var infoCode = number % 10;

            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_01))
            {
                if (infoCode == 5 || infoCode == 6)
                {
                    Logger.Warning(VersionWarningMessage.NotUnderInVariableAddress(
                        number,
                        VersionConfig.GetConfigWoditorVersion(),
                        WoditorVersion.Ver2_01));
                }
            }

            if (infoCode == 7 || infoCode == 8) return false;

            return true;
        }

        /// <summary>
        /// "主人公の座標"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが9180000～9180009の場合、true。
        ///     それ以外の場合、false。
        ///     ※位置の位が7,8の場合、範囲内の値であってもfalse。
        /// </returns>
        public static bool IsHeroPositionAddress(int number)
        {
            if (number < VariableAddressConstant.HeroPositionAddressMin) return false;
            if (number > VariableAddressConstant.HeroPositionAddressMax) return false;

            var infoCode = number % 10;
            if (infoCode == 7 || infoCode == 8) return false;

            return true;
        }

        /// <summary>
        /// "仲間の座標"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが9180010～9180059の場合、true。
        ///     それ以外の場合、false。
        ///     ※位置の位が7,8の場合、範囲内の値であってもfalse。
        /// </returns>
        public static bool IsMemberPositionAddress(int number)
        {
            if (number < VariableAddressConstant.MemberPositionAddressMin) return false;
            if (number > VariableAddressConstant.MemberPositionAddressMax) return false;

            var infoCode = number % 10;
            if (infoCode == 7 || infoCode == 8) return false;

            return true;
        }

        /// <summary>
        /// "システム文字列X番"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが9900000～9999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsSystemStringVariableAddress(int number)
        {
            if (number < VariableAddressConstant.SystemStringVariableAddressMin) return false;
            if (number > VariableAddressConstant.SystemStringVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "ユーザDB"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが1000000000～1099999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsUserDatabaseVariableAddress(int number)
        {
            if (number < VariableAddressConstant.UserDatabaseVariableAddressMin) return false;
            if (number > VariableAddressConstant.UserDatabaseVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "可変DB"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが1100000000～1199999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsChangeableDatabaseVariableAddress(int number)
        {
            if (number < VariableAddressConstant.ChangeableDatabaseVariableAddressMin) return false;
            if (number > VariableAddressConstant.ChangeableDatabaseVariableAddressMax) return false;
            return true;
        }

        /// <summary>
        /// "可変DB"を表す変数アドレス値かどうかを判定する。
        /// </summary>
        /// <param name="number">判定する数値</param>
        /// <returns>
        ///     numberが1300000000～1399999999の場合、true。
        ///     それ以外の場合、false。
        /// </returns>
        public static bool IsSystemDatabaseVariableAddress(int number)
        {
            if (number < VariableAddressConstant.SystemDatabaseVariableAddressMin) return false;
            if (number > VariableAddressConstant.SystemDatabaseVariableAddressMax) return false;
            return true;
        }
    }
}