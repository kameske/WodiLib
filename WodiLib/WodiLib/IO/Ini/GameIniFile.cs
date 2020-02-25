// ========================================
// Project Name : WodiLib
// File Name    : GameIniFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Ini;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// Game.iniファイルクラス
    /// </summary>
    public class GameIniFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しデータ
        /// </summary>
        public GameIniData Data { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="editorIniData">[NotNull] 書き出しマップデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, editorIniData がnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static GameIniFileWriter BuildGameIniFileWriter(string filePath, GameIniData editorIniData)
        {
            if (editorIniData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(editorIniData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var writer = new GameIniFileWriter(editorIniData, filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static GameIniFileReader BuildEditorFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var reader = new GameIniFileReader(filePath);
            return reader;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] ファイル名</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public GameIniFile(GameIniFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            FilePath = filePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="tileSetFileData">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">tileSetFileData がnullの場合</exception>
        public void WriteSync(GameIniData tileSetFileData)
        {
            if (tileSetFileData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetFileData)));

            Data = tileSetFileData;

            var writer = BuildGameIniFileWriter(FilePath, Data);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="tileSetFileData">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">tileSetFileData がnullの場合</exception>
        public async Task WriteAsync(GameIniData tileSetFileData)
        {
            if (tileSetFileData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetFileData)));

            Data = tileSetFileData;

            var writer = BuildGameIniFileWriter(FilePath, Data);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public GameIniData ReadSync()
        {
            var reader = BuildEditorFileReader(FilePath);
            Data = reader.ReadSync();
            return Data;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<GameIniData> ReadAsync()
        {
            var reader = BuildEditorFileReader(FilePath);
            await reader.ReadAsync();
            Data = reader.Data;
            return Data;
        }
    }
}