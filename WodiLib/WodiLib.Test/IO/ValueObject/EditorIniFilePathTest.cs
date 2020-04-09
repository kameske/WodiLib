using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class EditorIniFilePathTest
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

        [Test]
        public static void SerializeTest()
        {
            var target = (EditorIniFilePath) "test\\Editor.ini";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}