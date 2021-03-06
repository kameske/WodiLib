// ========================================
// Project Name : WodiLib
// File Name    : TileSetFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// タイルセットファイルクラス
    /// </summary>
    public class TileSetFile : WoditorFileBase<TileSetFilePath, TileSetFileData,
        TileSetFileWriter, TileSetFileReader>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public TileSetFile(TileSetFilePath filePath) : base(filePath)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイル書き出しクラスを生成する。
        /// </summary>
        /// <param name="filePath">書き出しファイル名</param>
        /// <returns>ライターインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        protected override TileSetFileWriter MakeFileWriter(TileSetFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            var writer = new TileSetFileWriter(filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        protected override TileSetFileReader MakeFileReader(TileSetFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            var reader = new TileSetFileReader(filePath);
            return reader;
        }
    }
}
