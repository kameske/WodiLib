// ========================================
// Project Name : WodiLib
// File Name    : MpsFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// マップファイル書き出しクラス
    /// </summary>
    internal class MpsFileWriter
    {
        /// <summary>書き出しファイルパス</summary>
        public string FilePath { get; }

        /// <summary>書き出すマップデータ</summary>
        public MapData MapData { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">[NotNull] 書き出しマップデータ</param>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">outputData, filePathがnullの場合</exception>
        /// <exception cref="ArgumentException">filePathが空文字の場合</exception>
        public MpsFileWriter(MapData outputData, string filePath)
        {
            if (outputData == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outputData)));
            if (filePath == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if(filePath.IsEmpty()) throw new ArgumentException(
                ErrorMessage.NotEmpty(nameof(filePath)));
            MapData = outputData;
            FilePath = filePath;
        }

        /// <summary>
        /// マップデータを同期的に書き出す。
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public void WriteSync()
        {
            var bin = MapData.ToBinary().ToArray();
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                stream.Write(bin, 0, bin.Length);
            }
        }


        /// <summary>
        /// マップデータを非同期的に書き出す。
        /// </summary>
        /// <returns>書き出しTask</returns>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public async Task WriteAsync()
        {
            await Task.Run(() => WriteSync());
        }
    }
}