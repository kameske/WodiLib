// ========================================
// Project Name : WodiLib
// File Name    : TileSetDataFile.cs
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
    /// タイルセットデータファイルクラス
    /// </summary>
    public class TileSetDataFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル名
        /// </summary>
        public TileSetDataFilePath FilePath { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しマップデータ
        /// </summary>
        public TileSetData TileSetData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="tileSetData">[NotNull] 書き出しマップデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, tileSetData がnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static TileSetDataFileWriter BuildTileSetDataFileWriter(string filePath, TileSetData tileSetData)
        {
            if (tileSetData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var writer = new TileSetDataFileWriter(tileSetData, filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static TileSetDataFileReader BuildTileSetDataFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var reader = new TileSetDataFileReader(filePath);
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
        public TileSetDataFile(TileSetDataFilePath filePath)
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
        /// <param name="tileSetData">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">tileSetData がnullの場合</exception>
        public void WriteSync(TileSetData tileSetData)
        {
            if (tileSetData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetData)));

            TileSetData = tileSetData;

            var writer = BuildTileSetDataFileWriter(FilePath, TileSetData);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="tileSetData">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">tileSetData がnullの場合</exception>
        public async Task WriteAsync(TileSetData tileSetData)
        {
            if (tileSetData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(tileSetData)));

            TileSetData = tileSetData;

            var writer = BuildTileSetDataFileWriter(FilePath, TileSetData);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public TileSetData ReadSync()
        {
            var reader = BuildTileSetDataFileReader(FilePath);
            TileSetData = reader.ReadSync();
            return TileSetData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<TileSetData> ReadAsync()
        {
            var reader = BuildTileSetDataFileReader(FilePath);
            await reader.ReadAsync();
            TileSetData = reader.Data;
            return TileSetData;
        }
    }
}