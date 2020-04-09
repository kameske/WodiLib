// ========================================
// Project Name : WodiLib
// File Name    : EditorIniProgramData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.IO;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// Editor.ini プログラムデータ設定値クラス
    /// </summary>
    internal class EditorIniProgramData : IIniTarget
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// セクション名
        /// </summary>
        public string SectionName => "ProgramData";

        /// <summary>
        /// 【Ver2.20以降】開始フラグ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_20))]
        public string StartFlag { get; set; } = "";

        /// <summary>
        /// 最終読み込みマップファイルパス
        /// </summary>
        [IniTarget]
        public string LastLoadFile { get; set; } = "";

        /// <summary>
        /// メインウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowPosX { get; set; } = "";

        /// <summary>
        /// メインウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowPosY { get; set; } = "";

        /// <summary>
        /// メインウィンドウサイズ幅
        /// </summary>
        [IniTarget]
        public string WindowSizeX { get; set; } = "";

        /// <summary>
        /// メインウィンドウサイズ高さ
        /// </summary>
        [IniTarget]
        public string WindowSizeY { get; set; } = "";

        /// <summary>
        /// マップチップウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowMapChipX { get; set; } = "";

        /// <summary>
        /// マップチップウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowMapChipY { get; set; } = "";

        /// <summary>
        /// マップイベントウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowEventX { get; set; } = "";

        /// <summary>
        /// マップイベントウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowEventY { get; set; } = "";

        /// <summary>
        /// 【Ver2.10以降】マップイベントウィンドウ幅
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_10))]
        public string WindowEventSizeX { get; set; } = "";

        /// <summary>
        /// 【Ver2.10以降】マップイベントウィンドウ高さ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_10))]
        public string WindowEventSizeY { get; set; } = "";

        /// <summary>
        /// マップイベントコマンド入力ウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowEventInputX { get; set; } = "";

        /// <summary>
        /// マップイベントコマンド入力ウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowEventInputY { get; set; } = "";

        /// <summary>
        /// コモンイベントウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowCommonX { get; set; } = "";

        /// <summary>
        /// コモンイベントウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowCommonY { get; set; } = "";

        /// <summary>
        /// 【Ver2.10以降】コモンイベントウィンドウ幅
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_10))]
        public string WindowCommonSizeX { get; set; } = "";

        /// <summary>
        /// 【Ver2.10以降】コモンイベントウィンドウ高さ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_10))]
        public string WindowCommonSizeY { get; set; } = "";

        /// <summary>
        /// コモンイベント入力ウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowCommonInputX { get; set; } = "";

        /// <summary>
        /// コモンイベント入力ウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowCommonInputY { get; set; } = "";

        /// <summary>
        /// ユーザDBウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowUserDBX { get; set; } = "";

        /// <summary>
        /// ユーザDBウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowUserDBY { get; set; } = "";

        /// <summary>
        /// 可変DBウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowCDBX { get; set; } = "";

        /// <summary>
        /// 可変DBウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowCDBY { get; set; } = "";

        /// <summary>
        /// システムDBウィンドウ左上X座標
        /// </summary>
        [IniTarget]
        public string WindowSysDBX { get; set; } = "";

        /// <summary>
        /// システムDBウィンドウ左上Y座標
        /// </summary>
        [IniTarget]
        public string WindowSysDBY { get; set; } = "";

        /// <summary>
        /// DBウィンドウ項目番号表示フラグ
        /// </summary>
        [IniTarget]
        public string DataBaseValueNumDraw { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】ウディタ作業時間表示フラグ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string EditTimeUseFlag { get; set; } = "";

        /// <summary>
        /// ウディタ作業時間（アクティブ時間）[1/2minute]
        /// </summary>
        [IniTarget]
        public string EditTime { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】ウディタ作業時間（非アクティブ時間）[1/2minute]
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string NotEditTime { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】デバッグウィンドウ使用フラグ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string UseDebugWindow { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】マップ編集・下レイヤーの暗さ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string LayerTransparent { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】マップ編集・Evレイヤー不透明度
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string EventLayerDraw { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド配色
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string EventCommandColorType { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】マップ編集・遠景表示有無
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string DrawBackGroundFlag { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】ゲームデータ作成時にコピーしないファイル拡張子
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string NotCopyExt { get; set; } = "";

        /// <summary>
        /// 【Ver2.20以降】？
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_20), nameof(WoditorVersion.Ver2_20))]
        public string CommandViewType { get; set; } = "";

        /// <summary>
        /// 【Ver2.20以降】Datファイル自動バックアップ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_20))]
        public string BackupNum { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー01
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut01 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー02
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut02 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー03
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut03 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー04
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut04 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー05
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut05 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー06
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut06 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー07
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut07 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー08
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut08 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー09
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut09 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー10
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut10 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー11
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut11 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー12
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut12 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー13
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut13 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー14
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut14 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー15
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut15 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー16
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut16 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー17
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut17 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー18
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut18 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー19
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut19 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー20
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut20 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー21
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut21 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー22
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut22 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー23
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut23 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー24
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut24 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー25
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut25 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー26
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut26 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー27
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut27 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー28
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut28 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー29
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut29 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンドウィンドウショートカットキー30
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut30 { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順01
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut01Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順02
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut02Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順03
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut03Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順04
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut04Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順05
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut05Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順06
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut06Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順07
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut07Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順08
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut08Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順09
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut09Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順10
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut10Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順11
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut11Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順12
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut12Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順13
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut13Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順14
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut14Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順15
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut15Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順16
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut16Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順17
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut17Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順18
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut18Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順19
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut19Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順20
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut20Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順21
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut21Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順22
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut22Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順23
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut23Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順24
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut24Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順25
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut25Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順26
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut26Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順27
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut27Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順28
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut28Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順29
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut29Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順30
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut30Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.00以降】イベントコマンド表示順31
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_00))]
        public string ShortCut31Pos { get; set; } = "";

        /// <summary>
        /// 【Ver2.10以降】上級者向けコマンド使用フラグ
        /// </summary>
        [IniTarget(nameof(WoditorVersion.Ver2_10))]
        public string ExpertCommand { get; set; } = "";
    }
}