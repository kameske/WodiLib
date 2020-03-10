// ========================================
// Project Name : WodiLib
// File Name    : MapTreeDataFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Map;

namespace WodiLib.IO
{
    /// <summary>
    /// マップツリーデータ書き出しクラス
    /// </summary>
    public class MapTreeDataFileWriter : WoditorBinaryFileWriterBase<MapTreeDataFilePath, MapTreeData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MapTreeDataFileWriter(MapTreeDataFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(MapTreeData data)
            => data.ToBinary();
    }
}