// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSettingReader.cs
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
    /// DBタイプ設定読み込みクラス
    /// </summary>
    internal class DBTypeSettingReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>DBタイプ設定数</summary>
        private int Length { get; }

        /// <summary>データ名リスト含有フラグ</summary>
        private bool HasDataNameList { get; }

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
        /// <param name="hasDataNameList">データ名リスト含有フラグ</param>
        public DBTypeSettingReader(FileReadStatus status, int length, bool hasDataNameList)
        {
            Status = status;
            Length = length;
            HasDataNameList = hasDataNameList;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBタイプ設定を読み込み、返す。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        public List<DBTypeSetting> Read()
        {
            Logger.Debug(FileIOMessage.StartCommonRead(GetType(), ""));

            var list = new List<DBTypeSetting>();
            for (var i = 0; i < Length; i++)
            {
                ReadOneDBTypeSetting(Status, HasDataNameList, list);
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
        /// <param name="isReadDataNameList">データ名リスト読み込みフラグ</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">バイナリデータがファイル仕様と異なる場合</exception>
        private void ReadOneDBTypeSetting(FileReadStatus status,
            bool isReadDataNameList, ICollection<DBTypeSetting> result)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(DBTypeSettingReader), "DBタイプ設定"));

            var setting = new DBTypeSetting();

            // DBタイプ名
            ReadTypeName(status, setting);

            // 項目名
            var itemNames = ReadItemName(status);

            if (isReadDataNameList)
            {
                // データ名
                ReadDataName(status, setting);
            }

            // メモ
            ReadMemo(status, setting);

            // 特殊指定
            var specialSettingTypes = ReadItemSpecialSettingType(status);

            // 項目メモ
            var itemMemos = ReadItemMemo(status);

            // 特殊指定文字列パラメータ
            var valueDescriptionLists = ReadSpecialStringValue(status);

            // 特殊指定数値パラメータ
            var valueCaseNumberLists = ReadSpecialNumberValue(status);

            // 初期値
            var initValues = ReadItemInitValue(status);

            // 特殊指定セット
            SetItemSetting(setting, specialSettingTypes, itemNames, itemMemos, valueDescriptionLists,
                valueCaseNumberLists, initValues);

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(DBTypeSettingReader), "DBタイプ設定"));

            result.Add(setting);
        }

        /// <summary>
        /// タイプ名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="setting">結果格納インスタンス</param>
        private void ReadTypeName(FileReadStatus status, DBTypeSetting setting)
        {
            var typeName = status.ReadString();
            setting.TypeName = typeName.String;

            status.AddOffset(typeName.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "タイプ名", setting.TypeName));
        }

        /// <summary>
        /// 項目名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>項目名リスト</returns>
        private List<ItemName> ReadItemName(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "項目数", length));

            var result = new List<ItemName>();

            for (var i = 0; i < length; i++)
            {
                var name = status.ReadString();
                status.AddOffset(name.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  項目名{i,2}", name.String));

                result.Add(name.String);
            }

            return result;
        }

        /// <summary>
        /// データ名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="setting">結果格納インスタンス</param>
        private void ReadDataName(FileReadStatus status, DBTypeSetting setting)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "データ数", length));

            var dataNameList = new List<DataName>();

            for (var i = 0; i < length; i++)
            {
                var name = status.ReadString();
                status.AddOffset(name.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  データ名{i,4}", name.String));

                dataNameList.Add(name.ToString());
            }

            setting.DataNameList = new DataNameList(dataNameList);
        }

        /// <summary>
        /// メモ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="setting">結果格納インスタンス</param>
        private void ReadMemo(FileReadStatus status, DBTypeSetting setting)
        {
            var memo = status.ReadString();
            setting.Memo = memo.String;

            status.AddOffset(memo.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "メモ", memo.String));
        }

        /// <summary>
        /// 項目特殊指定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>項目項目特殊指定リスト</returns>
        private List<DBItemSpecialSettingType> ReadItemSpecialSettingType(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "項目特殊指定数", length));

            var result = new List<DBItemSpecialSettingType>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadByte();
                status.IncreaseByteOffset();

                var type = DBItemSpecialSettingType.FromByte(value);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  項目特殊指定{i,2}", type));

                result.Add(type);
            }

            return result;
        }

        /// <summary>
        /// 項目メモ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>項目名リスト</returns>
        private List<ItemMemo> ReadItemMemo(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "項目メモ数", length));

            var result = new List<ItemMemo>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadString();
                status.AddOffset(value.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  項目メモ{i,2}", value.String));

                result.Add(value.String);
            }

            return result;
        }

        /// <summary>
        /// 特殊指定文字列パラメータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>特殊指定文字列パラメータリスト</returns>
        private List<List<DatabaseValueCaseDescription>> ReadSpecialStringValue(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "特殊指定文字列パラメータ数", length));

            var result = new List<List<DatabaseValueCaseDescription>>();

            for (var i = 0; i < length; i++)
            {
                var descriptionLength = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  項目{i,2}のパラメータ数", descriptionLength));

                var paramList = new List<DatabaseValueCaseDescription>();
                for (var j = 0; j < descriptionLength; j++)
                {
                    var value = status.ReadString();
                    status.AddOffset(value.ByteLength);

                    Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                        $"    パラメータ{j,2}", value.String));

                    paramList.Add(value.String);
                }

                result.Add(paramList);
            }

            return result;
        }

        /// <summary>
        /// 特殊指定数値パラメータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>特殊指定数値パラメータリスト</returns>
        private List<List<DatabaseValueCaseNumber>> ReadSpecialNumberValue(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "特殊指定数値パラメータ数", length));

            var result = new List<List<DatabaseValueCaseNumber>>();

            for (var i = 0; i < length; i++)
            {
                var descriptionLength = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  項目{i,2}のパラメータ数", descriptionLength));

                var paramList = new List<DatabaseValueCaseNumber>();
                for (var j = 0; j < descriptionLength; j++)
                {
                    var value = status.ReadInt();
                    status.IncreaseIntOffset();

                    Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                        $"    パラメータ{j,2}", value));

                    paramList.Add(value);
                }

                result.Add(paramList);
            }

            return result;
        }

        /// <summary>
        /// 初期値
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>初期値リスト</returns>
        private List<DBValueInt> ReadItemInitValue(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                "項目初期値数", length));

            var result = new List<DBValueInt>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBTypeSettingReader),
                    $"  初期値{i,2}", value));

                result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// DB項目設定セット
        /// </summary>
        /// <param name="setting">結果格納インスタンス</param>
        /// <param name="specialSettingTypes">特殊指定種別リスト</param>
        /// <param name="itemNames">項目名リスト</param>
        /// <param name="itemMemos">項目メモリスト</param>
        /// <param name="descriptionLists">特殊指定選択肢文字列リスト</param>
        /// <param name="caseNumberLists">特殊指定選択肢数値リスト</param>
        /// <param name="initValues">初期値リスト</param>
        /// <exception cref="ArgumentException">ファイルフォーマットが不正の場合</exception>
        private void SetItemSetting(DBTypeSetting setting,
            IReadOnlyList<DBItemSpecialSettingType> specialSettingTypes, IReadOnlyList<ItemName> itemNames,
            IReadOnlyList<ItemMemo> itemMemos,
            IReadOnlyList<IReadOnlyList<DatabaseValueCaseDescription>> descriptionLists,
            IReadOnlyList<List<DatabaseValueCaseNumber>> caseNumberLists, IReadOnlyList<DBValueInt> initValues)
        {
            // 項目名、項目メモ、文字列パラメータ、数値パラメータ、初期値の長さが一致する必要がある
            var itemNamesCount = itemNames.Count;
            var itemMemosCount = itemMemos.Count;
            var descriptionListsCount = descriptionLists.Count;
            var caseNumberListsCount = caseNumberLists.Count;
            var initValuesCount = initValues.Count;

            if (itemNamesCount != itemMemosCount
                || itemNamesCount != descriptionListsCount
                || itemNamesCount != caseNumberListsCount
                || itemNamesCount != initValuesCount)
                throw new ArgumentException(
                    "項目名、項目メモ、文字列パラメータ、数値パラメータ、初期値の要素数が一致しません。（" +
                    $"項目名数：{itemNamesCount}, 項目メモ数：{itemMemosCount}," +
                    $"文字列パラメータ数：{descriptionListsCount}, 数値パラメータ数：{caseNumberListsCount}" +
                    $"初期値数：{initValuesCount}）");

            // 特殊指定数が項目数より少ない場合は不正
            var specialSettingTypesCount = specialSettingTypes.Count;

            if (specialSettingTypesCount < itemNamesCount)
                throw new ArgumentException(
                    $"特殊指定種別の要素数が不正です。（要素数：{specialSettingTypesCount}）");

            var itemSettings = new List<DBItemSetting>();
            for (var i = 0; i < itemNamesCount; i++)
            {
                var specialSettingDesc = new DBItemSpecialSettingDesc
                {
                    ItemMemo = itemMemos[i],
                    InitValue = initValues[i]
                };

                var thisDescriptions = descriptionLists[i];
                var thisCaseNumbers = caseNumberLists[i];

                var thisItemSettingType = specialSettingTypes[i];

                try
                {
                    var caseList = MakeValueCases(thisItemSettingType, thisCaseNumbers, thisDescriptions);

                    specialSettingDesc.ChangeValueType(thisItemSettingType, caseList);

                    if (thisItemSettingType == DBItemSpecialSettingType.ReferDatabase)
                    {
                        specialSettingDesc.DatabaseReferKind = DBReferType.FromCode(thisCaseNumbers[0]);
                        specialSettingDesc.DatabaseDbTypeId = new TypeId(thisCaseNumbers[1]);
                        specialSettingDesc.DatabaseUseAdditionalItemsFlag = thisCaseNumbers[2] == 1;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(
                        $"項目{i,2}の特殊指定タイプと特殊指定パラメータが一致しません。" +
                        "詳細はInnerExceptionを確認してください。", ex);
                }

                var thisItemName = itemNames[i];

                var itemSetting = new DBItemSetting
                {
                    ItemName = thisItemName,
                    SpecialSettingDesc = specialSettingDesc
                };

                itemSettings.Add(itemSetting);
            }

            setting.ItemSettingList.AddRange(itemSettings);
        }

        /// <summary>
        /// 選択肢一覧を生成する。
        /// </summary>
        /// <param name="type">特殊指定タイプ</param>
        /// <param name="numbers">選択肢番号リスト</param>
        /// <param name="descriptions">選択肢文字列リスト</param>
        /// <returns>選択肢リスト</returns>
        /// <exception cref="ArgumentException">選択肢番号リストまたは文字列リストが不正の場合</exception>
        private IReadOnlyList<DatabaseValueCase> MakeValueCases(DBItemSpecialSettingType type,
            IReadOnlyList<DatabaseValueCaseNumber> numbers, IReadOnlyList<DatabaseValueCaseDescription> descriptions)
        {
            if (type == DBItemSpecialSettingType.Normal)
            {
                return MakeValueCasesNormal(numbers, descriptions);
            }

            if (type == DBItemSpecialSettingType.LoadFile)
            {
                return MakeValueCasesLoadFile(numbers, descriptions);
            }

            if (type == DBItemSpecialSettingType.ReferDatabase)
            {
                return MakeValueCasesReferDatabase(descriptions);
            }

            if (type == DBItemSpecialSettingType.Manual)
            {
                return MakeValueCasesManual(numbers, descriptions);
            }

            // 通常ここには来ない
            throw new InvalidOperationException(
                "定義されていない特殊指定タイプです。");
        }

        private IReadOnlyList<DatabaseValueCase> MakeValueCasesNormal(
            IReadOnlyList<DatabaseValueCaseNumber> numbers, IReadOnlyList<DatabaseValueCaseDescription> descriptions)
        {
            var type = DBItemSpecialSettingType.Normal;

            if (numbers.Count > 0)
            {
                Logger.Warning($"特殊設定タイプ：{type}：指定されている数値パラメータが無視されます。");
                for (var i = 0; i < numbers.Count; i++)
                {
                    Logger.Warning($"  数値パラメータ{i,2}：{numbers[i]}");
                }
            }

            if (descriptions.Count > 0)
            {
                Logger.Warning($"特殊設定タイプ：{type}：指定されている文字列パラメータが無視されます。");
                for (var i = 0; i < descriptions.Count; i++)
                {
                    Logger.Warning($"  文字列パラメータ{i,2}：{descriptions[i]}");
                }
            }

            return new List<DatabaseValueCase>();
        }

        private IReadOnlyList<DatabaseValueCase> MakeValueCasesLoadFile(
            IReadOnlyList<DatabaseValueCaseNumber> numbers, IReadOnlyList<DatabaseValueCaseDescription> descriptions)
        {
            var type = DBItemSpecialSettingType.LoadFile;

            if (numbers.Count < 1)
                throw new ArgumentException(
                    $"特殊設定タイプ：{type}： 数値パラメータが不足しています。（パラメータ数：{numbers.Count}）");
            if (descriptions.Count < 1)
                throw new ArgumentException(
                    $"特殊設定タイプ：{type}： 文字列パラメータが不足しています。（パラメータ数：{descriptions.Count}）");

            if (numbers.Count > 1)
            {
                Logger.Warning($"特殊設定タイプ：{type}：指定されている数値パラメータが無視されます。");
                for (var i = 1; i < numbers.Count; i++)
                {
                    Logger.Warning($"  数値パラメータ{i,2}：{numbers[i]}");
                }
            }

            if (descriptions.Count > 1)
            {
                Logger.Warning($"特殊設定タイプ：{type}：指定されている文字列パラメータが無視されます。");
                for (var i = 1; i < descriptions.Count; i++)
                {
                    Logger.Warning($"  文字列パラメータ{i,2}：{descriptions[i]}");
                }
            }

            return new List<DatabaseValueCase>
            {
                new DatabaseValueCase(numbers[0], descriptions[0])
            };
        }

        private IReadOnlyList<DatabaseValueCase> MakeValueCasesReferDatabase(
            IReadOnlyList<DatabaseValueCaseDescription> descriptions)
        {
            var type = DBItemSpecialSettingType.ReferDatabase;

            if (descriptions.Count == 0) return new List<DatabaseValueCase>();

            if (descriptions.Count < 3)
                throw new ArgumentException(
                    $"特殊設定タイプ：{type}： 文字列パラメータが不足しています。（パラメータ数：{descriptions.Count}）");


            if (descriptions.Count > 3)
            {
                Logger.Warning($"特殊設定タイプ：{type}：指定されている文字列パラメータが無視されます。");
                for (var i = 3; i < descriptions.Count; i++)
                {
                    Logger.Warning($"  文字列パラメータ{i,2}：{descriptions[i]}");
                }
            }

            return descriptions.Select((t, i) => new DatabaseValueCase(-1 * (i + 1), t))
                .ToList();
        }

        private IReadOnlyList<DatabaseValueCase> MakeValueCasesManual(
            IReadOnlyList<DatabaseValueCaseNumber> numbers, IReadOnlyList<DatabaseValueCaseDescription> descriptions)
        {
            var type = DBItemSpecialSettingType.Manual;

            // 選択肢番号数と文字列数が一致しない場合は不正
            if (numbers.Count != descriptions.Count)
                throw new ArgumentException(
                    $"特殊設定タイプ：{type}： 文字列パラメータ数と数値パラメータ数が一致しません。" +
                    $"（文字列パラメータ数：{descriptions.Count}、数値パラメータ数：{numbers.Count}）");

            return descriptions.Select((t, i) => new DatabaseValueCase(numbers[i], t))
                .ToList();
        }
    }
}