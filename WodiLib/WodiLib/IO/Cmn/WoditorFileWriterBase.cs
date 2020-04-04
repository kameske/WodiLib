// ========================================
// Project Name : WodiLib
// File Name    : WoditorFileWriterBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// ウディタ関連ファイル書き出し基底クラス
    /// </summary>
    /// <typeparam name="TFilePath">ファイルパス</typeparam>
    /// <typeparam name="TFileData">書き出し対象クラス</typeparam>
    public abstract class WoditorFileWriterBase<TFilePath, TFileData>
        where TFilePath : FilePath
    {
        /// <summary>書き出しファイルパス</summary>
        public TFilePath FilePath { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public WoditorFileWriterBase(TFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            FilePath = filePath;
        }

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 出力データ</param>
        /// <exception cref="ArgumentNullException">
        ///    data が null の場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     ファイル名が正しくない場合、
        ///     またはFilePathが非ファイルデバイスを参照している場合
        /// </exception>
        public abstract void WriteSync(TFileData data);

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">[NotNull] 出力データ</param>
        /// <returns>書き出しTask</returns>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public async Task WriteAsync(TFileData data)
        {
            await Task.Run(() => WriteSync(data));
        }
    }
}