// ========================================
// Project Name : WodiLib
// File Name    : CommonFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Common;

namespace WodiLib.IO
{
    /// <summary>
    /// コモンイベントデータファイル書き出しクラス
    /// </summary>
    public class CommonFileWriter : WoditorBinaryFileWriterBase<CommonFilePath, CommonFileData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public CommonFileWriter(CommonFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(CommonFileData data)
            => data.ToBinary();
    }
}