// ========================================
// Project Name : WodiLib
// File Name    : DBTypeFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// DBファイル書き出しクラス
    /// </summary>
    internal class DBTypeFileWriter
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>書き出しファイルパス</summary>
        public string FilePath { get; }

        /// <summary>書き出すDBファイル</summary>
        public DBType Data { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">[NotNull] 書き出しDBファイル</param>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">outputData, filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        public DBTypeFileWriter(DBType outputData, string filePath)
        {
            if (outputData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outputData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));
            Data = outputData;
            FilePath = filePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBファイルを同期的に書き出す。
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public void WriteSync()
        {
            Logger.Info(FileIOMessage.StartFileWrite(GetType()));

            var bin = Data.ToBinary().ToArray();
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                stream.Write(bin, 0, bin.Length);
            }

            Logger.Info(FileIOMessage.EndFileWrite(GetType()));
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