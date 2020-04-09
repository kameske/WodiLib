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
                (UserDatabaseProjectFilePath)
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputDatabase0.project"
            },
            new object[]
            {
                DatabaseProjectFileTestItemGenerator.GenerateCDatabase0Project(),
                (ChangeableDatabaseProjectFilePath)
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputCDatabase0.project"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DatabaseProject outputData, DatabaseProjectFilePath outputFileName)
        {
            var writer = new DatabaseProjectFileWriter(outputFileName);

            var isSuccess = false;
            var errorMessage = "";

            try
            {
                writer.WriteSync(outputData);
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