using System;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionStringDescTest
    {
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
                catch (Exception)
                {
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
                catch (Exception)
                {
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
                catch (Exception)
                {
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
                catch (Exception)
                {
                    isError = true;
                }

                Assert.IsTrue(isError);
            }
        }
    }
}