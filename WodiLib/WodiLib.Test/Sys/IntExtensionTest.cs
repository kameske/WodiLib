using System;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class IntExtensionTest
    {
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
            },
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
                new byte[] {0x03, 0x00, 0x00, 0x00},
                Endian.Little, 4
            },
            new object[]
            {
                new byte[] {0x03, 0x00, 0x00, 0x00},
                Endian.Little, 5
            },
        };

        [TestCaseSource(nameof(ToInt32ErrorTestCaseSource))]
        public static void ToInt32ErrorTest(byte[] value, Endian endian, long offset)
        {
            var errorOccured = false;
            try
            {
                var _ = value.ToInt32(endian, offset);
            }
            catch (ArgumentException)
            {
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
                new byte[] {0x00, 0x00, 0x00, 0x00}
            },
            new object[]
            {
                4, Endian.Little,
                new byte[] {0x04, 0x00, 0x00, 0x00}
            },
            new object[]
            {
                4, Endian.Big,
                new byte[] {0x00, 0x00, 0x00, 0x04}
            },
        };
        [TestCaseSource(nameof(ToBytesTestCaseSource))]
        public static void ToBytesTest(int value, Endian endian, byte[] result)
        {
            var bytes = value.ToBytes(endian);
            for(var i=0; i<4; i++)
            {
                Assert.AreEqual(bytes[i], result[i]);
            }
        }
    }
}