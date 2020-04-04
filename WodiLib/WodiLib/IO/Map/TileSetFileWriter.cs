// ========================================
// Project Name : WodiLib
// File Name    : TileSetFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Map;

namespace WodiLib.IO
{
    /// <summary>
    /// タイルセットファイル書き出しクラス
    /// </summary>
    public class TileSetFileWriter : WoditorBinaryFileWriterBase<TileSetFilePath, TileSetFileData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public TileSetFileWriter(TileSetFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(TileSetFileData data)
            => data.ToBinary();
    }
}