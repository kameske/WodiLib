// ========================================
// Project Name : WodiLib
// File Name    : CommonEventDatFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Common;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// CommonEvent.datファイルクラス
    /// </summary>
    public class CommonEventDatFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルパス
        /// </summary>
        public CommonEventDatFilePath FilePath { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しコモンイベントデータ
        /// </summary>
        public CommonEventData CommonEventData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="fileName">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="data">[NotNull] 書き出しコモンイベントデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePath, data がnullの場合</exception>
        /// <exception cref="ArgumentException">fileNameが空文字の場合</exception>
        private static CommonEventDatFileWriter BuildFileWriter(string fileName, CommonEventData data)
        {
            if (fileName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));
            if (fileName.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(fileName)));
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            var writer = new CommonEventDatFileWriter(data, fileName);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="fileName">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">fileNameがnullの場合</exception>
        /// <exception cref="ArgumentException">fileNameが空文字の場合</exception>
        private static CommonEventDatFileReader BuildFileReader(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));
            if (fileName.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(fileName)));

            var reader = new CommonEventDatFileReader(fileName);
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
        public CommonEventDatFile(CommonEventDatFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (((string) filePath).IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            FilePath = filePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">data がnullの場合</exception>
        public void WriteSync(CommonEventData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            CommonEventData = data;

            var writer = BuildFileWriter(FilePath, CommonEventData);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">data がnullの場合</exception>
        public async Task WriteAsync(CommonEventData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            CommonEventData = data;

            var writer = BuildFileWriter(FilePath, CommonEventData);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public CommonEventData ReadSync()
        {
            var reader = BuildFileReader(FilePath);
            CommonEventData = reader.ReadSync();
            return CommonEventData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<CommonEventData> ReadAsync()
        {
            var reader = BuildFileReader(FilePath);
            await reader.ReadAsync();
            CommonEventData = reader.CommonEventData;
            return CommonEventData;
        }
    }
}