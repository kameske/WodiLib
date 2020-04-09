// ========================================
// Project Name : WodiLib
// File Name    : MpsFile.cs
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
    /// マップファイルクラス
    /// </summary>
    public class MpsFile : WoditorFileBase<MpsFilePath, MapData,
        MpsFileWriter, MpsFileReader>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 読み取り/書き出しデータ
        /// </summary>
        [Obsolete("入出力データは Read/Write メソッドの戻値を使用してください。 Ver1.3 で削除します。")]
        public MapData MapData { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MpsFile(MpsFilePath filePath) : base(filePath)
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
        protected override MpsFileWriter MakeFileWriter(MpsFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            var writer = new MpsFileWriter(filePath);
            return writer;
        }

        /// <summary>
        /// ファイル読み込みクラスを生成する。
        /// </summary>
        /// <param name="filePath">読み込みファイル名</param>
        /// <returns>リーダーインスタンス</returns>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        protected override MpsFileReader MakeFileReader(MpsFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            var reader = new MpsFileReader(filePath);
            return reader;
        }

        /// <inheritdoc />
        [Obsolete("Ver1.1 以前と互換性を持たせるためだけのメソッドです。 Ver1.3 で削除します。")]
        protected override void CallbackIO(MapData data)
        {
            MapData = data;
        }
    }
}