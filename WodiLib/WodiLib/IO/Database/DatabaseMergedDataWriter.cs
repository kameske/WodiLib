// ========================================
// Project Name : WodiLib
// File Name    : DatabaseMergedDataWriter.cs
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
    /// DatabaseMergedData インスタンスの内容を
    /// XXXDatabase.Dat、 XXXDatabase.project に出力する書き出しクラス
    /// </summary>
    public class DatabaseMergedDataWriter
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みデータファイルパス</summary>
        public DatabaseDatFilePath DatFilePath { get; }

        /// <summary>読み込みプロジェクトファイルパス</summary>
        public DatabaseProjectFilePath ProjectFilePath { get; }

        /// <summary>書き出すDB種別</summary>
        public DBKind DbKind { get; }

        /// <summary>書き出すデータ</summary>
        public DatabaseMergedData Data { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        private readonly object writeLock = new object();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">書き出しデータ</param>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     outputData, datFilePath, projectFilePath が null の場合
        /// </exception>
        public DatabaseMergedDataWriter(DatabaseMergedData outputData, ChangeableDatabaseDatFilePath datFilePath,
            ChangeableDatabaseProjectFilePath projectFilePath) : this(outputData, datFilePath,
            (DatabaseProjectFilePath) projectFilePath)
        {
            DbKind = DBKind.Changeable;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">書き出しデータ</param>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     outputData, datFilePath, projectFilePath が null の場合
        /// </exception>
        public DatabaseMergedDataWriter(DatabaseMergedData outputData, UserDatabaseDatFilePath datFilePath,
            UserDatabaseProjectFilePath projectFilePath) : this(outputData, datFilePath,
            (DatabaseProjectFilePath) projectFilePath)
        {
            DbKind = DBKind.User;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">書き出しデータ</param>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     outputData, datFilePath, projectFilePath が null の場合
        /// </exception>
        public DatabaseMergedDataWriter(DatabaseMergedData outputData, SystemDatabaseDatFilePath datFilePath,
            SystemDatabaseProjectFilePath projectFilePath) : this(outputData, datFilePath,
            (DatabaseProjectFilePath) projectFilePath)
        {
            DbKind = DBKind.System;
        }

        /// <summary>
        /// コンストラクタ（DatFilePath, ProjectFilePathから生成するコンストラクタの統合版）
        /// </summary>
        /// <param name="outputData">書き出しデータ</param>
        /// <param name="datFilePath">データファイルパス</param>
        /// <param name="projectFilePath">プロジェクトファイルパス</param>
        /// <exception cref="ArgumentNullException">
        ///     outputData, datFilePath, projectFilePath が null の場合
        /// </exception>
        private DatabaseMergedDataWriter(DatabaseMergedData outputData, DatabaseDatFilePath datFilePath,
            DatabaseProjectFilePath projectFilePath)
        {
            if (datFilePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(datFilePath)));
            if (projectFilePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(projectFilePath)));

            Data = outputData;
            DatFilePath = datFilePath;
            ProjectFilePath = projectFilePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public void WriteSync()
        {
            lock (writeLock)
            {
                Logger.Info(FileIOMessage.StartFileWrite(GetType()));

                var writeDatabaseDat = Data.GenerateDatabaseDat(DbKind);
                var datFileWriter = new DatabaseDatFileWriter(DatFilePath);
                datFileWriter.WriteSync(writeDatabaseDat);

                var writeDatabaseProject = Data.GenerateDatabaseProject(DbKind);
                var projectFileWriter = new DatabaseProjectFileWriter(ProjectFilePath);
                projectFileWriter.WriteSync(writeDatabaseProject);

                Logger.Info(FileIOMessage.EndFileWrite(GetType()));
            }
        }

        /// <summary>
        /// DBファイルを非同期的に書き出す。
        /// </summary>
        /// <returns>書き出しTask</returns>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public async Task WriteAsync()
        {
            await Task.Run(WriteSync);
        }
    }
}