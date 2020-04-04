// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    /// DBプロジェクトデータファイル書き出しクラス
    /// </summary>
    public class DatabaseProjectFileWriter : WoditorBinaryFileWriterBase<DatabaseProjectFilePath, DatabaseProject>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DatabaseProjectFileWriter(DatabaseProjectFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(DatabaseProject data)
            => data.ToBinary();
    }
}