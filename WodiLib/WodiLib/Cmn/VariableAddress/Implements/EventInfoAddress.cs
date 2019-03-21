// ========================================
// Project Name : WodiLib
// File Name    : EventInfoAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(9100000, 9179999)] イベント情報アドレス値
    /// </summary>
    public class EventInfoAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public static int MinValue => 9100000;

        /// <summary>最大値</summary>
        public static int MaxValue => 9179999;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>最小値</summary>
        protected override int _MinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最小値</summary>
        protected override int _SafetyMinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最大値</summary>
        protected override int _SafetyMaxValue => MaxValue;

        /// <inheritdoc />
        /// <summary>最大値</summary>
        protected override int _MaxValue => MaxValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ロガー</summary>
        private static readonly WodiLibLogger Logger = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>取得情報</summary>
        public InfoAddressInfoType InfoType => InfoAddressInfoType.FromCode(Value.SubInt(0, 1));

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(9100000, 9179999)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがイベント座標アドレス値として不適切な場合</exception>
        public EventInfoAddress(int value) : base(value)
        {
            // 未対応チェック 未対応の場合警告ログ出力
            VersionCheck(value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バージョンによる定義チェックを行い、警告メッセージを出力する
        /// </summary>
        /// <param name="value">変数アドレス値</param>
        private static void VersionCheck(int value)
        {
            var infoCode = value % 10;

            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_01))
            {
                // 「イベントの座標」のうち、10の位が5または6のアドレスは
                // ウディタVer2.01未満では非対応
                if (infoCode == 5 || infoCode == 6)
                {
                    Logger.Warning(VersionWarningMessage.NotUnderInVariableAddress(
                        value,
                        VersionConfig.GetConfigWoditorVersion(),
                        WoditorVersion.Ver2_01));
                }
            }

            if (infoCode == 7 || infoCode == 8)
            {
                Logger.Warning(VersionWarningMessage.NotUsingVariableAddress(value));
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> EventPositionAddress への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator EventInfoAddress(int src)
        {
            var result = new EventInfoAddress(src);
            return result;
        }

        /// <summary>
        /// EventPositionAddress -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(EventInfoAddress src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// イベント座標アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">加算後の値がイベント座標アドレス値として不適切な場合</exception>
        public static EventInfoAddress operator +(EventInfoAddress src, int value)
        {
            try
            {
                return new EventInfoAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"イベント座標アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// イベント座標アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">減算後の値がイベント座標アドレス値値として不適切な場合</exception>
        public static EventInfoAddress operator -(EventInfoAddress src, int value)
        {
            try
            {
                return new EventInfoAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"イベント座標アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region EventPositionAddress
        /// <summary>
        /// イベント座標アドレス値 - イベント座標アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">イベント座標アドレス左辺</param>
        /// <param name="right">イベント座標アドレス右辺</param>
        /// <returns>イベント座標アドレス値の差</returns>
        public static int operator -(EventInfoAddress left, EventInfoAddress right)
        {
            return left.Value - right.Value;
        }

        #endregion
    }
}