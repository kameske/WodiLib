// ========================================
// Project Name : WodiLib
// File Name    : DatabaseMergedDataReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// XXXDatabase.Dat ファイルと XXXDatabase.project ファイルをまとめて読み込み
    /// 整合性のある一つのクラスを取得するファイル読み込みクラス
    /// </summary>
    public class DatabaseMergedDataReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みデータファイルパス</summary>
        public DatabaseDatFilePath DatFilePath { get; }

        /// <summary>読み込みプロジェクトファイルパス</summary>
        public DatabaseProjectFilePath ProjectFilePath { get; }

        /// <summary>読み込んだデータ</summary>
        public DatabaseMergedData Data { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>DB種別</summary>
        private DBKind DbKind { get; }

        private readonly object readLock = new object();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     datFilePath, projectFilePath が null の場合
        /// </exception>
        public DatabaseMergedDataReader(ChangeableDatabaseDatFilePath datFilePath,
            ChangeableDatabaseProjectFilePath projectFilePath) : this(datFilePath,
            (DatabaseProjectFilePath) projectFilePath)
        {
            DbKind = DBKind.Changeable;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     datFilePath, projectFilePath が null の場合
        /// </exception>
        public DatabaseMergedDataReader(UserDatabaseDatFilePath datFilePath,
            UserDatabaseProjectFilePath projectFilePath) : this(datFilePath,
            (DatabaseProjectFilePath) projectFilePath)
        {
            DbKind = DBKind.User;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     datFilePath, projectFilePath が null の場合
        /// </exception>
        public DatabaseMergedDataReader(SystemDatabaseDatFilePath datFilePath,
            SystemDatabaseProjectFilePath projectFilePath) : this(datFilePath,
            (DatabaseProjectFilePath) projectFilePath)
        {
            DbKind = DBKind.System;
        }

        /// <summary>
        /// コンストラクタ（DatFilePath, ProjectFilePathから生成するコンストラクタの統合版）
        /// </summary>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     datFilePath, projectFilePath が null の場合
        /// </exception>
        private DatabaseMergedDataReader(DatabaseDatFilePath datFilePath, DatabaseProjectFilePath projectFilePath)
        {
            if (datFilePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(datFilePath)));
            if (projectFilePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(projectFilePath)));

            DatFilePath = datFilePath;
            ProjectFilePath = projectFilePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public DatabaseMergedData ReadSync()
        {
            lock (readLock)
            {
                Logger.Info(FileIOMessage.StartFileRead(GetType()));

                if (!(Data is null))
                    throw new InvalidOperationException(
                        $"すでに読み込み完了しています。");

                var datFileReader = new DatabaseDatFileReader(DatFilePath, DbKind);
                var dataSettingList = datFileReader.ReadSync().SettingList;

                var projectFile = new DatabaseProjectFileReader(ProjectFilePath, DbKind);
                var typeSettingList = projectFile.ReadSync().TypeSettingList;

                Data = new DatabaseMergedData(typeSettingList, dataSettingList);

                Logger.Info(FileIOMessage.EndFileRead(GetType()));

                return Data;
            }
        }

        /// <summary>
        /// ファイルを非同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public async Task<DatabaseMergedData> ReadAsync()
        {
            return await Task.Run(ReadSync);
        }
    }
}