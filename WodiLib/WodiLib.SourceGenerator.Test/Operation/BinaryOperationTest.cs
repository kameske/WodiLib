using NUnit.Framework;
using WodiLib.SourceGenerator.Operation.Attributes;
using WodiLib.SourceGenerator.Operation.Enums;

namespace WodiLib.SourceGenerator.Test.Operation
{
    [TestFixture]
    public static partial class BinaryOperationTest
    {
        public const int LeftInt = 20;
        public const int RightInt = 5;
        public static AllOperationOverrideObject LeftItem { get; } = new(LeftInt);

        [Test]
        public static void AddTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem + RightInt;
            var expectedItem = LeftInt + RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void AddTest_OnlyOperationOverridden()
        {
            var leftItem = new AddableObject(LeftInt);
            var rightItem = new AddableObject(RightInt);

            var calculatedItem = leftItem + rightItem;
            var expectedItem = LeftInt + RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void AddTest_AddAndSubtractOperationOverridden()
        {
            var leftItem = new AddAndSubtractableObject(LeftInt);
            var rightItem = new AddAndSubtractableObject(RightInt);

            var calculatedItem = leftItem + rightItem;
            var expectedItem = LeftInt + RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void AddTest_FourArithmeticOperationsOverridden()
        {
            var leftItem = new FourArithmeticOperationsOverriddenObject(LeftInt);
            var rightItem = new FourArithmeticOperationsOverriddenObject(RightInt);

            var calculatedItem = leftItem + rightItem;
            var expectedItem = LeftInt + RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void SubtractTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem - RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt - RightInt);
        }

        [Test]
        public static void SubtractTest_OnlyOperationOverridden()
        {
            var leftItem = new SubtractableObject(LeftInt);
            var rightItem = new SubtractableObject(RightInt);

            var calculatedItem = leftItem - rightItem;
            var expectedItem = LeftInt - RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void SubtractTest_AddAndSubtractOperationOverridden()
        {
            var leftItem = new AddAndSubtractableObject(LeftInt);
            var rightItem = new AddAndSubtractableObject(RightInt);

            var calculatedItem = leftItem - rightItem;
            var expectedItem = LeftInt - RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void SubtractTest_FourArithmeticOperationsOverridden()
        {
            var leftItem = new FourArithmeticOperationsOverriddenObject(LeftInt);
            var rightItem = new FourArithmeticOperationsOverriddenObject(RightInt);

            var calculatedItem = leftItem - rightItem;
            var expectedItem = LeftInt - RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void MultipleTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem * RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt * RightInt);
        }

        [Test]
        public static void MultipleTest_OnlyOperationOverridden()
        {
            var leftItem = new MultipliableObject(LeftInt);
            var rightItem = new MultipliableObject(RightInt);

            var calculatedItem = leftItem * rightItem;
            var expectedItem = LeftInt * RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void MultipleTest_MultipleAndDivideOperationOverridden()
        {
            var leftItem = new MultipleAndDividableObject(LeftInt);
            var rightItem = new MultipleAndDividableObject(RightInt);

            var calculatedItem = leftItem * rightItem;
            var expectedItem = LeftInt * RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void MultipleTest_FourArithmeticOperationsOverridden()
        {
            var leftItem = new FourArithmeticOperationsOverriddenObject(LeftInt);
            var rightItem = new FourArithmeticOperationsOverriddenObject(RightInt);

            var calculatedItem = leftItem * rightItem;
            var expectedItem = LeftInt * RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void DivideTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem / RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt / RightInt);
        }

        [Test]
        public static void DivideTest_OnlyOperationOverridden()
        {
            var leftItem = new DividableObject(LeftInt);
            var rightItem = new DividableObject(RightInt);

            var calculatedItem = leftItem / rightItem;
            var expectedItem = LeftInt / RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void DivideTest_MultipleAndDivideOperationOverridden()
        {
            var leftItem = new MultipleAndDividableObject(LeftInt);
            var rightItem = new MultipleAndDividableObject(RightInt);

            var calculatedItem = leftItem / rightItem;
            var expectedItem = LeftInt / RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void DivideTest_FourArithmeticOperationsOverridden()
        {
            var leftItem = new FourArithmeticOperationsOverriddenObject(LeftInt);
            var rightItem = new FourArithmeticOperationsOverriddenObject(RightInt);

            var calculatedItem = leftItem / rightItem;
            var expectedItem = LeftInt / RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void ModTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem % RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt % RightInt);
        }

        [Test]
        public static void ModTest_OnlyOperationOverridden()
        {
            var leftItem = new ModulableObject(LeftInt);
            var rightItem = new ModulableObject(RightInt);

            var calculatedItem = leftItem % rightItem;
            var expectedItem = LeftInt % RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void AndTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem & RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt & RightInt);
        }

        [Test]
        public static void AndTest_OnlyOperationOverridden()
        {
            var leftItem = new AndableObject(LeftInt);
            var rightItem = new AndableObject(RightInt);

            var calculatedItem = leftItem & rightItem;
            var expectedItem = LeftInt & RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void OrTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem | RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt | RightInt);
        }

        [Test]
        public static void OrTest_OnlyOperationOverridden()
        {
            var leftItem = new OrableObject(LeftInt);
            var rightItem = new OrableObject(RightInt);

            var calculatedItem = leftItem | rightItem;
            var expectedItem = LeftInt | RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [Test]
        public static void XorTest_AllOperationOverridden()
        {
            var calculatedItem = LeftItem ^ RightInt;
            Assert.AreEqual((int)calculatedItem, LeftInt ^ RightInt);
        }

        [Test]
        public static void XorTest_OnlyOperationOverridden()
        {
            var leftItem = new XorableObject(LeftInt);
            var rightItem = new XorableObject(RightInt);

            var calculatedItem = leftItem ^ rightItem;
            var expectedItem = LeftInt ^ RightInt;
            Assert.AreEqual(expectedItem, (int)calculatedItem);
        }

        [BinaryOperate(
            Operation = BinaryOperationType.FourArithmeticOperations
                        | BinaryOperationType.Modulo
                        | BinaryOperationType.And
                        | BinaryOperationType.Or
                        | BinaryOperationType.Xor,
            OtherTypes = new[] { typeof(int), typeof(byte), typeof(AllOperationOverrideObject) },
            OtherPosition = BinaryOperateOtherPosition.Right,
            ReturnType = typeof(AllOperationOverrideObject),
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
    }

    [BinaryOperate(Operation = BinaryOperationType.Add,
        OtherTypes = new[] { typeof(AddableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(AddableObject),
        InnerCastType = typeof(int))]
    public partial class AddableObject
    {
        private readonly int value;

        public AddableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(AddableObject src) => src.value;
        public static explicit operator AddableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.Subtract,
        OtherTypes = new[] { typeof(SubtractableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(SubtractableObject),
        InnerCastType = typeof(int))]
    public partial class SubtractableObject
    {
        private readonly int value;

        public SubtractableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(SubtractableObject src) => src.value;
        public static explicit operator SubtractableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.AddAndSubtract,
        OtherTypes = new[] { typeof(AddAndSubtractableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(AddAndSubtractableObject),
        InnerCastType = typeof(int))]
    public partial class AddAndSubtractableObject
    {
        private readonly int value;

        public AddAndSubtractableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(AddAndSubtractableObject src) => src.value;
        public static explicit operator AddAndSubtractableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.Multiple,
        OtherTypes = new[] { typeof(MultipliableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(MultipliableObject),
        InnerCastType = typeof(int))]
    public partial class MultipliableObject
    {
        private readonly int value;

        public MultipliableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(MultipliableObject src) => src.value;
        public static explicit operator MultipliableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.Divide,
        OtherTypes = new[] { typeof(DividableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(DividableObject),
        InnerCastType = typeof(int))]
    public partial class DividableObject
    {
        private readonly int value;

        public DividableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(DividableObject src) => src.value;
        public static explicit operator DividableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.MultipleAndDivide,
        OtherTypes = new[] { typeof(MultipleAndDividableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(MultipleAndDividableObject),
        InnerCastType = typeof(int))]
    public partial class MultipleAndDividableObject
    {
        private readonly int value;

        public MultipleAndDividableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(MultipleAndDividableObject src) => src.value;
        public static explicit operator MultipleAndDividableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.FourArithmeticOperations,
        OtherTypes = new[] { typeof(FourArithmeticOperationsOverriddenObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(FourArithmeticOperationsOverriddenObject),
        InnerCastType = typeof(int))]
    public partial class FourArithmeticOperationsOverriddenObject
    {
        private readonly int value;

        public FourArithmeticOperationsOverriddenObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(FourArithmeticOperationsOverriddenObject src) => src.value;

        public static explicit operator FourArithmeticOperationsOverriddenObject(int src) =>
            new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.Modulo,
        OtherTypes = new[] { typeof(ModulableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(ModulableObject),
        InnerCastType = typeof(int))]
    public partial class ModulableObject
    {
        private readonly int value;

        public ModulableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(ModulableObject src) => src.value;
        public static explicit operator ModulableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.And,
        OtherTypes = new[] { typeof(AndableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(AndableObject),
        InnerCastType = typeof(int))]
    public partial class AndableObject
    {
        private readonly int value;

        public AndableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(AndableObject src) => src.value;
        public static explicit operator AndableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.Or,
        OtherTypes = new[] { typeof(OrableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(OrableObject),
        InnerCastType = typeof(int))]
    public partial class OrableObject
    {
        private readonly int value;

        public OrableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(OrableObject src) => src.value;
        public static explicit operator OrableObject(int src) => new(src);
    }

    [BinaryOperate(Operation = BinaryOperationType.Xor,
        OtherTypes = new[] { typeof(XorableObject) },
        OtherPosition = BinaryOperateOtherPosition.Right,
        ReturnType = typeof(XorableObject),
        InnerCastType = typeof(int))]
    public partial class XorableObject
    {
        private readonly int value;

        public XorableObject(int value)
        {
            this.value = value;
        }

        public static explicit operator int(XorableObject src) => src.value;
        public static explicit operator XorableObject(int src) => new(src);
    }
}
