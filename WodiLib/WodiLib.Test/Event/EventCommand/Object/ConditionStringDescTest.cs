using System;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionStringDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase]
        public static void NullGuardTest()
        {
            bool isError;
            var instance = new ConditionStringDesc();
            {
                try
                {
                    instance.Condition = null;
                    isError = false;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    isError = true;
                }

                Assert.IsTrue(isError);
            }
            {
                try
                {
                    instance.RightSide = null;
                    isError = false;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    isError = true;
                }

                Assert.IsTrue(isError);
            }
            {
                try
                {
                    instance.RightSide = null;
                    isError = false;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    isError = true;
                }

                Assert.IsTrue(isError);
            }
            {
                try
                {
                    var none = new IntOrStr();
                    instance.RightSide = none;
                    isError = false;
                }
                catch (Exception ex)
                {
                    logger.Exception(ex);
                    isError = true;
                }

                Assert.IsTrue(isError);
            }
        }
    }
}