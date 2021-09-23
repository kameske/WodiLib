using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class SystemDatabaseDatFilePathTest
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
        [TestCase("SysDataBase.dat", false)]
        [TestCase("SYSDATABASE.DAT", false)]
        [TestCase("CDatabase.dat", false)]
        [TestCase("SysDatabase.dat.bak", false)]
        [TestCase("./SysDataBase.dat", false)]
        [TestCase(@".\Data\SysDataBase.dat", false)]
        [TestCase(@"c:\MyProject\Data\SysDataBase.dat", false)]
        [TestCase(@"c:\MyProject\Data\SysDataBase.data", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            SystemDatabaseDatFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new SystemDatabaseDatFilePath(path);
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
