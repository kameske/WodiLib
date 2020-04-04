// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDatFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// DBファイル読み込みクラス
    /// </summary>
    public class DatabaseDatFileReader : WoditorFileReaderBase<DatabaseDatFilePath, DatabaseDat>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みDB種別</summary>
        private DBKind DBKind { get; }

        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; }

        private readonly object readLock = new object();

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <param name="dbKind">読み込みDB種別</param>
        /// <exception cref="ArgumentNullException">filePath, dbKindがnullの場合</exception>
        public DatabaseDatFileReader(DatabaseDatFilePath filePath, DBKind dbKind) : base(filePath)
        {
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));

            DBKind = dbKind;
            ReadStatus = new FileReadStatus(FilePath);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public override DatabaseDat ReadSync()
        {
            lock (readLock)
            {
                Logger.Info(FileIOMessage.StartFileRead(GetType()));

                var result = new DatabaseDat();

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // DBデータ
                ReadDBData(ReadStatus, result);

                // フッタチェック
                ReadFooter(ReadStatus);

                // DB種別
                result.DBKind = DBKind;

                Logger.Info(FileIOMessage.EndFileRead(GetType()));

                return result;
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
            foreach (var b in DatabaseDat.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug($"{nameof(DatabaseDatFileReader)} ヘッダチェックOK");
        }

        /// <summary>
        /// DBデータ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="data">結果格納インスタンス</param>
        private void ReadDBData(FileReadStatus status, DatabaseDat data)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                "タイプ数", length));

            var reader = new DBDataSettingReader(status, length);
            data.SettingList.AddRange(reader.Read());

            Logger.Debug(FileIOMessage.EndCommonRead(
                typeof(DatabaseDatFileReader), "DBデータ設定"));
        }


        /// <summary>
        /// フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadFooter(FileReadStatus status)
        {
            foreach (var b in DatabaseDat.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルフッタがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug($"{nameof(DatabaseDatFileReader)} フッタチェックOK");
        }
    }
}