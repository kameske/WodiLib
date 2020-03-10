// ========================================
// Project Name : WodiLib
// File Name    : MapTreeOpenStatusDataFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Map;

namespace WodiLib.IO
{
    /// <summary>
    /// マップツリー開閉状態データファイル書き出しクラス
    /// </summary>
    public class MapTreeOpenStatusDataFileWriter
        : WoditorBinaryFileWriterBase<MapTreeOpenStatusDataFilePath, MapTreeOpenStatusData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MapTreeOpenStatusDataFileWriter(MapTreeOpenStatusDataFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(MapTreeOpenStatusData data)
            => data.ToBinary();
    }
}