using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class ChangeableDatabaseDatFilePathTest
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
        [TestCase("CDataBase.dat", false)]
        [TestCase("CDATABASE.DAT", false)]
        [TestCase("Database.dat", false)]
        [TestCase("CDatabase.dat.bak", false)]
        [TestCase("./CDataBase.dat", false)]
        [TestCase(@".\Data\CDataBase.dat", false)]
        [TestCase(@"c:\MyProject\Data\CDataBase.dat", false)]
        [TestCase(@"c:\MyProject\Data\CDataBase.data", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            ChangeableDatabaseDatFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new ChangeableDatabaseDatFilePath(path);
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
