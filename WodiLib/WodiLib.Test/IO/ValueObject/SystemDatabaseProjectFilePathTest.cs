using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class SystemDatabaseProjectFilePathTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }


        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("SysDataBase.project", false)]
        [TestCase("SYSDATABASE.PROJECT", false)]
        [TestCase("Database.project", false)]
        [TestCase("SysDatabase.project.bak", false)]
        [TestCase("./SysDataBase.project", false)]
        [TestCase(@".\Data\SysDataBase.project", false)]
        [TestCase(@"c:\MyProject\Data\SysDataBase.project", false)]
        [TestCase(@"c:\MyProject\Data\SysDataBase.proj", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            SystemDatabaseProjectFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new SystemDatabaseProjectFilePath(path);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 内容が一致すること
            Assert.AreEqual((string) instance, path);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (SystemDatabaseProjectFilePath) "test\\SysDatabase.Project";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}