// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// DBプロジェクトデータファイル読み込みクラス
    /// </summary>
    internal class DBTypeSetFileReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みファイルパス</summary>
        public string FilePath { get; }

        /// <summary>[Nullable] 読み込んだDBプロジェクトデータ</summary>
        public DBTypeSet Data { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DBTypeSetFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            FilePath = filePath;
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
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public DBTypeSet ReadSync()
        {
            Logger.Info(FileIOMessage.StartFileRead(GetType()));

            if (!(Data is null))
                throw new InvalidOperationException(
                    $"すでに読み込み完了しています。");

            Data = new DBTypeSet();

            // ヘッダチェック
            ReadHeader(ReadStatus);

            // 項目種別
            ReadValueType(ReadStatus, out var itemTypes);

            // タイプ設定
            ReadTypeSetting(ReadStatus, Data, itemTypes);

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
        public async Task<DBTypeSet> ReadAsync()
        {
            return await Task.Run(ReadSync);
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
            foreach (var b in DBTypeSet.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(DBTypeSetFileReader),
                "ヘッダ"));
        }

        /// <summary>
        /// 設定種別 &amp; 種別順列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="itemTypes">取得した項目種別リスト格納先</param>
        private void ReadValueType(FileReadStatus status, out List<DBItemType> itemTypes)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSetFileReader),
                "項目数", length));

            var countDic = new Dictionary<DBItemType, int>
            {
                {DBItemType.Int, 0},
                {DBItemType.String, 0}
            };

            itemTypes = new List<DBItemType>();

            for (var i = 0; i < length; i++)
            {
                var settingCode = status.ReadInt();
                status.IncreaseIntOffset();

                var itemType = DBItemType.FromValue(settingCode);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSetFileReader),
                    $"  項目{i,2}設定種別", itemType));

                // 項目タイプ数集計
                countDic[itemType]++;

                // 種別順位は無視する

                itemTypes.Add(itemType);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(
                typeof(DBDataSettingReader), "項目設定種別"));
        }

        private void ReadTypeSetting(FileReadStatus status, DBTypeSet data, IReadOnlyList<DBItemType> itemTypes)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBTypeSetFileReader),
                "タイプ設定"));

            var reader = new DBTypeSettingReader(status, 1, false);

            var settings = reader.Read();

            data.TypeName = settings[0].TypeName;
            data.Memo = settings[0].Memo;
            data.ItemSettingList.AddRange(settings[0].ItemSettingList);

            if (data.ItemSettingList.Count != itemTypes.Count)
                throw new InvalidOperationException(
                    $"項目値種別数と項目設定数が一致しません。");

            for (var i = 0; i < data.ItemSettingList.Count; i++)
            {
                data.ItemSettingList[i].ItemType = itemTypes[i];
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBTypeSetFileReader),
                "タイプ設定リスト"));
        }
    }
}