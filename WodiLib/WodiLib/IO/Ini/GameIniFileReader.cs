// ========================================
// Project Name : WodiLib
// File Name    : GameIniFileReader.cs
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
    /// マップツリーデータ読み込みクラス
    /// </summary>
    public class GameIniFileReader : WoditorFileReaderBase<GameIniFilePath, GameIniData>
    {
        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        private readonly object readLock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public GameIniFileReader(GameIniFilePath filePath) : base(filePath)
        {
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public override GameIniData ReadSync()
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
        private static GameIniData ReadData(string filePath)
        {
            var result = new List<GameIniRootData>
            {
                new GameIniRootData()
            };

            var innerReader = new NonSectionIniFileReader<GameIniRootData>(
                filePath, result);

            innerReader.ReadSync();

            return new GameIniData(result[0]);
        }
    }
}