using NUnit.Framework;
using WodiLib.SourceGenerator.Operation.Attributes;
using WodiLib.SourceGenerator.Operation.Enums;

namespace WodiLib.SourceGenerator.Test.Operation
{
    [TestFixture]
    public static partial class ShiftOperationTest
    {
        public const int LeftInt = 0x60_32_E0_0F;
        public const int RightInt = 3;
        public static AllOperationOverrideObject LeftItem { get; } = new(LeftInt);

        [Test]
        public static void LeftShiftTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem << RightInt;
            var expectedItem = LeftInt << RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void LeftShiftTest_OnlyOperationOverridden()
        {
            var leftItem = new LeftShiftableObject(LeftInt);

            var calculatedItem = leftItem << RightInt;
            var expectedItem = LeftInt << RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void RightShiftTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem >> RightInt;
            var expectedItem = LeftInt >> RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void RightShiftTest_OnlyOperationOverridden()
        {
            var leftItem = new RightShiftableObject(LeftInt);

            var calculatedItem = leftItem >> RightInt;
            var expectedItem = LeftInt >> RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [ShiftOperate(Operation = ShiftOperationType.Both, InnerCastType = typeof(int))]
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
    }

    [ShiftOperate(Operation = ShiftOperationType.Left, InnerCastType = typeof(int))]
    public partial class LeftShiftableObject
    {
        private readonly int value;

        public LeftShiftableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(LeftShiftableObject src) => src.value;
        public static explicit operator LeftShiftableObject(int src) => new(src);
    }

    [ShiftOperate(Operation = ShiftOperationType.Right, InnerCastType = typeof(int))]
    public partial class RightShiftableObject
    {
        private readonly int value;

        public RightShiftableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(RightShiftableObject src) => src.value;
        public static explicit operator RightShiftableObject(int src) => new(src);
    }
}
