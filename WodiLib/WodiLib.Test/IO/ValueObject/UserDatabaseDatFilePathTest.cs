using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class UserDatabaseDatFilePathTest
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
        [TestCase("DataBase.dat", false)]
        [TestCase("DATABASE.DAT", false)]
        [TestCase("CDatabase.dat", false)]
        [TestCase("Database.dat.bak", false)]
        [TestCase("./DataBase.dat", false)]
        [TestCase(@".\Data\DataBase.dat", false)]
        [TestCase(@"c:\MyProject\Data\DataBase.dat", false)]
        [TestCase(@"c:\MyProject\Data\DataBase.data", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            UserDatabaseDatFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new UserDatabaseDatFilePath(path);
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
            var target = (UserDatabaseDatFilePath) "test\\Database.dat";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}