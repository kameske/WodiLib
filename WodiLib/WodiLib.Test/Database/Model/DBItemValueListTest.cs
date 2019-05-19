using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBItemValueListTest
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
    }
}