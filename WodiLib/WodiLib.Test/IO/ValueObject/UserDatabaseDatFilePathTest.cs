using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class UserDatabaseDatFilePathTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
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
            Assert.AreEqual((string)instance, path);
        }
    }
}
