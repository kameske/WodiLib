using NUnit.Framework;
using WodiLib.SourceGenerator.Operation.Attributes;
using WodiLib.SourceGenerator.Operation.Enums;

namespace WodiLib.SourceGenerator.Test.Operation
{
    [TestFixture]
    public static partial class UnaryOperationTest
    {
        public const int SrcInt = 20;

        [Test]
        public static void IncreaseTest_AllOperationOverridden()
        {
            var srcItem = new AllOperationOverrideObject(SrcInt);
            srcItem++;
            var expectedItem = SrcInt;
            expectedItem++;
            Assert.AreEqual(expectedItem, (int)srcItem);
        }

        [Test]
        public static void IncreaseTest_OnlyOperationOverridden()
        {
            var srcItem = new IncreasableObject(SrcInt);
            srcItem++;
            var expectedItem = SrcInt;
            expectedItem++;
            Assert.AreEqual(expectedItem, (int)srcItem);
        }

        [Test]
        public static void DecreaseTest_AllOperationOverridden()
        {
            var srcItem = new AllOperationOverrideObject(SrcInt);
            srcItem--;
            var expectedItem = SrcInt;
            expectedItem--;
            Assert.AreEqual(expectedItem, (int)srcItem);
        }

        [Test]
        public static void DecreaseTest_OnlyOperationOverridden()
        {
            var srcItem = new DecreasableObject(SrcInt);
            srcItem--;
            var expectedItem = SrcInt;
            expectedItem--;
            Assert.AreEqual(expectedItem, (int)srcItem);
        }

        [Test]
        public static void ComplementTest_AllOperationOverridden()
        {
            var srcItem = new AllOperationOverrideObject(SrcInt);
            var calculated = ~srcItem;
            var expectedItem = ~SrcInt;
            Assert.AreEqual(expectedItem, (int)calculated);
        }

        [Test]
        public static void ComplementTest_OnlyOperationOverridden()
        {
            var srcItem = new ComplementableObject(SrcInt);
            var calculated = ~srcItem;
            var expectedItem = ~SrcInt;
            Assert.AreEqual(expectedItem, (int)calculated);
        }

        [UnaryOperate(
            Operation = UnaryOperationType.Xecrease
                        | UnaryOperationType.Complement,
            InnerCastType = typeof(int))]
        public partial class AllOperationOverrideObject
        {
            private readonly int value;

            public AllOperationOverrideObject(int value)
            {
                this.value = value;
            }

            public static explicit operator int(AllOperationOverrideObject src) => src.value;
            public static explicit operator AllOperationOverrideObject(int src) => new(src);
        }

        [UnaryOperate(
            Operation = UnaryOperationType.Increase,
            InnerCastType = typeof(int))]
        public partial class IncreasableObject
        {
            private readonly int value;

            public IncreasableObject(int value)
            {
                this.value = value;
            }

            public static explicit operator int(IncreasableObject src) => src.value;
            public static explicit operator IncreasableObject(int src) => new(src);
        }

        [UnaryOperate(
            Operation = UnaryOperationType.Decrease,
            InnerCastType = typeof(int))]
        public partial class DecreasableObject
        {
            private readonly int value;

            public DecreasableObject(int value)
            {
                this.value = value;
            }

            public static explicit operator int(DecreasableObject src) => src.value;
            public static explicit operator DecreasableObject(int src) => new(src);
        }

        [UnaryOperate(
            Operation = UnaryOperationType.Complement,
            InnerCastType = typeof(int))]
        public partial class ComplementableObject
        {
            private readonly int value;

            public ComplementableObject(int value)
            {
                this.value = value;
            }

            public static explicit operator int(ComplementableObject src) => src.value;
            public static explicit operator ComplementableObject(int src) => new(src);
        }
    }
}
