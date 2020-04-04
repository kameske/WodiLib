// ========================================
// Project Name : WodiLib
// File Name    : TileSetWriter.cs
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
    public class TileSetDataFileWriter : WoditorBinaryFileWriterBase<TileSetDataFilePath, TileSetData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public TileSetDataFileWriter(TileSetDataFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(TileSetData data)
            => data.ToBinary();
    }
}