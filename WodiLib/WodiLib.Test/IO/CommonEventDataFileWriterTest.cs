using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.IO;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class CommonEventDataFileWriterTest
    {
        private static readonly object[] WriteSyncTestCaseSource =
        {
            new object[]
            {
                CommonEventDataFileTestItemGenerator.GenerateCommonEvent00Data(),
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\OutputCommonEvent_00.dat"
            }
        };

        [TestCaseSource(nameof(WriteSyncTestCaseSource))]
        public static void WriteSyncTest(CommonEventData outputData, string outputFileName)
        {
            var writer = new CommonEventDatFileWriter(outputFileName);

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