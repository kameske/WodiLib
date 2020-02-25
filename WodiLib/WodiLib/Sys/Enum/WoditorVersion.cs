// ========================================
// Project Name : WodiLib
// File Name    : WoditorVersion.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// ウディタバージョン列挙
    /// </summary>
    public class WoditorVersion : TypeSafeEnum<WoditorVersion>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>Ver2.24</summary>
        public static readonly WoditorVersion Ver2_24;

        /// <summary>Ver2.23</summary>
        public static readonly WoditorVersion Ver2_23;

        /// <summary>Ver2.22</summary>
        public static readonly WoditorVersion Ver2_22;

        /// <summary>Ver2.21</summary>
        public static readonly WoditorVersion Ver2_21;

        /// <summary>Ver2.20</summary>
        public static readonly WoditorVersion Ver2_20;

        /// <summary>Ver2.10</summary>
        public static readonly WoditorVersion Ver2_10;

        /// <summary>Ver2.02a</summary>
        public static readonly WoditorVersion Ver2_02a;

        /// <summary>Ver2.02</summary>
        public static readonly WoditorVersion Ver2_02;

        /// <summary>Ver2.01</summary>
        public static readonly WoditorVersion Ver2_01;

        /// <summary>Ver2.00</summary>
        public static readonly WoditorVersion Ver2_00;

        /// <summary>Ver1.31</summary>
        public static readonly WoditorVersion Ver1_31;

        /// <summary>Ver1.30</summary>
        public static readonly WoditorVersion Ver1_30;

        /// <summary>Ver1.20</summary>
        public static readonly WoditorVersion Ver1_20;

        /// <summary>デフォルト設定値</summary>
        public static WoditorVersion Default => Ver2_24;

        /// <summary>最新バージョン</summary>
        public static WoditorVersion Latest => Ver2_24;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バージョン識別コード
        /// </summary>
        public int VersionCode { get; }

        /// <summary>
        /// バージョン名
        /// </summary>
        public string VersionName { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// staticコンストラクタ
        /// </summary>
        static WoditorVersion()
        {
            Ver2_24 = new WoditorVersion(nameof(Ver2_24), 2240, nameof(Ver2_24));
            Ver2_23 = new WoditorVersion(nameof(Ver2_23), 2230, nameof(Ver2_23));
            Ver2_22 = new WoditorVersion(nameof(Ver2_22), 2220, nameof(Ver2_22));
            Ver2_21 = new WoditorVersion(nameof(Ver2_21), 2210, nameof(Ver2_21));
            Ver2_20 = new WoditorVersion(nameof(Ver2_20), 2200, nameof(Ver2_20));
            Ver2_10 = new WoditorVersion(nameof(Ver2_10), 2100, nameof(Ver2_10));
            Ver2_02a = new WoditorVersion(nameof(Ver2_02a), 2021, nameof(Ver2_02a));
            Ver2_02 = new WoditorVersion(nameof(Ver2_02), 2020, nameof(Ver2_02));
            Ver2_01 = new WoditorVersion(nameof(Ver2_01), 2010, nameof(Ver2_01));
            Ver2_00 = new WoditorVersion(nameof(Ver2_00), 2000, nameof(Ver2_00));
            Ver1_31 = new WoditorVersion(nameof(Ver1_31), 1310, nameof(Ver1_31));
            Ver1_30 = new WoditorVersion(nameof(Ver1_30), 1300, nameof(Ver1_30));
            Ver1_20 = new WoditorVersion(nameof(Ver1_20), 1200, nameof(Ver1_20));
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">識別子</param>
        /// <param name="versionCode">バージョン識別コード</param>
        /// <param name="versionName">バージョン名</param>
        private WoditorVersion(string id, int versionCode, string versionName) : base(id)
        {
            VersionCode = versionCode;
            VersionName = versionName;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// &lt;比較演算子
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺のバージョンコード &lt; 右辺のバージョンコードの場合、true</returns>
        public static bool operator <(WoditorVersion left, WoditorVersion right)
        {
            return left.VersionCode < right.VersionCode;
        }

        /// <summary>
        /// &gt;比較演算子
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺のバージョンコード &gt; 右辺のバージョンコードの場合、true</returns>
        public static bool operator >(WoditorVersion left, WoditorVersion right)
        {
            return left.VersionCode > right.VersionCode;
        }

        /// <summary>
        /// &lt;=比較演算子
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺のバージョンコード &lt; 右辺のバージョンコードの場合、true</returns>
        public static bool operator <=(WoditorVersion left, WoditorVersion right)
        {
            return left.VersionCode <= right.VersionCode;
        }

        /// <summary>
        /// &gt;=比較演算子
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺のバージョンコード &gt; 右辺のバージョンコードの場合、true</returns>
        public static bool operator >=(WoditorVersion left, WoditorVersion right)
        {
            return left.VersionCode >= right.VersionCode;
        }

        /// <summary>
        /// バージョンコードを指定してインスタンスを取得する。
        /// </summary>
        /// <param name="versionCode">バージョンコード</param>
        /// <returns>バージョンインスタンス</returns>
        public static WoditorVersion FromCode(int versionCode)
        {
            return _FindFirst(x => x.VersionCode == versionCode);
        }

        /// <summary>
        /// バージョン名を指定してインスタンスを取得する。
        /// </summary>
        /// <param name="versionName">バージョン名</param>
        /// <returns>バージョンインスタンス</returns>
        public static WoditorVersion FromName(string versionName)
        {
            var resultWithoutSpecial = _FindAll().FirstOrDefault(x => x.VersionName.Equals(versionName));
            if (!(resultWithoutSpecial is null))
            {
                return resultWithoutSpecial;
            }

            // "Default", "Latest"

            if (versionName.Equals(nameof(Default)))
            {
                return Default;
            }

            if (versionName.Equals(nameof(Latest)))
            {
                return Latest;
            }

            // 通常ここには来ない
            throw new InvalidOperationException();
        }
    }
}