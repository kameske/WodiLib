// ========================================
// Project Name : WodiLib
// File Name    : WoditorProject.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Event;
using WodiLib.Ini;
using WodiLib.IO;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// ウディタプロジェクトクラス
    /// </summary>
    [Serializable]
    public class WoditorProject
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 対象プロジェクトのルートディレクトリ
        /// </summary>
        public string TargetDirectory { get; }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        public CommonEventList CommonEventList => CommonEventData.CommonEventList;

        /// <summary>
        /// マップツリーノードリスト
        /// </summary>
        public MapTreeNodeList MapTreeNodeList => MapTreeData.TreeNodeList;

        /// <summary>
        /// 読み込んだマップファイルのプール
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Dictionary<MpsFilePath, MapData> MpsFilesPool => new Dictionary<MpsFilePath, MapData>();

        /// <summary>
        /// マップツリー開閉状態リスト
        /// </summary>
        public MapTreeOpenStatusList MapTreeOpenStatusList => MapTreeOpenStatusData.StatusList;

        /// <summary>
        /// タイルセット設定リスト
        /// </summary>
        public TileSetSettingList TileSetSettingList => TileSetData.TileSetSettingList;

        /// <summary>
        /// 可変DB
        /// </summary>
        public DatabaseMergedData ChangeableDatabase { get; private set; }

        /// <summary>
        /// ユーザDB
        /// </summary>
        public DatabaseMergedData UserDatabase { get; private set; }

        /// <summary>
        /// システムDB
        /// </summary>
        public DatabaseMergedData SystemDatabase { get; private set; }

        /// <summary>
        /// Editor.iniデータ
        /// </summary>
        public EditorIniData EditorIni { get; private set; }

        /// <summary>
        /// Game.iniデータ
        /// </summary>
        public GameIniData GameIni { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private CommonEventData CommonEventData { get; set; }

        private MapTreeData MapTreeData { get; set; }

        private MapTreeOpenStatusData MapTreeOpenStatusData { get; set; }

        private TileSetData TileSetData { get; set; }

        private CommonEventDatFilePath CommonEventDatFilePath => $"{TargetDirectory}/Data/BasicData/CommonEvent.dat";

        private MapTreeDataFilePath MapTreeDataFilePath => $"{TargetDirectory}/Data/BasicData/MapTree.dat";

        private MapTreeOpenStatusDataFilePath MapTreeOpenStatusDataFilePath =>
            $"{TargetDirectory}/Data/BasicData/MapTreeOpenStatus.dat";

        private TileSetDataFilePath TileSetDataFilePath =>
            $"{TargetDirectory}/Data/BasicData/TileSetData.dat";

        private ChangeableDatabaseProjectFilePath ChangeableDatabaseProjectFilePath =>
            $"{TargetDirectory}/Data/BasicData/CDataBase.project";

        private ChangeableDatabaseDatFilePath ChangeableDatabaseDatFilePath =>
            $"{TargetDirectory}/Data/BasicData/CDataBase.dat";

        private UserDatabaseProjectFilePath UserDatabaseProjectFilePath =>
            $"{TargetDirectory}/Data/BasicData/DataBase.project";

        private UserDatabaseDatFilePath UserDatabaseDatFilePath => $"{TargetDirectory}/Data/BasicData/DataBase.dat";

        private SystemDatabaseProjectFilePath SystemDatabaseProjectFilePath =>
            $"{TargetDirectory}/Data/BasicData/SysDataBase.project";

        private SystemDatabaseDatFilePath SystemDatabaseDatFilePath =>
            $"{TargetDirectory}/Data/BasicData/SysDataBase.dat";

        private EditorIniFilePath EditorIniFilePath => $"{TargetDirectory}/Editor.ini";

        private GameIniFilePath GameIniFilePath => $"{TargetDirectory}/Game.ini";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetDirectory">
        ///     [NotNullOrEmpty][NotNewLine]
        ///     対象プロジェクトのルートディレクトリ（Game.exe等があるフォルダ）
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     targetDirectory が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     targetDirectory が空白の場合、
        ///     または targetDirectory がディレクトリ名として不適切な場合
        /// </exception>
        /// <exception cref="ArgumentNewLineException">
        ///     targetDirectory に改行コードが含まれる場合
        /// </exception>
        /// <exception cref="IOException">
        ///     targetDirectoryがファイル名の場合、
        ///     またはネットワークエラーの場合
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     アクセス許可がない場合
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     targetDirectory がシステム定義の最大長を超えている場合
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     指定されたディレクトリが存在しない場合
        /// </exception>
        public WoditorProject(string targetDirectory)
        {
            if (targetDirectory is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(targetDirectory)));
            if (targetDirectory.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(targetDirectory)));
            if (targetDirectory.HasNewLine())
                throw new ArgumentNewLineException(
                    ErrorMessage.NotNewLine(nameof(targetDirectory), targetDirectory));

            // ディレクトリの存在チェックとアクセス権限チェックを兼ねる
            var _ = Directory.GetFiles(targetDirectory);

            TargetDirectory = targetDirectory;

            ReadAllSync();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region ReadMethod

        /// <summary>
        /// コモンイベントデータを同期的に再読込する。
        /// マップファイルは対象外。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadAllSync()
        {
            ReadCommonEventDataSync();
            ReadMapTreeDataSync();
            ReadMapTreeOpenStatusDataSync();
            ReadTileSetDataSync();
            ReadChangeableDatabaseSync();
            ReadUserDatabaseSync();
            ReadSystemDatabaseSync();
            ReadEditorIniSync();
            ReadGameIniSync();
        }

        /// <summary>
        /// コモンイベントデータを非同期的に再読込する。
        /// マップファイルは対象外。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadAllAsync()
        {
            await ReadCommonEventDataAsync();
            await ReadMapTreeDataAsync();
            await ReadMapTreeOpenStatusDataAsync();
            await ReadTileSetDataAsync();
            await ReadChangeableDatabaseAsync();
            await ReadUserDatabaseAsync();
            await ReadSystemDatabaseAsync();
            await ReadEditorIniAsync();
            await ReadGameIniAsync();
        }

        /// <summary>
        /// コモンイベントデータを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadCommonEventDataSync()
        {
            var file = new CommonEventDatFile(CommonEventDatFilePath);
            CommonEventData = file.ReadSync();
        }

        /// <summary>
        /// コモンイベントデータを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadCommonEventDataAsync()
        {
            var file = new CommonEventDatFile(CommonEventDatFilePath);
            CommonEventData = await file.ReadAsync();
        }

        /// <summary>
        /// マップツリーデータを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadMapTreeDataSync()
        {
            var file = new MapTreeDataFile(MapTreeDataFilePath);
            MapTreeData = file.ReadSync();
        }

        /// <summary>
        /// マップツリーデータを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadMapTreeDataAsync()
        {
            var file = new MapTreeDataFile(MapTreeDataFilePath);
            MapTreeData = await file.ReadAsync();
        }

        /// <summary>
        /// マップツリー開閉状態データを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadMapTreeOpenStatusDataSync()
        {
            if (!Directory.Exists(MapTreeOpenStatusDataFilePath))
            {
                // MapTreeOpenStatus ファイルが存在しない場合、すでに読み込んだ NapTreeData に合わせてデータを補完する
                ComplementNapTreeOpenStatusData();
                return;
            }

            var file = new MapTreeOpenStatusDataFile(MapTreeOpenStatusDataFilePath);
            MapTreeOpenStatusData = file.ReadSync();
        }

        /// <summary>
        /// マップツリー開閉状態データを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadMapTreeOpenStatusDataAsync()
        {
            if (!Directory.Exists(MapTreeOpenStatusDataFilePath))
            {
                // MapTreeOpenStatus ファイルが存在しない場合、すでに読み込んだ NapTreeData に合わせてデータを補完する
                ComplementNapTreeOpenStatusData();
                return;
            }

            var file = new MapTreeOpenStatusDataFile(MapTreeOpenStatusDataFilePath);
            MapTreeOpenStatusData = await file.ReadAsync();
        }

        /// <summary>
        /// タイルセットデータを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadTileSetDataSync()
        {
            var file = new TileSetDataFile(TileSetDataFilePath);
            TileSetData = file.ReadSync();
        }

        /// <summary>
        /// タイルセットデータを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadTileSetDataAsync()
        {
            var file = new TileSetDataFile(TileSetDataFilePath);
            TileSetData = await file.ReadAsync();
        }

        /// <summary>
        /// MapTreeData に合わせて MapTreeOpenStatusData を補完する。
        /// </summary>
        private void ComplementNapTreeOpenStatusData()
        {
            // MapTreeDataが読み込まれていなければエラー（通常ありえない）
            if (MapTreeData is null) throw new InvalidOperationException();

            MapTreeOpenStatusData = new MapTreeOpenStatusData();
            MapTreeOpenStatusData.StatusList.AdjustLength(MapTreeData.TreeNodeList.Count);
        }

        /// <summary>
        /// マップファイルを同期的に読み込む。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public MapData ReadMpsFileSync(MpsFilePath filePath, bool useCache = true)
        {
            if (useCache && MpsFilesPool.ContainsKey(filePath))
            {
                return MpsFilesPool[filePath];
            }

            RemoveMpsFilesCache(filePath);

            var fullPath = Path.GetFullPath($@"{TargetDirectory}\{filePath}");

            var file = new MpsFile(fullPath);
            var mapData = file.ReadSync();
            MpsFilesPool.Add(filePath, mapData);
            return mapData;
        }

        /// <summary>
        /// マップファイルを非同期的に読み込む。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task<MapData> ReadMpsFileAsync(MpsFilePath filePath, bool useCache = true)
        {
            if (useCache && MpsFilesPool.ContainsKey(filePath))
            {
                return MpsFilesPool[filePath];
            }

            RemoveMpsFilesCache(filePath);

            var fullPath = Path.GetFullPath($@"{TargetDirectory}\{filePath}");

            var file = new MpsFile(fullPath);
            var mapData = await file.ReadAsync();
            MpsFilesPool.Add(filePath, mapData);
            return mapData;
        }

        /// <summary>
        /// 可変DBデータを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadChangeableDatabaseSync()
        {
            var reader = new DatabaseMergedDataReader(
                ChangeableDatabaseDatFilePath,
                ChangeableDatabaseProjectFilePath);
            ChangeableDatabase = reader.ReadSync();
        }

        /// <summary>
        /// 可変DBデータを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadChangeableDatabaseAsync()
        {
            var reader = new DatabaseMergedDataReader(
                ChangeableDatabaseDatFilePath,
                ChangeableDatabaseProjectFilePath);
            ChangeableDatabase = await reader.ReadAsync();
        }

        /// <summary>
        /// ユーザDBデータを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadUserDatabaseSync()
        {
            var reader = new DatabaseMergedDataReader(
                UserDatabaseDatFilePath,
                UserDatabaseProjectFilePath);
            UserDatabase = reader.ReadSync();
        }

        /// <summary>
        /// ユーザDBデータを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadUserDatabaseAsync()
        {
            var reader = new DatabaseMergedDataReader(
                UserDatabaseDatFilePath,
                UserDatabaseProjectFilePath);
            UserDatabase = await reader.ReadAsync();
        }

        /// <summary>
        /// システムDBデータを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadSystemDatabaseSync()
        {
            var reader = new DatabaseMergedDataReader(
                SystemDatabaseDatFilePath,
                SystemDatabaseProjectFilePath);
            SystemDatabase = reader.ReadSync();
        }

        /// <summary>
        /// システムDBデータを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadSystemDatabaseAsync()
        {
            var reader = new DatabaseMergedDataReader(
                SystemDatabaseDatFilePath,
                SystemDatabaseProjectFilePath);
            SystemDatabase = await reader.ReadAsync();
        }

        /// <summary>
        /// Editor.iniファイルを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadEditorIniSync()
        {
            var datFile = new EditorIniFile(EditorIniFilePath);
            EditorIni = datFile.ReadSync();
        }

        /// <summary>
        /// Editor.iniファイルを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadEditorIniAsync()
        {
            var datFile = new EditorIniFile(EditorIniFilePath);
            EditorIni = await datFile.ReadAsync();
        }

        /// <summary>
        /// Game.iniファイルを同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public void ReadGameIniSync()
        {
            var datFile = new GameIniFile(GameIniFilePath);
            GameIni = datFile.ReadSync();
        }

        /// <summary>
        /// Game.iniファイルを非同期的に再読込する。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが正しく読み込めなかった場合</exception>
        public async Task ReadGameIniAsync()
        {
            var datFile = new GameIniFile(GameIniFilePath);
            GameIni = await datFile.ReadAsync();
        }

        #endregion

        #region WriteMethod

        /// <summary>
        /// コモンイベントデータを同期的に書き出す。
        /// </summary>
        public void WriteCommonEventDataSync()
        {
            var file = new CommonEventDatFile(CommonEventDatFilePath);
            file.WriteSync(CommonEventData);
        }

        /// <summary>
        /// コモンイベントデータを非同期的に書き出す。
        /// </summary>
        public async Task WriteCommonEventDataAsync()
        {
            var file = new CommonEventDatFile(CommonEventDatFilePath);
            await file.WriteAsync(CommonEventData);
        }

        /// <summary>
        /// マップツリーデータを同期的に書き出す。
        /// </summary>
        public void WriteMapTreeDataSync()
        {
            var file = new MapTreeDataFile(MapTreeDataFilePath);
            file.WriteSync(MapTreeData);
        }

        /// <summary>
        /// マップツリーデータを非同期的に書き出す。
        /// </summary>
        public async Task WriteMapTreeDataAsync()
        {
            var file = new MapTreeDataFile(MapTreeDataFilePath);
            await file.WriteAsync(MapTreeData);
        }

        /// <summary>
        /// マップツリー開閉状態データを同期的に書き出す。
        /// </summary>
        public void WriteMapTreeOpenStatusDataSync()
        {
            var file = new MapTreeOpenStatusDataFile(MapTreeOpenStatusDataFilePath);
            file.WriteSync(MapTreeOpenStatusData);
        }

        /// <summary>
        /// マップツリー開閉状態データを非同期的に書き出す。
        /// </summary>
        public async Task WriteMapTreeOpenStatusDataAsync()
        {
            var file = new MapTreeOpenStatusDataFile(MapTreeOpenStatusDataFilePath);
            await file.WriteAsync(MapTreeOpenStatusData);
        }

        /// <summary>
        /// プール内のマップファイルを同期的に書き出す。
        /// </summary>
        /// <param name="filePath">[NotNull] ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="InvalidOperationException">指定されたファイルデータがプール内に存在しない場合</exception>
        public void WriteMpsFileInCacheSync(MpsFilePath filePath)
        {
            var file = new MpsFile(filePath);
            if (!MpsFilesPool.ContainsKey(filePath))
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("指定されたマップファイルのデータがプール内に存在しないため、"));

            var outputData = MpsFilesPool[filePath];
            file.WriteSync(outputData);
        }

        /// <summary>
        /// プール内のマップファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="filePath">[NotNull] ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="InvalidOperationException">指定されたファイルデータがプール内に存在しない場合</exception>
        public async Task WriteMpsFileAsync(MpsFilePath filePath)
        {
            var file = new MpsFile(filePath);
            if (!MpsFilesPool.ContainsKey(filePath))
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("指定されたマップファイルのデータがプール内に存在しないため、"));

            var outputData = MpsFilesPool[filePath];
            await file.WriteAsync(outputData);
        }

        /// <summary>
        /// 可変DBを同期的に書き出す。
        /// </summary>
        public void WriteChangeableDatabaseSync()
        {
            var writer = new DatabaseMergedDataWriter(
                ChangeableDatabase,
                ChangeableDatabaseDatFilePath,
                ChangeableDatabaseProjectFilePath);
            writer.WriteSync();
        }

        /// <summary>
        /// 可変DBを非同期的に書き出す。
        /// </summary>
        public async Task WriteChangeableDatabaseAsync()
        {
            var writer = new DatabaseMergedDataWriter(
                ChangeableDatabase,
                ChangeableDatabaseDatFilePath,
                ChangeableDatabaseProjectFilePath);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ユーザDBを同期的に書き出す。
        /// </summary>
        public void WriteUserDatabaseSync()
        {
            var writer = new DatabaseMergedDataWriter(
                UserDatabase,
                UserDatabaseDatFilePath,
                UserDatabaseProjectFilePath);
            writer.WriteSync();
        }

        /// <summary>
        /// ユーザDBを非同期的に書き出す。
        /// </summary>
        public async Task WriteUserDatabaseAsync()
        {
            var writer = new DatabaseMergedDataWriter(
                UserDatabase,
                UserDatabaseDatFilePath,
                UserDatabaseProjectFilePath);
            await writer.WriteAsync();
        }

        /// <summary>
        /// システムDBを同期的に書き出す。
        /// </summary>
        public void WriteSystemDatabaseSync()
        {
            var writer = new DatabaseMergedDataWriter(
                SystemDatabase,
                SystemDatabaseDatFilePath,
                SystemDatabaseProjectFilePath);
            writer.WriteSync();
        }

        /// <summary>
        /// システムDBを非同期的に書き出す。
        /// </summary>
        public async Task WriteSystemDatabaseAsync()
        {
            var writer = new DatabaseMergedDataWriter(
                SystemDatabase,
                SystemDatabaseDatFilePath,
                SystemDatabaseProjectFilePath);
            await writer.WriteAsync();
        }

        /// <summary>
        /// Editor.iniを同期的に書き出す。
        /// </summary>
        public void WriteEditorIniSync()
        {
            var file = new EditorIniFile(EditorIniFilePath);
            file.WriteSync(EditorIni);
        }

        /// <summary>
        /// Editor.iniを非同期的に書き出す。
        /// </summary>
        public async Task WriteEditorIniAsync()
        {
            var file = new EditorIniFile(EditorIniFilePath);
            await file.WriteAsync(EditorIni);
        }

        /// <summary>
        /// Game.iniを同期的に書き出す。
        /// </summary>
        public void WriteGameIniSync()
        {
            var file = new GameIniFile(GameIniFilePath);
            file.WriteSync(GameIni);
        }

        /// <summary>
        /// Game.iniを非同期的に書き出す。
        /// </summary>
        public async Task WriteGameIniAsync()
        {
            var file = new GameIniFile(GameIniFilePath);
            await file.WriteAsync(GameIni);
        }

        #endregion

        #region MpsFilePool

        /// <summary>
        /// マップファイルプールから指定したファイルのプールを削除する。
        /// プールが存在しない場合は何もしない。
        /// </summary>
        /// <param name="filePath">[NotNull] ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public void RemoveMpsFilesCache(MpsFilePath filePath)
        {
            if (!MpsFilesPool.ContainsKey(filePath)) return;

            MpsFilesPool.Remove(filePath);
        }

        /// <summary>
        /// マップファイルプールから全ファイルのプールを削除する。
        /// プールが存在しない場合は何もしない。
        /// </summary>
        public void ClearMpsFilesCache() => MpsFilesPool.Clear();

        #endregion

        #region MpsFile

        /// <summary>
        /// 指定したマップデータ・マップイベントIDのマップイベントを取得する。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込むMpsファイルパス</param>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>マップイベント（存在しない場合null）</returns>
        public MapEvent GetMapEvent(MpsFilePath filePath, MapEventId mapEventId, bool useCache = true)
        {
            var mpsData = ReadMpsFileSync(filePath, useCache);
            return mpsData.GetMapEvent(mapEventId);
        }

        /// <summary>
        /// 指定したマップデータ・マップイベントIDのマップイベントページリストを取得する。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込むMpsファイルパス</param>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>マップイベントページリスト</returns>
        /// <exception cref="ArgumentException">マップイベントIDで指定したマップイベントが存在しない場合</exception>
        public MapEventPageList GetEventPageList(MpsFilePath filePath, MapEventId mapEventId,
            bool useCache = true)
        {
            var mpsData = ReadMpsFileSync(filePath, useCache);
            return mpsData.GetEventPageList(mapEventId);
        }

        /// <summary>
        /// 指定したマップデータ・マップイベントID、ページインデックスのマップイベントページ情報を取得する。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込むMpsファイルパス</param>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="pageIndex">[Range(1, {対象イベントのページ数})] マップイベントページインデックス</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>マップイベントページ情報</returns>
        /// <exception cref="ArgumentException">マップイベントIDで指定したマップイベントが存在しない場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">pageIndex が指定範囲外の場合</exception>
        public MapEventPage GetMapEventPage(MpsFilePath filePath, MapEventId mapEventId,
            MapEventPageIndex pageIndex, bool useCache = true)
        {
            var mpsData = ReadMpsFileSync(filePath, useCache);
            return mpsData.GetMapEventPage(mapEventId, pageIndex);
        }


        /// <summary>
        /// 指定したマップデータ・マップイベント・ページのイベントコマンド文字列情報を返す。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込むMpsファイルパス</param>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="eventPageNumber">マップイベントページ番号</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>イベントコマンド文字列情報リスト</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="InvalidOperationException">Mpsファイルが正しく読み込めない場合</exception>
        public IReadOnlyList<EventCommandSentenceInfo> GetMapEventEventCommandSentenceInfoListSync(
            MpsFilePath filePath, MapEventId mapEventId, int eventPageNumber, bool useCache = true)
        {
            var mpsData = ReadMpsFileSync(filePath, useCache);

            var resolver = new EventCommandSentenceResolver(CommonEventList,
                ChangeableDatabase, UserDatabase, SystemDatabase, mpsData);
            var desc = new EventCommandSentenceResolveDesc();

            return mpsData.MakeEventCommandSentenceInfoList(resolver, desc, mapEventId, eventPageNumber);
        }

        /// <summary>
        /// 指定したマップデータ・マップイベント・ページのイベントコマンド文字列情報を返す。
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込むMpsファイルパス</param>
        /// <param name="mapEventId">マップイベントID</param>
        /// <param name="eventPageNumber">マップイベントページ番号</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>イベントコマンド文字列情報リスト</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="InvalidOperationException">Mpsファイルが正しく読み込めない場合</exception>
        public async Task<IReadOnlyList<EventCommandSentenceInfo>> GetMapEventEventCommandSentenceInfoListAsync(
            MpsFilePath filePath, MapEventId mapEventId, int eventPageNumber, bool useCache = true)
        {
            var mpsData = await ReadMpsFileAsync(filePath, useCache);

            var resolver = MakeEventCommandSentenceResolver(mpsData);
            var desc = new EventCommandSentenceResolveDesc();

            return mpsData.MakeEventCommandSentenceInfoList(resolver, desc, mapEventId, eventPageNumber);
        }

        #endregion

        #region CommonEventMethod

        /// <summary>
        /// コモンイベントを取得する。
        /// </summary>
        /// <param name="commonEventId">[Range(0, CommonEventList.Count - 1)] コモンイベントID</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">commonEventIdが指定範囲外の場合</exception>
        public CommonEvent GetCommonEvent(CommonEventId commonEventId)
        {
            var max = CommonEventList.Count;
            const int min = 0;
            if (commonEventId < min || max < commonEventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(commonEventId), min, max, commonEventId));

            return CommonEventList[commonEventId];
        }

        /// <summary>
        /// 指定したコモンイベントのイベントコードリストを取得する。
        /// </summary>
        /// <returns>イベントコードリスト</returns>
        public IReadOnlyList<string> GetEventCodeStringList(CommonEventId commonEventId)
            => CommonEventList.GetEventCodeStringList(commonEventId);


        /// <summary>
        /// 指定したコモンイベントのイベントコマンド文字列情報を返す。
        /// </summary>
        /// <param name="commonEventId">コモンイベントID</param>
        /// <param name="filePath">[Nullable] 表示対象マップファイルパス</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>イベントコマンド文字列情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">指定されたcommonEventIdが存在しない場合</exception>
        /// <exception cref="InvalidOperationException">Mpsファイルが正しく読み込めない場合</exception>
        /// <remarks>
        ///     コモンイベントのイベントコマンド文字列について、
        ///     マップイベント名を表示する箇所については現在ウディタで開いているマップのマップイベントを参照する。
        ///     WodiLibではこのメソッドを呼ぶときに表示対象となるマップイベントのパスを指定することで
        ///     "開いているマップ情報"の代わりとする。
        ///     マップイベント情報は指定しないことも可能。このとき、マップイベント名は表示されない。
        /// </remarks>
        public IReadOnlyList<EventCommandSentenceInfo> GetCommonEventEventCommandSentenceInfoListSync(
            CommonEventId commonEventId, MpsFilePath filePath = null, bool useCache = true)
        {
            var mapData = filePath != null
                ? ReadMpsFileSync(filePath, useCache)
                : null;

            var resolver = MakeEventCommandSentenceResolver(mapData);
            var desc = new EventCommandSentenceResolveDesc {CommonEventId = commonEventId};

            return CommonEventList.GetCommonEventEventCommandSentenceInfoList(commonEventId, resolver, desc);
        }

        /// <summary>
        /// 指定したコモンイベントのイベントコマンド文字列情報を返す。
        /// </summary>
        /// <param name="commonEventId">コモンイベントID</param>
        /// <param name="filePath">[Nullable] 表示対象マップファイルパス</param>
        /// <param name="useCache">
        ///     プール使用フラグ
        ///     （読み込みプール内に該当データが存在する場合、プールデータを返す）
        /// </param>
        /// <returns>イベントコマンド文字列情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">指定されたcommonEventIdが存在しない場合</exception>
        /// <exception cref="InvalidOperationException">Mpsファイルが正しく読み込めない場合</exception>
        public async Task<IReadOnlyList<EventCommandSentenceInfo>> GetCommonEventEventCommandSentenceInfoListAsync(
            CommonEventId commonEventId, MpsFilePath filePath = null, bool useCache = true)
        {
            var mapData = filePath != null
                ? await ReadMpsFileAsync(filePath, useCache)
                : null;

            var resolver = MakeEventCommandSentenceResolver(mapData);
            var desc = new EventCommandSentenceResolveDesc {CommonEventId = commonEventId};

            return CommonEventList.GetCommonEventEventCommandSentenceInfoList(commonEventId, resolver, desc);
        }

        #endregion

        #region Database

        /// <summary>
        /// DB種別を指定してDatabaseMergedDataのインスタンスを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        public DatabaseMergedData GetDatabaseMergedData(DBKind dbKind)
        {
            if (dbKind == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));

            if (dbKind == DBKind.Changeable) return ChangeableDatabase;
            if (dbKind == DBKind.User) return UserDatabase;
            if (dbKind == DBKind.System) return SystemDatabase;

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }

        /// <summary>
        /// 指定したDB、タイプIDのタイプ情報を取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <returns>DBタイプ情報</returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">typeId が指定範囲外の場合</exception>
        public DatabaseTypeDesc GetDatabaseTypeDesc(DBKind dbKind, TypeId typeId)
        {
            var mergedData = GetDatabaseMergedData(dbKind);
            return mergedData.TypeDescList[typeId];
        }

        /// <summary>
        /// 指定したDB、タイプIDのデータ名リストを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <returns>DBデータ情報リスト</returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">typeId が指定範囲外の場合</exception>
        public IReadOnlyDataNameList GetDatabaseDataNameList(DBKind dbKind, TypeId typeId)
        {
            var mergedData = GetDatabaseMergedData(dbKind);
            return mergedData.GetDataNameList(typeId);
        }

        /// <summary>
        /// 指定したDB、タイプIDのデータ情報リストを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <returns>DBデータ情報リスト</returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">typeId が指定範囲外の場合</exception>
        public DatabaseDataDescList GetDatabaseDataDescList(DBKind dbKind, TypeId typeId)
        {
            var mergedData = GetDatabaseMergedData(dbKind);
            return mergedData.GetDataDescList(typeId);
        }

        /// <summary>
        /// 指定したDB、タイプIDのデータ情報リストを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <returns>DBデータ情報リスト</returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">typeId, dataId が指定範囲外の場合</exception>
        public DatabaseDataDesc GetDatabaseDataDesc(DBKind dbKind, TypeId typeId, DataId dataId)
        {
            var mergedData = GetDatabaseMergedData(dbKind);
            return mergedData.GetDataDesc(typeId, dataId);
        }

        /// <summary>
        /// 指定したDB、タイプID、データIDの項目値リストを取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <returns>DB項目値リスト</returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">typeId, dataId が指定範囲外の場合</exception>
        public DBItemValueList GetDatabaseItemValueList(DBKind dbKind, TypeId typeId, DataId dataId)
        {
            var mergedData = GetDatabaseMergedData(dbKind);
            return mergedData.GetItemValueList(typeId, dataId);
        }

        /// <summary>
        /// 指定したDB、タイプID、データID、項目IDの項目値を取得する。
        /// </summary>
        /// <param name="dbKind">[NotNull] DB種別</param>
        /// <param name="typeId">[Range(0, {対象DBのタイプ数} - 1)] タイプID</param>
        /// <param name="dataId">[Range(0, {対象DB・タイプのデータ数} - 1)] データID</param>
        /// <param name="itemId">[Range(0, {対象DB・タイプ・データの項目数} - 1)] データID</param>
        /// <returns>DB項目値リスト</returns>
        /// <exception cref="ArgumentNullException">dbKind が null の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">typeId, dataId, itemId が指定範囲外の場合</exception>
        public DBItemValue GetDatabaseItemValue(DBKind dbKind, TypeId typeId, DataId dataId, ItemId itemId)
        {
            var mergedData = GetDatabaseMergedData(dbKind);
            return mergedData.GetItemValue(typeId, dataId, itemId);
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文字列解決クラスを生成する。
        /// </summary>
        /// <param name="mpsData">[Nullable] 表示に使用するマップデータ</param>
        /// <returns></returns>
        private EventCommandSentenceResolver MakeEventCommandSentenceResolver(MapData mpsData)
        {
            return new EventCommandSentenceResolver(CommonEventList,
                ChangeableDatabase, UserDatabase, SystemDatabase, mpsData);
        }
    }
}