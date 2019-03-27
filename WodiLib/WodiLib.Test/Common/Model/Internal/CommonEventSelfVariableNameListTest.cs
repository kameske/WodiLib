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

        [TestCase(-1, true)]
        [TestCase(99, true)]
        [TestCase(100, false)]
        [TestCase(101, true)]
        public static void ConstructorTest(int nameListLength, bool isError)
        {
            var nameList = nameListLength == -1 ? null : MakeStringList(nameListLength);

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

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void UpdateVariableNameTest(bool isNameNull, bool isError)
        {
            var instance = new CommonEventSelfVariableNameList();

            var index = (CommonEventSelfVariableIndex) 10;
            var name = isNameNull ? null : (CommonEventSelfVariableName) "testName";

            var errorOccured = false;
            try
            {
                instance.UpdateVariableName(index, name);
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
        public static void GetVariableNameTest()
        {
            var instance = new CommonEventSelfVariableNameList();
            var index = (CommonEventSelfVariableIndex) 10;

            var errorOccured = false;
            try
            {
                instance.GetVariableName(index);
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


        private static List<CommonEventSelfVariableName> MakeStringList(int length)
        {
            var list = new List<CommonEventSelfVariableName>();

            for (var i = 0; i < length; i++)
            {
                list.Add((CommonEventSelfVariableName) "");
            }

            return list;
        }
    }
}