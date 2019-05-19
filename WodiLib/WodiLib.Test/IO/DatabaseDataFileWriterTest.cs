using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseDataFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                DatabaseDatFileTestItemGenerator.GenerateDataBaseDat0Data(),
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputDatabase0.dat"
            },
            new object[]
            {
                DatabaseDatFileTestItemGenerator.GenerateCDatabaseData0Data(),
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputCDatabase0.dat"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DatabaseDat outputDat, string outputFileName)
        {
            var writer = new DatabaseDatFileWriter(outputDat, outputFileName);

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