using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBDataFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                DBDataFileTestItemGenerator.GenerateCDB0DBData(),
                $@"{DBDataFileTestItemGenerator.TestWorkRootDir}\Outputあいうえお_データ_000to000_a.dbdata"
            },
            new object[]
            {
                DBDataFileTestItemGenerator.GenerateUDB0DBData(),
                $@"{DBDataFileTestItemGenerator.TestWorkRootDir}\OutputUDB0_データ_001to003_7.dbdata"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBData outputDat, string outputFileName)
        {
            var writer = new DBDataFileWriter(outputFileName);

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