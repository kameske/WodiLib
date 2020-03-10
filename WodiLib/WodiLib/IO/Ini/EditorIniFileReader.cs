// ========================================
// Project Name : WodiLib
// File Name    : EditorIniFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Ini;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// Editor.ini読み込みクラス
    /// </summary>
    public class EditorIniFileReader : WoditorFileReaderBase<EditorIniFilePath, EditorIniData>
    {
        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        private readonly object readLock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public EditorIniFileReader(EditorIniFilePath filePath) : base(filePath)
        {
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public override EditorIniData ReadSync()
        {
            lock (readLock)
            {
                Logger.Info(FileIOMessage.StartFileRead(GetType()));

                var result = ReadData(FilePath);

                Logger.Info(FileIOMessage.EndFileRead(GetType()));

                return result;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データ読み込みRoot
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>読み込んだデータインスタンス</returns>
        private static EditorIniData ReadData(string filePath)
        {
            var result = new List<EditorIniProgramData>
            {
                new EditorIniProgramData()
            };

            var innerReader = new HasSectionIniFileReader<EditorIniProgramData>(
                filePath, result);

            innerReader.ReadSync();

            return new EditorIniData(result[0]);
        }
    }
}