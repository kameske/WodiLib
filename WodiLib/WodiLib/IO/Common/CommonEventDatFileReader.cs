// ========================================
// Project Name : WodiLib
// File Name    : CommonEventDatFileReader.cs
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
    public class CommonEventDatFileReader : WoditorFileReaderBase<CommonEventDatFilePath, CommonEventData>
    {
        private FileReadStatus ReadStatus { get; }

        private readonly object readLock = new object();

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public CommonEventDatFileReader(CommonEventDatFilePath filePath) : base(filePath)
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
        public override CommonEventData ReadSync()
        {
            lock (readLock)
            {
                Logger.Info(FileIOMessage.StartFileRead(GetType()));

                var commonEventData = new CommonEventData();

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // コモンイベント
                ReadCommonEvent(ReadStatus, commonEventData);

                // フッタチェック
                ReadFooter(ReadStatus);

                Logger.Info(FileIOMessage.EndFileRead(GetType()));

                return commonEventData;
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
            foreach (var b in CommonEventData.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug($"{nameof(CommonEventDatFileReader)} ヘッダチェック完了");
        }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        private void ReadCommonEvent(FileReadStatus status, CommonEventData data)
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

            Logger.Debug($"{nameof(CommonEventDatFileReader)} コモンイベント数読み込み完了 コモンイベント数：{length}");

            return length;
        }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">コモンイベント数</param>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        private void ReadCommonEventList(FileReadStatus status, int length, CommonEventData data)
        {
            var reader = new CommonEventReader(status, length);

            var commonEventList = reader.Read();
            data.CommonEventList = new CommonEventList(commonEventList);
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadFooter(FileReadStatus status)
        {
            foreach (var b in CommonEventData.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルフッタがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug($"{nameof(CommonEventDatFileReader)} フッタチェック完了");
        }
    }
}