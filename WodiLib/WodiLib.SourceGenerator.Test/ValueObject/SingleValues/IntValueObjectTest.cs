using System;
using System.ComponentModel;
using System.Globalization;
using NUnit.Framework;
using WodiLib.SourceGenerator.Test.ValueObject.SingleValues;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.SourceGenerator.Test.ValueObject.SingleValues
{
    public static partial class IntValueObjectTests
    {
        [TestCase(ObjectType.Simple, int.MinValue, true)]
        [TestCase(ObjectType.Simple, 0, true)]
        [TestCase(ObjectType.Simple, int.MaxValue, true)]
        [TestCase(ObjectType.ValueLimited, LimitMinValue - 1, false)]
        [TestCase(ObjectType.ValueLimited, LimitMinValue, true)]
        [TestCase(ObjectType.ValueLimited, LimitMaxValue, true)]
        [TestCase(ObjectType.ValueLimited, LimitMaxValue + 1, false)]
        [TestCase(ObjectType.MinValueLimited, LimitMinValue - 1, false)]
        [TestCase(ObjectType.MinValueLimited, LimitMinValue, true)]
        [TestCase(ObjectType.MinValueLimited, LimitMaxValue + 1, true)]
        [TestCase(ObjectType.MaxValueLimited, LimitMinValue - 1, true)]
        [TestCase(ObjectType.MaxValueLimited, LimitMaxValue, true)]
        [TestCase(ObjectType.MaxValueLimited, LimitMaxValue + 1, false)]
        public static void ValueLimitTest(ObjectType testInstanceType, int setValue, bool expectedNotError)
        {
            var actualNotError = true;
            try
            {
                switch (testInstanceType)
                {
                    case ObjectType.Simple:
                    {
                        var _ = new SimpleIntValueObject(setValue);
                        break;
                    }
                    case ObjectType.ValueLimited:
                    {
                        var _ = new ValueLimitedIntValueObject(setValue);
                        break;
                    }
                    case ObjectType.MaxValueLimited:
                    {
                        var _ = new MaxValueLimitedIntValueObject(setValue);
                        break;
                    }
                    case ObjectType.MinValueLimited:
                    {
                        var _ = new MinValueLimitedIntValueObject(setValue);
                        break;
                    }
                    default:
                        Assert.Fail();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                actualNotError = false;
            }

            Assert.AreEqual(expectedNotError, actualNotError);
        }

        [Test]
        public static void RawValueGetTest()
        {
            var setValue = LimitMaxValue;
            var obj = new SimpleIntValueObject(setValue);
            Assert.AreEqual(setValue, obj.RawValue);
        }

        [Test]
        public static void RawValuePropertyNameCustomizeTest()
        {
            var setValue = LimitMinValue;
            var obj = new RowValuePropertyNameChangedIntValueObject(setValue);
            Assert.AreEqual(setValue, obj.CustomName);
        }

        [Test]
        public static void EqualsSameInstanceTest()
        {
            var instance = new SimpleIntValueObject(LimitMaxValue);
            Assert.IsTrue(instance.Equals(instance));
        }

        [TestCase(LimitMaxValue, true)]
        [TestCase(LimitMinValue, false)]
        [TestCase(null, false)]
        public static void EqualsTest(int? otherValue, bool expectedEquals)
        {
            var left = new SimpleIntValueObject(LimitMaxValue);
            var right = otherValue is null
                ? null
                : new SimpleIntValueObject(otherValue.Value);

            var actualEquals = left.Equals(right);
            Assert.AreEqual(expectedEquals, actualEquals);
        }

        public static void ExplicitCastFromIntTest()
        {
            var originalValue = LimitMaxValue;

            var castedObject = (ExplicitCastableIntValueObject)originalValue;
            Assert.AreEqual(originalValue, castedObject.RawValue);

            // var failureCasted = (SimpleIntValueObject) originalValue;    // Error
        }

        [Test]
        public static void ExplicitCastToIntTest()
        {
            var originalValue = LimitMaxValue;
            var originalObj = new ExplicitCastableIntValueObject(originalValue);

            var castedInt = (int)originalObj;
            Assert.AreEqual(originalValue, castedInt);

            // var failureCasted = (int) (new SimpleIntValueObject(originalValue));     // Error
        }

        [Test]
        public static void ImplicitCastFromIntTest()
        {
            var originalValue = LimitMinValue;

            ImplicitCastableIntValueObject castedObject = originalValue;
            Assert.AreEqual(originalValue, castedObject.RawValue);

            // SimpleIntValueObject failureCasted = originalValue;    // Error
        }

        [Test]
        public static void ImplicitCastToIntTest()
        {
            var originalValue = LimitMinValue;
            var originalObj = new ImplicitCastableIntValueObject(originalValue);

            int castedInt = originalObj;
            Assert.AreEqual(originalValue, castedInt);

            // int failureCasted = new SimpleIntValueObject(originalValue);     // Error
        }

        [Test]
        public static void FormattableTest1()
        {
            var originalValue = LimitMaxValue;
            var testFormat = "X4";
            var expected = originalValue.ToString(testFormat);

            var instance = new SimpleIntValueObject(originalValue);

            var formatted = instance.ToString(testFormat);

            Assert.AreEqual(expected, formatted);
        }

        [Test]
        public static void FormattableTest2()
        {
            var originalValue = LargeValue;
            var provider = CultureInfo.CreateSpecificCulture("ja-jp");
            var expected = originalValue.ToString(provider);

            var instance = new SimpleIntValueObject(originalValue);

            var formatted = instance.ToString(provider);

            Assert.AreEqual(expected, formatted);
        }

        [Test]
        public static void FormattableTest3()
        {
            var originalValue = LargeValue;
            var testFormat = "C";
            var provider = CultureInfo.CreateSpecificCulture("ja-jp");
            var expected = originalValue.ToString(testFormat, provider);

            var instance = new SimpleIntValueObject(originalValue);

            var formatted = instance.ToString(testFormat, provider);

            Assert.AreEqual(expected, formatted);
        }

        [Test]
        public static void AddTest()
        {
            var left = new IntAddableIntValueObject(20);
            var right = 50;
            var _ = left + right;
        }

        public const int LargeValue = 1234567;
        public const int LimitMaxValue = 20;
        public const int LimitMinValue = 5;

        public enum ObjectType
        {
            Simple,
            ValueLimited,
            MaxValueLimited,
            MinValueLimited
        }

        [IntValueObject]
        public partial class InnerSimpleIntValueObject
        {
        }

        [IntValueObject]
        internal partial class InternalProtectedInnerSimpleValueObject
        {
        }

        [IntValueObject]
        private partial class ProtectedPrivateInnerSimpleValueObject
        {
        }

        [IntValueObject]
        public abstract partial class AbstractInnerSimpleValueObject
        {
        }

        [IntValueObject]
        public partial struct StructInnerSimpleValueObject
        {
        }

        [IntValueObject]
        public partial record RecordInnerSimpleValueObject;
    }

    [IntValueObject]
    public partial class SimpleIntValueObject
    {
    }

    [IntValueObject(MaxValue = IntValueObjectTests.LimitMaxValue, MinValue = IntValueObjectTests.LimitMinValue)]
    public partial class ValueLimitedIntValueObject
    {
    }

    [IntValueObject(MaxValue = IntValueObjectTests.LimitMaxValue)]
    public partial class MaxValueLimitedIntValueObject
    {
    }

    [IntValueObject(MinValue = IntValueObjectTests.LimitMinValue)]
    public partial class MinValueLimitedIntValueObject
    {
    }

    [IntValueObject(PropertyName = "CustomName")]
    public partial class RowValuePropertyNameChangedIntValueObject
    {
    }

    [IntValueObject(CastType = CastType.Explicit)]
    public partial class ExplicitCastableIntValueObject
    {
    }

    [IntValueObject(CastType = CastType.Implicit)]
    public partial class ImplicitCastableIntValueObject
    {
    }

    [IntValueObject(IsUseBasicFormattable = true)]
    public partial class FormattableIntValueObject
    {
    }

    [IntValueObject(Operations = IntegralNumericOperation.And
                                 | IntegralNumericOperation.Or
                                 | IntegralNumericOperation.Xor
                                 | IntegralNumericOperation.Complement)]
    public partial class BoolOperatableIntValueObject
    {
    }

    [IntValueObject(Operations = IntegralNumericOperation.Compare)]
    public partial class ComparableIntValueObject
    {
    }

    [IntValueObject(AddAndSubtractTypes = new[] { typeof(IntAddableIntValueObject), typeof(int) })]
    //[IntValueObject(Operations = IntegralNumericOperation.Compare)]
    public partial class IntAddableIntValueObject
    {
    }

    [IntValueObject(
        MaxValue = 99,
        MinValue = 1,
        SafetyMaxValue = 80,
        SafetyMinValue = 10,
        Operations = IntegralNumericOperation.IncreaseAndDecreasable
                     | IntegralNumericOperation.AddAndSubtractable
                     | IntegralNumericOperation.MultipleAndDivide
                     | IntegralNumericOperation.Modulo
                     | IntegralNumericOperation.Complement
                     | IntegralNumericOperation.Shift
                     | IntegralNumericOperation.And
                     | IntegralNumericOperation.Or
                     | IntegralNumericOperation.Xor
                     | IntegralNumericOperation.Compare,
        AddAndSubtractTypes = new[] { typeof(int) },
        MultipleAndDivideOtherTypes = new[] { typeof(FullPropertyIntValueObject), typeof(int), typeof(short) },
        CompareOtherTypes = new[] { typeof(byte) },
        IsUseBasicFormattable = true,
        PropertyName = "Value",
        IsComparable = true
    )]
    public partial class FullPropertyIntValueObject
    {
    }

    [ExtendIntValueObjectAttribute(SafetyMinValue = -900)]
    public partial class ExtendIntValueObject
    {
    }

    public class ExtendIntValueObjectAttribute : IntValueObjectAttribute
    {
        [DefaultValue(9998)] public override int MaxValue { get; init; }
    }

    [ExtendIntValueObjectAttribute(SafetyMinValue = -225)]
    public partial class MoreExtendIntValueObject : ExtendIntValueObject
    {
    }
}

namespace TestNameSpace
{
    public class ExExtendIntValueObjectAttribute : ExtendIntValueObjectAttribute
    {
        [DefaultValue(-20)] public override int MinValue { get; init; }
    }

    [ExExtendIntValueObjectAttribute(SafetyMinValue = -900)]
    public partial class ExExtendIntValueObject
    {
    }
}
