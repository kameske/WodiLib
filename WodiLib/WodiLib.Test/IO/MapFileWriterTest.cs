using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;

namespace WodiLib.Test
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
            var writer = new MapFileWriter(outputData, outputFileName);
            var result = writer.WriteSync();

            // 出力成功すること
            if (!result)
            {
                throw new InvalidOperationException(
                    $"Error message: {writer.ErrorMessage}");
            }

            Assert.True(true);
        }
    }
}