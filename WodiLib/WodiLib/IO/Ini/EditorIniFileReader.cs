// ========================================
// Project Name : WodiLib
// File Name    : EditorIniFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WodiLib.Ini;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// Editor.ini読み込みクラス
    /// </summary>
    internal class EditorIniFileReader
    {
        /// <summary>読み込みファイルパス</summary>
        public EditorIniFilePath FilePath { get; }

        /// <summary>[Nullable] 読み込んだデータ</summary>
        public EditorIniData Data { get; private set; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public EditorIniFileReader(EditorIniFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            FilePath = filePath;
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public EditorIniData ReadSync()
        {
            if (!(Data is null))
                throw new InvalidOperationException(
                    "すでに読み込み完了しています。");

            Logger.Info(FileIOMessage.StartFileRead(GetType()));

            Data = ReadData(FilePath);

            Logger.Info(FileIOMessage.EndFileRead(GetType()));

            return Data;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public async Task<EditorIniData> ReadAsync()
        {
            return await Task.Run(ReadSync);
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