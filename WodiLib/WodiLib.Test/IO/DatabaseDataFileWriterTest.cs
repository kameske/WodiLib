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
                (UserDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputDatabase0.dat"
            },
            new object[]
            {
                DatabaseDatFileTestItemGenerator.GenerateCDatabaseData0Data(),
                (ChangeableDatabaseDatFilePath)
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputCDatabase0.dat"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DatabaseDat outputDat, DatabaseDatFilePath outputFileName)
        {
            var writer = new DatabaseDatFileWriter(outputFileName);

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

            Assert.True(true);
        }
    }
}