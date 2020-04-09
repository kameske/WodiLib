using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class UserDatabaseProjectFilePathTest
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
        [TestCase("DataBase.project", false)]
        [TestCase("DATABASE.PROJECT", false)]
        [TestCase("CDatabase.project", false)]
        [TestCase("Database.project.bak", false)]
        [TestCase("./DataBase.project", false)]
        [TestCase(@".\Data\DataBase.project", false)]
        [TestCase(@"c:\MyProject\Data\DataBase.project", false)]
        [TestCase(@"c:\MyProject\Data\DataBase.proj", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            UserDatabaseProjectFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new UserDatabaseProjectFilePath(path);
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
            var target = (UserDatabaseProjectFilePath) "test\\Database.project";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}