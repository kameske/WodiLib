// ========================================
// Project Name : WodiLib
// File Name    : TileSetDataFile.cs
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
    /// タイルセットデータファイルクラス
    /// </summary>
    public class TileSetDataFile : WoditorFileBase<TileSetDataFilePath, TileSetData,
        TileSetDataFileWriter, TileSetDataFileReader>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 読み取り/書き出しデータ
        /// </summary>
        [Obsolete("入出力データは Read/Write メソッドの戻値を使用してください。 Ver1.3 で削除します。")]
        public TileSetData TileSetData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public TileSetDataFile(TileSetDataFilePath filePath) : base(filePath)
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
        protected override TileSetDataFileWriter MakeFileWriter(TileSetDataFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            var writer = new TileSetDataFileWriter(filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        protected override TileSetDataFileReader MakeFileReader(TileSetDataFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            var reader = new TileSetDataFileReader(filePath);
            return reader;
        }

        /// <inheritdoc />
        [Obsolete("Ver1.1 以前と互換性を持たせるためだけのメソッドです。 Ver1.3 で削除します。")]
        protected override void CallbackIO(TileSetData data)
        {
            TileSetData = data;
        }
    }
}
