// ========================================
// Project Name : WodiLib
// File Name    : DBDataSettingReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// DBデータ設定読み込みクラス
    /// </summary>
    internal class DBDataSettingReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>DBタイプ設定数</summary>
        private int Length { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">DBタイプ設定数</param>
        public DBDataSettingReader(FileReadStatus status, int length)
        {
            Status = status;
            Length = length;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBタイプ設定を読み込み、返す。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        public List<DBDataSetting> Read()
        {
            Logger.Debug(FileIOMessage.StartCommonRead(GetType(), ""));

            var list = new List<DBDataSetting>();
            for (var i = 0; i < Length; i++)
            {
                ReadOneDBTypeSetting(Status, list);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(GetType(), ""));

            return list;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBタイプ設定一つ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">バイナリデータがファイル仕様と異なる場合</exception>
        private void ReadOneDBTypeSetting(FileReadStatus status, ICollection<DBDataSetting> result)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBDataSettingReader), "DBタイプ設定"));

            var setting = new DBDataSetting();

            // ヘッダ
            ReadHeader(status);

            // データIDの設定方法
            ReadDataSettingType(status, setting);

            // 設定種別 & 種別順列
            ReadValueType(status, out var types);

            // DBデータ設定値
            ReadDataSettingValue(status, setting, types);

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBDataSettingReader), "DBタイプ設定"));

            result.Add(setting);
        }

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in DBDataSetting.Header)
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
        /// データIDの設定方法
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="setting">結果格納インスタンス</param>
        private void ReadDataSettingType(FileReadStatus status, DBDataSetting setting)
        {
            var typeCode = status.ReadInt();
            status.IncreaseIntOffset();

            var settingType = DBDataSettingType.FromValue(typeCode);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                "データID設定方法", settingType));

            // 「指定DBの指定タイプ」の場合、DB種別とタイプIDを取り出す
            DBKind dbKind = null;
            TypeId typeId = 0;
            if (settingType == DBDataSettingType.DesignatedType)
            {
                dbKind = DbKindFromSettingTypeCode(typeCode);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                    "DB種別", dbKind));

                typeId = TypeIdFromSettingTypeCode(typeCode);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                    "タイプID", typeId));
            }

            setting.SetDataSettingType(settingType, dbKind, typeId);
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

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
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

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                    $"  項目{i,2}設定種別", itemType));

                // 項目タイプ数集計
                countDic[itemType]++;

                // 種別順位は無視する

                itemTypes.Add(itemType);
            }

            Logger.Debug(FileIOMessage.EndCommonRead(
                typeof(DBDataSettingReader), "項目設定種別"));
        }

        /// <summary>
        /// DBデータ設定値
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="setting">結果格納インスタンス</param>
        /// <param name="itemTypes">項目種別リスト</param>
        private void ReadDataSettingValue(FileReadStatus status, DBDataSetting setting,
            IReadOnlyCollection<DBItemType> itemTypes)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                "データ数", length));

            var numberItemCount = itemTypes.Count(x => x == DBItemType.Int);
            var stringItemCount = itemTypes.Count(x => x == DBItemType.String);

            var valuesList = new List<List<DBItemValue>>();

            for (var i = 0; i < length; i++)
            {
                ReadOneDataSettingValue(status, valuesList, itemTypes, numberItemCount, stringItemCount);
            }

            setting.SettingValuesList = new DBItemValuesList(valuesList);
        }

        /// <summary>
        /// DBデータ設定値ひとつ分
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <param name="itemTypes">項目種別リスト</param>
        /// <param name="numberItemCount">数値項目数</param>
        /// <param name="stringItemCount">文字列項目数</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private void ReadOneDataSettingValue(FileReadStatus status, List<List<DBItemValue>> result,
            IEnumerable<DBItemType> itemTypes, int numberItemCount, int stringItemCount)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBDataSettingReader),
                "データ設定値"));

            var numberItems = new List<DBValueInt>();
            var stringItems = new List<DBValueString>();

            for (var i = 0; i < numberItemCount; i++)
            {
                var numberItem = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                    $"  数値項目{i,2}", numberItem));

                numberItems.Add(numberItem);
            }

            for (var i = 0; i < stringItemCount; i++)
            {
                var stringItem = status.ReadString();
                status.AddOffset(stringItem.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataSettingReader),
                    $"  文字列項目{i,2}", stringItem));

                stringItems.Add(stringItem.String);
            }

            var valueList = new List<DBItemValue>();

            var numberIndex = 0;
            var stringIndex = 0;
            foreach (var itemType in itemTypes)
            {
                if (itemType == DBItemType.Int)
                {
                    valueList.Add(numberItems[numberIndex]);
                    numberIndex++;
                }
                else if (itemType == DBItemType.String)
                {
                    valueList.Add(stringItems[stringIndex]);
                    stringIndex++;
                }
                else
                {
                    // 通常ここへは来ない
                    throw new InvalidOperationException(
                        "未対応のデータ種別です。");
                }
            }

            result.Add(valueList);

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBDataSettingReader),
                "データ設定値"));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データIDの設定方法コードからDB種別を取得する。
        /// </summary>
        /// <param name="code">設定種別コード</param>
        /// <returns>DB種別</returns>
        private DBKind DbKindFromSettingTypeCode(int code)
        {
            var dbKindCode = (byte) code.SubInt(4, 1);
            return DBKind.FromDBDataSettingTypeCode(dbKindCode);
        }

        /// <summary>
        /// データIDの設定方法コードからタイプIDを取得する。
        /// </summary>
        /// <param name="code">設定種別コード</param>
        /// <returns>タイプID</returns>
        private TypeId TypeIdFromSettingTypeCode(int code)
        {
            return code.SubInt(0, 4);
        }
    }
}