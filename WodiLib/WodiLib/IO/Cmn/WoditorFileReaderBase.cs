// ========================================
// Project Name : WodiLib
// File Name    : WoditorFileReaderBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// ウディタ関連ファイル読み込み基底クラス
    /// </summary>
    /// <typeparam name="TFilePath">ファイルパス</typeparam>
    /// <typeparam name="TFileData">読み込み結果クラス</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class WoditorFileReaderBase<TFilePath, TFileData>
        where TFilePath : FilePath
    {
        /// <summary>[NotNull] 読み込みファイルパス</summary>
        public TFilePath FilePath { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public WoditorFileReaderBase(TFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            FilePath = filePath;
        }

        /// <summary>
        /// ファイルを同期的に読み込む。
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public abstract TFileData ReadSync();

        /// <summary>
        /// ファイルを非同期的に読み込む。
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public async Task<TFileData> ReadAsync()
        {
            return await Task.Run(ReadSync);
        }
    }
}