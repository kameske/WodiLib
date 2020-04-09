using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBTypeTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            DBTypeFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] TypeNameTestCaseSource =
        {
            new object[] {(TypeName) "TypeName", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TypeNameTestCaseSource))]
        public static void TypeNameTest(TypeName typeName, bool isError)
        {
            var instance = new DBType();
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBType.TypeName)));
            }
        }

        private static readonly object[] MemoTestCaseSource =
        {
            new object[] {(DatabaseMemo) "Memo", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(MemoTestCaseSource))]
        public static void MemoTest(DatabaseMemo memo, bool isError)
        {
            var instance = new DBType();
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBType.Memo)));
            }
        }

        [Test]
        public static void DataSettingTypeGetterTest()
        {
            var instance = new DBType();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DataSettingType;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] DBKindGetterTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, true},
            new object[] {DBDataSettingType.EqualBefore, true},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.FirstStringData, true},
        };

        [TestCaseSource(nameof(DBKindGetterTestCaseSource))]
        public static void DBKindGetterTest(DBDataSettingType type, bool isError)
        {
            var instance = new DBType();
            instance.SetDataSettingType(type, DBKind.User, 10);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DBKind;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] TypeIdGetterTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, true},
            new object[] {DBDataSettingType.EqualBefore, true},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.FirstStringData, true},
        };

        [TestCaseSource(nameof(TypeIdGetterTestCaseSource))]
        public static void TypeIdGetterTest(DBDataSettingType type, bool isError)
        {
            var instance = new DBType();
            instance.SetDataSettingType(type, DBKind.User, 10);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.TypeId;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ItemDescListGetterTest()
        {
            var instance = new DBType();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.ItemDescList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DataDescListGetterTest()
        {
            var instance = new DBType();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DataDescList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] ConstructorATestCaseSource =
        {
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
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, (TypeId) 3, false},
            new object[] {DBDataSettingType.FirstStringData, null, (TypeId) 3, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, null, false},
            new object[] {DBDataSettingType.FirstStringData, null, null, false},
        };

        [TestCaseSource(nameof(ConstructorATestCaseSource))]
        public static void ConstructorATest(DBDataSettingType type, DBKind dbKind, TypeId? typeId,
            bool isError)
        {
            DBType instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBType(type, dbKind, typeId);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 各プロパティがセットした値と一致すること
            Assert.AreEqual(instance.DataSettingType, type);
            if (type == DBDataSettingType.DesignatedType)
            {
                Assert.NotNull(dbKind);
                Assert.AreEqual(instance.DBKind, dbKind);
                Assert.NotNull(typeId);
                Assert.AreEqual(instance.TypeId, typeId.Value);
            }
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, 100, true)]
        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 100, false)]
        [TestCase(10000, -1, true)]
        [TestCase(10000, 0, false)]
        [TestCase(10000, 100, false)]
        public static void ConstructorBTest(int dataDescListLength, int itemDescListLength,
            bool isError)
        {
            var dataDescList = CreateDataDescList(dataDescListLength, itemDescListLength);
            var itemDescList = CreateItemDescList(itemDescListLength);

            DBType instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBType(dataDescList, itemDescList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // データ情報がセットした情報と一致すること
            var answerDataDescListCount = dataDescListLength == 0
                ? 1
                : dataDescListLength;
            Assert.AreEqual(instance.DataDescList.Count, answerDataDescListCount);

            Assert.AreEqual(instance.ItemDescList.Count, itemDescListLength);

            if (dataDescListLength <= 0) return;

            for (var i = 0; i < instance.DataDescList.Count; i++)
            {
                var valueList = instance.DataDescList[i].ItemValueList;
                Assert.AreEqual(valueList.Count, itemDescListLength);
                for (var j = 0; j < itemDescListLength; j++)
                {
                    Assert.AreEqual(valueList[j], (DBItemValue) CreateDBValue(i, j));
                }
            }
        }

        private static readonly object[] SetDataSettingTypeTestCaseSource =
        {
            new object[] {null, null, null, true},
            new object[] {DBDataSettingType.Manual, null, null, false},
            new object[] {DBDataSettingType.EqualBefore, null, null, false},
            new object[] {DBDataSettingType.DesignatedType, DBKind.System, (TypeId) 10, false},
            new object[] {DBDataSettingType.FirstStringData, null, null, false},
        };

        [TestCaseSource(nameof(SetDataSettingTypeTestCaseSource))]
        public static void SetDataSettingTypeTestA(DBDataSettingType settingType, DBKind dbKind,
            TypeId? typeId, bool isError)
        {
            var instance = new DBType();
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

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 3);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBType.DataSettingType)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(DBType.DBKind)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(DBType.TypeId)));
            }
        }

        [TestCaseSource(nameof(SetDataSettingTypeTestCaseSource))]
        public static void SetDataSettingTypeTestB(DBDataSettingType settingType, DBKind dbKind,
            TypeId? typeId, bool isError)
        {
            var instance = new DBType();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var setting = settingType == null ? null : new DBDataSetting(settingType, dbKind, typeId);

            var errorOccured = false;
            try
            {
                instance.SetDataSettingType(setting);
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
                // 各設定が意図した値と一致すること
                Assert.AreEqual(instance.DataSettingType, settingType);
                if (settingType == DBDataSettingType.DesignatedType)
                {
                    Assert.AreEqual(instance.DBKind, dbKind);
                    Assert.AreEqual(instance.TypeId, typeId);
                }
            }


            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 3);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBType.DataSettingType)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(DBType.DBKind)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(DBType.TypeId)));
            }
        }

        private static readonly object[] ToBinaryTestCaseSource =
        {
            new object[]
            {
                $@"{DBTypeFileTestItemGenerator.TestWorkRootDir}\タイプ(データ含む)_000_UDB0.dbtype",
                DBTypeFileTestItemGenerator.GenerateUDB0DBType(),
                822
            },
            new object[]
            {
                $@"{DBTypeFileTestItemGenerator.TestWorkRootDir}\タイプ(データ含む)_000_あいうえお.dbtype",
                DBTypeFileTestItemGenerator.GenerateCDB0DBType(),
                702
            },
        };

        [TestCaseSource(nameof(ToBinaryTestCaseSource))]
        public static void ToBinaryTest(string testFilePath, DBType generatedData, int fileSize)
        {
            var generatedDataBuf = generatedData.ToBinary();

            using (var fs = new FileStream(testFilePath, FileMode.Open))
            {
                var length = (int) fs.Length;
                // ファイルサイズが規定でない場合誤作動防止の為テスト失敗にする
                Assert.AreEqual(length, fileSize);


                var fileData = new byte[length];
                fs.Read(fileData, 0, length);

                // binデータ出力
                fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\"").ToList()
                    .ForEach(Console.WriteLine);

                Console.WriteLine();

                generatedDataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\"").ToList()
                    .ForEach(Console.WriteLine);

                for (var i = 0; i < generatedDataBuf.Length; i++)
                {
                    if (i == fileData.Length)
                        Assert.Fail(
                            $"データ帳が異なります。（期待値：{fileData.Length}, 実際：{generatedDataBuf.Length}）");

                    if (fileData[i] != generatedDataBuf[i])
                        Assert.Fail(
                            $"offset: {i} のバイナリが異なります。（期待値：{fileData[i]}, 実際：{generatedDataBuf[i]}）");
                }

                if (fileData.Length != generatedDataBuf.Length)
                    Assert.Fail(
                        $"データ帳が異なります。（期待値：{fileData.Length}, 実際：{generatedDataBuf.Length}）");
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DBType
            {
                Memo = "Memo"
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBTypeFileTestItemGenerator.DeleteFile();
        }


        private static DatabaseDataDescList CreateDataDescList(int length, int itemLength)
        {
            if (length == -1) return null;

            var result = new List<DatabaseDataDesc>();
            for (var i = 0; i < length; i++)
            {
                var itemValueList = new DBItemValueList();
                for (var j = 0; j < itemLength; j++)
                {
                    itemValueList.Add(CreateDBValue(i, j));
                }

                result.Add(new DatabaseDataDesc(CreateDataName(i), itemValueList));
            }

            return new DatabaseDataDescList(result);
        }

        private static DatabaseItemDescList CreateItemDescList(int length)
        {
            if (length == -1) return null;

            var result = new List<DatabaseItemDesc>();
            for (var i = 0; i < length; i++)
            {
                result.Add(new DatabaseItemDesc
                {
                    ItemName = CreateItemName(i),
                    ItemType = DBItemType.Int
                });
            }

            return new DatabaseItemDescList(result);
        }

        private static DataName CreateDataName(int dataId) => $"data{dataId}";

        private static DBValueInt CreateDBValue(int dataId, int itemId) => dataId * 100 + itemId;

        private static ItemName CreateItemName(int itemId) => $"item{itemId}";
    }
}