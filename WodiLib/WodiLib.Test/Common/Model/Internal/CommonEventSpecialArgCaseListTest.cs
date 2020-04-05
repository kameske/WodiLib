using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialArgCaseListTest
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
            CommonEventSpecialArgCaseList instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventSpecialArgCaseList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が0件であること
            Assert.AreEqual(instance.Count, 0);
        }

        [TestCase(-1, false, true)]
        [TestCase(0, false, false)]
        [TestCase(1, false, false)]
        [TestCase(1, true, true)]
        [TestCase(9999, false, false)]
        [TestCase(9999, true, true)]
        public static void ConstructorTestB(int initLength, bool hasNullItem, bool isError)
        {
            var initItemList = MakeInitList(initLength, hasNullItem);
            CommonEventSpecialArgCaseList instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventSpecialArgCaseList(initItemList);
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
            var instance = new CommonEventSpecialArgCaseList();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, CommonEventSpecialArgCaseList.MaxCapacity);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new CommonEventSpecialArgCaseList();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var minCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(minCapacity, CommonEventSpecialArgCaseList.MinCapacity);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonEventSpecialArgCaseList(MakeInitList(3, false));
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static IReadOnlyList<CommonEventSpecialArgCase> MakeInitList(int length, bool hasNullItem)
        {
            if (length == -1) return null;

            var result = new List<CommonEventSpecialArgCase>();
            for (var i = 0; i < length; i++)
            {
                result.Add(hasNullItem && i == length / 2
                    ? null
                    : new CommonEventSpecialArgCase(i, i.ToString())
                );
            }

            return result;
        }
    }
}