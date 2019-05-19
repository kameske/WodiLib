using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseProjectFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                DatabaseProjectFileTestItemGenerator.GenerateDatabase0Project(),
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputDatabase0.project"
            },
            new object[]
            {
                DatabaseProjectFileTestItemGenerator.GenerateCDatabase0Project(),
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputCDatabase0.project"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DatabaseProject outputData, string outputFileName)
        {
            var writer = new DatabaseProjectFileWriter(outputData, outputFileName);

            var isSuccess = false;
            var errorMessage = "";

            try
            {
                writer.WriteSync();
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

            Assert.True(true);
        }
    }
}