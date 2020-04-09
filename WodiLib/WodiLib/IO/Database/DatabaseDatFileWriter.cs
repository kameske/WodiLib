// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDatFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    /// DBファイル書き出しクラス
    /// </summary>
    public class DatabaseDatFileWriter : WoditorBinaryFileWriterBase<DatabaseDatFilePath, DatabaseDat>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        public DatabaseDatFileWriter(DatabaseDatFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(DatabaseDat data)
            => data.ToBinary();
    }
}