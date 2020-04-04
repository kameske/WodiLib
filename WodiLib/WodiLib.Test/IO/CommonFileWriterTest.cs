using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class CommonFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                CommonFileTestItemGenerator.GenerateCommon000Data(),
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\Output000_コモンイベント000.common"
            },
            new object[]
            {
                CommonFileTestItemGenerator.GenerateCommon003To005Data(),
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\OutputCommon003to005_コモンイベント003.common"
            },
            new object[]
            {
                CommonFileTestItemGenerator.GenerateCommon005To006Data(),
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\OutputCommon005to006_コモンイベント006.common"
            },
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(CommonFileData outputData, string outputFileName)
        {
            var writer = new CommonFileWriter(outputFileName);

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