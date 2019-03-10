using System;
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

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("New\r\nLine\r\nCRLF", false)]
        [TestCase("New\nLine\nLF", false)]
        public static void ReturnValueDescriptionTest(string src, bool isError)
        {
            var instance = new CommonEventReturnValue();

            var errorOccured = false;
            try
            {
                instance.Description = src;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-2, true, null)]
        [TestCase(-1, false, false)]
        [TestCase(0, false, true)]
        [TestCase(99, false, true)]
        [TestCase(100, true, null)]
        public static void SetReturnVariableIndexTest(int commonVariableIndex, bool isError, bool isReturnFlag)
        {
            var instance = new CommonEventReturnValue();

            var errorOccured = false;
            try
            {
                instance.SetReturnVariableIndex(commonVariableIndex);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 返戻値フラグがTrueであること
            Assert.AreEqual(instance.IsReturnValue, isReturnFlag);
        }

        [Test]
        public static void SetReturnValueNoneTest()
        {
            var instance = new CommonEventReturnValue();

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
            Assert.AreEqual(instance.ReturnVariableIndex, -1);
        }
    }
}