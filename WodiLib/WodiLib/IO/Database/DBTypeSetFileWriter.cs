// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetFileWriter.cs
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
    public class DBTypeSetFileWriter : WoditorBinaryFileWriterBase<DBTypeSetFilePath, DBTypeSet>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DBTypeSetFileWriter(DBTypeSetFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(DBTypeSet data)
            => data.ToBinary();
    }
}