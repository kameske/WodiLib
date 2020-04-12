// ========================================
// Project Name : WodiLib
// File Name    : WoditorBinaryFileWriterBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Commons;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// ウディタ関連バイナリファイル書き出し基底クラス
    /// </summary>
    /// <typeparam name="TFilePath">ファイルパス</typeparam>
    /// <typeparam name="TFileData">書き出し対象クラス</typeparam>
    public abstract class WoditorBinaryFileWriterBase<TFilePath, TFileData>
        : WoditorFileWriterBase<TFilePath, TFileData>
        where TFilePath : FilePath
    {
        /// <summary>ロガー</summary>
        private Logger Logger { get; } = Logger.GetInstance();

        private readonly object writeLock = new object();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public WoditorBinaryFileWriterBase(TFilePath filePath) : base(filePath)
        {
        }

        /// <summary>
        /// ファイルを同期的に書き出す。
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <exception cref="ArgumentNullException">
        ///    data が null の場合
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     ファイル名が正しくない場合、
        ///     またはFilePathが非ファイルデバイスを参照している場合
        /// </exception>
        public override void WriteSync([NotNull] TFileData data)
        {
            if (data is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(data)));

            lock (writeLock)
            {
                Logger.Info(FileIOMessage.StartFileWrite(GetType()));

                var bin = GetDataBytes(data).ToArray();
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    stream.Write(bin, 0, bin.Length);
                }

                Logger.Info(FileIOMessage.EndFileWrite(GetType()));
            }
        }

        /// <summary>
        /// 出力データのバイナリデータを取得する。
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <returns>出力バイナリデータ</returns>
        protected abstract byte[] GetDataBytes([NotNull] TFileData data);
    }
}