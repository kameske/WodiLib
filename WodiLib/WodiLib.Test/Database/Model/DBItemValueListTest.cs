using System;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBItemValueListTest
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
            var instance = new DBItemValueList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DBItemValueList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new DBItemValueList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, DBItemValueList.MinCapacity);
        }

        [Test]
        public static void ToFixedLengthListTest()
        {
            var instance = new DBItemValueList();

            var errorOccured = false;
            try
            {
                var _ = instance.ToFixedLengthList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [Test]
        public static void ToLengthChangeableItemValueListTest()
        {
            var instance = new DBItemValueList();

            DBItemValueList result = null;
            var errorOccured = false;
            try
            {
                result = instance.ToLengthChangeableItemValueList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            try
            {
                result.Add((DBValueInt) 10);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [Test]
        public static void RelationshipTest()
        {
            var valuesList = new DBItemValuesList();
            valuesList.AdjustLength(2);

            // 操作対象
            var target = (DBItemValueList) valuesList[0];

            {
                // テスト1：DBItemValuesListに紐付けられた状態で操作する
                var errorOccured = false;
                try
                {
                    target.Add(new DBItemValue(20));
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生すること
                Assert.IsTrue(errorOccured);
            }

            {
                // テスト2：紐付けを解除した状態で操作する

                // 紐付け解除
                valuesList.RemoveAt(0);

                var errorOccured = false;
                try
                {
                    target.Add(new DBItemValue(20));
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    errorOccured = true;
                }

                // エラーが発生しないこと
                Assert.IsFalse(errorOccured);
            }
        }

        [Test]
        public static void CreateDefaultValueListInstanceTest()
        {
            var instance = new DBItemValueList(new[]
            {
                new DBItemValue(1),
                new DBItemValue("two"),
                new DBItemValue(3),
                new DBItemValue("four"),
                new DBItemValue("five"),
                new DBItemValue(6),
            });

            var answer = new[]
            {
                DBItemType.Int.DBItemDefaultValue,
                DBItemType.String.DBItemDefaultValue,
                DBItemType.Int.DBItemDefaultValue,
                DBItemType.String.DBItemDefaultValue,
                DBItemType.String.DBItemDefaultValue,
                DBItemType.Int.DBItemDefaultValue,
            };

            var result = instance.CreateDefaultValueListInstance();

            // 意図した値が取得されること
            Assert.IsTrue(answer.SequenceEqual(result));
        }
    }
}
