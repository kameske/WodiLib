using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeSetFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                DBTypeSetFileTestItemGenerator.GenerateCDB0Data(),
                $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\Outputタイプ設定_000_あいうえお.dbtypeset"
            },
            new object[]
            {
                DBTypeSetFileTestItemGenerator.GenerateUDB0Data(),
                $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\Outputタイプ設定_000_UDB0.dbtypeset"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBTypeSet outputDat, string outputFileName)
        {
            var writer = new DBTypeSetFileWriter(outputFileName);

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