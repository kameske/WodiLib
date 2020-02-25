// ========================================
// Project Name : WodiLib
// File Name    : DBDataFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// DBタイプセットファイル
    /// </summary>
    public class DBDataFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル名
        /// </summary>
        public DBDataFilePath FileName { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しファイル
        /// </summary>
        public DBData Data { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="fileName">[NotNull] 書き出しファイル名</param>
        /// <param name="data">[NotNull] 書き出しDBファイル</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, data がnullの場合</exception>
        private static DBDataFileWriter BuildFileWriter(DBDataFilePath fileName, DBData data)
        {
            if (fileName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));

            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            var writer = new DBDataFileWriter(data, fileName);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="fileName">[NotNull] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">fileNameがnullの場合</exception>
        private static DBDataFileReader BuildFileReader(DBDataFilePath fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));

            var reader = new DBDataFileReader(fileName);
            return reader;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName">[NotNull] ファイル名</param>
        /// <exception cref="ArgumentNullException">fileNameがnullの場合</exception>
        public DBDataFile(DBDataFilePath fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));

            FileName = fileName;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">data がnullの場合</exception>
        public void WriteSync(DBData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            Data = data;

            var writer = BuildFileWriter(FileName, Data);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">data がnullの場合</exception>
        public async Task WriteAsync(DBData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            Data = data;

            var writer = BuildFileWriter(FileName, Data);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public DBData ReadSync()
        {
            var reader = BuildFileReader(FileName);
            Data = reader.ReadSync();
            return Data;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<DBData> ReadAsync()
        {
            var reader = BuildFileReader(FileName);
            await reader.ReadAsync();
            Data = reader.Data;
            return Data;
        }
    }
}