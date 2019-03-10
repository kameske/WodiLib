using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Event;
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

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("New\r\nLine\r\nCRLF", false)]
        [TestCase("New\nLine\nLF", false)]
        public static void NameTest(string name, bool isError)
        {
            var instance = new CommonEvent();

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

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("New\r\nLine\r\nCRLF", false)]
        [TestCase("New\nLine\nLF", false)]
        public static void BeforeMemoTest(string str, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.BeforeMemo = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.BeforeMemo;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(str));
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("New\r\nLine\r\nCRLF", false)]
        [TestCase("New\nLine\nLF", false)]
        public static void MemoTest(string str, bool isError)
        {
            var instance = new CommonEvent();

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
            Assert.IsTrue(setValue.Equals(str));
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

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("New\r\nLine\r\nCRLF", false)]
        [TestCase("New\nLine\nLF", false)]
        public static void FooterStringTest(string str, bool isError)
        {
            var instance = new CommonEvent();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var setValue = instance.FooterString;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(str));
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("New\r\nLine\r\nCRLF", false)]
        [TestCase("New\nLine\nLF", false)]
        public static void ReturnValueDescriptionTest(string str, bool isError)
        {
            var instance = new CommonEvent();

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
            Assert.IsTrue(setValue.Equals(str));
        }

        private static readonly object[] UpdateSpecialNumberArgDescTestCaseSource =
        {
            new object[] {-1, null, true},
            new object[] {0, null, true},
            new object[] {4, null, true},
            new object[] {5, null, true},
            new object[] {-1, new CommonEventSpecialNumberArgDesc(), true},
            new object[] {0, new CommonEventSpecialNumberArgDesc(), false},
            new object[] {4, new CommonEventSpecialNumberArgDesc(), false},
            new object[] {5, new CommonEventSpecialNumberArgDesc(), true},
        };

        [TestCaseSource(nameof(UpdateSpecialNumberArgDescTestCaseSource))]
        public static void UpdateSpecialNumberArgDescTest(int index, CommonEventSpecialNumberArgDesc desc, bool isError)
        {
            var instance = new CommonEvent();

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

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void GetSpecialNumberArgDescTest(int index, bool isError)
        {
            var instance = new CommonEvent();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] UpdateSpecialStringArgDescTestCaseSource =
        {
            new object[] {-1, null, true},
            new object[] {0, null, true},
            new object[] {4, null, true},
            new object[] {5, null, true},
            new object[] {-1, new CommonEventSpecialStringArgDesc(), true},
            new object[] {0, new CommonEventSpecialStringArgDesc(), false},
            new object[] {4, new CommonEventSpecialStringArgDesc(), false},
            new object[] {5, new CommonEventSpecialStringArgDesc(), true},
        };

        [TestCaseSource(nameof(UpdateSpecialStringArgDescTestCaseSource))]
        public static void UpdateSpecialStringArgDescTest(int index, CommonEventSpecialStringArgDesc desc, bool isError)
        {
            var instance = new CommonEvent();

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

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void GetSpecialStringArgDescTest(int index, bool isError)
        {
            var instance = new CommonEvent();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, null, true)]
        [TestCase(-1, "", true)]
        [TestCase(-1, "abc", true)]
        [TestCase(-1, "あいうえお", true)]
        [TestCase(-1, "New\r\nLine\r\nCRLF", true)]
        [TestCase(-1, "New\nLine\nLF", true)]
        [TestCase(0, null, true)]
        [TestCase(0, "", false)]
        [TestCase(0, "abc", false)]
        [TestCase(0, "あいうえお", false)]
        [TestCase(0, "New\r\nLine\r\nCRLF", false)]
        [TestCase(0, "New\nLine\nLF", false)]
        [TestCase(99, null, true)]
        [TestCase(99, "", false)]
        [TestCase(99, "abc", false)]
        [TestCase(99, "あいうえお", false)]
        [TestCase(99, "New\r\nLine\r\nCRLF", false)]
        [TestCase(99, "New\nLine\nLF", false)]
        [TestCase(100, null, true)]
        [TestCase(100, "", true)]
        [TestCase(100, "abc", true)]
        [TestCase(100, "あいうえお", true)]
        [TestCase(100, "New\r\nLine\r\nCRLF", true)]
        [TestCase(100, "New\nLine\nLF", true)]
        public static void UpdateVariableNameTest(int number, string variableName, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.UpdateVariableName(number, variableName);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99, false)]
        [TestCase(100, true)]
        public static void GetVariableNameTest(int number, bool isError)
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.GetVariableName(number);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(99, true)]
        [TestCase(100, false)]
        [TestCase(101, true)]
        public static void UpdateAllVariableNameTest(int argLength, bool isError)
        {
            // argLengthの要素数を持つ文字列リストを作成（ただしargLength=-1の場合null）
            var nameList = new List<string>();
            if (argLength == -1) nameList = null;
            else
            {
                for (var i = 0; i < argLength; i++)
                {
                    nameList.Add("");
                }
            }

            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.UpdateAllVariableName(nameList);
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
        public static void GetAllVariableNameTest()
        {
            var instance = new CommonEvent();

            var errorOccured = false;
            try
            {
                instance.GetAllVariableName();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.AreEqual(errorOccured, false);
        }

        [TestCase(-2, true)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(99, false)]
        [TestCase(100, true)]
        public static void SetReturnVariableIndexTest(int commonVarAddress, bool isError)
        {
            var instance = new CommonEvent();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
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