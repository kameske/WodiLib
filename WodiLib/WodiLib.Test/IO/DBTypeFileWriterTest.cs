using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                DBTypeFileTestItemGenerator.GenerateCDB0DBType(),
                $@"{DBTypeFileTestItemGenerator.TestWorkRootDir}\Outputタイプ(データ含む)_000_あいうえお.dbtype"
            },
            new object[]
            {
                DBTypeFileTestItemGenerator.GenerateUDB0DBType(),
                $@"{DBTypeFileTestItemGenerator.TestWorkRootDir}\Outputタイプ(データ含む)_000_UDB0.dbtype"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(DBType outputDat, string outputFileName)
        {
            var writer = new DBTypeFileWriter(outputFileName);

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