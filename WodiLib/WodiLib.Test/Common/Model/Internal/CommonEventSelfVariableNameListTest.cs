using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSelfVariableNameListTest
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
            CommonEventSelfVariableNameList instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventSelfVariableNameList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が100件であること
            Assert.AreEqual(instance.Count, 100);
        }

        [TestCase(-1, false, true)]
        [TestCase(100, false, false)]
        [TestCase(100, true, true)]
        [TestCase(101, false, true)]
        [TestCase(101, true, true)]
        public static void ConstructorTestB(int initLength, bool hasNullItem, bool isError)
        {
            var initItemList = MakeInitList(initLength, hasNullItem);
            CommonEventSelfVariableNameList instance = null;

            var errorOccured = false;
            try
            {
                instance = new CommonEventSelfVariableNameList(initItemList);
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
        public static void GetCapacityTest()
        {
            var instance = new CommonEventSelfVariableNameList();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var maxCapacity = instance.GetCapacity();

            // 取得した値が制限容量と一致すること
            Assert.AreEqual(maxCapacity, CommonEventSelfVariableNameList.Capacity);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonEventSelfVariableNameList
            {
                [3] = "SelfName",
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static List<CommonEventSelfVariableName> MakeInitList(int length, bool hasNullItem)
        {
            if (length == -1) return null;

            var result = new List<CommonEventSelfVariableName>();
            for (var i = 0; i < length; i++)
            {
                result.Add(hasNullItem && i == length / 2
                    ? null
                    : new CommonEventSelfVariableName(i.ToString())
                );
            }

            return result;
        }
    }
}