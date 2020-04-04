using System;
using System.IO;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class EditorIniFileWriterTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                EditorIniDataTestItemGenerator.GenerateData0(),
                $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\OutputDir0\Editor.ini"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(EditorIniData outputDat, string outputFileName)
        {
            Path.GetDirectoryName(outputFileName).CreateDirectoryIfNeed();
            var writer = new EditorIniFileWriter(outputFileName);

            var isSuccess = false;
            var errorMessage = "";

            try
            {
                writer.WriteSync(outputDat);
                isSuccess = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            // 出力成功すること
            if (!isSuccess)
            {
                throw new InvalidOperationException(
                    $"Error message: {errorMessage}");
            }

            // デバッグログにファイルの内容を出力
            logger.Debug("Outputファイル内容出力開始");
            var outputTextLines = File.ReadAllLines(outputFileName);
            foreach (var outputTextLine in outputTextLines)
            {
                logger.Debug(outputTextLine);
            }
            logger.Debug("Outputファイル内容出力完了");

            Assert.True(true);
        }
    }
}