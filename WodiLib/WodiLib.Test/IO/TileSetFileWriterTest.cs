using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class TileSetFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                TileSetFileDataTestItemGenerator.GenerateData0(),
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\Output_000_設定名.tile"
            },
            new object[]
            {
                TileSetFileDataTestItemGenerator.GenerateData1(),
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\Output_001_aaa.tile"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(TileSetFileData outputData, string outputFileName)
        {
            var writer = new TileSetFileWriter(outputFileName);

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