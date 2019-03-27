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

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void ReturnValueDescriptionTest(bool isNull, bool isError)
        {
            var instance = new CommonEventReturnValue();

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
        }

        [TestCase(-1, false)]
        [TestCase(0, true)]
        public static void SetReturnVariableIndexTest(int commonVariableIndex, bool isReturnFlag)
        {
            var instance = new CommonEventReturnValue();

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
            Assert.AreEqual((int)instance.ReturnVariableIndex, -1);
        }
    }
}