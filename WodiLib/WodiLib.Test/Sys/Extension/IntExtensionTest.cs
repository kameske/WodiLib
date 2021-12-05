using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class IntExtensionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(10, 9, 9, false)]
        [TestCase(10, 9, 10, true)]
        [TestCase(10, 9, 11, true)]
        [TestCase(10, 10, 10, true)]
        [TestCase(10, 10, 11, true)]
        [TestCase(10, 11, 11, false)]
        public static void IsBetweenTest(int target, int min, int max, bool actual)
        {
            var expected = target.IsBetween(min, max);

            // 意図した結果であること
            Assert.AreEqual(expected, actual);
        }

        private static readonly object[] ToInt32TestCaseSource =
        {
            new object[]
            {
                new byte[]
                {
                    0x03, 0x00, 0x00, 0x00
                },
                Endian.Little, 0, 3
            },
            new object[]
            {
                new byte[]
                {
                    0x00, 0x00, 0x00, 0x03
                },
                Endian.Big, 0, 3
            },
            new object[]
            {
                new byte[]
                {
                    0x03
                },
                Endian.Little, 0, 3
            },
            new object[]
            {
                new byte[]
                {
                    0x00, 0x00, 0x00, 0x10
                },
                Endian.Little, 3, 16
            }
        };

        [TestCaseSource(nameof(ToInt32TestCaseSource))]
        public static void ToInt32Test(byte[] value, Endian endian, long offset, int result)
        {
            var i = value.ToInt32(endian, offset);
            Assert.AreEqual(i, result);
        }


        private static readonly object[] ToInt32ErrorTestCaseSource =
        {
            new object[]
            {
                new byte[] { },
                Endian.Little, 0
            },
            new object[]
            {
                new byte[] { },
                Endian.Big, 0
            },
            new object[]
            {
                new byte[] { 0x03, 0x00, 0x00, 0x00 },
                Endian.Little, 4
            },
            new object[]
            {
                new byte[] { 0x03, 0x00, 0x00, 0x00 },
                Endian.Little, 5
            }
        };

        [TestCaseSource(nameof(ToInt32ErrorTestCaseSource))]
        public static void ToInt32ErrorTest(byte[] value, Endian endian, long offset)
        {
            var errorOccured = false;
            try
            {
                var _ = value.ToInt32(endian, offset);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.IsTrue(errorOccured);
        }

        [TestCase(0x00, 0)]
        [TestCase(0x01, 1)]
        [TestCase(0x12, 18)]
        [TestCase(0xFF, 255)]
        public static void ToInt32OneByteTest(byte value, int result)
        {
            var i = value.ToInt32();
            Assert.AreEqual(i, result);
        }


        private static readonly object[] ToBytesTestCaseSource =
        {
            new object[]
            {
                0, Endian.Little,
                new byte[] { 0x00, 0x00, 0x00, 0x00 }
            },
            new object[]
            {
                4, Endian.Little,
                new byte[] { 0x04, 0x00, 0x00, 0x00 }
            },
            new object[]
            {
                4, Endian.Big,
                new byte[] { 0x00, 0x00, 0x00, 0x04 }
            }
        };

        [TestCaseSource(nameof(ToBytesTestCaseSource))]
        public static void ToBytesTest(int value, Endian endian, byte[] result)
        {
            var bytes = value.ToBytes(endian);
            for (var i = 0; i < 4; i++)
            {
                Assert.AreEqual(bytes[i], result[i]);
            }
        }

        [TestCase(0, -1, -1, true, null)]
        [TestCase(0, -1, 0, true, null)]
        [TestCase(0, -1, 1, true, null)]
        [TestCase(0, -1, 2, true, null)]
        [TestCase(0, 0, -1, true, null)]
        [TestCase(0, 0, 0, true, null)]
        [TestCase(0, 0, 1, false, 0)]
        [TestCase(0, 0, 2, false, 0)]
        [TestCase(0, 1, -1, true, null)]
        [TestCase(0, 1, 0, true, null)]
        [TestCase(0, 1, 1, true, null)]
        [TestCase(0, 1, 2, true, null)]
        [TestCase(6, -1, -1, true, null)]
        [TestCase(6, -1, 0, true, null)]
        [TestCase(6, -1, 1, true, null)]
        [TestCase(6, -1, 2, true, null)]
        [TestCase(6, 0, -1, true, null)]
        [TestCase(6, 0, 0, true, null)]
        [TestCase(6, 0, 1, false, 6)]
        [TestCase(6, 0, 2, false, 6)]
        [TestCase(6, 1, -1, true, null)]
        [TestCase(6, 1, 0, true, null)]
        [TestCase(6, 1, 1, true, null)]
        [TestCase(6, 1, 2, true, null)]
        [TestCase(123456, -1, 0, true, null)]
        [TestCase(123456, -1, 1, true, null)]
        [TestCase(123456, -1, 2, true, null)]
        [TestCase(123456, -1, 5, true, null)]
        [TestCase(123456, -1, 6, true, null)]
        [TestCase(123456, -1, 7, true, null)]
        [TestCase(123456, 0, 0, true, null)]
        [TestCase(123456, 0, 1, false, 6)]
        [TestCase(123456, 0, 2, false, 56)]
        [TestCase(123456, 0, 5, false, 23456)]
        [TestCase(123456, 0, 6, false, 123456)]
        [TestCase(123456, 0, 7, false, 123456)]
        [TestCase(123456, 1, 0, true, null)]
        [TestCase(123456, 1, 1, false, 5)]
        [TestCase(123456, 1, 2, false, 45)]
        [TestCase(123456, 1, 5, false, 12345)]
        [TestCase(123456, 1, 6, false, 12345)]
        [TestCase(123456, 5, 0, true, null)]
        [TestCase(123456, 5, 1, false, 1)]
        [TestCase(123456, 5, 2, false, 1)]
        [TestCase(123456, 6, 0, true, null)]
        [TestCase(123456, 6, 1, true, null)]
        [TestCase(123456, 6, 2, true, null)]
        public static void SubIntTest(int value, int beginColumn, int length, bool isError, int answer)
        {
            var errorOccured = false;
            var result = 0;
            try
            {
                result = value.SubInt(beginColumn, length);
            }
            catch
            {
                errorOccured = true;
            }

            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            Assert.AreEqual(result, answer);
        }
    }
}
