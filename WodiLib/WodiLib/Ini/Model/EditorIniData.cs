// ========================================
// Project Name : WodiLib
// File Name    : EditorIniData.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// Editor.iniデータクラス
    /// </summary>
    [Serializable]
    public class EditorIniData : ModelBase<EditorIniData>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private StartFlag startFlag = 0;

        /// <summary>
        /// 【Ver2.20以降】開始フラグ
        /// </summary>
        public StartFlag StartFlag
        {
            get => startFlag;
            set
            {
                startFlag = value;
                NotifyPropertyChanged();
            }
        }

        private LastLoadMapFilePath lastLoadFile = "";

        /// <summary>
        /// 最終読み込みマップファイルパス
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public LastLoadMapFilePath LastLoadFile
        {
            get => lastLoadFile;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(LastLoadFile)));

                lastLoadFile = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition mainWindowPosition = new WindowPosition(500, 120);

        /// <summary>
        /// メインウィンドウ左上座標
        /// </summary>
        public WindowPosition MainWindowPosition
        {
            get => mainWindowPosition;
            set
            {
                mainWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowSize mainWindowSize = new WindowSize(0, 0);

        /// <summary>
        /// メインウィンドウサイズ
        /// </summary>
        public WindowSize MainWindowSize
        {
            get => mainWindowSize;
            set
            {
                mainWindowSize = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition mapChipWindowPosition = new WindowPosition(0, 0);

        /// <summary>
        /// マップチップウィンドウ左上座標
        /// </summary>
        public WindowPosition MapChipWindowPosition
        {
            get => mapChipWindowPosition;
            set
            {
                mapChipWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition mapEventWindowPosition = new WindowPosition(0, 0);

        /// <summary>
        /// マップイベントウィンドウ左上座標
        /// </summary>
        public WindowPosition MapEventWindowPosition
        {
            get => mapEventWindowPosition;
            set
            {
                mapEventWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowSize mapEventWindowSize = new WindowSize(840, 410);

        /// <summary>
        /// 【Ver2.10以降】マップイベントウィンドウサイズ
        /// </summary>
        public WindowSize MapEventWindowSize
        {
            get => mapEventWindowSize;
            set
            {
                mapEventWindowSize = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition mapEventInputWindowPosition = new WindowPosition(0, 0);

        /// <summary>
        /// マップイベントコマンド入力ウィンドウ左上座標
        /// </summary>
        public WindowPosition MapEventInputWindowPosition
        {
            get => mapEventInputWindowPosition;
            set
            {
                mapEventInputWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition commonEventWindowPosition = new WindowPosition(0, 0);

        /// <summary>
        /// コモンイベントウィンドウ左上座標
        /// </summary>
        public WindowPosition CommonEventWindowPosition
        {
            get => commonEventWindowPosition;
            set
            {
                commonEventWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowSize commonEventWindowSize = new WindowSize(0, 0);

        /// <summary>
        /// 【Ver2.10以降】コモンイベントウィンドウサイズ
        /// </summary>
        public WindowSize CommonEventWindowSize
        {
            get => commonEventWindowSize;
            set
            {
                commonEventWindowSize = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition commonEventInputWindowPosition = new WindowPosition(0, 0);

        /// <summary>
        /// コモンイベントコマンド入力ウィンドウ左上座標
        /// </summary>
        public WindowPosition CommonEventInputWindowPosition
        {
            get => commonEventInputWindowPosition;
            set
            {
                commonEventInputWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition userDbWindowPosition = new WindowPosition(3, 6);

        /// <summary>
        /// ユーザDBウィンドウ左上座標
        /// </summary>
        public WindowPosition UserDbWindowPosition
        {
            get => userDbWindowPosition;
            set
            {
                userDbWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition changeableDbWindowPosition = new WindowPosition(3, 6);

        /// <summary>
        /// 可変DBウィンドウ左上座標
        /// </summary>
        public WindowPosition ChangeableDbWindowPosition
        {
            get => changeableDbWindowPosition;
            set
            {
                changeableDbWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private WindowPosition systemDbWindowPosition = new WindowPosition(3, 6);

        /// <summary>
        /// システムDBウィンドウ左上座標
        /// </summary>
        public WindowPosition SystemDbWindowPosition
        {
            get => systemDbWindowPosition;
            set
            {
                systemDbWindowPosition = value;
                NotifyPropertyChanged();
            }
        }

        private DatabaseValueNumberDrawType databaseValueNumberDrawType = DatabaseValueNumberDrawType.Off;

        /// <summary>
        /// [NotNull] DBウィンドウ項目番号表示種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DatabaseValueNumberDrawType DatabaseValueNumberDrawType
        {
            get => databaseValueNumberDrawType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DatabaseValueNumberDrawType)));

                databaseValueNumberDrawType = value;
                NotifyPropertyChanged();
            }
        }

        private EditTimeDrawType editTimeDrawType = EditTimeDrawType.On;

        /// <summary>
        /// [NotNull]【Ver2.00以降】ウディタ作業時間表示種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EditTimeDrawType EditTimeDrawType
        {
            get => editTimeDrawType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EditTimeDrawType)));

                editTimeDrawType = value;
                NotifyPropertyChanged();
            }
        }

        private WorkTime editTime = 0;

        /// <summary>
        /// ウディタ作業時間（アクティブ時間）[1/2minute]
        /// </summary>
        public WorkTime EditTime
        {
            get => editTime;
            set
            {
                editTime = value;
                NotifyPropertyChanged();
            }
        }

        private WorkTime notEditTime = 0;

        /// <summary>
        /// 【Ver2.00以降】ウディタ作業時間（非アクティブ時間）[1/2minute]
        /// </summary>
        public WorkTime NotEditTime
        {
            get => notEditTime;
            set
            {
                notEditTime = value;
                NotifyPropertyChanged();
            }
        }

        private bool isShowDebugWindow = true;

        /// <summary>
        /// 【Ver2.00以降】デバッグウィンドウ使用フラグ
        /// </summary>
        public bool IsShowDebugWindow
        {
            get => isShowDebugWindow;
            set
            {
                isShowDebugWindow = value;
                NotifyPropertyChanged();
            }
        }

        private LaterTransparentType layerTransparent = LaterTransparentType.SomewhatDark;

        /// <summary>
        /// [NotNull] Ver2.00以降】マップ編集・下レイヤーの暗さ
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public LaterTransparentType LayerTransparent
        {
            get => layerTransparent;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(LayerTransparent)));

                layerTransparent = value;
                NotifyPropertyChanged();
            }
        }

        private EventLayerOpacityType eventLayerOpacity = EventLayerOpacityType.Quoter;

        /// <summary>
        /// [NotNull]【Ver2.00以降】マップ編集・Evレイヤー不透明度
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventLayerOpacityType EventLayerOpacity
        {
            get => eventLayerOpacity;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventLayerOpacity)));

                eventLayerOpacity = value;
                NotifyPropertyChanged();
            }
        }

        private CommandColorType commandColorType = CommandColorType.Type1;

        /// <summary>
        /// [NotNull]【Ver2.00以降】イベントコマンド配色
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommandColorType CommandColorType
        {
            get => commandColorType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CommandColorType)));

                commandColorType = value;
                NotifyPropertyChanged();
            }
        }

        private bool isDrawBackgroundImage = true;

        /// <summary>
        /// 【Ver2.00以降】マップ編集・遠景表示有無
        /// </summary>
        public bool IsDrawBackgroundImage
        {
            get => isDrawBackgroundImage;
            set
            {
                isDrawBackgroundImage = value;
                NotifyPropertyChanged();
            }
        }

        private ExtensionList notCopyExtList = new ExtensionList(new Extension[]
        {
            ".psd", ".sai", ".svg", ".xls", ".db", ".tmp",
            ".bak", ".db", "dummy_file"
        });

        /// <summary>
        /// [NotNull]【Ver2.00以降】ゲームデータ作成時にコピーしないファイル拡張子
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ExtensionList NotCopyExtList
        {
            get => notCopyExtList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(NotCopyExtList)));

                notCopyExtList = value;
                NotifyPropertyChanged();
            }
        }


        private CommandViewType commandViewType = 0;

        /// <summary>
        /// 【Ver2.20以降】？
        /// </summary>
        public CommandViewType CommandViewType
        {
            get => commandViewType;
            set
            {
                commandViewType = value;
                NotifyPropertyChanged();
            }
        }

        private ProjectBackupType backupType = ProjectBackupType.ThreeTimes;

        /// <summary>
        /// [NotNull]【Ver2.20以降】Datファイル自動バックアップ
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ProjectBackupType BackupType
        {
            get => backupType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(BackupType)));

                backupType = value;
                NotifyPropertyChanged();
            }
        }

        private IFixedLengthEventCommandShortCutKeyList shortCutKeyList = new EventCommandShortCutKeyList();

        /// <summary>
        /// [NotNull]【Ver2.00以降】イベントコマンドウィンドウショートカットキーリスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IFixedLengthEventCommandShortCutKeyList ShortCutKeyList
        {
            get => shortCutKeyList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ShortCutKeyList)));

                shortCutKeyList = value;
                NotifyPropertyChanged();
            }
        }

        private IFixedLengthShortCutPositionList commandPositionList = new ShortCutPositionList();

        /// <summary>
        /// [NotNull]【Ver2.00以降】イベントコマンド表示順リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public IFixedLengthShortCutPositionList CommandPositionList
        {
            get => commandPositionList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CommandPositionList)));

                commandPositionList = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUseExpertCommand;

        /// <summary>
        /// 【Ver2.10以降】上級者向けコマンド使用フラグ
        /// </summary>
        public bool IsUseExpertCommand
        {
            get => isUseExpertCommand;
            set
            {
                isUseExpertCommand = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EditorIniData()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="programData">ProgramData</param>
        internal EditorIniData(EditorIniProgramData programData)
        {
            if (programData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(programData)));

            StartFlag = programData.StartFlag.TryToInt() ?? 0;
            LastLoadFile = programData.LastLoadFile;
            MainWindowPosition = new WindowPosition(
                programData.WindowPosX.TryToInt() ?? 0,
                programData.WindowPosY.TryToInt() ?? 0);
            MainWindowSize = new WindowSize(
                programData.WindowSizeX.TryToInt() ?? 0,
                programData.WindowSizeY.TryToInt() ?? 0);
            MapChipWindowPosition = new WindowPosition(
                programData.WindowMapChipX.TryToInt() ?? 0,
                programData.WindowMapChipY.TryToInt() ?? 0);
            MapEventWindowPosition = new WindowPosition(
                programData.WindowEventX.TryToInt() ?? 0,
                programData.WindowEventY.TryToInt() ?? 0);
            MapEventWindowSize = new WindowSize(
                programData.WindowEventSizeX.TryToInt() ?? 0,
                programData.WindowEventSizeY.TryToInt() ?? 0);
            MapEventInputWindowPosition = new WindowPosition(
                programData.WindowEventInputX.TryToInt() ?? 0,
                programData.WindowEventInputY.TryToInt() ?? 0);
            CommonEventWindowPosition = new WindowPosition(
                programData.WindowCommonX.TryToInt() ?? 0,
                programData.WindowCommonY.TryToInt() ?? 0);
            CommonEventWindowSize = new WindowSize(
                programData.WindowCommonSizeX.TryToInt() ?? 0,
                programData.WindowCommonSizeY.TryToInt() ?? 0);
            CommonEventInputWindowPosition = new WindowPosition(
                programData.WindowCommonInputX.TryToInt() ?? 0,
                programData.WindowCommonInputY.TryToInt() ?? 0);
            UserDbWindowPosition = new WindowPosition(
                programData.WindowUserDBX.TryToInt() ?? 0,
                programData.WindowUserDBY.TryToInt() ?? 0);
            ChangeableDbWindowPosition = new WindowPosition(
                programData.WindowCDBX.TryToInt() ?? 0,
                programData.WindowCDBY.TryToInt() ?? 0);
            SystemDbWindowPosition = new WindowPosition(
                programData.WindowSysDBX.TryToInt() ?? 0,
                programData.WindowSysDBY.TryToInt() ?? 0);
            DatabaseValueNumberDrawType = DatabaseValueNumberDrawType.FromCodeOrDefault(
                programData.DataBaseValueNumDraw);
            EditTimeDrawType = EditTimeDrawType.FromCodeOrDefault(programData.EditTimeUseFlag);
            EditTime = new WorkTime(programData.EditTime.TryToInt() ?? 0);
            NotEditTime = new WorkTime(programData.NotEditTime.TryToInt() ?? 0);
            IsShowDebugWindow = programData.UseDebugWindow.TryToInt() == 1;
            LayerTransparent = LaterTransparentType.FromCodeOrDefault(
                programData.LayerTransparent);
            EventLayerOpacity = EventLayerOpacityType.FromCodeOrDefault(programData.EventLayerDraw);
            CommandColorType = CommandColorType.FromCodeOrDefault(programData.EventCommandColorType);
            IsDrawBackgroundImage = programData.DrawBackGroundFlag.TryToInt() == 1;
            NotCopyExtList = ((Func<ExtensionList>) (() =>
            {
                var extensions = programData.NotCopyExt.Split(',')
                    .Select(x => new Extension(x.Trim())).ToList();

                return new ExtensionList(extensions);
            }))();
            CommandViewType = new CommandViewType(programData.CommandViewType.TryToInt() ?? 0);
            BackupType = ProjectBackupType.FromCodeOrDefault(programData.BackupNum);
            ShortCutKeyList = ((Func<EventCommandShortCutKeyList>) (() =>
            {
                var result = new EventCommandShortCutKeyList(new[]
                {
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut01),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut02),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut03),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut04),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut05),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut06),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut07),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut08),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut09),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut10),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut11),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut12),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut13),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut14),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut15),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut16),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut17),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut18),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut19),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut20),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut21),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut22),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut23),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut24),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut25),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut26),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut27),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut28),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut29),
                    EventCommandShortCutKey.FromCodeOrDefault(programData.ShortCut30),
                });

                return result;
            }))();
            CommandPositionList = ((Func<ShortCutPositionList>) (() =>
            {
                var result = new ShortCutPositionList(new List<ShortCutPosition>
                {
                    programData.ShortCut01Pos.TryToInt() ?? -1,
                    programData.ShortCut02Pos.TryToInt() ?? -1,
                    programData.ShortCut03Pos.TryToInt() ?? -1,
                    programData.ShortCut04Pos.TryToInt() ?? -1,
                    programData.ShortCut05Pos.TryToInt() ?? -1,
                    programData.ShortCut06Pos.TryToInt() ?? -1,
                    programData.ShortCut07Pos.TryToInt() ?? -1,
                    programData.ShortCut08Pos.TryToInt() ?? -1,
                    programData.ShortCut09Pos.TryToInt() ?? -1,
                    programData.ShortCut10Pos.TryToInt() ?? -1,
                    programData.ShortCut11Pos.TryToInt() ?? -1,
                    programData.ShortCut12Pos.TryToInt() ?? -1,
                    programData.ShortCut13Pos.TryToInt() ?? -1,
                    programData.ShortCut14Pos.TryToInt() ?? -1,
                    programData.ShortCut15Pos.TryToInt() ?? -1,
                    programData.ShortCut16Pos.TryToInt() ?? -1,
                    programData.ShortCut17Pos.TryToInt() ?? -1,
                    programData.ShortCut18Pos.TryToInt() ?? -1,
                    programData.ShortCut19Pos.TryToInt() ?? -1,
                    programData.ShortCut20Pos.TryToInt() ?? -1,
                    programData.ShortCut21Pos.TryToInt() ?? -1,
                    programData.ShortCut22Pos.TryToInt() ?? -1,
                    programData.ShortCut23Pos.TryToInt() ?? -1,
                    programData.ShortCut24Pos.TryToInt() ?? -1,
                    programData.ShortCut25Pos.TryToInt() ?? -1,
                    programData.ShortCut26Pos.TryToInt() ?? -1,
                    programData.ShortCut27Pos.TryToInt() ?? -1,
                    programData.ShortCut28Pos.TryToInt() ?? -1,
                    programData.ShortCut29Pos.TryToInt() ?? -1,
                    programData.ShortCut30Pos.TryToInt() ?? -1,
                    programData.ShortCut31Pos.TryToInt() ?? -1,
                });

                return result;
            }))();
            IsUseExpertCommand = programData.ExpertCommand.TryToInt() == 1;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 各プロパティが適切な設定であることを検証する。
        /// <remarks>
        /// 以下のプロパティのValidateメソッドを実行する。
        /// <pre>ShortCutKeyList</pre>
        /// <pre>CommandPositionList</pre>
        /// </remarks>
        /// </summary>
        /// <param name="errorMsg">
        ///     返戻エラーメッセージ。
        ///     設定値が適切である場合、null。
        /// </param>
        /// <returns>設定値が適切である場合、true</returns>
        public bool Validate(out string errorMsg)
        {
            if (!ShortCutKeyList.Validate(out errorMsg))
            {
                return false;
            }

            errorMsg = null;
            return true;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(EditorIniData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return lastLoadFile == other.lastLoadFile
                   && databaseValueNumberDrawType == other.databaseValueNumberDrawType
                   && editTimeDrawType == other.editTimeDrawType
                   && layerTransparent == other.layerTransparent
                   && eventLayerOpacity == other.eventLayerOpacity
                   && commandColorType == other.commandColorType
                   && notCopyExtList.Equals(other.notCopyExtList)
                   && backupType == other.backupType
                   && shortCutKeyList.Equals(other.shortCutKeyList)
                   && commandPositionList.Equals(other.commandPositionList)
                   && StartFlag == other.StartFlag
                   && MainWindowPosition == other.MainWindowPosition
                   && MainWindowSize == other.MainWindowSize
                   && MapChipWindowPosition == other.MapChipWindowPosition
                   && MapEventWindowPosition == other.MapEventWindowPosition
                   && MapEventWindowSize == other.MapEventWindowSize
                   && MapEventInputWindowPosition == other.MapEventInputWindowPosition
                   && CommonEventWindowPosition == other.CommonEventWindowPosition
                   && CommonEventWindowSize == other.CommonEventWindowSize
                   && CommonEventInputWindowPosition == other.CommonEventInputWindowPosition
                   && UserDbWindowPosition == other.UserDbWindowPosition
                   && ChangeableDbWindowPosition == other.ChangeableDbWindowPosition
                   && SystemDbWindowPosition == other.SystemDbWindowPosition
                   && EditTime == other.EditTime
                   && NotEditTime == other.NotEditTime
                   && IsShowDebugWindow == other.IsShowDebugWindow
                   && IsDrawBackgroundImage == other.IsDrawBackgroundImage
                   && CommandViewType == other.CommandViewType
                   && IsUseExpertCommand == other.IsUseExpertCommand;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自身のプロパティ値をEditorIniProgramDataインスタンスに変換する。
        /// </summary>
        /// <returns>EditorIniProgramDataインスタンス</returns>
        /// <exception cref="InvalidOperationException">プロパティの設定値が不適切な場合</exception>
        internal EditorIniProgramData ToProgramData()
        {
            if (!Validate(out var errorMsg))
            {
                throw new InvalidOperationException(errorMsg);
            }

            return new EditorIniProgramData
            {
                StartFlag = StartFlag.ToString(),
                LastLoadFile = LastLoadFile.ToString(),
                WindowPosX = MainWindowPosition.X.ToString(),
                WindowPosY = MainWindowPosition.Y.ToString(),
                WindowSizeX = MainWindowSize.X.ToString(),
                WindowSizeY = MainWindowSize.Y.ToString(),
                WindowMapChipX = MapChipWindowPosition.X.ToString(),
                WindowMapChipY = MapChipWindowPosition.Y.ToString(),
                WindowEventX = MapEventWindowPosition.X.ToString(),
                WindowEventY = MapEventWindowPosition.Y.ToString(),
                WindowEventSizeX = MapEventWindowSize.X.ToString(),
                WindowEventSizeY = MapEventWindowSize.Y.ToString(),
                WindowEventInputX = MapEventInputWindowPosition.X.ToString(),
                WindowEventInputY = MapEventInputWindowPosition.Y.ToString(),
                WindowCommonX = CommonEventWindowPosition.X.ToString(),
                WindowCommonY = CommonEventWindowPosition.Y.ToString(),
                WindowCommonSizeX = CommonEventWindowSize.X.ToString(),
                WindowCommonSizeY = CommonEventWindowSize.Y.ToString(),
                WindowCommonInputX = CommonEventInputWindowPosition.X.ToString(),
                WindowCommonInputY = CommonEventInputWindowPosition.Y.ToString(),
                WindowUserDBX = UserDbWindowPosition.X.ToString(),
                WindowUserDBY = UserDbWindowPosition.Y.ToString(),
                WindowCDBX = ChangeableDbWindowPosition.X.ToString(),
                WindowCDBY = ChangeableDbWindowPosition.Y.ToString(),
                WindowSysDBX = SystemDbWindowPosition.X.ToString(),
                WindowSysDBY = SystemDbWindowPosition.Y.ToString(),
                DataBaseValueNumDraw = DatabaseValueNumberDrawType.Code,
                EditTimeUseFlag = EditTimeDrawType.Code,
                EditTime = EditTime.ToString(),
                NotEditTime = NotEditTime.ToString(),
                UseDebugWindow = IsShowDebugWindow.ToIntString(),
                LayerTransparent = LayerTransparent.Code,
                EventLayerDraw = EventLayerOpacity.Code,
                EventCommandColorType = CommandColorType.Code,
                DrawBackGroundFlag = IsDrawBackgroundImage.ToIntString(),
                NotCopyExt = NotCopyExtList.ToStringItems(),
                CommandViewType = CommandViewType.ToString(),
                BackupNum = BackupType.Code,
                ShortCut01 = ShortCutKeyList[0].Code,
                ShortCut02 = ShortCutKeyList[1].Code,
                ShortCut03 = ShortCutKeyList[2].Code,
                ShortCut04 = ShortCutKeyList[3].Code,
                ShortCut05 = ShortCutKeyList[4].Code,
                ShortCut06 = ShortCutKeyList[5].Code,
                ShortCut07 = ShortCutKeyList[6].Code,
                ShortCut08 = ShortCutKeyList[7].Code,
                ShortCut09 = ShortCutKeyList[8].Code,
                ShortCut10 = ShortCutKeyList[9].Code,
                ShortCut11 = ShortCutKeyList[10].Code,
                ShortCut12 = ShortCutKeyList[11].Code,
                ShortCut13 = ShortCutKeyList[12].Code,
                ShortCut14 = ShortCutKeyList[13].Code,
                ShortCut15 = ShortCutKeyList[14].Code,
                ShortCut16 = ShortCutKeyList[15].Code,
                ShortCut17 = ShortCutKeyList[16].Code,
                ShortCut18 = ShortCutKeyList[17].Code,
                ShortCut19 = ShortCutKeyList[18].Code,
                ShortCut20 = ShortCutKeyList[19].Code,
                ShortCut21 = ShortCutKeyList[20].Code,
                ShortCut22 = ShortCutKeyList[21].Code,
                ShortCut23 = ShortCutKeyList[22].Code,
                ShortCut24 = ShortCutKeyList[23].Code,
                ShortCut25 = ShortCutKeyList[24].Code,
                ShortCut26 = ShortCutKeyList[25].Code,
                ShortCut27 = ShortCutKeyList[26].Code,
                ShortCut28 = ShortCutKeyList[27].Code,
                ShortCut29 = ShortCutKeyList[28].Code,
                ShortCut30 = ShortCutKeyList[29].Code,
                ShortCut01Pos = CommandPositionList[0].ToString(),
                ShortCut02Pos = CommandPositionList[1].ToString(),
                ShortCut03Pos = CommandPositionList[2].ToString(),
                ShortCut04Pos = CommandPositionList[3].ToString(),
                ShortCut05Pos = CommandPositionList[4].ToString(),
                ShortCut06Pos = CommandPositionList[5].ToString(),
                ShortCut07Pos = CommandPositionList[6].ToString(),
                ShortCut08Pos = CommandPositionList[7].ToString(),
                ShortCut09Pos = CommandPositionList[8].ToString(),
                ShortCut10Pos = CommandPositionList[9].ToString(),
                ShortCut11Pos = CommandPositionList[10].ToString(),
                ShortCut12Pos = CommandPositionList[11].ToString(),
                ShortCut13Pos = CommandPositionList[12].ToString(),
                ShortCut14Pos = CommandPositionList[13].ToString(),
                ShortCut15Pos = CommandPositionList[14].ToString(),
                ShortCut16Pos = CommandPositionList[15].ToString(),
                ShortCut17Pos = CommandPositionList[16].ToString(),
                ShortCut18Pos = CommandPositionList[17].ToString(),
                ShortCut19Pos = CommandPositionList[18].ToString(),
                ShortCut20Pos = CommandPositionList[19].ToString(),
                ShortCut21Pos = CommandPositionList[20].ToString(),
                ShortCut22Pos = CommandPositionList[21].ToString(),
                ShortCut23Pos = CommandPositionList[22].ToString(),
                ShortCut24Pos = CommandPositionList[23].ToString(),
                ShortCut25Pos = CommandPositionList[24].ToString(),
                ShortCut26Pos = CommandPositionList[25].ToString(),
                ShortCut27Pos = CommandPositionList[26].ToString(),
                ShortCut28Pos = CommandPositionList[27].ToString(),
                ShortCut29Pos = CommandPositionList[28].ToString(),
                ShortCut30Pos = CommandPositionList[29].ToString(),
                ShortCut31Pos = CommandPositionList[30].ToString(),
                ExpertCommand = IsUseExpertCommand.ToIntString(),
            };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(StartFlag), StartFlag);
            info.AddValue(nameof(lastLoadFile), lastLoadFile);
            info.AddValue(nameof(MainWindowPosition), MainWindowPosition);
            info.AddValue(nameof(MainWindowSize), MainWindowSize);
            info.AddValue(nameof(MapChipWindowPosition), MapChipWindowPosition);
            info.AddValue(nameof(MapEventWindowPosition), MapEventWindowPosition);
            info.AddValue(nameof(MapEventWindowSize), MapEventWindowSize);
            info.AddValue(nameof(MapEventInputWindowPosition), MapEventInputWindowPosition);
            info.AddValue(nameof(CommonEventWindowPosition), CommonEventWindowPosition);
            info.AddValue(nameof(CommonEventWindowSize), CommonEventWindowSize);
            info.AddValue(nameof(CommonEventInputWindowPosition), CommonEventInputWindowPosition);
            info.AddValue(nameof(UserDbWindowPosition), UserDbWindowPosition);
            info.AddValue(nameof(ChangeableDbWindowPosition), ChangeableDbWindowPosition);
            info.AddValue(nameof(SystemDbWindowPosition), SystemDbWindowPosition);
            info.AddValue(nameof(databaseValueNumberDrawType), databaseValueNumberDrawType.Code);
            info.AddValue(nameof(editTimeDrawType), editTimeDrawType.Code);
            info.AddValue(nameof(EditTime), EditTime);
            info.AddValue(nameof(NotEditTime), NotEditTime);
            info.AddValue(nameof(IsShowDebugWindow), IsShowDebugWindow);
            info.AddValue(nameof(layerTransparent), layerTransparent.Code);
            info.AddValue(nameof(eventLayerOpacity), eventLayerOpacity.Code);
            info.AddValue(nameof(commandColorType), commandColorType.Code);
            info.AddValue(nameof(IsDrawBackgroundImage), IsDrawBackgroundImage);
            info.AddValue(nameof(notCopyExtList), notCopyExtList);
            info.AddValue(nameof(CommandViewType), CommandViewType);
            info.AddValue(nameof(shortCutKeyList), shortCutKeyList);
            info.AddValue(nameof(backupType), backupType.Code);
            info.AddValue(nameof(commandPositionList), commandPositionList);
            info.AddValue(nameof(IsUseExpertCommand), IsUseExpertCommand);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected EditorIniData(SerializationInfo info, StreamingContext context)
        {
            StartFlag = info.GetInt32(nameof(StartFlag));
            lastLoadFile = info.GetValue<LastLoadMapFilePath>(nameof(lastLoadFile));
            MainWindowPosition = info.GetValue<WindowPosition>(nameof(MainWindowPosition));
            MainWindowSize = info.GetValue<WindowSize>(nameof(MainWindowSize));
            MapChipWindowPosition = info.GetValue<WindowPosition>(nameof(MapChipWindowPosition));
            MapEventWindowPosition = info.GetValue<WindowPosition>(nameof(MapEventWindowPosition));
            MapEventWindowSize = info.GetValue<WindowSize>(nameof(MapEventWindowSize));
            MapEventInputWindowPosition = info.GetValue<WindowPosition>(nameof(MapEventInputWindowPosition));
            CommonEventWindowPosition = info.GetValue<WindowPosition>(nameof(CommonEventWindowPosition));
            CommonEventWindowSize = info.GetValue<WindowSize>(nameof(CommonEventWindowSize));
            CommonEventInputWindowPosition = info.GetValue<WindowPosition>(nameof(CommonEventInputWindowPosition));
            UserDbWindowPosition = info.GetValue<WindowPosition>(nameof(UserDbWindowPosition));
            ChangeableDbWindowPosition = info.GetValue<WindowPosition>(nameof(ChangeableDbWindowPosition));
            SystemDbWindowPosition = info.GetValue<WindowPosition>(nameof(SystemDbWindowPosition));
            databaseValueNumberDrawType =
                DatabaseValueNumberDrawType.FromCode(info.GetValue<string>(nameof(databaseValueNumberDrawType)));
            editTimeDrawType = EditTimeDrawType.FromCode(info.GetValue<string>(nameof(editTimeDrawType)));
            EditTime = info.GetInt32(nameof(EditTime));
            NotEditTime = info.GetInt32(nameof(NotEditTime));
            IsShowDebugWindow = info.GetBoolean(nameof(IsShowDebugWindow));
            layerTransparent = LaterTransparentType.FromCode(info.GetValue<string>(nameof(layerTransparent)));
            eventLayerOpacity = EventLayerOpacityType.FromCode(info.GetValue<string>(nameof(eventLayerOpacity)));
            commandColorType = CommandColorType.FromCode(info.GetValue<string>(nameof(commandColorType)));
            IsDrawBackgroundImage = info.GetBoolean(nameof(IsDrawBackgroundImage));
            notCopyExtList = info.GetValue<ExtensionList>(nameof(notCopyExtList));
            CommandViewType = info.GetInt32(nameof(CommandViewType));
            shortCutKeyList = info.GetValue<IFixedLengthEventCommandShortCutKeyList>(nameof(shortCutKeyList));
            backupType = ProjectBackupType.FromCode(info.GetValue<string>(nameof(backupType)));
            commandPositionList = info.GetValue<IFixedLengthShortCutPositionList>(nameof(commandPositionList));
            IsUseExpertCommand = info.GetBoolean(nameof(IsUseExpertCommand));
        }
    }
}