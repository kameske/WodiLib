using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class ChangeableDatabaseProjectFilePathTest
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
        [TestCase("CDataBase.project", false)]
        [TestCase("CDATABASE.PROJECT", false)]
        [TestCase("Database.project", false)]
        [TestCase("CDatabase.project.bak", false)]
        [TestCase("./CDataBase.project", false)]
        [TestCase(@".\Data\CDataBase.project", false)]
        [TestCase(@"c:\MyProject\Data\CDataBase.project", false)]
        [TestCase(@"c:\MyProject\Data\CDataBase.proj", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            ChangeableDatabaseProjectFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new ChangeableDatabaseProjectFilePath(path);
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
