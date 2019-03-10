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

        [TestCase(99, true)]
        [TestCase(100, false)]
        [TestCase(101, true)]
        public static void ConstructorTest(int nameListLength, bool isError)
        {
            var nameList = MakeStringList(nameListLength);

            var errorOccured = false;
            try
            {
                var _ = new CommonEventSelfVariableNameList(nameList);
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
            var instance = new CommonEventSelfVariableNameList();

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
            var instance = new CommonEventSelfVariableNameList();

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

        [Test]
        public static void GetAllNameTest()
        {
            var instance = new CommonEventSelfVariableNameList();

            var errorOccured = false;
            try
            {
                instance.GetAllName();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, false);
        }


        private static List<string> MakeStringList(int length)
        {
            var list = new List<string>();

            for (var i = 0; i < length; i++)
            {
                list.Add("");
            }

            return list;
        }
    }
}