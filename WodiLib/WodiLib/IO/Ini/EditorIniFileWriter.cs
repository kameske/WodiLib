// ========================================
// Project Name : WodiLib
// File Name    : EditorIniFileWriter.cs
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
    /// Editor.ini書き出しクラス
    /// </summary>
    public class EditorIniFileWriter : WoditorFileWriterBase<EditorIniFilePath, EditorIniData>
    {
        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        private readonly object writeLock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public EditorIniFileWriter(EditorIniFilePath filePath) : base(filePath)
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
        public override void WriteSync(EditorIniData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            lock (writeLock)
            {
                Logger.Info(FileIOMessage.StartFileWrite(GetType()));

                var innerWriter = new HasSectionIniFileWriter<EditorIniProgramData>(
                    FilePath, new List<EditorIniProgramData> {data.ToProgramData()});
                innerWriter.WriteSync();

                Logger.Info(FileIOMessage.EndFileWrite(GetType()));
            }
        }
    }
}