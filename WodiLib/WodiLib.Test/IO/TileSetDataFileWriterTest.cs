using System;
using System.IO;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class TileSetDataFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                TileSetDataTestItemGenerator.GenerateData0(),
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\Output0\TileSetData.dat"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(TileSetData outputData, string outputFileName)
        {
            var dir = Path.GetDirectoryName(outputFileName);
            dir.CreateDirectoryIfNeed();

            var writer = new TileSetDataFileWriter(outputFileName);

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