using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialArgDescListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void UpdateSpecialNumberArgDescTest(bool isUpdateNull, bool isError)
        {
            var item = isUpdateNull ? null : new CommonEventSpecialNumberArgDesc();
            var index = (CommonEventNumberArgIndex) 3;

            var instance = new CommonEventSpecialArgDescList();

            var errorOccured = false;
            try
            {
                instance.UpdateSpecialNumberArgDesc(index, item);
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
            var instance = new CommonEventSpecialArgDescList();

            var index = (CommonEventNumberArgIndex) 0;

            var errorOccured = false;
            try
            {
                instance.GetSpecialNumberArgDesc(index);
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
        public static void UpdateSpecialStringArgDescTest(bool isUpdateNull, bool isError)
        {
            var item = isUpdateNull ? null : new CommonEventSpecialStringArgDesc();
            var index = (CommonEventStringArgIndex) 2;

            var instance = new CommonEventSpecialArgDescList();

            var errorOccured = false;
            try
            {
                instance.UpdateSpecialStringArgDesc(index, item);
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
            var instance = new CommonEventSpecialArgDescList();

            var index = (CommonEventStringArgIndex) 1;

            var errorOccured = false;
            try
            {
                instance.GetSpecialStringArgDesc(index);
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