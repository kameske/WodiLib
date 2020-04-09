using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseTypeDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] TypeNameTestCaseSource =
        {
            new object[] {(TypeName) "type", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TypeNameTestCaseSource))]
        public static void TypeNameTest(TypeName typeName, bool isError)
        {
            var instance = new DatabaseTypeDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.TypeName = typeName;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.TypeName;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(typeName));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseTypeDesc.TypeName)));
            }
        }

        private static readonly object[] MemoTestCaseSource =
        {
            new object[] {(DatabaseMemo) "memo", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(MemoTestCaseSource))]
        public static void MemoTest(DatabaseMemo memo, bool isError)
        {
            var instance = new DatabaseTypeDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Memo = memo;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.Memo;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(memo));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseTypeDesc.Memo)));
            }
        }

        private static readonly object[] DataSettingTypeGetterTestCaseSource =
        {
            new object[] {DBDataSettingType.FirstStringData},
            new object[] {DBDataSettingType.Manual},
            new object[] {DBDataSettingType.DesignatedType},
            new object[] {DBDataSettingType.EqualBefore},
        };

        [TestCaseSource(nameof(DataSettingTypeGetterTestCaseSource))]
        public static void DataSettingTypeGetterTest(DBDataSettingType type)
        {
            var typeSetting = new DBTypeSetting();
            var dataSetting = new DBDataSetting(type, DBKind.User, 0);
            var instance = new DatabaseTypeDesc(typeSetting, dataSetting);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBDataSettingType result = null;
            var errorOccured = false;
            try
            {
                result = instance.DataSettingType;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した結果が意図した値であること
            Assert.AreEqual(result, type);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] DBKindGetterTestCaseSource =
        {
            new object[] {DBDataSettingType.FirstStringData, true},
            new object[] {DBDataSettingType.Manual, true},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.EqualBefore, true},
        };

        [TestCaseSource(nameof(DBKindGetterTestCaseSource))]
        public static void DBKindGetterTest(DBDataSettingType type, bool isError)
        {
            var answer = DBKind.User;

            var typeSetting = new DBTypeSetting();
            var dataSetting = new DBDataSetting(type, answer, 0);
            var instance = new DatabaseTypeDesc(typeSetting, dataSetting);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBKind result = null;
            var errorOccured = false;
            try
            {
                result = instance.DBKind;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // 取得した結果が意図した値であること
                Assert.AreEqual(result, answer);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] TypeIdGetterTestCaseSource =
        {
            new object[] {DBDataSettingType.FirstStringData, true},
            new object[] {DBDataSettingType.Manual, true},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.EqualBefore, true},
        };

        [TestCaseSource(nameof(TypeIdGetterTestCaseSource))]
        public static void TypeIdGetterTest(DBDataSettingType type, bool isError)
        {
            var answer = (TypeId) 4;

            var typeSetting = new DBTypeSetting();
            var dataSetting = new DBDataSetting(type, DBKind.User, answer);
            var instance = new DatabaseTypeDesc(typeSetting, dataSetting);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            TypeId result = 0;
            var errorOccured = false;
            try
            {
                result = instance.TypeId;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 取得した結果が意図した値であること
            Assert.AreEqual(result, answer);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ItemDescListGetterTest()
        {
            const int dataLength = 2;
            const int itemLength = 3;
            var typeSetting = CreateTypeSetting(dataLength, itemLength);
            var dataSetting = CreateDataSetting(dataLength, itemLength);

            DatabaseItemDescList result = null;

            var instance = new DatabaseTypeDesc(typeSetting, dataSetting);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                result = instance.ItemDescList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数が項目数と一致すること
            Assert.AreEqual(result.Count, itemLength);

            // 各要素が意図した値と一致すること
            for (var i = 0; i < itemLength; i++)
            {
                Assert.AreEqual(result[i].ItemName, MakeItemName(i));
                Assert.AreEqual(result[i].ItemType, MakeItemType());
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DataDescListGetterTest()
        {
            const int dataLength = 2;
            const int itemLength = 3;

            DatabaseDataDescList result = null;

            var instance = CreateTypeDesc(dataLength, itemLength);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                result = instance.DataDescList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数がデータ数と一致すること
            Assert.AreEqual(result.Count, dataLength);

            // 各要素が意図した値と一致すること
            for (var i = 0; i < dataLength; i++)
            {
                Assert.AreEqual(result[i].DataName, MakeDataName(i));
                Assert.AreEqual(result[i].ItemValueList.Count, itemLength);
                for (var j = 0; j < itemLength; j++)
                {
                    Assert.AreEqual(result[i].ItemValueList[j], (DBItemValue) MakeItemValue(i, j));
                }
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DataNameListGetterTest()
        {
            const int dataLength = 2;
            const int itemLength = 3;

            IReadOnlyDataNameList result = null;

            var instance = CreateTypeDesc(dataLength, itemLength);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                result = instance.DataNameList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数がデータ数と一致すること
            Assert.AreEqual(result.Count, dataLength);

            // 各要素が意図した値と一致すること
            for (var i = 0; i < dataLength; i++)
            {
                Assert.AreEqual(result[i], MakeDataName(i));
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DBItemSettingListGetterTest()
        {
            const int dataLength = 2;
            const int itemLength = 3;

            IReadOnlyDBItemSettingList result = null;

            var instance = CreateTypeDesc(dataLength, itemLength);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                result = instance.DBItemSettingList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数が項目数と一致すること
            Assert.AreEqual(result.Count, itemLength);

            // 各要素が意図した値と一致すること
            for (var i = 0; i < itemLength; i++)
            {
                Assert.AreEqual(result[i].ItemName, MakeItemName(i));
                Assert.AreEqual(result[i].ItemType, MakeItemType());
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ConstructorTest()
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseTypeDesc();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] SetDataSettingTypeTestCaseSource =
        {
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, (TypeId) 3, false},
            new object[] {DBDataSettingType.FirstStringData, null, (TypeId) 3, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, null, false},
            new object[] {DBDataSettingType.FirstStringData, null, null, false},
            new object[] {DBDataSettingType.Manual, DBKind.User, (TypeId) 3, false},
            new object[] {DBDataSettingType.Manual, null, (TypeId) 3, false},
            new object[] {DBDataSettingType.Manual, DBKind.User, null, false},
            new object[] {DBDataSettingType.Manual, null, null, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.User, (TypeId) 3, false},
            new object[] {DBDataSettingType.EqualBefore, null, (TypeId) 3, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.User, null, false},
            new object[] {DBDataSettingType.EqualBefore, null, null, false},
            new object[] {DBDataSettingType.DesignatedType, DBKind.User, (TypeId) 3, false},
            new object[] {DBDataSettingType.DesignatedType, null, (TypeId) 3, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.User, null, true},
            new object[] {DBDataSettingType.DesignatedType, null, null, true},
            new object[] {null, DBKind.User, (TypeId) 3, true},
            new object[] {null, null, (TypeId) 3, true},
            new object[] {null, DBKind.User, null, true},
            new object[] {null, null, null, true},
        };

        [TestCaseSource(nameof(SetDataSettingTypeTestCaseSource))]
        public static void SetDataSettingTypeTest(DBDataSettingType settingType, DBKind dbKind,
            TypeId? typeId, bool isError)
        {
            var instance = CreateTypeDesc(1, 0);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetDataSettingType(settingType, dbKind, typeId);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // 各プロパティが意図した値と一致すること
                Assert.AreEqual(instance.DataSettingType, settingType);

                if (settingType != DBDataSettingType.DesignatedType) return;
                Assert.NotNull(typeId);

                Assert.AreEqual(instance.DBKind, dbKind);
                Assert.AreEqual(instance.TypeId, typeId.Value);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 3);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseTypeDesc.DataSettingType)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(DatabaseTypeDesc.DBKind)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(DatabaseTypeDesc.TypeId)));
            }
        }

        [Test]
        public static void GenerateDBTypeSetTest()
        {
            const int dataLength = 2;
            const int itemLength = 3;
            const string typeName = "タイプ名";
            const string memo = "メモ";
            var instance = CreateTypeDesc(dataLength, itemLength);
            instance.TypeName = typeName;
            instance.Memo = memo;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBTypeSet result = null;

            var errorOccured = false;
            try
            {
                result = instance.GenerateDBTypeSet();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得したインスタンスの各要素が元の値と一致すること
            Assert.AreEqual(result.TypeName.ToString(), typeName);
            Assert.AreEqual(result.Memo.ToString(), memo);
            Assert.AreEqual(result.ItemSettingList.Count, itemLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void GenerateDBTypeTest()
        {
            const int dataLength = 2;
            const int itemLength = 3;
            const string typeName = "タイプ名";
            const string memo = "メモ";
            var instance = CreateTypeDesc(dataLength, itemLength);
            instance.TypeName = typeName;
            instance.Memo = memo;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBType result = null;

            var errorOccured = false;
            try
            {
                result = instance.GenerateDBType();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得したインスタンスの各要素が元の値と一致すること
            Assert.AreEqual(result.TypeName.ToString(), typeName);
            Assert.AreEqual(result.Memo.ToString(), memo);
            Assert.AreEqual(result.DataDescList.Count, dataLength);
            Assert.AreEqual(result.ItemDescList.Count, itemLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DatabaseTypeDesc
            {
                TypeName = "TypeName",
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static DatabaseTypeDesc CreateTypeDesc(int dataLength, int itemLength)
        {
            var typeSetting = CreateTypeSetting(dataLength, itemLength);
            var dataSetting = CreateDataSetting(dataLength, itemLength);
            return new DatabaseTypeDesc(typeSetting, dataSetting);
        }

        private static DBTypeSetting CreateTypeSetting(int dataLength, int itemLength)
        {
            var typeSetting = new DBTypeSetting
            {
                TypeName = MakeTypeName(),
                Memo = MakeMemo(),
            };
            typeSetting.DataNameList.AdjustLength(dataLength);
            for (var i = 0; i < typeSetting.DataNameList.Count; i++)
            {
                typeSetting.DataNameList[i] = MakeDataName(i);
            }

            typeSetting.ItemSettingList.AdjustLength(itemLength);
            for (var i = 0; i < typeSetting.ItemSettingList.Count; i++)
            {
                typeSetting.ItemSettingList[i].ItemName = MakeItemName(i);
                typeSetting.ItemSettingList[i].ItemType = MakeItemType();
            }

            return typeSetting;
        }

        private static DBDataSetting CreateDataSetting(int dataLength, int itemLength)
        {
            var dataSetting = new DBDataSetting(DBDataSettingType.Manual);
            for (var i = 0; i < itemLength; i++)
            {
                dataSetting.SettingValuesList.AddField(MakeItemType());
            }

            for (var i = 0; i < dataLength; i++)
            {
                dataSetting.SettingValuesList.AddNewValues();
                if (i == 0) dataSetting.SettingValuesList.RemoveAt(0);
                for (var j = 0; j < dataSetting.SettingValuesList[i].Count; j++)
                {
                    dataSetting.SettingValuesList[i][j] = MakeItemValue(i, j);
                }
            }

            return dataSetting;
        }

        private static TypeName MakeTypeName() => "TypeName";
        private static DatabaseMemo MakeMemo() => "Memo";
        private static DataName MakeDataName(int i) => $"DataName{i}";
        private static ItemName MakeItemName(int i) => $"ItemName{i}";
        private static DBItemType MakeItemType() => DBItemType.Int;
        private static DBValueInt MakeItemValue(int i, int j) => i * 100 + j;
    }
}