// ========================================
// Project Name : WodiLib
// File Name    : MapTreeOpenStatusDataFile.cs
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
    /// マップツリー開閉状態データファイルクラス
    /// </summary>
    public class MapTreeOpenStatusDataFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル名
        /// </summary>
        public MapTreeOpenStatusDataFilePath FilePath { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しマップデータ
        /// </summary>
        public MapTreeOpenStatusData MapTreeOpenStatusData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="mapTreeData">[NotNull] 書き出しマップデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, mapTreeData がnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static MapTreeOpenStatusDataFileWriter BuildMapTreeOpenStatusDataFileWriter(string filePath,
            MapTreeOpenStatusData mapTreeData)
        {
            if (mapTreeData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapTreeData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var writer = new MapTreeOpenStatusDataFileWriter(mapTreeData, filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        private static MapTreeOpenStatusDataFileReader BuildMapTreeOpenStatusDataFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            var reader = new MapTreeOpenStatusDataFileReader(filePath);
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
        public MapTreeOpenStatusDataFile(MapTreeOpenStatusDataFilePath filePath)
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
        /// <param name="mapTreeData">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">mapTreeData がnullの場合</exception>
        public void WriteSync(MapTreeOpenStatusData mapTreeData)
        {
            if (mapTreeData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapTreeData)));

            MapTreeOpenStatusData = mapTreeData;

            var writer = BuildMapTreeOpenStatusDataFileWriter(FilePath, MapTreeOpenStatusData);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="mapTreeData">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">mapTreeData がnullの場合</exception>
        public async Task WriteAsync(MapTreeOpenStatusData mapTreeData)
        {
            if (mapTreeData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(mapTreeData)));

            MapTreeOpenStatusData = mapTreeData;

            var writer = BuildMapTreeOpenStatusDataFileWriter(FilePath, MapTreeOpenStatusData);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public MapTreeOpenStatusData ReadSync()
        {
            var reader = BuildMapTreeOpenStatusDataFileReader(FilePath);
            MapTreeOpenStatusData = reader.ReadSync();
            return MapTreeOpenStatusData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<MapTreeOpenStatusData> ReadAsync()
        {
            var reader = BuildMapTreeOpenStatusDataFileReader(FilePath);
            await reader.ReadAsync();
            MapTreeOpenStatusData = reader.Data;
            return MapTreeOpenStatusData;
        }
    }
}