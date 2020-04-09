using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBItemValuesListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void ConstructorTestA()
        {
            DBItemValuesList instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBItemValuesList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が1件であること
            Assert.AreEqual(instance.Count, 1);
        }

        [TestCase(-1, false, 0, false, true)]
        [TestCase(0, false, 0, false, true)]
        [TestCase(1, false, 0, false, false)]
        [TestCase(1, true, 0, false, true)]
        [TestCase(1, false, 1, false, false)]
        [TestCase(1, false, 1, true, true)]
        [TestCase(1, true, 100, false, true)]
        [TestCase(1, true, 100, true, true)]
        [TestCase(1, true, 101, false, true)]
        [TestCase(1, true, 101, true, true)]
        [TestCase(10000, false, 0, false, false)]
        [TestCase(10000, true, 0, false, true)]
        [TestCase(10000, false, 1, false, false)]
        [TestCase(10000, false, 1, true, true)]
        [TestCase(10000, true, 100, false, true)]
        [TestCase(10000, true, 100, true, true)]
        [TestCase(10000, true, 101, false, true)]
        [TestCase(10000, true, 101, true, true)]
        [TestCase(10001, false, 0, false, true)]
        [TestCase(10001, true, 0, false, true)]
        [TestCase(10001, false, 1, false, true)]
        [TestCase(10001, false, 1, true, true)]
        [TestCase(10001, true, 100, false, true)]
        [TestCase(10001, true, 100, true, true)]
        [TestCase(10001, true, 101, false, true)]
        [TestCase(10001, true, 101, true, true)]
        public static void ConstructorTestB(int initDataLength, bool hasNullItemInData,
            int initFieldLength, bool hasNullItemInField, bool isError)
        {
            var initItemList = MakeInitList(initDataLength, hasNullItemInData,
                initFieldLength, hasNullItemInField);
            DBItemValuesList instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBItemValuesList(initItemList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // データ数が意図した数であること
            Assert.AreEqual(instance.Count, initDataLength);

            // すべてのデータの項目数が意図した値であること
            foreach (var values in instance)
            {
                Assert.AreEqual(values.Count, initFieldLength);
            }
        }

        [TestCase("isi,isi,isi", false)]
        [TestCase("isi,iss", true)]
        [TestCase("ii,ii,iis", true)]
        [TestCase("iis,iis,ii", true)]
        public static void ConstructorTestC(string setTypeCode, bool isError)
        {
            // setTypeCode を List<List<DBItemValue>> に変換。
            // ',' が行の切れ目、 'i', 's' はそれぞれintValue, stringValueに変換する。

            var items = setTypeCode.Split(',')
                .Select(s => s.Select(c =>
                {
                    if (c.Equals('i')) return (DBItemValue) new DBValueInt(0);
                    if (c.Equals('s')) return (DBItemValue) new DBValueString("");
                    throw new InvalidOperationException();
                }).ToList()).ToList();

            var errorOccured = false;
            try
            {
                var _ = new DBItemValuesList(items);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new DBItemValuesList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DBItemValuesList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new DBItemValuesList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DBItemValuesList.MinCapacity);
        }

        [TestCase(-1, false)]
        [TestCase(1, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void AddNewValuesTest(int initDataLength, bool isError)
        {
            var initList = MakeInitList(initDataLength, false, 0, false);

            // initList がnullかどうかでコンストラクタを分ける
            var instance = initList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddNewValues();
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
                // データ数が一致すること
                var beforeLength = initDataLength == -1 ? 1 : initDataLength;
                Assert.AreEqual(instance.Count, beforeLength + 1);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 2);
                Assert.IsTrue(changedDataPropertyList[0].Equals(nameof(DBItemValuesList.Count)));
                Assert.IsTrue(changedDataPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedDataCollectionList.Count, 1);
                Assert.IsTrue((changedDataCollectionList[0].Action == NotifyCollectionChangedAction.Add));
            }
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, false)]
        [TestCase(-1, 9999, false)]
        [TestCase(-1, 10000, true)]
        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 9999, false)]
        [TestCase(1, 10000, true)]
        [TestCase(9999, -1, true)]
        [TestCase(9999, 0, false)]
        [TestCase(9999, 1, false)]
        [TestCase(9999, 2, true)]
        [TestCase(10000, -1, true)]
        [TestCase(10000, 0, false)]
        [TestCase(10000, 1, true)]
        public static void AddNewValuesRangeTest(int initDataLength, int count, bool isError)
        {
            var initList = MakeInitList(initDataLength, false, 0, false);

            var instance = initList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddNewValuesRange(count);
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
                // データ数が一致すること
                var beforeLength = initDataLength == -1 ? 1 : initDataLength;
                Assert.AreEqual(instance.Count, beforeLength + count);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 2);
                Assert.IsTrue(changedDataPropertyList[0].Equals(nameof(DBItemValuesList.Count)));
                Assert.IsTrue(changedDataPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedDataCollectionList.Count, 1);
                Assert.IsTrue((changedDataCollectionList[0].Action == NotifyCollectionChangedAction.Add));
            }
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, false)]
        [TestCase(-1, 1, false)]
        [TestCase(-1, 2, true)]
        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, false)]
        [TestCase(1, 2, true)]
        [TestCase(9999, -1, true)]
        [TestCase(9999, 0, false)]
        [TestCase(9999, 9999, false)]
        [TestCase(9999, 100000, true)]
        [TestCase(10000, -1, true)]
        [TestCase(10000, 0, true)]
        [TestCase(10000, 10000, true)]
        public static void InsertNewValuesTest(int initDataLength, int index, bool isError)
        {
            var initList = MakeInitList(initDataLength, false, 0, false);

            // initList がnullかどうかでコンストラクタを分ける
            var instance = initList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertNewValues(index);
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
                // データ数が一致すること
                var beforeLength = initDataLength == -1 ? 1 : initDataLength;
                Assert.AreEqual(instance.Count, beforeLength + 1);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 2);
                Assert.IsTrue(changedDataPropertyList[0].Equals(nameof(DBItemValuesList.Count)));
                Assert.IsTrue(changedDataPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedDataCollectionList.Count, 1);
                Assert.IsTrue((changedDataCollectionList[0].Action == NotifyCollectionChangedAction.Add));
            }
        }

        [TestCase(-1, -1, -1, true)]
        [TestCase(-1, -1, 0, true)]
        [TestCase(-1, -1, 9999, true)]
        [TestCase(-1, -1, 10000, true)]
        [TestCase(-1, 0, -1, true)]
        [TestCase(-1, 0, 0, false)]
        [TestCase(-1, 0, 9999, false)]
        [TestCase(-1, 0, 10000, true)]
        [TestCase(-1, 1, -1, true)]
        [TestCase(-1, 1, 0, false)]
        [TestCase(-1, 1, 9999, false)]
        [TestCase(-1, 1, 10000, true)]
        [TestCase(-1, 2, -1, true)]
        [TestCase(-1, 2, 0, true)]
        [TestCase(-1, 2, 9999, true)]
        [TestCase(-1, 2, 10000, true)]
        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 9999, true)]
        [TestCase(1, -1, 10000, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 9999, false)]
        [TestCase(1, 0, 10000, true)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, false)]
        [TestCase(1, 1, 9999, false)]
        [TestCase(1, 1, 10000, true)]
        [TestCase(1, 2, -1, true)]
        [TestCase(1, 2, 0, true)]
        [TestCase(1, 2, 9999, true)]
        [TestCase(1, 2, 10000, true)]
        [TestCase(9999, -1, -1, true)]
        [TestCase(9999, -1, 0, true)]
        [TestCase(9999, -1, 9999, true)]
        [TestCase(9999, -1, 10000, true)]
        [TestCase(9999, 0, -1, true)]
        [TestCase(9999, 0, 0, false)]
        [TestCase(9999, 0, 1, false)]
        [TestCase(9999, 0, 2, true)]
        [TestCase(9999, 9999, -1, true)]
        [TestCase(9999, 9999, 0, false)]
        [TestCase(9999, 9999, 1, false)]
        [TestCase(9999, 9999, 2, true)]
        [TestCase(9999, 10000, -1, true)]
        [TestCase(9999, 10000, 0, true)]
        [TestCase(9999, 10000, 1, true)]
        [TestCase(10000, -1, -1, true)]
        [TestCase(10000, -1, 0, true)]
        [TestCase(10000, -1, 9999, true)]
        [TestCase(10000, -1, 10000, true)]
        [TestCase(10000, 0, -1, true)]
        [TestCase(10000, 0, 0, false)]
        [TestCase(10000, 0, 9999, true)]
        [TestCase(10000, 0, 10000, true)]
        [TestCase(10000, 9999, -1, true)]
        [TestCase(10000, 9999, 0, false)]
        [TestCase(10000, 9999, 1, true)]
        [TestCase(10000, 9999, 2, true)]
        [TestCase(10000, 10000, -1, true)]
        [TestCase(10000, 10000, 0, false)]
        [TestCase(10000, 10000, 1, true)]
        public static void InsertNewValuesRangeTest(int initDataLength, int index, int count, bool isError)
        {
            var initList = MakeInitList(initDataLength, false, 0, false);

            var instance = initList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertNewValuesRange(index, count);
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
                // データ数が一致すること
                var beforeLength = initDataLength == -1 ? 1 : initDataLength;
                Assert.AreEqual(instance.Count, beforeLength + count);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 2);
                Assert.IsTrue(changedDataPropertyList[0].Equals(nameof(DBItemValuesList.Count)));
                Assert.IsTrue(changedDataPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedDataCollectionList.Count, 1);
                Assert.IsTrue((changedDataCollectionList[0].Action == NotifyCollectionChangedAction.Add));
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("iii")]
        [TestCase("issi")]
        public static void CreateValuesInstanceTestA(string setTypeCode)
        {
            // ConstructorTestC 同様に setTypeCode -> List<List<DBItemValue>> に変換。
            // ただしデータ数は1

            var items = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return (DBItemValue) new DBValueInt(0);
                if (c.Equals('s')) return (DBItemValue) new DBValueString("");
                throw new InvalidOperationException();
            }).ToList();

            // setTypeCode がnullかどうかでコンストラクタを分ける
            //   データ数が0の場合にメソッドを呼び出したときのテストを行うため
            var instance = items == null
                ? new DBItemValuesList()
                : new DBItemValuesList(new List<IReadOnlyList<DBItemValue>> {items});
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };

            IFixedLengthDBItemValueList madeInstance = null;

            var errorOccured = false;
            try
            {
                madeInstance = instance.CreateValueListInstance();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 基準データが存在しない場合、取得した項目数が0であること
            if (items == null)
            {
                Assert.AreEqual(madeInstance.Count, 0);
                // 順序チェックはできないので終了
                return;
            }

            // 値リストインスタンス中の値種別が意図した順序になっていること。
            // また、各値にデフォルト値が格納されていること
            Assert.AreEqual(madeInstance.Count, items.Count);
            for (var i = 0; i < madeInstance.Count; i++)
            {
                Assert.AreEqual(madeInstance[i].Type, items[i].Type);
                if (madeInstance[i].Type == DBItemType.Int)
                {
                    Assert.AreEqual(madeInstance[i], (DBItemValue) (DBValueInt) 0);
                }
                else if (madeInstance[i].Type == DBItemType.String)
                {
                    Assert.AreEqual(madeInstance[i], (DBItemValue) (DBValueString) "");
                }
                else
                {
                    // 来ないはず
                    Assert.Fail();
                }
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDataPropertyList.Count, 0);
            Assert.AreEqual(changedDataCollectionList.Count, 0);
        }

        [TestCase(null, null, true)]
        [TestCase(null, "issi", true)]
        [TestCase("", null, true)]
        [TestCase("", "iii", true)]
        [TestCase("iii", null, true)]
        [TestCase("iii", "iii", false)]
        [TestCase("issi", "issi", false)]
        [TestCase("iii", "ii", true)]
        [TestCase("iii", "iis", true)]
        [TestCase("ss", "sss", true)]
        public static void CreateValuesInstanceTestB(string setTypeCode, string itemValuesCode,
            bool isError)
        {
            // ConstructorTestC 同様に setTypeCode -> List<List<DBItemValue>> に変換。
            // ただしデータ数は 0 or 1

            var items = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return (DBItemValue) new DBValueInt(0);
                if (c.Equals('s')) return (DBItemValue) new DBValueString("");
                throw new InvalidOperationException();
            }).ToList();

            // setTypeCode がnullかどうかでコンストラクタを分ける
            //   データ数が0の場合にメソッドを呼び出したときのテストを行うため
            var instance = items == null
                ? new DBItemValuesList()
                : new DBItemValuesList(new List<IReadOnlyList<DBItemValue>> {items});
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };

            // itemValuesCodeも同様に変換する。
            // ただし格納する値はデフォルト値ではなくindexに絡めた値

            var initValues = itemValuesCode?.Select((c, index) =>
            {
                if (c.Equals('i')) return (DBItemValue) new DBValueInt(index);
                if (c.Equals('s')) return (DBItemValue) new DBValueString(index.ToString());
                throw new InvalidOperationException();
            }).ToList();

            IFixedLengthDBItemValueList madeInstance = null;

            var errorOccured = false;
            try
            {
                madeInstance = instance.CreateValueListInstance(initValues);
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
                // この時点でinitValuesはnullではないはずだが、念のため
                Assert.NotNull(initValues);

                // 基準データが存在しない場合
                if (items == null)
                {
                    // 取得した項目数がセットした項目数と同じであること
                    Assert.AreEqual(madeInstance.Count, initValues.Count);

                    // 値リストインスタンス中の値種別が意図した順序になっていること。
                    // また、各値に意図した値が格納されていること
                    for (var i = 0; i < madeInstance.Count; i++)
                    {
                        Assert.AreEqual(madeInstance[i].Type, initValues[i].Type);
                        if (madeInstance[i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(madeInstance[i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (madeInstance[i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(madeInstance[i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }

                    return;
                }

                // 基準データが存在する場合

                // 取得した項目数が基準項目数と同じであること
                Assert.AreEqual(madeInstance.Count, items.Count);

                // 値リストインスタンス中の値種別が意図した順序になっていること。
                // また、各値に意図した値が格納されていること
                for (var i = 0; i < madeInstance.Count; i++)
                {
                    Assert.AreEqual(madeInstance[i].Type, items[i].Type);
                    if (madeInstance[i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(madeInstance[i], (DBItemValue) (DBValueInt) i);
                    }
                    else if (madeInstance[i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(madeInstance[i], (DBItemValue) (DBValueString) i.ToString());
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }
                }
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDataPropertyList.Count, 0);
            Assert.AreEqual(changedDataCollectionList.Count, 0);
        }


        private static readonly object[] SetFieldTestACaseSource =
        {
            new object[] {0, -1, DBItemType.Int, true},
            new object[] {0, 0, DBItemType.Int, true},
            new object[] {0, 1, DBItemType.Int, true},
            new object[] {0, -1, DBItemType.String, true},
            new object[] {0, 0, DBItemType.String, true},
            new object[] {0, 1, DBItemType.String, true},
            new object[] {0, -1, null, true},
            new object[] {0, 0, null, true},
            new object[] {0, 1, null, true},
            new object[] {100, -1, DBItemType.Int, true},
            new object[] {100, 0, DBItemType.Int, false},
            new object[] {100, 99, DBItemType.Int, false},
            new object[] {100, 100, DBItemType.Int, true},
            new object[] {100, -1, DBItemType.String, true},
            new object[] {100, 0, DBItemType.String, false},
            new object[] {100, 99, DBItemType.String, false},
            new object[] {100, 100, DBItemType.String, true},
            new object[] {100, -1, null, true},
            new object[] {100, 0, null, true},
            new object[] {100, 99, null, true},
            new object[] {100, 100, null, true},
        };

        [TestCaseSource(nameof(SetFieldTestACaseSource))]
        public static void SetFieldTestA(int initLength, int index, DBItemType type, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.SetField(index, type);
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
                // データ数が変化していないこと
                Assert.AreEqual(instance[0].Count, initLength);

                // 元のデータ（更新箇所より前）が変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                // 更新したデータが反映されていること
                Assert.AreEqual(instance[0][i].Type, type);
                if (instance[0][i].Type == DBItemType.Int)
                {
                    // 元の値種別がIntだったため、値が変化していないこと
                    Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                }
                else if (instance[0][i].Type == DBItemType.String)
                {
                    // 元の値種別がIntだったため、デフォルト値で初期化されていること
                    Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) "");
                }
                else
                {
                    // 来ないはず
                    Assert.Fail();
                }

                i++;

                // 元のデータ（更新箇所より後）が変化していないこと
                if (initItemList != null)
                {
                    for (; i < initLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 1);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Replace);
            }
        }

        private static readonly object[] SetFieldBTestCaseSource =
        {
            new object[] {0, -1, (DBItemValue) (DBValueInt) 4000, true},
            new object[] {0, 0, (DBItemValue) (DBValueInt) 4000, true},
            new object[] {0, 1, (DBItemValue) (DBValueInt) 4000, true},
            new object[] {0, -1, (DBItemValue) (DBValueString) "test", true},
            new object[] {0, 0, (DBItemValue) (DBValueString) "test", true},
            new object[] {0, 1, (DBItemValue) (DBValueString) "test", true},
            new object[] {0, -1, null, true},
            new object[] {0, 0, null, true},
            new object[] {0, 1, null, true},
            new object[] {100, -1, (DBItemValue) (DBValueInt) 4000, true},
            new object[] {100, 0, (DBItemValue) (DBValueInt) 4000, false},
            new object[] {100, 99, (DBItemValue) (DBValueInt) 4000, false},
            new object[] {100, 100, (DBItemValue) (DBValueInt) 4000, true},
            new object[] {100, -1, (DBItemValue) (DBValueString) "test", true},
            new object[] {100, 0, (DBItemValue) (DBValueString) "test", false},
            new object[] {100, 99, (DBItemValue) (DBValueString) "test", false},
            new object[] {100, 100, (DBItemValue) (DBValueString) "test", true},
            new object[] {100, -1, null, true},
            new object[] {100, 0, null, true},
            new object[] {100, 99, null, true},
            new object[] {100, 100, null, true},
        };

        [TestCaseSource(nameof(SetFieldBTestCaseSource))]
        public static void SetFieldBTest(int initLength, int index, DBItemValue value, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.SetField(index, value);
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
                // データ数が変化していないこと
                Assert.AreEqual(instance[0].Count, initLength);

                // 元のデータ（更新箇所より前）が変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                // 更新したデータが反映されていること
                Assert.AreEqual(instance[0][i].Type, value.Type);
                Assert.AreEqual(instance[0][i], value);

                // 元のデータ（更新箇所より後）が変化していないこと
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 1);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Replace);
            }
        }

        private static readonly object[] AddFieldTestACaseSource =
        {
            new object[] {-1, DBItemType.Int, false},
            new object[] {-1, DBItemType.String, false},
            new object[] {-1, null, true},
            new object[] {0, DBItemType.Int, false},
            new object[] {0, DBItemType.String, false},
            new object[] {0, null, true},
            new object[] {99, DBItemType.Int, false},
            new object[] {99, DBItemType.String, false},
            new object[] {99, null, true},
            new object[] {100, DBItemType.Int, true},
            new object[] {100, DBItemType.String, true},
            new object[] {100, null, true},
        };

        [TestCaseSource(nameof(AddFieldTestACaseSource))]
        public static void AddFieldTestA(int initLength, DBItemType type, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddField(type);
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
                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + 1;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータが変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < beforeLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                // 追加したデータが反映されていること
                Assert.AreEqual(instance[0][i].Type, type);
                if (instance[0][i].Type == DBItemType.Int)
                {
                    Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) 0);
                }
                else if (instance[0][i].Type == DBItemType.String)
                {
                    Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) "");
                }
                else
                {
                    // 来ないはず
                    Assert.Fail();
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] AddFieldBTestCaseSource =
        {
            new object[] {-1, (DBItemValue) (DBValueInt) 3, false},
            new object[] {-1, (DBItemValue) (DBValueString) "test", false},
            new object[] {-1, null, true},
            new object[] {0, (DBItemValue) (DBValueInt) 3, false},
            new object[] {0, (DBItemValue) (DBValueString) "test", false},
            new object[] {0, null, true},
            new object[] {99, (DBItemValue) (DBValueInt) 3, false},
            new object[] {99, (DBItemValue) (DBValueString) "test", false},
            new object[] {99, null, true},
            new object[] {100, (DBItemValue) (DBValueInt) 3, true},
            new object[] {100, (DBItemValue) (DBValueString) "test", true},
            new object[] {100, null, true},
        };

        [TestCaseSource(nameof(AddFieldBTestCaseSource))]
        public static void AddFieldBTest(int initLength, DBItemValue value, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddField(value);
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
                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + 1;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータが変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < beforeLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                // 追加したデータが反映されていること
                Assert.AreEqual(instance[0][i].Type, value.Type);
                Assert.AreEqual(instance[0][i], value);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] AddFieldRangeTestCaseSource =
        {
            new object[] {-1, "", false},
            new object[] {-1, "is", false},
            new object[] {-1, null, true},
            new object[] {0, "", false},
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                false
            },
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                true
            },
            new object[] {0, null, true},
            new object[] {99, "", false},
            new object[] {99, "i", false},
            new object[] {99, "ii", true},
            new object[] {99, null, true},
            new object[] {100, "", false},
            new object[] {100, "i", true},
            new object[] {100, null, true},
        };

        [TestCaseSource(nameof(AddFieldRangeTestCaseSource))]
        public static void AddFieldRangeTestA(int initLength, string setTypeCode, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var types = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return DBItemType.Int;
                if (c.Equals('s')) return DBItemType.String;
                throw new InvalidOperationException();
            }).ToList();

            var errorOccured = false;
            try
            {
                instance.AddFieldRange(types);
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
                // typesはnullではないはずだが念の為
                Assert.NotNull(types);

                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + types.Count;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータが変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < beforeLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                for (; i < resultLength; i++)
                {
                    // 追加したデータが反映されていること
                    Assert.AreEqual(instance[0][i].Type, types[i - beforeLength]);
                    if (instance[0][i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) 0);
                    }
                    else if (instance[0][i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) "");
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        [TestCaseSource(nameof(AddFieldRangeTestCaseSource))]
        public static void AddFieldRangeBTest(int initLength, string setTypeCode, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var values = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return (DBItemValue) (DBValueInt) 10;
                if (c.Equals('s')) return (DBItemValue) (DBValueString) "init";
                throw new InvalidOperationException();
            }).ToList();

            var errorOccured = false;
            try
            {
                instance.AddFieldRange(values);
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
                // valuesはnullではないはずだが念の為
                Assert.NotNull(values);

                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + values.Count;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータが変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < beforeLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                for (; i < resultLength; i++)
                {
                    // 追加したデータが反映されていること
                    Assert.AreEqual(instance[0][i].Type, values[i - beforeLength].Type);
                    Assert.AreEqual(instance[0][i], values[i - beforeLength]);
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] InsertFieldTestACaseSource =
        {
            new object[] {-1, DBItemType.Int, -1, true},
            new object[] {-1, DBItemType.Int, 0, false},
            new object[] {-1, DBItemType.Int, 1, true},
            new object[] {-1, DBItemType.String, -1, true},
            new object[] {-1, DBItemType.String, 0, false},
            new object[] {-1, DBItemType.String, 1, true},
            new object[] {-1, null, -1, true},
            new object[] {-1, null, 0, true},
            new object[] {-1, null, 1, true},
            new object[] {0, DBItemType.Int, -1, true},
            new object[] {0, DBItemType.Int, 0, false},
            new object[] {0, DBItemType.Int, 1, true},
            new object[] {0, DBItemType.String, -1, true},
            new object[] {0, DBItemType.String, 0, false},
            new object[] {0, DBItemType.String, 1, true},
            new object[] {0, null, -1, true},
            new object[] {0, null, 0, true},
            new object[] {0, null, 1, true},
            new object[] {99, DBItemType.Int, -1, true},
            new object[] {99, DBItemType.Int, 0, false},
            new object[] {99, DBItemType.Int, 99, false},
            new object[] {99, DBItemType.Int, 100, true},
            new object[] {99, DBItemType.String, -1, true},
            new object[] {99, DBItemType.String, 0, false},
            new object[] {99, DBItemType.String, 99, false},
            new object[] {99, DBItemType.String, 100, true},
            new object[] {99, null, -1, true},
            new object[] {99, null, 0, true},
            new object[] {99, null, 99, true},
            new object[] {99, null, 100, true},
            new object[] {100, DBItemType.Int, -1, true},
            new object[] {100, DBItemType.Int, 0, true},
            new object[] {100, DBItemType.Int, 100, true},
            new object[] {100, DBItemType.Int, 101, true},
            new object[] {100, DBItemType.String, -1, true},
            new object[] {100, DBItemType.String, 0, true},
            new object[] {100, DBItemType.String, 100, true},
            new object[] {100, DBItemType.String, 101, true},
            new object[] {100, null, -1, true},
            new object[] {100, null, 0, true},
            new object[] {100, null, 100, true},
            new object[] {100, null, 101, true},
        };

        [TestCaseSource(nameof(InsertFieldTestACaseSource))]
        public static void InsertFieldTestA(int initLength, DBItemType type,
            int index, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertField(index, type);
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
                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + 1;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータ（挿入箇所より前）が変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                // 挿入したデータが反映されていること
                Assert.AreEqual(instance[0][i].Type, type);
                if (instance[0][i].Type == DBItemType.Int)
                {
                    Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) 0);
                }
                else if (instance[0][i].Type == DBItemType.String)
                {
                    Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) "");
                }
                else
                {
                    // 来ないはず
                    Assert.Fail();
                }

                i++;

                // 元のデータ（挿入箇所より後）が変化していないこと
                if (initItemList != null)
                {
                    for (; i < resultLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i - 1].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) (i - 1));
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) (i - 1).ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] InsertFieldTestBCaseSource =
        {
            new object[] {-1, (DBItemValue) (DBValueInt) 3, -1, true},
            new object[] {-1, (DBItemValue) (DBValueInt) 3, 0, false},
            new object[] {-1, (DBItemValue) (DBValueInt) 3, 1, true},
            new object[] {-1, (DBItemValue) (DBValueString) "test", -1, true},
            new object[] {-1, (DBItemValue) (DBValueString) "test", 0, false},
            new object[] {-1, (DBItemValue) (DBValueString) "test", 1, true},
            new object[] {-1, null, -1, true},
            new object[] {-1, null, 0, true},
            new object[] {-1, null, 1, true},
            new object[] {0, (DBItemValue) (DBValueInt) 3, -1, true},
            new object[] {0, (DBItemValue) (DBValueInt) 3, 0, false},
            new object[] {0, (DBItemValue) (DBValueInt) 3, 1, true},
            new object[] {0, (DBItemValue) (DBValueString) "test", -1, true},
            new object[] {0, (DBItemValue) (DBValueString) "test", 0, false},
            new object[] {0, (DBItemValue) (DBValueString) "test", 1, true},
            new object[] {0, null, -1, true},
            new object[] {0, null, 0, true},
            new object[] {0, null, 1, true},
            new object[] {99, (DBItemValue) (DBValueInt) 3, -1, true},
            new object[] {99, (DBItemValue) (DBValueInt) 3, 0, false},
            new object[] {99, (DBItemValue) (DBValueInt) 3, 99, false},
            new object[] {99, (DBItemValue) (DBValueInt) 3, 100, true},
            new object[] {99, (DBItemValue) (DBValueString) "test", -1, true},
            new object[] {99, (DBItemValue) (DBValueString) "test", 0, false},
            new object[] {99, (DBItemValue) (DBValueString) "test", 99, false},
            new object[] {99, (DBItemValue) (DBValueString) "test", 100, true},
            new object[] {99, null, -1, true},
            new object[] {99, null, 0, true},
            new object[] {99, null, 99, true},
            new object[] {99, null, 100, true},
            new object[] {100, (DBItemValue) (DBValueInt) 3, -1, true},
            new object[] {100, (DBItemValue) (DBValueInt) 3, 0, true},
            new object[] {100, (DBItemValue) (DBValueInt) 3, 100, true},
            new object[] {100, (DBItemValue) (DBValueInt) 3, 101, true},
            new object[] {100, (DBItemValue) (DBValueString) "test", -1, true},
            new object[] {100, (DBItemValue) (DBValueString) "test", 0, true},
            new object[] {100, (DBItemValue) (DBValueString) "test", 100, true},
            new object[] {100, (DBItemValue) (DBValueString) "test", 101, true},
            new object[] {100, null, -1, true},
            new object[] {100, null, 0, true},
            new object[] {100, null, 100, true},
            new object[] {100, null, 101, true},
        };

        [TestCaseSource(nameof(InsertFieldTestBCaseSource))]
        public static void InsertFieldTestB(int initLength, DBItemValue value,
            int index, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertField(index, value);
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
                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + 1;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータ（挿入箇所より前）が変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                // 挿入したデータが反映されていること
                Assert.AreEqual(instance[0][i].Type, value.Type);
                Assert.AreEqual(instance[0][i], value);

                i++;

                // 元のデータ（挿入箇所より後）が変化していないこと
                if (initItemList != null)
                {
                    for (; i < resultLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i - 1].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) (i - 1));
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) (i - 1).ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] InsertFieldRangeTestCaseSource =
        {
            new object[] {-1, "", -1, true},
            new object[] {-1, "", 0, false},
            new object[] {-1, "", 1, true},
            new object[]
            {
                -1,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                -1, true
            },
            new object[]
            {
                -1,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                0, false
            },
            new object[]
            {
                -1,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                1, true
            },
            new object[]
            {
                -1,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                -1, true
            },
            new object[]
            {
                -1,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                0, true
            },
            new object[]
            {
                -1,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                1, true
            },
            new object[] {-1, null, -1, true},
            new object[] {-1, null, 0, true},
            new object[] {-1, null, 1, true},
            new object[] {0, "", -1, true},
            new object[] {0, "", 0, false},
            new object[] {0, "", 1, true},
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                -1, true
            },
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                0, false
            },
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssss",
                1, true
            },
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                -1, true
            },
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                0, true
            },
            new object[]
            {
                0,
                "iiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssiiiiisssssi",
                1, true
            },
            new object[] {0, null, -1, true},
            new object[] {0, null, 0, true},
            new object[] {0, null, 1, true},
            new object[] {99, "", -1, true},
            new object[] {99, "", 0, false},
            new object[] {99, "", 99, false},
            new object[] {99, "", 100, true},
            new object[] {99, "i", -1, true},
            new object[] {99, "i", 0, false},
            new object[] {99, "i", 99, false},
            new object[] {99, "i", 100, true},
            new object[] {99, "ii", -1, true},
            new object[] {99, "ii", 0, true},
            new object[] {99, "ii", 99, true},
            new object[] {99, "ii", 100, true},
            new object[] {99, null, -1, true},
            new object[] {99, null, 0, true},
            new object[] {99, null, 99, true},
            new object[] {99, null, 100, true},
            new object[] {100, "", -1, true},
            new object[] {100, "", 0, false},
            new object[] {100, "", 100, false},
            new object[] {100, "", 101, true},
            new object[] {100, "i", -1, true},
            new object[] {100, "i", 0, true},
            new object[] {100, "i", 100, true},
            new object[] {100, "i", 101, true},
            new object[] {100, null, -1, true},
            new object[] {100, null, 0, true},
            new object[] {100, null, 100, true},
            new object[] {100, null, 101, true},
        };

        [TestCaseSource(nameof(InsertFieldRangeTestCaseSource))]
        public static void InsertFieldRangeTestA(int initLength, string setTypeCode,
            int index, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var types = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return DBItemType.Int;
                if (c.Equals('s')) return DBItemType.String;
                throw new InvalidOperationException();
            }).ToList();

            var errorOccured = false;
            try
            {
                instance.InsertFieldRange(index, types);
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
                // typesはnullではないはずだが念の為
                Assert.NotNull(types);

                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + types.Count;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータ（挿入箇所より前）が変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                var typesIndex = 0;
                for (; i < index + types.Count; i++)
                {
                    // 追加したデータが反映されていること
                    Assert.AreEqual(instance[0][i].Type, types[typesIndex]);
                    if (instance[0][i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) 0);
                    }
                    else if (instance[0][i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) "");
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }

                    typesIndex++;
                }

                // 元のデータ（挿入箇所より後）が変化していないこと
                var offset = types.Count;
                if (initItemList != null)
                {
                    for (; i < resultLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i - offset].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) (i - offset));
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) (i - offset).ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        [TestCaseSource(nameof(InsertFieldRangeTestCaseSource))]
        public static void InsertFieldRangeTestB(int initLength, string setTypeCode,
            int index, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var values = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return (DBItemValue) (DBValueInt) 9;
                if (c.Equals('s')) return (DBItemValue) (DBValueString) "value";
                throw new InvalidOperationException();
            }).ToList();

            var errorOccured = false;
            try
            {
                instance.InsertFieldRange(index, values);
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
                // typesはnullではないはずだが念の為
                Assert.NotNull(values);

                // データ数が意図した数であること
                var beforeLength = initLength == -1 ? 0 : initLength;
                var resultLength = beforeLength + values.Count;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータ（挿入箇所より前）が変化していないこと
                var i = 0;
                if (initItemList != null)
                {
                    for (; i < index; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }

                var typesIndex = 0;
                for (; i < index + values.Count; i++)
                {
                    // 追加したデータが反映されていること
                    Assert.AreEqual(instance[0][i].Type, values[typesIndex].Type);
                    Assert.AreEqual(instance[0][i], values[typesIndex]);

                    typesIndex++;
                }

                // 元のデータ（挿入箇所より後）が変化していないこと
                var offset = values.Count;
                if (initItemList != null)
                {
                    for (; i < resultLength; i++)
                    {
                        Assert.AreEqual(instance[0][i].Type, initItemList[0][i - offset].Type);
                        if (instance[0][i].Type == DBItemType.Int)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) (i - offset));
                        }
                        else if (instance[0][i].Type == DBItemType.String)
                        {
                            Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) (i - offset).ToString());
                        }
                        else
                        {
                            // 来ないはず
                            Assert.Fail();
                        }
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] RemoveFieldAtTestCaseSource =
        {
            new object[] {-1, -1, true},
            new object[] {-1, 0, true},
            new object[] {-1, 1, true},
            new object[] {0, -1, true},
            new object[] {0, 0, true},
            new object[] {0, 1, true},
            new object[] {1, -1, true},
            new object[] {1, 0, false},
            new object[] {1, 1, true},
            new object[] {100, -1, true},
            new object[] {100, 0, false},
            new object[] {100, 99, false},
            new object[] {100, 100, true},
        };

        [TestCaseSource(nameof(RemoveFieldAtTestCaseSource))]
        public static void RemoveFieldAtTest(int initLength, int index, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.RemoveFieldAt(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!(errorOccured || initItemList == null))
            {
                // データ数が意図した数であること
                var beforeLength = initLength;
                var resultLength = beforeLength - 1;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータ（除去箇所より前）が変化していないこと
                var i = 0;
                for (; i < index; i++)
                {
                    Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                    if (instance[0][i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                    }
                    else if (instance[0][i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }
                }

                // 元のデータ（除去箇所より後）が変化していないこと
                var offset = 1;
                for (; i < resultLength; i++)
                {
                    Assert.AreEqual(instance[0][i].Type, initItemList[0][i + offset].Type);
                    if (instance[0][i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) (i + offset));
                    }
                    else if (instance[0][i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) (i + offset).ToString());
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured || initItemList == null)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Remove);
            }
        }

        private static readonly object[] RemoveFieldRangeTestCaseSource =
        {
            new object[] {-1, -1, -1, true},
            new object[] {-1, -1, 0, true},
            new object[] {-1, -1, 1, true},
            new object[] {-1, 0, -1, true},
            new object[] {-1, 0, 0, true},
            new object[] {-1, 0, 1, true},
            new object[] {-1, 1, -1, true},
            new object[] {-1, 1, 0, true},
            new object[] {-1, 1, 1, true},
            new object[] {0, -1, -1, true},
            new object[] {0, -1, 0, true},
            new object[] {0, -1, 1, true},
            new object[] {0, 0, -1, true},
            new object[] {0, 0, 0, true},
            new object[] {0, 0, 1, true},
            new object[] {0, 1, -1, true},
            new object[] {0, 1, 0, true},
            new object[] {0, 1, 1, true},
            new object[] {100, -1, -1, true},
            new object[] {100, -1, 0, true},
            new object[] {100, -1, 100, true},
            new object[] {100, -1, 101, true},
            new object[] {100, 0, -1, true},
            new object[] {100, 0, 0, false},
            new object[] {100, 0, 100, false},
            new object[] {100, 0, 101, true},
            new object[] {100, 1, -1, true},
            new object[] {100, 1, 0, false},
            new object[] {100, 1, 99, false},
            new object[] {100, 1, 100, true},
            new object[] {100, 99, -1, true},
            new object[] {100, 99, 0, false},
            new object[] {100, 99, 1, false},
            new object[] {100, 99, 2, true},
            new object[] {100, 100, -1, true},
            new object[] {100, 100, 0, true},
            new object[] {100, 100, 1, true},
        };

        [TestCaseSource(nameof(RemoveFieldRangeTestCaseSource))]
        public static void RemoveFieldRangeTest(int initLength, int index,
            int count, bool isError)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.RemoveFieldRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!(errorOccured || initItemList == null))
            {
                // データ数が意図した数であること
                var beforeLength = initLength;
                var resultLength = beforeLength - count;
                Assert.AreEqual(instance[0].Count, resultLength);

                // 元のデータ（除去箇所より前）が変化していないこと
                var i = 0;
                for (; i < index; i++)
                {
                    Assert.AreEqual(instance[0][i].Type, initItemList[0][i].Type);
                    if (instance[0][i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) i);
                    }
                    else if (instance[0][i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) i.ToString());
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }
                }

                // 元のデータ（除去箇所より後）が変化していないこと
                var offset = count;
                for (; i < resultLength; i++)
                {
                    Assert.AreEqual(instance[0][i].Type, initItemList[0][i + offset].Type);
                    if (instance[0][i].Type == DBItemType.Int)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueInt) (i + offset));
                    }
                    else if (instance[0][i].Type == DBItemType.String)
                    {
                        Assert.AreEqual(instance[0][i], (DBItemValue) (DBValueString) (i + offset).ToString());
                    }
                    else
                    {
                        // 来ないはず
                        Assert.Fail();
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured || initItemList == null)
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 0);
                Assert.AreEqual(changedFieldCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDataPropertyList.Count, 0);
                Assert.AreEqual(changedDataCollectionList.Count, 0);
                Assert.AreEqual(changedFieldPropertyList.Count, 2);
                Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
                Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionList.Count, 1);
                Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Remove);
            }
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public static void ClearFieldTest(int initLength)
        {
            var initItemList = MakeInitList2(initLength);
            var instance = initItemList == null
                ? new DBItemValuesList()
                : new DBItemValuesList(initItemList);
            var changedDataPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDataPropertyList.Add(args.PropertyName); };
            var changedDataCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedDataCollectionList.Add(args); };
            var changedFieldPropertyList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => { changedFieldPropertyList.Add(args.PropertyName); };
            var changedFieldCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => { changedFieldCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.ClearField();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 項目数が0であること
            Assert.AreEqual(instance[0].Count, 0);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedDataPropertyList.Count, 0);
            Assert.AreEqual(changedDataCollectionList.Count, 0);
            Assert.AreEqual(changedFieldPropertyList.Count, 2);
            Assert.IsTrue(changedFieldPropertyList[0].Equals(nameof(DBItemValueList.Count)));
            Assert.IsTrue(changedFieldPropertyList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedFieldCollectionList.Count, 1);
            Assert.IsTrue(changedFieldCollectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //        データ整合性チェック
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBItemValuesListインスタンスに対してフィールド操作すると
        /// インスタンスが生成したすべてのValuesインスタンスに反映されることの確認
        /// </summary>
        [Test]
        public static void ChildFieldsReflectionTest()
        {
            var instance = new DBItemValuesList();

            // 引数無しでValuesインスタンスが正常に生成されること
            var child1 = instance.CreateValueListInstance();

            // 項目数が0であること
            Assert.AreEqual(child1.Count, 0);

            var child2 = instance.CreateValueListInstance();

            instance.Add(child2);

            // DBValuesListインスタンスに対して項目操作
            instance.AddField(DBItemType.Int);

            // 生成した2つのValuesインスタンスに反映されていること
            Assert.AreEqual(child1.Count, 1);
            Assert.AreEqual(child1[0].Type, DBItemType.Int);
            Assert.AreEqual(child2.Count, 1);
            Assert.AreEqual(child2[0].Type, DBItemType.Int);

            logger.Info("Test A Check.");

            // 項目操作2
            instance.AddFieldRange(new List<DBItemType> {DBItemType.Int, DBItemType.String});

            // 2つのValuesインスタンスに反映されていること
            Assert.AreEqual(child1.Count, 3);
            Assert.AreEqual(child1[1].Type, DBItemType.Int);
            Assert.AreEqual(child1[2].Type, DBItemType.String);
            Assert.AreEqual(child2.Count, 3);
            Assert.AreEqual(child2[1].Type, DBItemType.Int);
            Assert.AreEqual(child2[2].Type, DBItemType.String);

            logger.Info("Test B Check.");

            // 新規Valuesインスタンス生成 - 正常に生成されること
            var child3 = instance.CreateValueListInstance();

            // child3 の項目が正しいこと
            Assert.AreEqual(child3.Count, 3);
            Assert.AreEqual(child3[0].Type, DBItemType.Int);
            Assert.AreEqual(child3[1].Type, DBItemType.Int);
            Assert.AreEqual(child3[2].Type, DBItemType.String);

            // 引数ありで新規Valuesインスタンス生成 - 正常に生成されること
            var _ = instance.CreateValueListInstance(new DBItemValue[]
                {(DBValueInt) 0, (DBValueInt) 10, (DBValueString) "test"});
        }

        /// <summary>
        /// PropertyChanged, CollectionChanged, FieldPropertyChanged, FieldPropertyChanged イベントのテスト
        /// </summary>
        /// <remarks>
        /// DBItemValuesList のインスタンスに対し 各種操作メソッドを実行したうえで
        /// Field変更が正しく検知できることをテストする。
        /// </remarks>
        [Test]
        public static void ChangedEventTest()
        {
            var instance = new DBItemValuesList();
            var changedPropertyNameList = new List<string>();
            instance.PropertyChanged += (sender, args) => changedPropertyNameList.Add(args.PropertyName);
            var changedCollectionArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => changedCollectionArgsList.Add(args);
            var changedFieldPropertyNameList = new List<string>();
            instance.FieldPropertyChanged += (sender, args) => changedFieldPropertyNameList.Add(args.PropertyName);
            var changedFieldCollectionArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.FieldCollectionChanged += (sender, args) => changedFieldCollectionArgsList.Add(args);

            var clearNotifications = new Action(() =>
            {
                changedPropertyNameList.Clear();
                changedCollectionArgsList.Clear();
                changedFieldPropertyNameList.Clear();
                changedFieldCollectionArgsList.Clear();
            });

            var checkChangeFieldState = new Action(() =>
            {
                instance.AddField(DBItemType.Int);

                // 各変更通知が意図したとおり発火していること
                Assert.AreEqual(changedPropertyNameList.Count, 0);
                Assert.AreEqual(changedCollectionArgsList.Count, 0);
                Assert.AreEqual(changedFieldPropertyNameList.Count, 2);
                Assert.IsTrue(changedFieldPropertyNameList[0].Equals(nameof(instance.Count)));
                Assert.IsTrue(changedFieldPropertyNameList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedFieldCollectionArgsList.Count, 1);
                Assert.IsTrue(changedFieldCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
            });

            {
                // Before
                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Add
                {
                    // Execute
                    instance.Add(instance.CreateValueListInstance());

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // AddRange
                {
                    // Execute
                    instance.AddRange(new[] {instance.CreateValueListInstance(), instance.CreateValueListInstance()});

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Insert Index 1
                {
                    // Execute
                    instance.Insert(1, instance.CreateValueListInstance());

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Insert Index 0
                {
                    // Execute
                    instance.Insert(0, instance.CreateValueListInstance());

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // InsertRange Index 1
                {
                    // Execute
                    instance.InsertRange(1,
                        new[]
                        {
                            instance.CreateValueListInstance(), instance.CreateValueListInstance(),
                            instance.CreateValueListInstance(), instance.CreateValueListInstance()
                        });

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // InsertRange Index 0
                {
                    // Execute
                    instance.InsertRange(0,
                        new[]
                        {
                            instance.CreateValueListInstance(), instance.CreateValueListInstance(),
                            instance.CreateValueListInstance(), instance.CreateValueListInstance()
                        });

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Add));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Move 1 to 2
                {
                    // Execute
                    instance.Move(1, 2);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 1);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Move));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Move 0 to 2
                {
                    // Execute
                    instance.Move(0, 2);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 1);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Move));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Move 2 to 0
                {
                    // Execute
                    instance.Move(0, 2);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 1);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Move));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Move 0 to 0
                {
                    // Execute
                    instance.Move(0, 0);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 1);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Move));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Remove 1
                {
                    // Execute
                    instance.RemoveAt(1);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Remove));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Remove 0
                {
                    // Execute
                    instance.RemoveAt(0);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Remove));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // RemoveRange 1 of 2
                {
                    // Execute
                    instance.RemoveRange(1, 2);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Remove));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // RemoveRange 0 of 2
                {
                    // Execute
                    instance.RemoveRange(0, 2);

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Remove));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Set 1
                {
                    // Execute
                    instance[1] = instance.CreateValueListInstance();

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 1);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Replace));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Set 0
                {
                    // Execute
                    instance[0] = instance.CreateValueListInstance();

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 1);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Replace));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }

            {
                // Clear
                {
                    // Execute
                    instance.Clear();

                    // 各変更通知が意図したとおり発火していること
                    Assert.AreEqual(changedPropertyNameList.Count, 2);
                    Assert.IsTrue(changedPropertyNameList[0].Equals(nameof(instance.Count)));
                    Assert.IsTrue(changedPropertyNameList[1].Equals(ListConstant.IndexerName));
                    Assert.AreEqual(changedCollectionArgsList.Count, 1);
                    Assert.IsTrue(changedCollectionArgsList[0].Action.Equals(NotifyCollectionChangedAction.Reset));
                    Assert.AreEqual(changedFieldPropertyNameList.Count, 0);
                    Assert.AreEqual(changedFieldCollectionArgsList.Count, 0);
                }
                clearNotifications.Invoke();

                // Change Field state
                checkChangeFieldState();
                clearNotifications.Invoke();
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DBItemValuesList();
            target.AdjustLength(3);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }

        private static IReadOnlyList<IReadOnlyList<DBItemValue>> MakeInitList(int dataLength, bool hasNullInData,
            int fieldLength, bool hasNullInField)
        {
            if (dataLength == -1) return null;

            var result = new List<List<DBItemValue>>();
            for (var i = 0; i < dataLength; i++)
            {
                if (hasNullInData && i == dataLength / 2)
                {
                    result.Add(null);
                    continue;
                }

                var subList = new List<DBItemValue>();
                for (var j = 0; j < fieldLength; j++)
                {
                    subList.Add(hasNullInField && i == fieldLength / 2
                        ? null
                        : new DBItemValue(0)
                    );
                }

                result.Add(subList);
            }

            return result;
        }

        private static IReadOnlyList<IReadOnlyList<DBItemValue>> MakeInitList2(int length)
        {
            if (length == -1) return null;

            var result = new List<List<DBItemValue>>();

            var subList = new List<DBItemValue>();
            for (var i = 0; i < length; i++)
            {
                subList.Add(new DBItemValue(i));
            }

            result.Add(subList);

            return result;
        }
    }
}