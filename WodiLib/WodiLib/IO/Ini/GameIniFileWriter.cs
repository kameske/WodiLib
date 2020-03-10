// ========================================
// Project Name : WodiLib
// File Name    : GameIniFileWriter.cs
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
    /// Game.ini書き出しクラス
    /// </summary>
    public class GameIniFileWriter : WoditorFileWriterBase<GameIniFilePath, GameIniData>
    {
        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        private readonly object writeLock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public GameIniFileWriter(GameIniFilePath filePath) : base(filePath)
        {
        }

        /// <summary>
        /// データを同期的に書き出す。
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <exception cref="ArgumentNullException">
        ///    data が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public override void WriteSync(GameIniData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            lock (writeLock)
            {
                Logger.Info(FileIOMessage.StartFileWrite(GetType()));

                var innerWriter = new NonSectionIniFileWriter<GameIniRootData>(
                    FilePath, new List<GameIniRootData> {data.ToIniRootData()});
                innerWriter.WriteSync();

                Logger.Info(FileIOMessage.EndFileWrite(GetType()));
            }
        }
    }
}