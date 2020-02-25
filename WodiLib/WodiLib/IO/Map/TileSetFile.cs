// ========================================
// Project Name : WodiLib
// File Name    : TileSetFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// タイルセットファイルクラス
    /// </summary>
    public class TileSetFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル名
        /// </summary>
        public TileSetFilePath FilePath { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しマップデータ
        /// </summary>
        public TileSetFileData TileSetFileData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="tileSetFileData">[NotNull] 書き出しマップデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, tileSetFileData がnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static TileSetFileWriter BuildTileSetFileWriter(string filePath, TileSetFileData tileSetFileData)
        {
            if (tileSetFileData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetFileData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var writer = new TileSetFileWriter(tileSetFileData, filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static TileSetFileReader BuildTileSetFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var reader = new TileSetFileReader(filePath);
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
        public TileSetFile(TileSetFilePath filePath)
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
        public void WriteSync(TileSetFileData tileSetFileData)
        {
            if (tileSetFileData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetFileData)));

            TileSetFileData = tileSetFileData;

            var writer = BuildTileSetFileWriter(FilePath, TileSetFileData);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="tileSetFileData">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">tileSetFileData がnullの場合</exception>
        public async Task WriteAsync(TileSetFileData tileSetFileData)
        {
            if (tileSetFileData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetFileData)));

            TileSetFileData = tileSetFileData;

            var writer = BuildTileSetFileWriter(FilePath, TileSetFileData);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public TileSetFileData ReadSync()
        {
            var reader = BuildTileSetFileReader(FilePath);
            TileSetFileData = reader.ReadSync();
            return TileSetFileData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<TileSetFileData> ReadAsync()
        {
            var reader = BuildTileSetFileReader(FilePath);
            await reader.ReadAsync();
            TileSetFileData = reader.Data;
            return TileSetFileData;
        }
    }
}