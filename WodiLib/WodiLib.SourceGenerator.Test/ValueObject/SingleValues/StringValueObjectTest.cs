using System;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.SourceGenerator.Test.ValueObject.SingleValues
{
    public static class SingleValueObjectTests
    {
        [SetUp]
        public static void SetUp()
        {
            // SJISを扱えるようにする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // Simple: no constraint
        [TestCase(ObjectType.Simple, null, true)]
        [TestCase(ObjectType.Simple, "", false)]
        [TestCase(ObjectType.Simple, "AbCdEfG", false)]
        // AToG: Pattern constraint "abcdefg"(ignoreCase)
        [TestCase(ObjectType.AToG, null, true)]
        [TestCase(ObjectType.AToG, "abcdefgh", false)]
        [TestCase(ObjectType.AToG, "ABCDEFg", false)]
        [TestCase(ObjectType.AToG, "ABCEFg", true)]
        // HttpScheme: Pattern constraint "^https?://"
        [TestCase(ObjectType.HttpScheme, null, true)]
        [TestCase(ObjectType.HttpScheme, "http://localhost:8080/example", false)]
        [TestCase(ObjectType.HttpScheme, "https://localhost:8080", false)]
        [TestCase(ObjectType.HttpScheme, "HTTP://localhost:8080/example", true)]
        // SJis: ByteLength constraint 5-10 and Length constraint 4-8
        [TestCase(ObjectType.SJis, null, true)]
        [TestCase(ObjectType.SJis, "1234", true)]
        [TestCase(ObjectType.SJis, "あいう", true)]
        [TestCase(ObjectType.SJis, "あいう7", false)]
        [TestCase(ObjectType.SJis, "あいうえ", false)]
        [TestCase(ObjectType.SJis, "あいうえお", false)]
        [TestCase(ObjectType.SJis, "あいうえお1", true)]
        [TestCase(ObjectType.SJis, "12345678", false)]
        [TestCase(ObjectType.SJis, "123456789", true)]
        // Utf8: ByteLength constraint -13 and Length 1-
        [TestCase(ObjectType.Utf8, null, true)]
        [TestCase(ObjectType.Utf8, "", true)]
        [TestCase(ObjectType.Utf8, "1", false)]
        [TestCase(ObjectType.Utf8, "1234567890123", false)]
        [TestCase(ObjectType.Utf8, "12345678901234", true)]
        [TestCase(ObjectType.Utf8, "あいうえ3", false)]
        [TestCase(ObjectType.Utf8, "あいうえ34", true)]
        public static void ConstructorTest(ObjectType type, string initValue, bool isError)
        {
            /* バリデーションが正しく行われること */

            var errorOccued = false;
            try
            {
                var instance = CreateStringValueObject(type, initValue);
                Assert.NotNull(instance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorOccued = true;
            }

            Assert.AreEqual(isError, errorOccued);
        }

        private static dynamic? CreateStringValueObject(ObjectType type, string initValue)
        {
            return type switch
            {
                ObjectType.Simple => new SimpleStringValueObject(initValue),
                ObjectType.AToG => new AToGStringValueObject(initValue),
                ObjectType.HttpScheme => new HttpSchemeStringValueObject(initValue),
                ObjectType.SJis => new SJisStringValueObject(initValue),
                ObjectType.Utf8 => new Utf8StringValueObject(initValue),
                _ => null
            };
        }

        [Test]
        public static void ToStringTest()
        {
            /* 元の文字列に変換されること。 */

            var testString = "Test string is NotNull.";

            var instance = new SimpleStringValueObject(testString);

            var toStringResult = instance.ToString();

            var equalsOriginalValue = toStringResult.Equals(testString);
            Assert.IsTrue(equalsOriginalValue);
        }

        [TestCase("abc", "abc", true)]
        [TestCase("あいうえ", "あいうえ", true)]
        [TestCase("あいうえ", "あいう", false)]
        [TestCase("", null, false)]
        [TestCase("abc", null, false)]
        [TestCase("abc", "ab\nc", false)]
        public static void EqualsSameClassTest(string left, string right, bool answer)
        {
            /* 文字列としての比較が正しく行われること */

            var leftInstance = new SimpleStringValueObject(left);
            var rightInstance = right is null ? null : new SimpleStringValueObject(right);

            var equalsResult = leftInstance.Equals(rightInstance);

            Assert.AreEqual(answer, equalsResult);
        }

        [TestCase(ObjectType.Simple, "abc", true)]
        [TestCase(ObjectType.Simple, null, false)]
        [TestCase(ObjectType.Utf8, "abc", false)]
        public static void EqualsOtherClassTest(ObjectType rightType, string right, bool answer)
        {
            /* 異なる文字列値オブジェクトクラスの場合同じ文字列でも一致しないと判定されること */

            var leftInstance = new SimpleStringValueObject("abc");
            object? rightInstance = right is null ? null : CreateStringValueObject(rightType, right);

            var equalsResult = leftInstance.Equals(rightInstance);

            Assert.AreEqual(answer, equalsResult);
        }

        [Test]
        public static void GetHashCodeTest()
        {
            /* 値が取得できること */

            var instance = new SimpleStringValueObject("testValue");
            var _ = instance.GetHashCode();
            // no error
            Assert.True(true);
        }

        [TestCase("abc", "abc", true)]
        [TestCase("あいうえ", "あいうえ", true)]
        [TestCase("あいうえ", "あいう", false)]
        [TestCase("", null, false)]
        [TestCase("abc", null, false)]
        [TestCase("abc", "ab\nc", false)]
        public static void OperationEqualTest(string left, string right, bool answer)
        {
            /* 文字列としての比較が正しく行われること */

            var leftInstance = new SimpleStringValueObject(left);
            var rightInstance = right is null ? null : new SimpleStringValueObject(right);

            var equalsResult = leftInstance == rightInstance;

            Assert.AreEqual(answer, equalsResult);
        }

        [TestCase("abc", "abc", false)]
        [TestCase("あいうえ", "あいうえ", false)]
        [TestCase("あいうえ", "あいう", true)]
        [TestCase("", null, true)]
        [TestCase("abc", null, true)]
        [TestCase("abc", "ab\nc", true)]
        public static void OperationNotEqualTest(string left, string right, bool answer)
        {
            /* 文字列としての比較が正しく行われること */

            var leftInstance = new SimpleStringValueObject(left);
            var rightInstance = right is null ? null : new SimpleStringValueObject(right);

            var equalsResult = leftInstance != rightInstance;

            Assert.AreEqual(answer, equalsResult);
        }

        [TestCase("abc", "abc")]
        [TestCase("aabbcc", "abc")]
        [TestCase("abc", "aabbcc")]
        [TestCase("abc", null)]
        public static void CompareToTest(string left, string right)
        {
            /* 文字列としての比較が正しく行われること */

            var leftInstance = new ComparableStringValueObject(left);
            var rightInstance = right is null ? null : new ComparableStringValueObject(right);

            var expected = string.Compare(left, right, StringComparison.Ordinal);

            var compareToResult = leftInstance.CompareTo(rightInstance);
            Assert.AreEqual(expected, compareToResult);
        }

        [Test]
        public static void ExplicitCastTest()
        {
            /* 明示的な型変換が正しく行えること */
            var testValue = "Cast String Value";

            var instance = (ExplicitCastableStringValueObject)testValue;
            Assert.IsTrue(instance.RawValue.Equals(testValue));

            var castedString = (string)instance;
            Assert.IsTrue(castedString.Equals(testValue));
        }

        [Test]
        public static void ImplicitCastTest()
        {
            /* 暗黙的な型変換が正しく行えること */
            var testValue = "Cast String Value";

            ImplicitCastableStringValueObject instance = testValue;
            Assert.IsTrue(instance.RawValue.Equals(testValue));

            string castedString = instance;
            Assert.IsTrue(castedString.Equals(testValue));
        }
    }

    #region TestClasses

    [StringValueObject]
    public partial class SimpleStringValueObject
    {
    }

    [StringValueObject(Pattern = "abcdefg",
        PatternOption = RegexOptions.IgnoreCase)]
    public partial class AToGStringValueObject
    {
    }

    [StringValueObject(Pattern = "^https?://.*")]
    public partial class HttpSchemeStringValueObject
    {
    }

    [StringValueObject(ByteLengthEncoding = "Shift_JIS",
        ByteMaxLength = 10, ByteMinLength = 5,
        MaxLength = 8, MinLength = 4)]
    public partial class SJisStringValueObject
    {
    }

    [StringValueObject(ByteLengthEncoding = "utf-8",
        ByteMaxLength = 13, MinLength = 1)]
    public partial class Utf8StringValueObject
    {
    }

    [StringValueObject(IsComparable = true)]
    public partial class ComparableStringValueObject
    {
    }

    [StringValueObject(CastType = CastType.Explicit)]
    public partial class ExplicitCastableStringValueObject
    {
    }

    [StringValueObject(CastType = CastType.Implicit)]
    public partial class ImplicitCastableStringValueObject
    {
    }

    [StringValueObject(
        OverrideBasicMethods = true,
        ImplementEquatable = true,
        CastType = CastType.Explicit,
        PropertyName = "Value",
        IsComparable = true,
        IsAllowEmpty = false,
        IsAllowNewLine = false,
        Pattern = "^[A-Za-z]+$",
        SafetyPattern = "^[A-Z]$",
        PatternOption = RegexOptions.IgnoreCase,
        ByteLengthEncoding = "utf-8",
        ByteMaxLength = 255,
        ByteMinLength = 10,
        MaxLength = 100,
        MinLength = 1
    )]
    public partial class FullPropertyStringValueObject
    {
    }

    public enum ObjectType
    {
        Simple,
        AToG,
        HttpScheme,
        SJis,
        Utf8
    }

    #endregion
}
