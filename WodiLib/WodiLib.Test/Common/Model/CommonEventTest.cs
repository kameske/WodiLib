using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Event.EventCommand;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] EventBootConditionTestCaseSource =
        {
            new object[] {new CommonEventBootCondition(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(EventBootConditionTestCaseSource))]
        public static void EventBootConditionTest(CommonEventBootCondition bootCondition, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.BootCondition = bootCondition;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var condition = instance.BootCondition;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(condition == bootCondition);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void NumberArgsLengthTest(int length, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.NumberArgsLength = length;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var len = instance.NumberArgsLength;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(len == length);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void StrArgsLengthTest(int length, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.StrArgsLength = length;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var len = instance.StrArgsLength;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(len == length);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void NameTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var name = isNull ? null : (CommonEventName) "test";

            var errorOccured = false;
            try
            {
                instance.Name = name;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var n = instance.Name;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(n == name);
        }

        private static readonly object[] EventCommandsTestCaseSource =
        {
            new object[] {new EventCommandList(new List<IEventCommand> {new Blank()}), false},
            // new object[] {new EventCommandList(new List<IEventCommand>()), true}, イベント0行のEventCommandListは作成不可能
            new object[] {null, true},
        };

        [TestCaseSource(nameof(EventCommandsTestCaseSource))]
        public static void EventCommandsTest(EventCommandList list, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.EventCommands = list;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.EventCommands;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(ReferenceEquals(setValue, list));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void DescriptionTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var str = isNull ? null : (CommonEventDescription) "test";

            var errorOccured = false;
            try
            {
                instance.Description = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.Description;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue?.Equals(str));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void MemoTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var str = isNull ? null : (CommonEventMemo) "test";

            var errorOccured = false;
            try
            {
                instance.Memo = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.Memo;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue?.Equals(str));
        }

        private static readonly object[] LabelColorTestCaseSource =
        {
            new object[] {CommonEventLabelColor.Black, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(LabelColorTestCaseSource))]
        public static void LabelColorTest(CommonEventLabelColor color, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.LabelColor = color;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.LabelColor;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue == color);
        }

        [TestCase(false, false)]
        [TestCase(true, false)]
        public static void FooterStringTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var str = isNull ? null : (CommonEventFooterString) "test";

            var errorOccured = false;
            try
            {
                instance.FooterString = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.FooterString;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(ReferenceEquals(setValue, str));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void ReturnValueDescriptionTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var str = isNull ? null : (CommonEventResultDescription) "test";

            var errorOccured = false;
            try
            {
                instance.ReturnValueDescription = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.ReturnValueDescription;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue?.Equals(str));
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void SelfVariableNameListTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var nameList = isNull ? null : new CommonEventSelfVariableNameList();

            var errorOccured = false;
            try
            {
                instance.SelfVariableNameList = nameList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void UpdateSpecialNumberArgDescTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var index = (CommonEventNumberArgIndex) 1;
            var desc = isNull ? null : new CommonEventSpecialNumberArgDesc();

            var errorOccured = false;
            try
            {
                instance.UpdateSpecialNumberArgDesc(index, desc);
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
        public static void GetSpecialNumberArgDescTest()
        {
            var instance = new CommonEvent();

            var index = (CommonEventNumberArgIndex) 0;

            var errorOccured = false;
            try
            {
                var _ = instance.GetSpecialNumberArgDesc(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void UpdateSpecialStringArgDescTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();

            var index = (CommonEventStringArgIndex) 2;
            var desc = isNull ? null : new CommonEventSpecialStringArgDesc();

            var errorOccured = false;
            try
            {
                instance.UpdateSpecialStringArgDesc(index, desc);
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
        public static void GetSpecialStringArgDescTest()
        {
            var instance = new CommonEvent();

            var index = (CommonEventStringArgIndex) 1;

            var errorOccured = false;
            try
            {
                var _ = instance.GetSpecialStringArgDesc(index);
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
        public static void SetReturnVariableIndexTest()
        {
            var instance = new CommonEvent();

            var commonVarAddress = (CommonEventReturnVariableIndex) 10;

            var errorOccured = false;
            try
            {
                instance.SetReturnVariableIndex(commonVarAddress);
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
        public static void SetReturnValueNoneTest()
        {
            var instance = new CommonEvent();

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

            // エラーが発生しないこと
            Assert.AreEqual(errorOccured, false);
        }
    }
}