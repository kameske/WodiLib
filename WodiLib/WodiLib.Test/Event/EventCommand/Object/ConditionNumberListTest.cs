using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionNumberListTest
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
            ConditionNumberList instance = null;

            var errorOccured = false;
            try
            {
                instance = new ConditionNumberList();
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
        [TestCase(0, false, true)]
        [TestCase(1, false, false)]
        [TestCase(1, true, true)]
        [TestCase(15, false, false)]
        [TestCase(15, true, true)]
        [TestCase(16, false, true)]
        [TestCase(16, true, true)]
        public static void ConstructorTestB(int initLength, bool hasNullItem, bool isError)
        {
            var initItemList = MakeInitList(initLength, hasNullItem);
            ConditionNumberList instance = null;

            var errorOccured = false;
            try
            {
                instance = new ConditionNumberList(initItemList);
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
            var instance = new ConditionNumberList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, ConditionNumberList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new ConditionNumberList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, ConditionNumberList.MinCapacity);
        }


        private static IReadOnlyList<ConditionNumberDesc> MakeInitList(int length, bool hasNullItem)
        {
            if (length == -1) return null;

            var result = new List<ConditionNumberDesc>();
            for (var i = 0; i < length; i++)
            {
                result.Add(hasNullItem && i == length / 2
                    ? null
                    : new ConditionNumberDesc()
                );
            }

            return result;
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new ConditionNumberList(MakeInitList(2, false));
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}