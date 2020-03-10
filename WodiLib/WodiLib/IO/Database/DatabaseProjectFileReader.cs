// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectFileReader.cs
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
    /// DBプロジェクトデータファイル読み込みクラス
    /// </summary>
    public class DatabaseProjectFileReader : WoditorFileReaderBase<DatabaseProjectFilePath, DatabaseProject>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みDB種別</summary>
        private DBKind DBKind { get; }

        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        private readonly object readLock = new object();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <param name="dbKind">読み込みDB種別</param>
        /// <exception cref="ArgumentNullException">filePath, dbKindがnullの場合</exception>
        public DatabaseProjectFileReader(DatabaseProjectFilePath filePath, DBKind dbKind) : base(filePath)
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
        public override DatabaseProject ReadSync()
        {
            lock (readLock)
            {
                Logger.Info(FileIOMessage.StartFileRead(GetType()));

                var result = new DatabaseProject();

                ReadTypeSettingList(ReadStatus, result);

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
        /// タイプ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadTypeSettingList(FileReadStatus status, DatabaseProject data)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBTypeSettingReader),
                "タイプ設定リスト"));

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DatabaseProjectFileReader),
                "タイプ設定数", length));

            var reader = new DBTypeSettingReader(status, length, true);

            var settings = reader.Read();

            data.TypeSettingList.AddRange(settings);

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBTypeSettingReader),
                "タイプ設定リスト"));
        }
    }
}