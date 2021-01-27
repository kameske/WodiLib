using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void ConstructorTestA()
        {
            CommonEventList instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventList();
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

        [TestCase(-1, false, true)]
        [TestCase(1, false, false)]
        [TestCase(2, false, false)]
        [TestCase(2, true, true)]
        [TestCase(10000, false, false)]
        [TestCase(10000, true, true)]
        [TestCase(10001, false, true)]
        [TestCase(10001, true, true)]
        public static void ConstructorTestB(int initLength, bool hasNullItem, bool isError)
        {
            var initItemList = MakeInitList(initLength, hasNullItem);
            CommonEventList instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventList(initItemList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 選択肢が意図した数であること
            var answerResultLength = initLength != -1
                ? initLength
                : 0;
            Assert.AreEqual(instance.Count, answerResultLength);
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new CommonEventList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, CommonEventList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new CommonEventList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, CommonEventList.MinCapacity);
        }


        private static List<CommonEvent> MakeInitList(int length, bool hasNullItem)
        {
            if (length == -1) return null;

            var result = new List<CommonEvent>();
            for (var i = 0; i < length; i++)
            {
                result.Add(hasNullItem && i == length / 2
                    ? null
                    : new CommonEvent()
                );
            }

            return result;
        }
    }
}
