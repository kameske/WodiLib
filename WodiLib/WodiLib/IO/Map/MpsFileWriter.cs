// ========================================
// Project Name : WodiLib
// File Name    : MpsFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Map;

namespace WodiLib.IO
{
    /// <summary>
    /// マップファイル書き出しクラス
    /// </summary>
    public class MpsFileWriter : WoditorBinaryFileWriterBase<MpsFilePath, MapData>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MpsFileWriter(MpsFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc />
        protected override byte[] GetDataBytes(MapData data)
            => data.ToBinary();
    }
}