// ========================================
// Project Name : WodiLib
// File Name    : DBDataFileWriter.cs
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
    public class DBDataFileWriter : WoditorBinaryFileWriterBase<DBDataFilePath, DBData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DBDataFileWriter(DBDataFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(DBData data)
            => data.ToBinary();
    }
}