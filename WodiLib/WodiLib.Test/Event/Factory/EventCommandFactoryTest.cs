using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event
{
    using Factory = EventCommandFactory;

    /// <summary>
    /// イベントコマンドFactoryテスト
    /// </summary>
    [TestFixture]
    public class EventCommandFactoryTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        /// <summary>
        /// イベントコマンド「空白行」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateBlankTest()
        {
            var _ = new Blank();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「選択肢の強制中断」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateBreakChoiceForceTest()
        {
            var _ = new BreakChoiceForce();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「コモンイベント（ID指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCallCommonEventByIdTest()
        {
            var _ = new CallCommonEventById();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「コモンイベント（イベント名）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCallCommonEventByNameTest()
        {
            var _ = new CallCommonEventByName();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「マップチップ通行設定」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChangeMapChipSettingTest()
        {
            var _ = new ChangeMapChipSetting();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「色調変更」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChangeScreenColorTest()
        {
            var _ = new ChangeScreenColor();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キャラエフェクト」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCharacterEffectTest()
        {
            var _ = new CharacterEffect();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「チェックポイント」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCheckPointTest()
        {
            var _ = new CheckPoint();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「分岐終端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateForkEndTest()
        {
            var _ = new ForkEnd();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「選択肢・開始」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChoiceStartTest()
        {
            var _ = new ChoiceStart();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キャンセル始端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChoiceStartForkingCancelTest()
        {
            var _ = new ChoiceStartForkingCancel();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「強制中断分岐」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChoiceStartForkingForceExitTest()
        {
            var _ = new ChoiceStartForkingForceExit();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「左キー分岐」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChoiceStartForkingLeftKeyTest()
        {
            var _ = new ChoiceStartForkingLeftKey();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「右キー分岐」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateChoiceStartForkingRightKeyTest()
        {
            var _ = new ChoiceStartForkingRightKey();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「デバッグ文全消去」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateClearDebugTextTest()
        {
            var _ = new ClearDebugText();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「コメント文」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCommentTest()
        {
            var _ = new Comment();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「コモンイベント予約」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCommonEventReserveTest()
        {
            var _ = new CommonEventReserve();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「条件（変数）・始端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateConditionNumberStartTest()
        {
            var _ = new ConditionNumberStart();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「条件（文字列）・始端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateConditionStringStartTest()
        {
            var _ = new ConditionStringStart();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「CSV入出力」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateCsvIOTest()
        {
            var _ = new CsvIO();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（全データ初期化）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementClearDataTest()
        {
            var _ = new DBManagementClearData();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（全項目初期化）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementClearFieldTest()
        {
            var _ = new DBManagementClearField();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（データ番号取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetDataIdTest()
        {
            var _ = new DBManagementGetDataId();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（データ数取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetDataLengthTest()
        {
            var _ = new DBManagementGetDataLength();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（データ名取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetDataNameTest()
        {
            var _ = new DBManagementGetDataName();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（項目番号取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetItemIdTest()
        {
            var _ = new DBManagementGetItemId();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（項目数取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetItemLengthTest()
        {
            var _ = new DBManagementGetItemLength();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（項目名取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetItemNameTest()
        {
            var _ = new DBManagementGetItemName();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（タイプ番号取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetTypeIdTest()
        {
            var _ = new DBManagementGetTypeId();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（タイプ名取得）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementGetTypeNameTest()
        {
            var _ = new DBManagementGetTypeName();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（数値入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementInputNumberTest()
        {
            var _ = new DBManagementInputNumber();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（文字入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementInputStringTest()
        {
            var _ = new DBManagementInputString();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「DB操作（出力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDBManagementOutputTest()
        {
            var _ = new DBManagementOutput();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「デバッグ文」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDebugTextTest()
        {
            var _ = new DebugText();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キー入力禁止（デバイス入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDeviceInputControlTest()
        {
            var _ = new DeviceInputControl();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ダウンロード」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateDownloadTest()
        {
            var _ = new Download();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「イベント一時消去」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateEraseEventTest()
        {
            var _ = new EraseEvent();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「トランジション実行」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateExecutionTransitionTest()
        {
            var _ = new ExecutionTransition();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「イベント処理中断」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateForceExitEventTest()
        {
            var _ = new ForceExitEvent();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ゲーム終了」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateGameEndTest()
        {
            var _ = new GameEnd();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「タイトル画面へ戻る」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateGoToTitleScreenTest()
        {
            var _ = new GoToTitleScreen();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ以外の画像更新[停止]」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateHaltNonPictureUpdateTest()
        {
            var _ = new HaltNonPictureUpdate();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「指定ラベルに飛ぶ」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateJumpToLabelTest()
        {
            var _ = new JumpToLabel();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「自動キー入力（基本入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputAutoBasicTest()
        {
            var _ = new KeyInputAutoBasic();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「自動キー入力（キーボード）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputAutoKeyboardTest()
        {
            var _ = new KeyInputAutoKeyboard();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「自動キー入力（マウス）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputAutoMouseTest()
        {
            var _ = new KeyInputAutoMouse();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キー入力（基本）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputBasicTest()
        {
            var _ = new KeyInputBasic();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キー入力（キーボード）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputKeyboardTest()
        {
            var _ = new KeyInputKeyboard();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キー入力（マウス）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputMouseTest()
        {
            var _ = new KeyInputMouse();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キー入力（パッド）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateKeyInputPadTest()
        {
            var _ = new KeyInputPad();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ロード」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoadTest()
        {
            var _ = new Load();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ループ中断」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoopBreakTest()
        {
            var _ = new LoopBreak();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ループ開始へ戻る」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoopContinueTest()
        {
            var _ = new LoopContinue();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ループ終端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoopFiniteEndTest()
        {
            var _ = new LoopEnd();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「回数指定ループ始端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoopFiniteStartTest()
        {
            var _ = new LoopFiniteStart();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「無限ループ始端」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoopInfiniteStartTest()
        {
            var _ = new LoopInfiniteStart();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「マップエフェクト（シェイク）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateMapEffectShakeTest()
        {
            var _ = new MapEffectShake();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「マップエフェクト（ズーム）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateMapEffectZoomTest()
        {
            var _ = new MapEffectZoom();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「文章の表示」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateMessageTest()
        {
            var _ = new Message();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「イベント処理中のEv移動OFF」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateMoveDuringEventsOffTest()
        {
            var _ = new MoveDuringEventsOff();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「イベント処理中のEv移動ON」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateMoveDuringEventsOnTest()
        {
            var _ = new MoveDuringEventsOn();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「動作指定」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateMoveRouteTest()
        {
            var _ = new MoveRoute();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「マップチップ上書き」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateOverwriteMapChipsTest()
        {
            var _ = new OverwriteMapChips();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「パーティ画像（挿入）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePartyGraphicInsertTest()
        {
            var _ = new PartyGraphicInsert();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「パーティ画像（削除）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePartyGraphicRemoveTest()
        {
            var _ = new PartyGraphicRemove();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「パーティ画像（画像指定削除）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePartyGraphicRemoveGraphicTest()
        {
            var _ = new PartyGraphicRemoveGraphic();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「パーティ画像（入れ替え）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePartyGraphicReplaceTest()
        {
            var _ = new PartyGraphicReplace();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「パーティ画像（特殊処理）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePartyGraphicSpecialTest()
        {
            var _ = new PartyGraphicSpecial();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（ディレイリセット）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureDelayResetTest()
        {
            var _ = new PictureDelayReset();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャエフェクト」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureEffectTest()
        {
            var _ = new PictureEffect();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（消去）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureEraseTest()
        {
            var _ = new PictureErase();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（移動）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureMoveTest()
        {
            var _ = new PictureMove();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（表示） ファイル読み込み（直接指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureShowLoadFileDirectTest()
        {
            var _ = new PictureShowLoadFileDirect();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（表示） ファイル読み込み（文字列変数でファイル指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureShowLoadFileVariableTest()
        {
            var _ = new PictureShowLoadFileVariable();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（表示） お手軽ウィンドウ（直接指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureShowSimpleWindowDirectTest()
        {
            var _ = new PictureShowSimpleWindowDirect();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（表示） お手軽ウィンドウ（文字列変数でファイル指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureShowSimpleWindowVariableTest()
        {
            var _ = new PictureShowSimpleWindowVariable();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ（表示） ファイル読み込み（文字列をピクチャとして描画）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePictureShowStringTest()
        {
            var _ = new PictureShowString();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「トランジション準備」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreatePreparationTransitionTest()
        {
            var _ = new PreparationTransition();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「セーブデータの内容読込」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateLoadSpecificSaveDataTest()
        {
            var _ = new LoadSpecificSaveData();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ピクチャ以外の画像更新[再開]」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateResumeNonPictureUpdateTest()
        {
            var _ = new ResumeNonPictureUpdate();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「セーブ」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSaveTest()
        {
            var _ = new Save();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「内容書込み」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSaveSpecificSaveDataTest()
        {
            var _ = new SaveSpecificSaveData();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「画面スクロール」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateScrollScreenTest()
        {
            var _ = new ScrollScreen();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ラベル設置」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetLabelTest()
        {
            var _ = new SetLabel();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「文字列操作（キーボード入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetStringKeyboardInputTest()
        {
            var _ = new SetStringKeyboardInput();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「文字列操作（手動入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetStringManualTest()
        {
            var _ = new SetStringManual();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「文字列操作（変数指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetStringReferVarTest()
        {
            var _ = new SetStringReferVar();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「文字列操作（文字列変数）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetStringStringVarTest()
        {
            var _ = new SetStringStringVar();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「オプション指定トランジション」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetTransitionTest()
        {
            var _ = new TransitionWithOption();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「変数操作」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetVariableTest()
        {
            var _ = new SetVariable();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「変数操作+（キャラ）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetVariablePlusCharaTest()
        {
            var _ = new SetVariablePlusChara();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「変数操作+（その他）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetVariablePlusEtcTest()
        {
            var _ = new SetVariablePlusEtc();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「変数操作+（ピクチャ番号）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetVariablePlusPictureTest()
        {
            var _ = new SetVariablePlusPicture();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「変数操作+（位置）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSetVariablePlusPositionTest()
        {
            var _ = new SetVariablePlusPosition();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「サウンド（通常再生）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSoundPlaybackTest()
        {
            var _ = new SoundPlayback();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「サウンド（メモリロード）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSoundPreloadTest()
        {
            var _ = new SoundPreload();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「サウンド（メモリ解放）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSoundReleaseTest()
        {
            var _ = new SoundReleaseAll();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「チェックポイント（特殊）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSpecialCheckPointTest()
        {
            var _ = new SpecialCheckPoint();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「キー入力禁止（基本入力）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateStandardKeyInputControlTest()
        {
            var _ = new StandardKeyInputControl();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「文章強制中断」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateStopForceMessageTest()
        {
            var _ = new StopForceMessage();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「チップセット切り替え」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSwitchChipSetTest()
        {
            var _ = new SwitchChipSet();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「合成音声」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateSyntheticVoiceTest()
        {
            var _ = new SyntheticVoice();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「場所移動（移動先指定）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateTransferDestinationTest()
        {
            var _ = new TransferDestination();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「場所移動（登録位置）」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateTransferSavedPositionTest()
        {
            var _ = new TransferSavedPosition();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「ウェイト」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateWaitTest()
        {
            var _ = new WaitEventCommand();
            Assert.True(true);
        }

        /// <summary>
        /// イベントコマンド「動作完了までウェイト」のインスタンス生成
        /// </summary>
        /// <returns></returns>
        [Test]
        public static void CreateWaitForMovementTest()
        {
            var _ = new WaitForMovement();
            Assert.True(true);
        }

        /// <summary>
        /// 文字列からインスタンス生成テスト
        /// </summary>
        /// <param name="src"></param>
        [TestCase("[0][0,0]<1>()()")]
        [TestCase("[99][1,0]<0>(1)()")]
        [TestCase("[101][0,1]<0>()(\"文章の表示\")")]
        [TestCase("[102][1,5]<0>(869)(\"選択肢A\",\"選択肢B\",\"選択肢C\",\"選択肢D\",\"選択肢E（キャンセル分岐）\")")]
        [TestCase("[103][0,1]<0>()(\"コメント\")")]
        [TestCase("[105][0,0]<0>()()")]
        [TestCase("[106][0,1]<0>()(\"デバッグ文<\\n>\\cdb[0:1:2]\")")]
        [TestCase("[107][0,0]<0>()()")]
        [TestCase("[111][10,0]<0>(3,1100000,20,2,1000000,1024000000,0,1000014,2900034,20)()")]
        [TestCase("[112][7,4]<0>(4,3000000,288212672,539870912,808306368,0,3000004)(\"条件1\",\"\",\"条件3\",\"条件4\")")]
        [TestCase("[121][4,0]<0>(1000000,1025000,2000000,68)()")]
        [TestCase("[121][4,0]<0>(2000003,1100006,2000000,8778)()")]
        [TestCase("[121][4,0]<0>(2000003,30,400,17154)()")]
        [TestCase("[121][4,0]<0>(2000004,30,40,1536)()")]
        [TestCase("[121][4,0]<0>(2000004,30,40,1792)()")]
        [TestCase("[121][4,0]<0>(2000004,30,40,2048)()")]
        [TestCase("[121][4,0]<0>(2000004,30,40,2560)()")]
        [TestCase("[121][4,0]<0>(2000004,30,40,2816)()")]
        [TestCase("[121][4,0]<0>(2000004,30,40,63744)()")]
        [TestCase("[121][4,0]<0>(2000004,30,400,25944)()")]
        [TestCase("[121][5,0]<0>(2000004,30,400,87057,3)()")]
        [TestCase("[121][7,0]<0>(1100000000,1100006,2000000,4409,3,2,10)()")]
        [TestCase("[122][2,1]<0>(3000002,2560)(\"文字列入力\")")]
        [TestCase("[122][2,1]<0>(3000002,2816)(\"文字列入力\")")]
        [TestCase("[122][2,1]<0>(9900000,0)(\"文字列入力\")")]
        [TestCase("[122][2,2]<0>(3000002,2304)(\"文字列入力\",\"置換先\")")]
        [TestCase("[122][3,0]<0>(2000002,273,9900000)()")]
        [TestCase("[122][3,0]<0>(3000000,8963,15)()")]
        [TestCase("[122][3,0]<0>(3000002,1026,2000010)()")]
        [TestCase("[122][3,0]<0>(3000002,1281,9900000)()")]
        [TestCase("[122][3,0]<0>(3000002,1537,9900000)()")]
        [TestCase("[122][3,0]<0>(3000002,1793,9900000)()")]
        [TestCase("[122][3,0]<0>(3000002,2049,9900000)()")]
        [TestCase("[122][3,0]<0>(9900002,4611,15)()")]
        [TestCase("[123][2,0]<0>(2000000,241)()")]
        [TestCase("[123][2,0]<0>(2000000,4)()")]
        [TestCase("[123][2,0]<0>(2000000,770)()")]
        [TestCase("[123][2,0]<0>(2000000,864)()")]
        [TestCase("[123][3,0]<0>(2000000,260,100)()")]
        [TestCase("[123][3,0]<0>(2000000,4356,130)()")]
        [TestCase("[123][3,0]<0>(2000000,4610,301)()")]
        [TestCase("[123][3,0]<0>(2000000,514,301)()")]
        [TestCase("[125][1,0]<0>(21)()")]
        [TestCase("[125][1,0]<0>(226)()")]
        [TestCase("[125][1,0]<0>(536870915)()")]
        [TestCase("[125][2,0]<0>(268435456,122)()")]
        [TestCase("[125][3,0]<0>(536870925,20,400)()")]
        [TestCase("[125][3,0]<0>(536870933,3,0)()")]
        [TestCase("[126][1,0]<0>(211)()")]
        [TestCase("[126][1,0]<0>(247)()")]
        [TestCase("[126][1,0]<0>(256)()")]
        [TestCase("[126][1,0]<0>(268435458)()")]
        [TestCase("[126][1,0]<0>(268435459)()")]
        [TestCase("[126][1,0]<0>(268435713)()")]
        [TestCase("[126][1,0]<0>(53)()")]
        [TestCase("[126][1,0]<0>(641)()")]
        [TestCase("[126][2,0]<0>(268435456,180)()")]
        [TestCase("[130][5,0]<0>(-1,3,5,1,0)()")]
        [TestCase("[130][5,0]<0>(-1,6,8,-1,17)()")]
        [TestCase("[130][5,0]<0>(-10001,5,0,0,33)()")]
        [TestCase("[130][5,0]<0>(1100002,3,5,0,16)()")]
        [TestCase("[130][5,0]<0>(2,1100000,1100001,0,0)()")]
        [TestCase("[130][5,0]<0>(-2,6,8,0,16)()")]
        [TestCase("[140][4,0]<0>(16777216,5,2000040,2)()")]
        [TestCase("[140][4,0]<0>(16777232,5,2000002,2)()")]
        [TestCase("[140][4,0]<0>(16777248,5,1100003,2)()")]
        [TestCase("[140][4,0]<0>(288,5,0,2)()")]
        [TestCase("[140][4,0]<0>(512,1,0,4)()")]
        [TestCase("[140][4,0]<0>(784,5,0,2)()")]
        [TestCase("[140][6,1]<0>(33554464,3,0,4,105,100)(\"SE/サウンド.wav\")")]
        [TestCase("[140][7,1]<0>(33554432,3,0,4,100,105,20)(\"BGM/bgm.mp3\")")]
        [TestCase("[140][7,1]<0>(33554433,0,0,0,105,100,20)(\"BGM/bgm.mp3\")")]
        [TestCase("[140][7,1]<0>(33554435,0,0,0,105,100,20)(\"BGM/bgm.mp3\")")]
        [TestCase("[140][7,1]<0>(33554448,3,0,4,100,105,20)(\"BGS/音楽.mp3\")")]
        [TestCase("[150][2,0]<0>(33559043,100)()")]
        [TestCase("[150][4,0]<0>(33559042,100,11,22)()")]
        [TestCase(
            "[150][15,1]<0>(54533920,120,11,1,10,-1000000,-1000000,10,20,-1000000,-1000000,0,16777216,22,123)(\"表示文字列\")")]
        [TestCase("[150][18,0]<0>(33559041,100,11,0,0,3,65,30,32,110,4000,0,33554432,22,0,70,80,90)()")]
        [TestCase("[150][18,0]<0>(33571392,120,11,4,4,3,65,10,20,1000060,4000,3000015,33554432,22,0,70,80,90)()")]
        [TestCase(
            "[150][18,1]<0>(0,120,20,4,3,1,255,200,300,95,18,0,33554432,30,0,101,102,103)(\"CharaChip/[Animal]Chicken.png\")")]
        [TestCase(
            "[150][18,1]<0>(33567280,120,11,4,4,3,65,10,20,1000060,4000,0,33554432,22,0,70,80,90)(\"SystemGraphic/Text_Pause.png\")")]
        [TestCase("[150][18,1]<0>(32,3221,0,1,1,1,255,0,0,100,0,0,33554432,51,0,100,100,100)(\"ディレイあり、カラー100、同値false\")")]
        [TestCase("[150][18,0]<0>(16777217,3,0,0,0,1,255,0,0,100,0,0,33554432,0,4,100,100,100)()")]
        [TestCase(
            "[150][19,0]<0>(19996944,120,1000030,1,10,1000010,1000020,1000000,1000001,1000060,1000050,3000020,33554432,1000040,123,1000080,1000081,1000082,1000070)()")]
        [TestCase(
            "[150][25,1]<0>(100667904,120,11,4,4,3,65,1,2,1000060,4000,0,33554432,22,0,70,80,90,0,3,4,5,6,7,8)(\"3000015\")")]
        [TestCase("[151][2,0]<0>(3942420,10)()")]
        [TestCase("[151][5,0]<0>(16777216,1100003,1100000,1100001,1100002)()")]
        [TestCase("[160][2,0]<1>(-1,3)()")]
        [TestCase("[160][2,0]<1>(1,65566)()")]
        [TestCase("[161][0,0]<1>()()")]
        [TestCase("[162][0,0]<1>()()")]
        [TestCase("[170][0,0]<0>()()")]
        [TestCase("[171][0,0]<1>()()")]
        [TestCase("[172][0,0]<1>()()")]
        [TestCase("[173][2,0]<1>(0,1100000)()")]
        [TestCase("[173][2,0]<1>(1100002,30)()")]
        [TestCase("[173][2,0]<1>(-2,1)()")]
        [TestCase("[174][0,0]<1>()()")]
        [TestCase("[175][0,0]<1>()()")]
        [TestCase("[176][0,0]<1>()()")]
        [TestCase("[177][0,0]<1>()()")]
        [TestCase("[178][0,0]<1>()()")]
        [TestCase("[179][1,0]<1>(5)()")]
        [TestCase("[179][1,0]<2>(2900032)()")]
        [TestCase("[180][1,0]<1>(1020000000)()")]
        [TestCase("[180][1,0]<1>(5)()")]
        [TestCase("[202][0,0]<1>()()")]
        [TestCase("[210][10,0]<2>(500001,68,1,2,-2,4,3000000,3000001,3000002,3000003)()")]
        [TestCase("[210][10,0]<2>(500005,68,0,1,2,3,3000000,3000001,3000002,3000003)()")]
        [TestCase("[210][10,5]<2>(500005,61508,0,1,2,3,0,0,0,0)(\"\",\"0\",\"1\",\"2\",\"3\")")]
        [TestCase("[210][2,0]<2>(1,0)()")]
        [TestCase("[210][2,0]<2>(2100000,5)()")]
        [TestCase("[210][2,0]<2>(3,9)()")]
        [TestCase("[210][2,0]<2>(500000,0)()")]
        [TestCase("[210][2,0]<2>(500002,0)()")]
        [TestCase("[210][3,0]<2>(500002,16777216,1100001)()")]
        [TestCase("[211][2,0]<2>(1,0)()")]
        [TestCase("[211][2,0]<2>(1100001,5)()")]
        [TestCase("[211][2,0]<2>(2000003,5)()")]
        [TestCase("[211][2,0]<2>(500001,0)()")]
        [TestCase("[212][0,1]<3>()(\"ラベル名\")")]
        [TestCase("[213][0,1]<3>()(\"ラベル名\\s[1]\")")]
        [TestCase("[220][2,0]<0>(0,5)()")]
        [TestCase("[220][2,0]<0>(1,9)()")]
        [TestCase("[221][4,0]<0>(1100005,7,1100000,0)()")]
        [TestCase("[221][4,0]<0>(2500000,1,2000900,1)()")]
        [TestCase("[222][4,0]<0>(1100000,1,2000000,0)()")]
        [TestCase("[222][4,0]<0>(31,5,1024000000,1)()")]
        [TestCase("[230][0,0]<1>()()")]
        [TestCase("[231][0,0]<1>()()")]
        [TestCase("[240][2,0]<0>(-1000000,0)()")]
        [TestCase("[240][2,0]<0>(-12,538)()")]
        [TestCase("[240][2,0]<0>(13,199)()")]
        [TestCase("[240][2,0]<0>(8,644)()")]
        [TestCase("[241][1,0]<0>(1100004)()")]
        [TestCase("[241][1,0]<0>(2)()")]
        [TestCase("[242][6,0]<0>(1,4,6,20,21,-13)()")]
        [TestCase("[242][6,0]<0>(1100000,1100001,1100002,1100004,1100005,1100003)()")]
        [TestCase("[250][5,4]<0>(1,0,-1,4096,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(1,1,-3,135424,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(1,-2,0,65648,0)(\"\",\"┣ 技能習得Lv\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(1,-3,2,4096,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(1,-3,-3,4096,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(1,3,6,4096,1100000)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(1100002,5,2,96,1100000)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(2,0,-3,4096,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(2,1,0,332032,1100001)(\"\",\"BGSリスト\",\"\",\"ファイル名\")")]
        [TestCase("[250][5,4]<0>(2,-1,0,4608,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(2,-3,0,266240,1100001)(\"\",\"\",\"\",\"\\udb[8:0]の残り歩数\")")]
        [TestCase("[250][5,4]<0>(3,0,1,131120,4)(\"\",\"\",\"メイン設定\",\"\")")]
        [TestCase("[250][5,4]<0>(3,0,-3,200704,1100001)(\"\",\"パーティー情報\",\"メイン設定\",\"\")")]
        [TestCase("[250][5,4]<0>(3,1100006,2,64,1100001)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(3,2,0,266241,1100001)(\"\",\"\",\"\",\"所持金\")")]
        [TestCase("[250][5,4]<0>(3,2000002,2,332288,1100001)(\"\",\"UDB3\",\"\",\"項目\")")]
        [TestCase("[250][5,4]<0>(3,-3,-3,69632,1100001)(\"\",\"パーティー情報\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(3,4,1,262160,220)(\"\",\"\",\"\",\"メンバー1\")")]
        [TestCase("[250][5,4]<0>(3,4,2000000,33,220)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(3,4,5,0,220)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(3,5,-2,112,0)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(3,5,2,112,2700000)(\"\",\"\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(3,5,2,65617,1100000)(\"\",\"パーティー情報\",\"\",\"\")")]
        [TestCase("[250][5,4]<0>(6,-1,0,69632,1100001)(\"\",\"┗所持防具リスト\",\"\",\"\")")]
        [TestCase("[260][2,2]<1>(0,3000003)(\"http://DownloadURL.jp\",\"Data/save.dat\")")]
        [TestCase("[260][2,2]<1>(3,0)(\"http://DownloadURL.jp\",\"\")")]
        [TestCase("[270][1,0]<0>(100)()")]
        [TestCase("[270][1,0]<0>(116)()")]
        [TestCase("[270][1,0]<0>(132)()")]
        [TestCase("[270][1,0]<0>(148)()")]
        [TestCase("[270][1,0]<0>(164)()")]
        [TestCase("[270][1,0]<0>(20)()")]
        [TestCase("[270][1,0]<0>(36)()")]
        [TestCase("[270][1,0]<0>(4)()")]
        [TestCase("[270][1,0]<0>(52)()")]
        [TestCase("[270][1,0]<0>(68)()")]
        [TestCase("[270][1,0]<0>(84)()")]
        [TestCase("[270][1,1]<0>(3)(\"CharaChip/[Special]Edy.png\")")]
        [TestCase("[270][2,0]<0>(0,4)()")]
        [TestCase("[270][2,1]<0>(1,2)(\"CharaChip/[Special]Edy.png\")")]
        [TestCase("[270][2,1]<0>(2,2)(\"CharaChip/[Special]Edy.png\")")]
        [TestCase("[270][3,0]<0>(257,4,1000019)()")]
        [TestCase("[270][3,0]<0>(258,2,1000016)()")]
        [TestCase("[270][3,0]<0>(259,0,1000016)()")]
        [TestCase("[281][3,0]<0>(0,2,3)()")]
        [TestCase("[281][3,0]<0>(273,2,3)()")]
        [TestCase("[281][3,0]<0>(274,2,3)()")]
        [TestCase("[281][3,0]<0>(275,2,3)()")]
        [TestCase("[281][3,0]<0>(864,5,1)()")]
        [TestCase("[290][7,0]<0>(0,10,10,10,40,50,60)()")]
        [TestCase("[290][7,0]<0>(112,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(128,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(144,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(16,10,10,20,40,50,60)()")]
        [TestCase("[290][7,0]<0>(160,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(32,1100003,2000000,2000001,1100000,1100001,1100002)()")]
        [TestCase("[290][7,0]<0>(48,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(64,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(80,40,3,3,10,20,30)()")]
        [TestCase("[290][7,0]<0>(96,40,3,3,10,20,30)()")]
        [TestCase("[300][10,1]<2>(0,68,0,1,0,3,3000000,3000001,3000002,3000003)(\"コモンイベント001\")")]
        [TestCase("[300][9,3]<2>(0,16781348,0,2,4,3,0,3000001,1100002)(\"コモンイベント004\",\"aaaaa\",\"\")")]
        [TestCase("[401][1,0]<0>(2)()")]
        [TestCase("[498][0,0]<0>()()")]
        [TestCase("[498][0,0]<1>()()")]
        [TestCase("[498][0,0]<2>()()")]
        [TestCase("[499][0,0]<0>()()")]
        /* ----------以下標準ではないがウディタ基本システムで登場するコマンド */
        [TestCase(
            "[250][5,0]<0>(1,1600000,1600015,0,1600001)()")] // CEv58 L43  | "|■可変DB書込：DB[ 1 : CSelf0[主人公ID] : CSelf15[空き欄] ]  (┣ 技能習得Lv : - : -) =  CSelf1[技能番号]"
        [TestCase(
            "[250][4,4]<0>(21,90,1,65538)(\"\",\"基本ｼｽﾃﾑ用変数\",\"\",\"\")")] //CEv74 L25  "|■可変DB書込：DB[ 基本ｼｽﾃﾑ用変数 : 90 : 1 ]  (18 : [Lvup]習得技能[文字列] : 文字列) =  "
        /* ----------以下標準ではない形式 */
        [TestCase("[9999][2,0]<2>(500001,0)()")] // 仕様外イベントコマンドコード
        [TestCase("[101][0,3]<0>()(\"文章の表示\",\"テスト\",\"追加文字列\")")] // 本来は文字列引数1個
        [TestCase("[0][126,127]<0>(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                  "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                  "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                  "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)(\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\")")] // 与え得る最大の引数
        [TestCase("[0][2,0]<0>(10,255)()")] // 本来は数値引数0個
        [TestCase("[0][0,3]<0>()(\"文章の表示\",\"テスト\",\"追加文字列\")")] // 本来は文字列引数0個
        [TestCase("[0][0,0]<0>()() #コメント")] // 末尾に余計な文字が付与されている（ウディタ仕様ではこの余計な文字は復元されないが、WodiLibでは復元対象とする）
        [TestCase(
            "[102][1,12]<0>(12)(\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\",\"11\",\"12\")")] // 「選択肢」最大数
        [TestCase(
            "[111][46,0]<0>(15,1100000,0,2,1100001,1,2,1100002,2,3,1100003,3,6,1100000,4,2,1100000,5,2,1100000,6,2," +
            "1100000,7,2,1100000,8,2,1100000,9,2,1100000,10,2,1100000,11,2,1100000,12,2,1100000,13,2,1100000,14,2)()")] // 分岐条件（数値）・分岐数15
        [TestCase(
            "[112][30,15]<0>(15,3000000,19777216,3000000,19777216,19777216,19777216,19777216,3000001,19777216,3000000,3000000," +
            "3000000,19777216,19777216,3000000,0,3000000,0,3000002,3000003,3000000,3000007,0,3000009,0,0,0,3000013,3000014)" +
            "(\"1\",\"\",\"3\",\"\",\"\",\"\",\"\",\"8\",\"\",\"10\",\"11\",\"12\",\"\",\"\",\"15\")")] // 分岐条件（文字列）・分岐数15
        [TestCase("[210][2,0]<-1>(1,0)()")] // インデントが負数（通常使用でもインデント128以上にすると負数になる）
        [TestCase("[401][1,0]<0>(15)()")] // 分岐始端・最大値
        public static void CreateCommandStringTest(string src)
        {
            var instance = Factory.CreateCommandString(src);

            const string regex = @"^\[(.*)\]\[(.*)\]<(.*)>\((.*)\)\((.*)\)(.*)$";
            const string splitter = "__";
            var replaceDst = $"$1{splitter}$2{splitter}$3{splitter}$4{splitter}$5{splitter}$6";
            var replaced = Regex.Replace(src, regex, replaceDst);
            var split = Regex.Split(replaced, splitter);

            var commandCode = int.Parse(split[0]);
            if (instance.EventCommandCode != EventCommandCode.Unknown)
            {
                // 仕様外のコマンドはこの方法で本来のコードを取得することは出来ない
                Assert.AreEqual(instance.EventCommandCode.Code, commandCode);
            }

            // 仕様外のコマンドでも正しいコマンドを取得できる。
            Assert.AreEqual(instance.RawEventCommandCode, commandCode);

            var argLengths = split[1].Split(',');
            var numArgLength = int.Parse(argLengths[0].Trim());
            var strArgLength = int.Parse(argLengths[1].Trim());
            Assert.AreEqual(instance.AllNumberArgList.Count, numArgLength + 1);
            Assert.AreEqual(instance.AllStringArgList.Count, strArgLength);

            var indent = (sbyte) int.Parse(split[2]);
            Assert.AreEqual(instance.Indent.ToSbyte(), indent);

            var numArgs = split[3].IsEmpty() ? new string[] { } : split[3].Split(',');
            var instanceAllNumberList = instance.AllNumberArgList;
            // 与えた引数の数が足りない場合でもエラーにしないため、このテスト結果がfalseになる可能性がある
            // Assert.AreEqual(instanceAllNumberList.Count - 1, numArgs.Length);
            for (var i = 1; i < numArgs.Length; i++)
            {
                Assert.AreEqual(instanceAllNumberList[i + 1], int.Parse(numArgs[i].Trim()));
            }

            var strArgs = split[4].IsEmpty()
                ? new String[] { }
                : split[4].Split(',').Select(x => Regex.Replace(x.Trim(), "^\"(.*)\"$", "$1")).ToArray();
            var instanceAllStringArgList = instance.AllStringArgList;
            // 与えた引数の数が足りない場合でもエラーにしないため、このテスト結果がfalseになる可能性がある
            // Assert.AreEqual(instanceAllStringArgList.Count, strArgs.Length);
            for (var i = 0; i < strArgs.Length; i++)
            {
                Assert.AreEqual(instanceAllStringArgList[i], strArgs[i]);
            }

            Assert.AreEqual(instance.ExpansionString, split[5]);

            Assert.AreEqual(instance.ToEventCodeString(), src);
        }

        /// <summary>
        /// 文字列からインスタンス生成テスト（エラーパターン）
        /// </summary>
        /// <param name="src"></param>
        [TestCase(null)]
        [TestCase("[210][2,0]<128>(1,0)()")] // インデントが許容値を超える
        [TestCase("[0][127,0]<0>()(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                  "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                  "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0," +
                  "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)")] //数値引数の数が許容量を超える
        [TestCase("[0][0,128]<0>()(\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"," +
                  "\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\", \"\")")] // 文字列引数の数が許容量を超える
        [TestCase("[101][0,0]<0>()()")] // "文章の表示"コマンドは 本来文字列引数が1つ以上必須。
        public static void CreateCommandStringErrorTest(string src)
        {
            var errorOccured = false;
            try
            {
                var _ = Factory.CreateCommandString(src);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.IsTrue(errorOccured);
        }

        /// <summary>
        /// 文字列からインスタンス生成テスト（正常終了するがコマンドコードが一致しないパターン）
        /// <para>- コード途中の空白はトリムして読み込まれるが復元できない。</para>
        /// <para>- 引数の指定数と実際の数が異なる場合は自動で補正するため元のコードとは違う結果になる。</para>
        /// <para>- 「動作指定」コマンドは復元できない。（ウディタ仕様では数値引数も復元しないが、WodiLib仕様では数値引数を復元する）</para>
        /// </summary>
        [TestCase("[101][0, 1]<0>()(\"文章の表示\")")]
        [TestCase("[290][7,0]<0>(0,10,10, 10,40, 50,60)()")]
        [TestCase("[102][1,5]<0>(869)(\"選択肢A\", \"選択肢B\", \"選択肢C\",\"選択肢D\", \"選択肢E（キャンセル分岐）\")")]
        [TestCase("[0][126,127]<3>(10, 255)()")]
        [TestCase("[0][1,3]<3>(10, 255)(\"\", \"\", \"\", \"\", \"\")")]
        [TestCase("[101][0,1]<0>()(\"文章の表示コマンド\",)")] // 引数末尾に余分なカンマが付いていても「余計な引数が与えられた」と解釈されるべき
        // [TestCase("[101][0,1]<0>()(\"文章の表示コマンド\",  \")")] // 引数末尾に余計な " がついていても「余計な引数が与えられた」と解釈されるべき
        //     （NUnitが \") を文字列に含むテストケースを正しく解釈しないためテスト不可能）
        [TestCase("[103][1,1]<0>()(\"【動作指定】※動作指定はクリップボードにセットできません\")")] // 「動作指定」コマンドは復元できない
        public static void CreateCommandStringNotRecoverableTest(string src)
        {
            var instance = Factory.CreateCommandString(src);

            logger.Info(instance.ToEventCodeString());

            Assert.AreNotEqual(instance.ToEventCodeString(), src);
        }
    }
}