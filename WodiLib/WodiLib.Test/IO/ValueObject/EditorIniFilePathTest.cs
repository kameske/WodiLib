using System;
using Commons;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class EditorIniFilePathTest
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
        [TestCase("Editor.ini", false)]
        [TestCase("EDITOR.INI", false)]
        [TestCase("EditorIni", false)]
        [TestCase("Editor.ini.bak", false)]
        [TestCase("./editor.ini", false)]
        [TestCase(@".\Data\Editor.ini", false)]
        [TestCase(@"c:\MyProject\Data\Editor.ini", false)]
        [TestCase(@"c:\MyProject\Data\Editor.in", false)]
        public static void ConstructorTest(string path, bool isError)
        {
            EditorIniFilePath instance = null;

            var errorOccured = false;
            try
            {
                instance = new EditorIniFilePath(path);
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
    }
}
