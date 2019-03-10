// ========================================
// Project Name : WodiLib
// File Name    : CommonFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Threading.Tasks;
using WodiLib.Common;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// コモンイベントデータファイル読み込みクラス
    /// </summary>
    internal class CommonFileReader
    {
        /// <summary>読み込みファイルパス</summary>
        public string FilePath { get; }

        /// <summary>[Nullable] 読み込んだコモンイベントデータ</summary>
        public CommonFileData CommonFileData { get; private set; }

        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; set; }

        /// <summary>ロガー</summary>
        private static WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public CommonFileReader(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));
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
        public CommonFileData ReadSync()
        {
            Logger.Info(FileIOMessage.StartFileRead(GetType()));

            if (CommonFileData != null)
                throw new InvalidOperationException(
                    $"すでに読み込み完了しています。");

            ReadStatus = new FileReadStatus(FilePath);
            CommonFileData = new CommonFileData();

            // ヘッダチェック
            ReadHeader(ReadStatus);

            // コモンイベント
            ReadCommonEvent(ReadStatus, CommonFileData);

            Logger.Info(FileIOMessage.EndFileRead(GetType()));

            return CommonFileData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public async Task ReadAsync()
        {
            await Task.Run(() => ReadSync());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private static void ReadHeader(FileReadStatus status)
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
        private static void ReadCommonEvent(FileReadStatus status, CommonFileData data)
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
        private static int ReadCommonEventLength(FileReadStatus status)
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
        private static void ReadCommonEventList(FileReadStatus status, int length, CommonFileData data)
        {
            var reader = new CommonEventReader(status, length);

            var commonEventList = reader.Read();
            data.SetCommonEventList(commonEventList);
        }
    }
}