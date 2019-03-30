using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// イベントコマンドコード
    /// </summary>
    public class EventCommandCode : TypeSafeEnum<EventCommandCode>
    {
        ///<summary>空白行</summary>
        public static readonly EventCommandCode Blank;

        ///<summary>文章の表示</summary>
        public static readonly EventCommandCode Message;

        ///<summary>コメント文</summary>
        public static readonly EventCommandCode Comment;

        ///<summary>デバッグ文</summary>
        public static readonly EventCommandCode DebugText;

        ///<summary>デバッグ文全消去</summary>
        public static readonly EventCommandCode ClearDebugText;

        ///<summary>文章強制中断</summary>
        public static readonly EventCommandCode StopForceMessage;

        ///<summary>選択肢・開始</summary>
        public static readonly EventCommandCode ChoiceStart;

        ///<summary>選択肢始端</summary>
        public static readonly EventCommandCode ChoiceStartForkingNumber;

        ///<summary>その他分岐始端</summary>
        public static readonly EventCommandCode ChoiceStartForkingEtc;

        ///<summary>キャンセル始端</summary>
        public static readonly EventCommandCode ChoiceStartForkingCancel;

        ///<summary>分岐終端</summary>
        public static readonly EventCommandCode ForkEnd;

        ///<summary>選択肢の強制中断</summary>
        public static readonly EventCommandCode BreakChoiceForce;

        ///<summary>変数操作</summary>
        public static readonly EventCommandCode SetVariable;

        ///<summary>DB操作</summary>
        public static readonly EventCommandCode DBManagement;

        ///<summary>CSV入出力</summary>
        public static readonly EventCommandCode CsvIO;

        ///<summary>文字列操作</summary>
        public static readonly EventCommandCode SetString;

        ///<summary>変数操作+</summary>
        public static readonly EventCommandCode SetVariablePlus;

        ///<summary>条件（変数）・始端</summary>
        public static readonly EventCommandCode ConditionNumberStart;

        ///<summary>条件（変数）・分岐始端</summary>
        public static readonly EventCommandCode ConditionNumberStartForking;

        ///<summary>条件・上記以外</summary>
        public static readonly EventCommandCode ConditionElse;

        ///<summary>条件（文字列）・始端</summary>
        public static readonly EventCommandCode ConditionStringStart;

        ///<summary>条件（文字列）・分岐始端</summary>
        public static readonly EventCommandCode ConditionStringStartForking;

        ///<summary>キー入力</summary>
        public static readonly EventCommandCode KeyInput;

        ///<summary>自動キー入力</summary>
        public static readonly EventCommandCode KeyInputAuto;

        ///<summary>キー入力禁止（基本入力）</summary>
        public static readonly EventCommandCode StandardKeyInputControl;

        ///<summary>キー入力禁止（デバイス入力）</summary>
        public static readonly EventCommandCode DeviceInputControl;

        ///<summary>ピクチャ</summary>
        public static readonly EventCommandCode Picture;

        ///<summary>エフェクト</summary>
        public static readonly EventCommandCode Effect;

        ///<summary>マップエフェクト（シェイク）</summary>
        public static readonly EventCommandCode MapEffectShake;

        ///<summary>画面スクロール</summary>
        public static readonly EventCommandCode ScrollScreen;

        ///<summary>色調変更</summary>
        public static readonly EventCommandCode ChangeScreenColor;

        ///<summary>サウンド</summary>
        public static readonly EventCommandCode Sound;

        ///<summary>セーブ</summary>
        public static readonly EventCommandCode Save;

        ///<summary>ロード</summary>
        public static readonly EventCommandCode Load;

        ///<summary>内容読込</summary>
        public static readonly EventCommandCode LoadVariable;

        ///<summary>内容書込み</summary>
        public static readonly EventCommandCode SaveVariable;

        ///<summary>パーティ画像</summary>
        public static readonly EventCommandCode PartyGraphic;

        ///<summary>マップチップ通行設定</summary>
        public static readonly EventCommandCode ChangeMapChipSetting;

        ///<summary>チップセット切り替え</summary>
        public static readonly EventCommandCode SwitchChipSet;

        ///<summary>マップチップ上書き</summary>
        public static readonly EventCommandCode OverwriteMapChips;

        ///<summary>場所移動</summary>
        public static readonly EventCommandCode Transfer;

        ///<summary>合成音声</summary>
        public static readonly EventCommandCode SyntheticVoice;

        ///<summary>無限ループ始端</summary>
        public static readonly EventCommandCode LoopInfiniteStart;

        ///<summary>回数指定ループ始端</summary>
        public static readonly EventCommandCode LoopFiniteStart;

        ///<summary>ループ終端</summary>
        public static readonly EventCommandCode LoopEnd;

        ///<summary>ループ中断</summary>
        public static readonly EventCommandCode LoopBreak;

        ///<summary>ループ開始へ戻る</summary>
        public static readonly EventCommandCode LoopContinue;

        ///<summary>トランジション準備</summary>
        public static readonly EventCommandCode PreparationTransition;

        ///<summary>トランジション実行</summary>
        public static readonly EventCommandCode ExecutionTransition;

        ///<summary>オプション指定トランジション</summary>
        public static readonly EventCommandCode TransitionWithOption;

        ///<summary>イベント処理中のEv移動ON</summary>
        public static readonly EventCommandCode MoveDuringEventsOn;

        ///<summary>イベント処理中のEv移動OFF</summary>
        public static readonly EventCommandCode MoveDuringEventsOff;

        ///<summary>タイトル画面へ戻る</summary>
        public static readonly EventCommandCode GoToTitleScreen;

        ///<summary>ゲーム終了</summary>
        public static readonly EventCommandCode GameEnd;

        ///<summary>ピクチャ以外の画像更新[停止]</summary>
        public static readonly EventCommandCode HaltNonPictureUpdate;

        ///<summary>ピクチャ以外の画像更新[再開]</summary>
        public static readonly EventCommandCode ResumeNonPictureUpdate;

        ///<summary>イベント処理中断</summary>
        public static readonly EventCommandCode ForceExitEvent;

        ///<summary>動作指定</summary>
        public static readonly EventCommandCode MoveRoute;

        ///<summary>動作完了までウェイト</summary>
        public static readonly EventCommandCode WaitForMovement;

        ///<summary>イベント一時消去</summary>
        public static readonly EventCommandCode EraseEvent;

        ///<summary>ウェイト</summary>
        public static readonly EventCommandCode Wait;

        ///<summary>ラベル設置</summary>
        public static readonly EventCommandCode SetLabel;

        ///<summary>指定ラベルに飛ぶ</summary>
        public static readonly EventCommandCode JumpToLabel;

        ///<summary>コモンイベント（ID指定）</summary>
        public static readonly EventCommandCode CallCommonEventById;

        ///<summary>コモンイベント（イベント名）</summary>
        public static readonly EventCommandCode CallCommonEventByName;

        ///<summary>コモンイベント予約</summary>
        public static readonly EventCommandCode CommonEventReserve;

        ///<summary>ダウンロード</summary>
        public static readonly EventCommandCode Download;

        ///<summary>チェックポイント</summary>
        public static readonly EventCommandCode CheckPoint;

        /// <summary>値</summary>
        public int Code { get; }

        static EventCommandCode()
        {
            Blank = new EventCommandCode(nameof(Blank), 0);
            Message = new EventCommandCode(nameof(Message), 101);
            Comment = new EventCommandCode(nameof(Comment), 103);
            DebugText = new EventCommandCode(nameof(DebugText), 106);
            ClearDebugText = new EventCommandCode(nameof(ClearDebugText), 107);
            StopForceMessage = new EventCommandCode(nameof(StopForceMessage), 105);
            ChoiceStart = new EventCommandCode(nameof(ChoiceStart), 102);
            ChoiceStartForkingNumber = new EventCommandCode(nameof(ChoiceStartForkingNumber), 401);
            ChoiceStartForkingEtc = new EventCommandCode(nameof(ChoiceStartForkingEtc), 402);
            ChoiceStartForkingCancel = new EventCommandCode(nameof(ChoiceStartForkingCancel), 421);
            ForkEnd = new EventCommandCode(nameof(ForkEnd), 499);
            BreakChoiceForce = new EventCommandCode(nameof(BreakChoiceForce), 104);
            SetVariable = new EventCommandCode(nameof(SetVariable), 121);
            DBManagement = new EventCommandCode(nameof(DBManagement), 250);
            CsvIO = new EventCommandCode(nameof(CsvIO), 251);
            SetString = new EventCommandCode(nameof(SetString), 122);
            SetVariablePlus = new EventCommandCode(nameof(SetVariablePlus), 124);
            ConditionNumberStart = new EventCommandCode(nameof(ConditionNumberStart), 111);
            ConditionNumberStartForking = new EventCommandCode(nameof(ConditionNumberStartForking), 401);
            ConditionElse = new EventCommandCode(nameof(ConditionElse), 420);
            ConditionStringStart = new EventCommandCode(nameof(ConditionStringStart), 112);
            ConditionStringStartForking = new EventCommandCode(nameof(ConditionStringStartForking), 401);
            KeyInput = new EventCommandCode(nameof(KeyInput), 123);
            KeyInputAuto = new EventCommandCode(nameof(KeyInputAuto), 125);
            StandardKeyInputControl = new EventCommandCode(nameof(StandardKeyInputControl), 126);
            DeviceInputControl = new EventCommandCode(nameof(DeviceInputControl), 126);
            Picture = new EventCommandCode(nameof(Picture), 150);
            Effect = new EventCommandCode(nameof(Effect), 290);
            MapEffectShake = new EventCommandCode(nameof(MapEffectShake), 280);
            ScrollScreen = new EventCommandCode(nameof(ScrollScreen), 281);
            ChangeScreenColor = new EventCommandCode(nameof(ChangeScreenColor), 151);
            Sound = new EventCommandCode(nameof(Sound), 140);
            Save = new EventCommandCode(nameof(Save), 220);
            Load = new EventCommandCode(nameof(Load), 220);
            LoadVariable = new EventCommandCode(nameof(LoadVariable), 221);
            SaveVariable = new EventCommandCode(nameof(SaveVariable), 222);
            PartyGraphic = new EventCommandCode(nameof(PartyGraphic), 270);
            ChangeMapChipSetting = new EventCommandCode(nameof(ChangeMapChipSetting), 240);
            SwitchChipSet = new EventCommandCode(nameof(SwitchChipSet), 241);
            OverwriteMapChips = new EventCommandCode(nameof(OverwriteMapChips), 242);
            Transfer = new EventCommandCode(nameof(Transfer), 130);
            SyntheticVoice = new EventCommandCode(nameof(SyntheticVoice), 141);
            LoopInfiniteStart = new EventCommandCode(nameof(LoopInfiniteStart), 170);
            LoopFiniteStart = new EventCommandCode(nameof(LoopFiniteStart), 179);
            LoopEnd = new EventCommandCode(nameof(LoopEnd), 498);
            LoopBreak = new EventCommandCode(nameof(LoopBreak), 171);
            LoopContinue = new EventCommandCode(nameof(LoopContinue), 176);
            PreparationTransition = new EventCommandCode(nameof(PreparationTransition), 161);
            ExecutionTransition = new EventCommandCode(nameof(ExecutionTransition), 162);
            TransitionWithOption = new EventCommandCode(nameof(TransitionWithOption), 160);
            MoveDuringEventsOn = new EventCommandCode(nameof(MoveDuringEventsOn), 230);
            MoveDuringEventsOff = new EventCommandCode(nameof(MoveDuringEventsOff), 231);
            GoToTitleScreen = new EventCommandCode(nameof(GoToTitleScreen), 174);
            GameEnd = new EventCommandCode(nameof(GameEnd), 175);
            HaltNonPictureUpdate = new EventCommandCode(nameof(HaltNonPictureUpdate), 177);
            ResumeNonPictureUpdate = new EventCommandCode(nameof(ResumeNonPictureUpdate), 178);
            ForceExitEvent = new EventCommandCode(nameof(ForceExitEvent), 172);
            MoveRoute = new EventCommandCode(nameof(MoveRoute), 201);
            WaitForMovement = new EventCommandCode(nameof(WaitForMovement), 202);
            EraseEvent = new EventCommandCode(nameof(EraseEvent), 173);
            Wait = new EventCommandCode(nameof(Wait), 180);
            SetLabel = new EventCommandCode(nameof(SetLabel), 212);
            JumpToLabel = new EventCommandCode(nameof(JumpToLabel), 213);
            CallCommonEventById = new EventCommandCode(nameof(CallCommonEventById), 210);
            CallCommonEventByName = new EventCommandCode(nameof(CallCommonEventByName), 300);
            CommonEventReserve = new EventCommandCode(nameof(CommonEventReserve), 211);
            Download = new EventCommandCode(nameof(Download), 260);
            CheckPoint = new EventCommandCode(nameof(CheckPoint), 99);
        }

        private EventCommandCode(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static EventCommandCode FromCode(int code)
        {
            return _FindFirst(x => x.Code == code);
        }

    }
}