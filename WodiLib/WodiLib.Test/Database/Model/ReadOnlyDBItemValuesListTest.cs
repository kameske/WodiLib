using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class ReadOnlyDBItemValuesListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }


        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList());
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DBItemValuesList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList());
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DBItemValuesList.MinCapacity);
        }


        [TestCase(1, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void AddNewValuesTest(int initDataLength, bool isError)
        {
            var initList = MakeInitList(initDataLength, false, 0, false);
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(initList));

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

            if (errorOccured) return;

            // データ数が一致すること
            var beforeLength = initDataLength == -1 ? 1 : initDataLength;
            Assert.AreEqual(instance.Count, beforeLength + 1);
        }

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
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(initList));

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

            if (errorOccured) return;

            // データ数が一致すること
            var beforeLength = initDataLength == -1 ? 1 : initDataLength;
            Assert.AreEqual(instance.Count, beforeLength + count);
        }

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
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(initList));

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

            if (errorOccured) return;

            // データ数が一致すること
            var beforeLength = initDataLength == -1 ? 1 : initDataLength;
            Assert.AreEqual(instance.Count, beforeLength + 1);
        }

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
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(initList));

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

            if (errorOccured) return;

            // データ数が一致すること
            var beforeLength = initDataLength == -1 ? 1 : initDataLength;
            Assert.AreEqual(instance.Count, beforeLength + count);
        }

        [TestCase("")]
        [TestCase("iii")]
        [TestCase("issi")]
        public static void CreateValuesInstanceTestA(string setTypeCode)
        {
            // setTypeCode -> List<List<DBItemValue>> に変換。
            var items = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return (DBItemValue) new DBValueInt(0);
                if (c.Equals('s')) return (DBItemValue) new DBValueString("");
                throw new InvalidOperationException();
            }).ToList();

            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(
                new List<IReadOnlyList<DBItemValue>> {items}));

            IReadOnlyDBItemValueList madeInstance = null;

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
        }

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
            // setTypeCode -> List<List<DBItemValue>> に変換。
            // ただしデータ数は 0 or 1

            var items = setTypeCode?.Select(c =>
            {
                if (c.Equals('i')) return (DBItemValue) new DBValueInt(0);
                if (c.Equals('s')) return (DBItemValue) new DBValueString("");
                throw new InvalidOperationException();
            }).ToList();

            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(
                new List<IReadOnlyList<DBItemValue>> {items}));

            // itemValuesCodeも同様に変換する。
            // ただし格納する値はデフォルト値ではなくindexに絡めた値

            var initValues = itemValuesCode?.Select((c, index) =>
            {
                if (c.Equals('i')) return (DBItemValue) new DBValueInt(index);
                if (c.Equals('s')) return (DBItemValue) new DBValueString(index.ToString());
                throw new InvalidOperationException();
            }).ToList();

            IReadOnlyDBItemValueList madeInstance = null;

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

            if (errorOccured) return;

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

        [Test]
        public static void SetTest()
        {
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList());
            var valueList = instance.CreateValueListInstance();

            Assert.AreEqual(instance.Count, 1);

            var errorOccured = false;
            try
            {
                instance[0] = valueList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数が変化していないこと
            Assert.AreEqual(instance.Count, 1);
        }

        [Test]
        public static void AddTest()
        {
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
            {
                new List<DBItemValue>
                {
                    (DBValueInt) 0,
                    (DBValueInt) 1,
                }
            }));

            Assert.AreEqual(instance.Count, 1);

            var item0 = (DBValueInt) 30;
            var item1 = (DBValueInt) 128;
            var valueList = instance.CreateValueListInstance(new List<DBItemValue>
            {
                item0, item1
            });

            var errorOccured = false;
            try
            {
                instance.Add(valueList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数が増えていること
            Assert.AreEqual(instance.Count, 2);

            // 追加された要素が正しいこと
            Assert.AreEqual(instance[1][0], (DBItemValue) item0);
            Assert.AreEqual(instance[1][1], (DBItemValue) item1);
        }

        [Test]
        public static void RemoveTest()
        {
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
            {
                new List<DBItemValue>
                {
                    (DBValueInt) 0,
                    (DBValueInt) 1,
                },
                new List<DBItemValue>
                {
                    (DBValueInt) 3,
                    (DBValueInt) 4,
                }
            }));

            Assert.AreEqual(instance.Count, 2);

            var errorOccured = false;
            try
            {
                instance.RemoveAt(0);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数が減っていること
            Assert.AreEqual(instance.Count, 1);
        }

        [Test]
        public static void ClearTest()
        {
            var instance = new ReadOnlyDBItemValuesList(new DBItemValuesList(new List<IReadOnlyList<DBItemValue>>
            {
                new List<DBItemValue>
                {
                    (DBValueInt) 0,
                    (DBValueInt) 1,
                },
                new List<DBItemValue>
                {
                    (DBValueInt) 3,
                    (DBValueInt) 4,
                }
            }));

            Assert.AreEqual(instance.Count, 2);

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 要素数が初期化されていること
            Assert.AreEqual(instance.Count, instance.GetMinCapacity());
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
    }
}