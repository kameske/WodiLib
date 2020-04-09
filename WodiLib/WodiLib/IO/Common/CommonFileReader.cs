// ========================================
// Project Name : WodiLib
// File Name    : CommonFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Common;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// コモンイベントデータファイル読み込みクラス
    /// </summary>
    public class CommonFileReader : WoditorFileReaderBase<CommonFilePath, CommonFileData>
    {
        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; }

        private readonly object readLock = new object();

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public CommonFileReader(CommonFilePath filePath) : base(filePath)
        {
            ReadStatus = new FileReadStatus(FilePath);
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public override CommonFileData ReadSync()
        {
            lock (readLock)
            {
                var commonFileData = new CommonFileData();

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // コモンイベント
                ReadCommonEvent(ReadStatus, commonFileData);

                Logger.Info(FileIOMessage.EndFileRead(GetType()));

                return commonFileData;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in CommonFileData.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug($"{nameof(CommonFileReader)} ヘッダチェックOK");
        }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        private void ReadCommonEvent(FileReadStatus status, CommonFileData data)
        {
            // コモンイベント数
            var length = ReadCommonEventLength(status);

            // コモンイベントリスト
            ReadCommonEventList(status, length, data);
        }


        /// <summary>
        /// コモンイベント数
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>コモンイベント数</returns>
        private int ReadCommonEventLength(FileReadStatus status)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug($"{nameof(CommonFileReader)} コモンイベント数：{length}");

            return length;
        }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">コモンイベント数</param>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        private void ReadCommonEventList(FileReadStatus status, int length, CommonFileData data)
        {
            var reader = new CommonEventReader(status, length);

            var commonEventList = reader.Read();
            data.CommonEventList = new CommonEventList(commonEventList);
        }
    }
}