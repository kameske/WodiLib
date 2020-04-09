using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventReturnValueTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void ReturnValueDescriptionTest(bool isNull, bool isError)
        {
            var instance = new CommonEventReturnValue();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var description = isNull ? null : (CommonEventResultDescription) "test";

            var errorOccured = false;
            try
            {
                instance.Description = description;
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
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventReturnValue.Description)));
            }
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        public static void SetReturnVariableIndexTest(int commonVariableIndex, bool isReturnFlag)
        {
            var instance = new CommonEventReturnValue();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var index = (CommonEventReturnVariableIndex) commonVariableIndex;

            var errorOccured = false;
            try
            {
                instance.SetReturnVariableIndex(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 返戻値フラグが一致すること
            Assert.AreEqual(instance.IsReturnValue, isReturnFlag);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 2);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventReturnValue.ReturnVariableIndex)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(CommonEventReturnValue.IsReturnValue)));
        }

        [Test]
        public static void SetReturnValueNoneTest()
        {
            var instance = new CommonEventReturnValue();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetReturnValueNone();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, false);

            // 返戻値フラグがfalseであること
            Assert.AreEqual(instance.IsReturnValue, false);

            // 返戻アドレスが-1であること
            Assert.AreEqual((int) instance.ReturnVariableIndex, -1);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 2);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventReturnValue.ReturnVariableIndex)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(CommonEventReturnValue.IsReturnValue)));
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonEventReturnValue
            {
                Description = "Description",
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}