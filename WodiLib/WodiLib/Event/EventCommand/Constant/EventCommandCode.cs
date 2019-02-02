// ========================================
// Project Name : WodiLib
// File Name    : EventCommandCode.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドコード
    /// </summary>
    public static class EventCommandCode
    {
        ///<summary>空白行</summary>
        public static readonly int Blank = 0;

        ///<summary>文章の表示</summary>
        public static readonly int Message = 101;

        ///<summary>コメント文</summary>
        public static readonly int Comment = 103;

        ///<summary>デバッグ文</summary>
        public static readonly int DebugText = 106;

        ///<summary>デバッグ文全消去</summary>
        public static readonly int ClearDebugText = 107;

        ///<summary>文章強制中断</summary>
        public static readonly int StopForceMessage = 105;

        ///<summary>選択肢・開始</summary>
        public static readonly int ChoiceStart = 102;

        ///<summary>選択肢始端</summary>
        public static readonly int ChoiceStartForkingNumber = 401;

        ///<summary>その他分岐始端</summary>
        public static readonly int ChoiceStartForkingEtc = 402;

        ///<summary>キャンセル始端</summary>
        public static readonly int ChoiceStartForkingCancel = 421;

        ///<summary>分岐終端</summary>
        public static readonly int ForkEnd = 499;

        ///<summary>選択肢の強制中断</summary>
        public static readonly int BreakChoiceForce = 104;

        ///<summary>変数操作</summary>
        public static readonly int SetVariable = 121;

        ///<summary>DB操作</summary>
        public static readonly int DBManagement = 250;

        ///<summary>CSV入出力</summary>
        public static readonly int CsvIO = 251;

        ///<summary>文字列操作</summary>
        public static readonly int SetString = 122;

        ///<summary>変数操作+</summary>
        public static readonly int SetVariablePlus = 124;

        ///<summary>条件（変数）・始端</summary>
        public static readonly int ConditionNumberStart = 111;

        ///<summary>条件（変数）・分岐始端</summary>
        public static readonly int ConditionNumberStartForking = 401;

        ///<summary>条件・上記以外</summary>
        public static readonly int ConditionElse = 420;

        ///<summary>条件（文字列）・始端</summary>
        public static readonly int ConditionStringStart = 112;

        ///<summary>条件（文字列）・分岐始端</summary>
        public static readonly int ConditionStringStartForking = 401;

        ///<summary>キー入力</summary>
        public static readonly int KeyInput = 123;

        ///<summary>自動キー入力</summary>
        public static readonly int KeyInputAuto = 125;

        ///<summary>キー入力禁止（基本入力）</summary>
        public static readonly int StandardKeyInputControl = 126;

        ///<summary>キー入力禁止（デバイス入力）</summary>
        public static readonly int DeviceInputControl = 126;

        ///<summary>ピクチャ（表示）</summary>
        public static readonly int PictureShow = 150;

        ///<summary>ピクチャ（移動）</summary>
        public static readonly int PictureMove = 150;

        ///<summary>ピクチャ（消去）</summary>
        public static readonly int PictureErase = 150;

        ///<summary>ピクチャ（ディレイリセット）</summary>
        public static readonly int PictureDelayReset = 150;

        ///<summary>ピクチャエフェクト</summary>
        public static readonly int PictureEffect = 290;

        ///<summary>キャラエフェクト</summary>
        public static readonly int CharacterEffect = 290;

        ///<summary>マップエフェクト（ズーム）</summary>
        public static readonly int MapEffectZoom = 290;

        ///<summary>マップエフェクト（シェイク）</summary>
        public static readonly int MapEffectShake = 280;

        ///<summary>画面スクロール</summary>
        public static readonly int ScrollScreen = 281;

        ///<summary>色調変更</summary>
        public static readonly int ChangeScreenColor = 151;

        ///<summary>サウンド（通常再生）</summary>
        public static readonly int SoundPlayback = 140;

        ///<summary>サウンド（メモリロード）</summary>
        public static readonly int SoundPreload = 140;

        ///<summary>サウンド（メモリ解放）</summary>
        public static readonly int SoundRelease = 140;

        ///<summary>セーブ</summary>
        public static readonly int Save = 220;

        ///<summary>ロード</summary>
        public static readonly int Load = 220;

        ///<summary>内容読込</summary>
        public static readonly int LoadVariable = 221;

        ///<summary>内容書込み</summary>
        public static readonly int SaveVariable = 222;

        ///<summary>パーティ画像</summary>
        public static readonly int PartyGraphic = 270;

        ///<summary>マップチップ通行設定</summary>
        public static readonly int ChangeMapChipSetting = 240;

        ///<summary>チップセット切り替え</summary>
        public static readonly int SwitchChipSet = 241;

        ///<summary>マップチップ上書き</summary>
        public static readonly int OverwriteMapChips = 242;

        ///<summary>場所移動（移動先指定）</summary>
        public static readonly int TransferDestination = 130;

        ///<summary>場所移動（登録位置）</summary>
        public static readonly int TransferSavedPosition = 130;

        ///<summary>合成音声</summary>
        public static readonly int SyntheticVoice = 141;

        ///<summary>無限ループ始端</summary>
        public static readonly int LoopInfiniteStart = 170;

        ///<summary>回数指定ループ始端</summary>
        public static readonly int LoopFiniteStart = 179;

        ///<summary>ループ終端</summary>
        public static readonly int LoopEnd = 498;

        ///<summary>ループ中断</summary>
        public static readonly int LoopBreak = 171;

        ///<summary>ループ開始へ戻る</summary>
        public static readonly int LoopContinue = 176;

        ///<summary>トランジション準備</summary>
        public static readonly int PreparationTransition = 161;

        ///<summary>トランジション実行</summary>
        public static readonly int ExecutionTransition = 162;

        ///<summary>オプション指定トランジション</summary>
        public static readonly int TransitionWithOption = 160;

        ///<summary>イベント処理中のEv移動ON</summary>
        public static readonly int MoveDuringEventsOn = 230;

        ///<summary>イベント処理中のEv移動OFF</summary>
        public static readonly int MoveDuringEventsOff = 231;

        ///<summary>タイトル画面へ戻る</summary>
        public static readonly int GoToTitleScreen = 174;

        ///<summary>ゲーム終了</summary>
        public static readonly int GameEnd = 175;

        ///<summary>ピクチャ以外の画像更新[停止]</summary>
        public static readonly int HaltNonPictureUpdate = 177;

        ///<summary>ピクチャ以外の画像更新[再開]</summary>
        public static readonly int ResumeNonPictureUpdate = 178;

        ///<summary>イベント処理中断</summary>
        public static readonly int ForceExitEvent = 172;

        ///<summary>動作指定</summary>
        public static readonly int MoveRoute = 201;

        ///<summary>動作完了までウェイト</summary>
        public static readonly int WaitForMovement = 202;

        ///<summary>イベント一時消去</summary>
        public static readonly int EraseEvent = 173;

        ///<summary>ウェイト</summary>
        public static readonly int Wait = 180;

        ///<summary>ラベル設置</summary>
        public static readonly int SetLabel = 212;

        ///<summary>指定ラベルに飛ぶ</summary>
        public static readonly int JumpToLabel = 213;

        ///<summary>コモンイベント（ID指定）</summary>
        public static readonly int CallCommonEventById = 210;

        ///<summary>コモンイベント（イベント名）</summary>
        public static readonly int CallCommonEventByName = 300;

        ///<summary>コモンイベント予約</summary>
        public static readonly int CommonEventReserve = 211;

        ///<summary>ダウンロード</summary>
        public static readonly int Download = 260;

        ///<summary>チェックポイント</summary>
        public static readonly int CheckPoint = 99;

        ///<summary>チェックポイント（特殊）</summary>
        public static readonly int SpecialCheckPoint = 99;
    }
}