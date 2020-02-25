// ========================================
// Project Name : WodiLib
// File Name    : DBTypeFileReader.cs
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
    internal class DBTypeFileReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みファイルパス</summary>
        public string FilePath { get; }

        /// <summary>[Nullable] 読み込んだDBプロジェクトデータ</summary>
        public DBType Data { get; private set; }

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
        public DBTypeFileReader(string filePath)
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
        public DBType ReadSync()
        {
            Logger.Info(FileIOMessage.StartFileRead(GetType()));

            if (!(Data is null))
                throw new InvalidOperationException(
                    $"すでに読み込み完了しています。");

            Data = new DBType();

            // ヘッダチェック
            ReadHeader(ReadStatus);

            // タイプ設定
            ReadTypeSetting(ReadStatus, out var typeSetting);
            Data.TypeName = typeSetting.TypeName;
            Data.Memo = typeSetting.Memo;

            // データ設定
            ReadDataSetting(ReadStatus, out var dataSetting);
            Data.SetDataSettingType(dataSetting);

            Data.ItemDescList.AddRange(MakeItemDescList(typeSetting, dataSetting));

            Data.DataDescList.Overwrite(0, MakeDataDescList(typeSetting, dataSetting));

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
        public async Task<DBType> ReadAsync()
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
            foreach (var b in DBType.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(DBTypeFileReader),
                "ヘッダ"));
        }

        /// <summary>
        /// タイプ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="typeSetting">読み込み結果インスタンス</param>
        private void ReadTypeSetting(FileReadStatus status, out DBTypeSetting typeSetting)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBTypeFileReader),
                "タイプ設定"));

            var reader = new DBTypeSettingReader(status, 1, true);

            var settings = reader.Read();
            typeSetting = settings[0];

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBTypeFileReader),
                "タイプ設定"));
        }

        /// <summary>
        /// タイプ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="dataSetting">読み込み結果インスタンス</param>
        private void ReadDataSetting(FileReadStatus status, out DBDataSetting dataSetting)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBTypeFileReader),
                "データ設定"));

            var reader = new DBDataSettingReader(status, 1);

            var settings = reader.Read();
            dataSetting = settings[0];

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBTypeFileReader),
                "データ設定"));
        }

        /// <summary>
        /// DataDescのリスト
        /// </summary>
        /// <param name="typeSetting">タイプ設定</param>
        /// <param name="dataSetting">データ設定</param>
        /// <returns></returns>
        private IReadOnlyList<DatabaseDataDesc> MakeDataDescList(DBTypeSetting typeSetting,
            DBDataSetting dataSetting)
        {
            var result = new List<DatabaseDataDesc>();

            var dataNameList = typeSetting.DataNameList;
            var valuesList = dataSetting.SettingValuesList;

            if (dataNameList.Count != valuesList.Count)
                throw new InvalidOperationException(
                    $"データ名数とデータ数が異なります。");

            for (var i = 0; i < typeSetting.DataNameList.Count; i++)
            {
                var desc = new DatabaseDataDesc(dataNameList[i],
                    valuesList[i].ToLengthChangeableItemValueList());

                result.Add(desc);
            }

            return result;
        }

        private IReadOnlyList<DatabaseItemDesc> MakeItemDescList(DBTypeSetting typeSetting,
            DBDataSetting dataSetting)
        {
            var result = new List<DatabaseItemDesc>();

            var itemSettingList = typeSetting.ItemSettingList;
            var valueList = dataSetting.SettingValuesList[0];

            if (itemSettingList.Count != valueList.Count)
                throw new InvalidOperationException(
                    "項目設定数と項目数が異なります。");

            for (var i = 0; i < itemSettingList.Count; i++)
            {
                var desc = new DatabaseItemDesc
                {
                    ItemName = itemSettingList[i].ItemName,
                    SpecialSettingDesc = itemSettingList[i].SpecialSettingDesc,
                    ItemType = valueList[i].Type,
                };
                result.Add(desc);
            }

            return result;
        }
    }
}