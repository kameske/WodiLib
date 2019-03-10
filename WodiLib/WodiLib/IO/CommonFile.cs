// ========================================
// Project Name : WodiLib
// File Name    : CommonFile.cs
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
    /// XXX.commonファイルクラス
    /// </summary>
    public class CommonFile
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// [Nullable] 読み取り/書き出しコモンイベントデータ
        /// </summary>
        public CommonFileData CommonFileData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="fileName">[NotNullOrEmpty] 書き出しファイル名</param>
        /// <param name="data">[NotNull] 書き出しコモンイベントデータ</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">fileName, data がnullの場合</exception>
        /// <exception cref="ArgumentException">fileNameが空文字の場合</exception>
        private static CommonFileWriter BuildFileWriter(string fileName, CommonFileData data)
        {
            if (fileName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));
            if (fileName.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(fileName)));
            if (data == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            var writer = new CommonFileWriter(data, fileName);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="fileName">[NotNullOrEmpty] 読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">fileNameがnullの場合</exception>
        /// <exception cref="ArgumentException">fileNameが空文字の場合</exception>
        private static CommonFileReader BuildFileReader(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));
            if (fileName.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(fileName)));

            var reader = new CommonFileReader(fileName);
            return reader;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName">[NotNullOrEmpty] ファイル名</param>
        /// <exception cref="ArgumentNullException">fileNameがnullの場合</exception>
        /// <exception cref="ArgumentException">fileNameが空の場合</exception>
        public CommonFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(fileName)));
            if (fileName.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(fileName)));

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
        public void WriteSync(CommonFileData data)
        {
            if (data == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            CommonFileData = data;

            var writer = BuildFileWriter(FileName, CommonFileData);
            writer.WriteSync();
        }

        /// <summary>
        /// ファイルを非同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 書き出しデータ</param>
        /// <returns>非同期処理タスク</returns>
        /// <exception cref="ArgumentNullException">data がnullの場合</exception>
        public async Task WriteAsync(CommonFileData data)
        {
            if (data == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            CommonFileData = data;

            var writer = BuildFileWriter(FileName, CommonFileData);
            await writer.WriteAsync();
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public CommonFileData ReadSync()
        {
            var reader = BuildFileReader(FileName);
            CommonFileData = reader.ReadSync();
            return CommonFileData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込みデータを返すタスク</returns>
        public async Task<CommonFileData> ReadASync()
        {
            var reader = BuildFileReader(FileName);
            await reader.ReadAsync();
            CommonFileData = reader.CommonFileData;
            return CommonFileData;
        }
    }
}