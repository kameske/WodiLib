// ========================================
// Project Name : WodiLib
// File Name    : GameIniRootData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.IO;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// Game.ini データ設定値クラス
    /// </summary>
    internal class GameIniRootData : IIniTarget
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// セクション名
        /// </summary>
        public string SectionName => null;

        /// <summary>
        /// Game.exe起動済みフラグ
        /// </summary>
        [IniTarget]
        public string Start { get; set; } = "";

        /// <summary>
        /// グラフィック表示モード
        /// </summary>
        [IniTarget]
        public string SoftModeFlag { get; set; } = "";

        /// <summary>
        /// 画面ウィンドウ設定
        /// </summary>
        [IniTarget]
        public string WindowModeFlag { get; set; } = "";

        /// <summary>
        /// BGMの再生＋効果音の再生
        /// </summary>
        [IniTarget]
        public string SEandBGM { get; set; } = "";

        /// <summary>
        /// フレームスキップ
        /// </summary>
        [IniTarget]
        public string FrameSkip { get; set; } = "";

        /// <summary>
        /// プロキシホスト
        /// </summary>
        [IniTarget]
        public string Proxy { get; set; } = "";

        /// <summary>
        /// プロキシポート
        /// </summary>
        [IniTarget]
        public string ProxyPort { get; set; } = "";

        /// <summary>
        /// スクリーンショット許可フラグ
        /// </summary>
        [IniTarget]
        public string ScreenShotFlag { get; set; } = "";

        /// <summary>
        /// 【Ver2.20以降】F12リセット許可フラグ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_20))]
        public string F12_Reset { get; set; } = "";

        /// <summary>
        /// 【Ver2.20以降】起動ディスプレイ番号
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_20))]
        public string Display_Number { get; set; } = "";

        /// <summary>
        /// 【Ver2.22以降】DirectXバージョンフラグ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_22))]
        public string Old_DirectX_Use { get; set; } = "";
    }
}