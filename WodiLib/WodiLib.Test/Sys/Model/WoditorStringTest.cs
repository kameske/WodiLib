using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class WoditorStringTest
    {
        private static readonly object[] TestCaseSource =
        {
            new object[]
            {
                new byte[]
                {
                    0x05, 0x00, 0x00, 0x00,
                    0x82, 0xC8, 0x82, 0xB5, 0x00
                },
                "なし", 0
            },
            new object[]
            {
                new byte[]
                {
                    0x12, 0x00, 0x00, 0x00,
                    0x57, 0x6f, 0x6c, 0x66, 0x0d, 0x0a, 0x52, 0x50, 0x47, 0x0d, 0x0a, 0x45, 0x64, 0x69, 0x74, 0x6f,
                    0x72, 0x00
                },
                $"Wolf{Environment.NewLine}RPG{Environment.NewLine}Editor", 0
            },
            new object[]
            {
                new byte[]
                {
                    0x31, 0x00, 0x00, 0x00,
                    0x97, 0x5B, 0x88, 0xEA, 0x0A,
                    0x81, 0x75, 0x82, 0xE2, 0x82, 0xA0, 0x81, 0x41, 0x82, 0xDA, 0x82, 0xAD, 0x5C, 0x72, 0x5B, 0x97,
                    0x5B, 0x88, 0xEA, 0x2C, 0x82, 0xE4, 0x82, 0xA4, 0x82, 0xA2, 0x82, 0xBF, 0x5D, 0x82, 0xC1, 0x82,
                    0xC4, 0x82, 0xA2, 0x82, 0xA4, 0x82, 0xF1, 0x82, 0xBE, 0x81, 0x49, 0x00
                },
                "夕一\n「やあ、ぼく\\r[夕一,ゆういち]っていうんだ！", 0
            },
            new object[]
            {
                new byte[]
                {
                    0x12, 0x00, 0x00, 0x00,
                    0x57, 0x6f, 0x6c, 0x66, 0x0d, 0x0a, 0x52, 0x50, 0x47, 0x0d, 0x0a, 0x45, 0x64, 0x69, 0x74, 0x6f,
                    0x72, 0x00,
                    0x05, 0x00, 0x00, 0x00,
                    0x82, 0xC8, 0x82, 0xB5, 0x00
                },
                "なし", 22
            },
            new object[]
            {
                new byte[]
                {
                    0x0F, 0x00, 0x00, 0x00,
                    0x83, 0x65, 0x83, 0x58, 0x83, 0x67, 0x5C, 0x6E, 0x83, 0x65, 0x83, 0x58, 0x83, 0x67, 0x00
                },
                "テスト\\nテスト", 0
            },
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void FromByteArrayTest(byte[] value, string result, long offset)
        {
            var instance = new WoditorString(ref value, offset);

            Assert.AreEqual(instance.String, result);
            Assert.AreEqual(instance.ByteLength, value.Length - offset);

            var instanceStringBytes = instance.StringByte.ToArray();
            for (var i = 0; i < instance.ByteLength; i++)
            {
                Assert.AreEqual(instanceStringBytes[i], value[offset + i]);
            }
        }

        [TestCaseSource(nameof(TestCaseSource))]
        public static void FromStringTest(byte[] result, string value, long offset)
        {
            var instance = new WoditorString(value);

            Assert.AreEqual(instance.String, value);
            Assert.AreEqual(instance.ByteLength, result.Length - offset);

            var instanceStringBytes = instance.StringByte.ToArray();
            for (var i = 0; i < instance.ByteLength; i++)
            {
                Assert.AreEqual(instanceStringBytes[i], result[offset + i]);
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new WoditorString("String");
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}