using System;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseDataDescListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new DatabaseDataDescList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseDataDescList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new DatabaseDataDescList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DatabaseDataDescList.MinCapacity);
        }

        private static readonly object[] ConstructorTestCaseSource =
        {
            new object[]
            {
                new DataNameList(Enumerable.Range(0, 2).Select(_ => new DataName("Test Data"))),
                new DBItemValuesList(Enumerable.Range(0, 2).Select(_ => new[]
                {
                    new DBItemValue(10),
                    new DBItemValue(20),
                    new DBItemValue("value"),
                    new DBItemValue("string"),
                })),
                false
            },
            new object[]
            {
                new DataNameList(Enumerable.Range(0, 2).Select(_ => new DataName("Test Data"))),
                new DBItemValuesList(Enumerable.Range(0, 3).Select(_ => new[]
                {
                    new DBItemValue(10),
                    new DBItemValue(20),
                    new DBItemValue("value"),
                    new DBItemValue("string"),
                })),
                true
            },
            new object[]
            {
                null,
                new DBItemValuesList(Enumerable.Range(0, 2).Select(_ => new[]
                {
                    new DBItemValue(10),
                    new DBItemValue(20),
                    new DBItemValue("value"),
                    new DBItemValue("string"),
                })),
                true
            },
            new object[]
            {
                new DataNameList(Enumerable.Range(0, 2).Select(_ => new DataName("Test Data"))),
                null,
                true
            },
            new object[]
            {
                null, null, true
            },
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(DataNameList dataNameList, DBItemValuesList valuesList, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseDataDescList(dataNameList, valuesList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// 値型情報の異なるItemValueListを持つDatabaseDataDescインスタンスをセットするテスト
        /// </summary>
        [TestCase(1, false)]
        [TestCase(2, true)]
        public static void SetDifferenceItemValueTypeTest(int initValueListLength, bool isErrorSetDiffType)
        {
            var dataNameList =
                new DataNameList(Enumerable.Range(0, initValueListLength).Select(_ => new DataName("Test Data")));
            var valuesList = new DBItemValuesList(Enumerable.Range(0, initValueListLength).Select(_ => new[]
            {
                new DBItemValue(10),
                new DBItemValue(20),
                new DBItemValue("value"),
                new DBItemValue("string"),
            }));

            var instance = new DatabaseDataDescList(dataNameList, valuesList);

            {
                // テスト1：同じ型情報のDBItemValueList
                var testValueList = new DBItemValueList(new[]
                {
                    new DBItemValue(100),
                    new DBItemValue(2),
                    new DBItemValue("VAR"),
                    new DBItemValue("STR"),
                });
                var testDesc = new DatabaseDataDesc("Data 2", testValueList);
                var errorOccured = false;
                try
                {
                    instance[0] = testDesc;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            {
                // テスト2：異なる型情報のDBItemValueList
                var testValueList = new DBItemValueList(new[]
                {
                    new DBItemValue(100),
                    new DBItemValue(2),
                    new DBItemValue("VAR"),
                    new DBItemValue("STR"),
                    new DBItemValue("s"),
                });
                var testDesc = new DatabaseDataDesc("Data 2", testValueList);
                var errorOccured = false;
                try
                {
                    instance[0] = testDesc;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーフラグが一致すること
                Assert.AreEqual(errorOccured, isErrorSetDiffType);
            }
        }

        /// <summary>
        /// 値型情報の異なるItemValueListを持つDatabaseDataDescインスタンスを追加するテスト
        /// </summary>
        [Test]
        public static void AddDifferenceItemValueTypeTest()
        {
            var dataNameList =
                new DataNameList(Enumerable.Range(0, 1).Select(_ => new DataName("Test Data")));
            var valuesList = new DBItemValuesList(Enumerable.Range(0, 1).Select(_ => new[]
            {
                new DBItemValue(10),
                new DBItemValue(20),
                new DBItemValue("value"),
                new DBItemValue("string"),
            }));

            var instance = new DatabaseDataDescList(dataNameList, valuesList);

            {
                // テスト1：同じ型情報のDBItemValueList
                var testValueList = new DBItemValueList(new[]
                {
                    new DBItemValue(100),
                    new DBItemValue(2),
                    new DBItemValue("VAR"),
                    new DBItemValue("STR"),
                });
                var testDesc = new DatabaseDataDesc("Data 2", testValueList);
                var errorOccured = false;
                try
                {
                    instance.Add(testDesc);
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }

            {
                // テスト2：異なる型情報のDBItemValueList
                var testValueList = new DBItemValueList(new[]
                {
                    new DBItemValue(100),
                    new DBItemValue(2),
                    new DBItemValue("VAR"),
                    new DBItemValue("STR"),
                    new DBItemValue("s"),
                });
                var testDesc = new DatabaseDataDesc("Data 2", testValueList);
                var errorOccured = false;
                try
                {
                    instance[0] = testDesc;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生すること
                Assert.IsTrue(errorOccured);
            }
        }

        [Test]
        public static void CreateMatchItemInstanceTest()
        {
            var instance = new DatabaseDataDescList(new []
            {
                new DatabaseDataDesc("DataName", new []
                {
                    new DBItemValue(1),
                    new DBItemValue(2),
                    new DBItemValue("three"),
                    new DBItemValue("four"),
                    new DBItemValue(5),
                    new DBItemValue(6),
                }),
            });

            var answer = new DatabaseDataDesc("", new[]
            {
                DBItemType.Int.DBItemDefaultValue,
                DBItemType.Int.DBItemDefaultValue,
                DBItemType.String.DBItemDefaultValue,
                DBItemType.String.DBItemDefaultValue,
                DBItemType.Int.DBItemDefaultValue,
                DBItemType.Int.DBItemDefaultValue,
            });

            var result = instance.CreateMatchItemInstance();

            // 結果が意図した値と一致すること
            Assert.IsTrue(result.Equals(answer));
        }


        [Test]
        public static void SerializeTest()
        {
            var target = new DatabaseDataDescList();
            target.AdjustLength(2);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}