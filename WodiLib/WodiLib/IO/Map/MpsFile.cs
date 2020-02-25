// ========================================
// Project Name : WodiLib
// File Name    : MpsFile.cs
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
    /// マップファイルクラス
    /// </summary>
    public class MpsFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル名
        /// </summary>
        public MpsFilePath FilePath { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しマップデータ
        /// </summary>
        public MapData MapData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="mapData">[NotNull] 書き出しマップデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, mapData がnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static MpsFileWriter BuildMpsFileWriter(string filePath, MapData mapData)
        {
            if (mapData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var writer = new MpsFileWriter(mapData, filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static MpsFileReader BuildMpsFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var reader = new MpsFileReader(filePath);
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
        public MpsFile(MpsFilePath filePath)
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
        /// <param name="mapData">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">mapData がnullの場合</exception>
        public void WriteSync(MapData mapData)
        {
            if (mapData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapData)));

            MapData = mapData;

            var writer = BuildMpsFileWriter(FilePath, MapData);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="mapData">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">mapData がnullの場合</exception>
        public async Task WriteAsync(MapData mapData)
        {
            if (mapData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapData)));

            MapData = mapData;

            var writer = BuildMpsFileWriter(FilePath, MapData);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public MapData ReadSync()
        {
            var reader = BuildMpsFileReader(FilePath);
            MapData = reader.ReadSync();
            return MapData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<MapData> ReadAsync()
        {
            var reader = BuildMpsFileReader(FilePath);
            await reader.ReadAsync();
            MapData = reader.MapData;
            return MapData;
        }
    }
}