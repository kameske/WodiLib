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

        [TestCase(-1, false, true)]
        [TestCase(-1, true, true)]
        [TestCase(0, false, false)]
        [TestCase(0, true, true)]
        [TestCase(4, false, false)]
        [TestCase(4, true, true)]
        [TestCase(5, false, true)]
        [TestCase(5, true, true)]
        public static void UpdateSpecialNumberArgDescTest(int index, bool isUpdateNull, bool isError)
        {
            var item = isUpdateNull ? null : new CommonEventSpecialNumberArgDesc();

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

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void GetSpecialNumberArgDescTest(int index, bool isError)
        {
            var instance = new CommonEventSpecialArgDescList();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, false, true)]
        [TestCase(-1, true, true)]
        [TestCase(0, false, false)]
        [TestCase(0, true, true)]
        [TestCase(4, false, false)]
        [TestCase(4, true, true)]
        [TestCase(5, false, true)]
        [TestCase(5, true, true)]
        public static void UpdateSpecialStringArgDescTest(int index, bool isUpdateNull, bool isError)
        {
            var item = isUpdateNull ? null : new CommonEventSpecialStringArgDesc();

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

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void GetSpecialStringArgDescTest(int index, bool isError)
        {
            var instance = new CommonEventSpecialArgDescList();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}