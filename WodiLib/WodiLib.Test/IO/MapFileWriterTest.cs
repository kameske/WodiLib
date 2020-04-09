using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class MapFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                MapFileTestItemGenerator.GenerateMap023Data(),
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\OutputMap023.mps"
            },
            new object[]
            {
                MapFileTestItemGenerator.GenerateMap024Data(),
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\OutputMap024.mps"
            },
            new object[]
            {
                MapFileTestItemGenerator.GenerateMap025Data(),
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\OutputMap025.mps"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(MapData outputData, string outputFileName)
        {
            var writer = new MpsFileWriter(outputFileName);

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