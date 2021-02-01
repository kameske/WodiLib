using System;
using System.Text;
using System.Text.RegularExpressions;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class SingleStringValueRecordTest
    {
        private static Logger logger;
        private static Logger loggerLocked;

        private static readonly object ConstructorTestLockObject = "";

        private static Encoding ShiftJisEncoding { get; set; }

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ShiftJisEncoding = Encoding.GetEncoding("shift-jis");
            logger = Logger.GetInstance();
            lock (ConstructorTestLockObject)
            {
                loggerLocked = Logger.GetInstance();
            }
        }

        private static readonly object[] ConstructorTestCaseSource =
        {
            /* null */
            new object[] {null, false, false, null, null, null, true},
            new object[] {null, true, false, null, null, null, true},
            new object[] {null, false, true, null, null, null, true},
            new object[] {null, true, true, null, null, null, true},
            /* empty */
            new object[] {"", false, false, null, null, null, true},
            new object[] {"", true, false, null, null, null, false},
            new object[] {"", false, true, null, null, null, true},
            new object[] {"", true, true, null, null, null, false},
            /* new line (LF and CRLF) */
            new object[] {"new\nline", false, false, null, null, null, true},
            new object[] {"new\nline", true, false, null, null, null, true},
            new object[] {"new\nline", false, true, null, null, null, false},
            new object[] {"new\nline", true, true, null, null, null, false},
            new object[] {"new\r\nline", false, false, null, null, null, true},
            new object[] {"new\r\nline", true, false, null, null, null, true},
            new object[] {"new\r\nline", false, true, null, null, null, false},
            new object[] {"new\r\nline", true, true, null, null, null, false},
            /* normal value for 'IsAllowEmpty' and 'IsAllowNewLine' */
            new object[] {"TestString", false, false, null, null, null, false},
            new object[] {"TestString", true, false, null, null, null, false},
            new object[] {"TestString", false, true, null, null, null, false},
            new object[] {"TestString", true, true, null, null, null, false},
            /* regex (error and warning) */
            new object[] {"TestString", false, false, new Regex("^[a-zA-Z]*$"), null, null, false},
            new object[] {"TestString", true, false, new Regex("^[a-zA-Z]*$"), null, new Regex("^[a-z]*$"), false},
            new object[] {"TestString", false, true, new Regex("^[a-zA-Z]*$"), null, new Regex("^[a-zA-Z]*$"), false},
            new object[] {"TestString", true, true, new Regex("^[a-z]*$"), null, null, true},
            new object[] {"TestString", false, false, new Regex("^[a-z]*$"), null, new Regex("^[a-z]*$"), true},
            new object[] {"TestString", false, false, new Regex("^[a-z]*$"), null, new Regex("^[a-zA-Z]*$"), true},
            /* shift-jis length */
            new object[] {"TestString", false, false, null, 9, null, true},
            new object[] {"TestString", true, false, null, 10, null, false},
            new object[] {"テスト文字列", false, true, null, 11, null, true},
            new object[] {"テスト文字列", true, true, null, 12, null, false},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(string value, bool isAllowEmpty,
            bool isAllowNewLine, Regex requireRegex, int? requireSJisByteLengthMax,
            Regex safetyRegex, bool isError)
        {
            lock (ConstructorTestLockObject)
            {
                ValidationOption.IsAllowEmpty = isAllowEmpty;
                ValidationOption.IsAllowNewLine = isAllowNewLine;
                ValidationOption.RequireRegex = requireRegex;
                ValidationOption.RequireSJisByteLengthMax = requireSJisByteLengthMax;
                ValidationOption.SafetyRegex = safetyRegex;

                var errorOccured = false;

                try
                {
                    var _ = new ValidationTestRecord(value);
                }
                catch (Exception ex)
                {
                    loggerLocked.Exception(ex);
                    errorOccured = true;
                }

                // エラーフラグが一致すること
                Assert.AreEqual(errorOccured, isError);
            }
        }

        [TestCase("", "")]
        [TestCase("abc\nabc", "abc\rabc")]
        [TestCase("abc\nabc", "abc\r\nabc")]
        [TestCase("いろはにほへと", " ")]
        public static void GetHashCodeTest(string leftSrc, string rightSrc)
        {
            var left = new MethodTestRecord(leftSrc);
            var right = new MethodTestRecord(rightSrc);

            var leftHashCode = 0;
            var rightHashCode = 0;

            var errorOccured = false;

            try
            {
                leftHashCode = left.GetHashCode();
                rightHashCode = right.GetHashCode();
            }
            catch (Exception ex)
            {
                loggerLocked.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // left と right が等しいとき、HashCode が一致すること
            if (left == right)
            {
                Assert.IsTrue(leftHashCode == rightHashCode);
            }
        }

        [TestCase("String")]
        [TestCase("文字列")]
        [TestCase("")]
        [TestCase("\n")]
        [TestCase("あい\r\nうえお")]
        public static void ToStringTest(string src)
        {
            var instance = new MethodTestRecord(src);
            var value = instance.ToString();
            Assert.IsTrue(value.Equals(src));
        }

        [TestCase("String", "String")]
        [TestCase("String", "string")]
        [TestCase("String", "文字列")]
        [TestCase("", " ")]
        [TestCase("\n", "\r\n")]
        [TestCase("String", null)]
        [TestCase("", null)]
        public static void CompareToTest(string leftSrc, string rightSrc)
        {
            var left = new MethodTestRecord(leftSrc);
            var right = rightSrc is null ? null : new MethodTestRecord(rightSrc);

            var errorOccured = false;
            var result = 0;
            try
            {
                result = left.CompareTo(right);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値であること
            var answer = string.Compare(leftSrc, rightSrc, StringComparison.Ordinal);
            logger.Info($"result: {result}, answer: {answer}");
            Assert.AreEqual(result, answer);
        }

        private static readonly object[] EqualsOtherTestTestCaseSource =
        {
            new object[] {new MethodTestRecord("value"), new MethodTestRecord("value"), true},
            new object[] {new MethodTestRecord("value"), new MethodTestRecord(" "), false},
            new object[] {new MethodTestRecord("new\nline"), new MethodTestRecord("new\nline"), true},
            new object[] {new MethodTestRecord("new\r\nline"), new MethodTestRecord("new\nline"), false},
            new object[] {new MethodTestRecord("テスト文字列"), new MethodTestRecord("テスト文字列"), true},
            new object[] {new MethodTestRecord("テスト文字列"), new MethodTestRecord("Text"), false},
            new object[] {new MethodTestRecord(""), new MethodTestRecord(""), true},
            new object[] {new MethodTestRecord("value"), null, false},
            new object[] {new MethodTestRecord(""), null, false},
        };

        [TestCaseSource(nameof(EqualsOtherTestTestCaseSource))]
        public static void EqualsOtherTest(MethodTestRecord left, MethodTestRecord right, bool answer)
        {
            var errorOccured = false;
            var result = false;
            try
            {
                result = left.Equals(right);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値であること
            Assert.AreEqual(result, answer);
        }

        private static readonly object[] EqualsObjectTestCaseSource =
        {
            new object[] {new MethodTestRecord("value"), new MethodTestRecord("value"), true},
            new object[] {new MethodTestRecord("value"), new MethodTestRecord("文字列\r\n"), false},
            new object[] {new MethodTestRecord(""), new MethodTestRecord("value"), false},
            new object[] {new MethodTestRecord("value"), "value", false},
            new object[] {new MethodTestRecord("テキスト"), null, false},
            new object[] {new MethodTestRecord("value"), new EqualsTestRecord("value"), false},
        };

        [TestCaseSource(nameof(EqualsObjectTestCaseSource))]
        public static void EqualsObjectTest(MethodTestRecord left, object right, bool answer)
        {
            var errorOccured = false;
            var result = false;
            try
            {
                result = left.Equals(right);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値であること
            Assert.AreEqual(result, answer);
        }

        /// <summary>
        /// Initializeの検証テスト用
        /// </summary>
        /// <remarks>
        /// 検証オプションは static である <see cref="ValidationOption"/> から取得。
        /// </remarks>
        public record ValidationTestRecord : SingleStringValueRecord<ValidationTestRecord>
        {
            protected override bool IsAllowEmpty => ValidationOption.IsAllowEmpty;

            protected override bool IsAllowNewLine => ValidationOption.IsAllowNewLine;

            protected override Regex RequireRegex => ValidationOption.RequireRegex;

            protected override int? RequireSJisByteLengthMax => ValidationOption.RequireSJisByteLengthMax;

            protected override Regex SafetyRegex => ValidationOption.SafetyRegex;

            public ValidationTestRecord(string value) : base(value)
            {
            }
        }

        /// <summary>
        /// コンストラクタテスト用検証オプション
        /// </summary>
        private static class ValidationOption
        {
            /// <summary>
            /// 空文字許容フラグ
            /// </summary>
            public static bool IsAllowEmpty { get; set; }

            /// <summary>
            /// 改行コード許容フラグ
            /// </summary>
            public static bool IsAllowNewLine { get; set; }

            /// <summary>
            /// 必須文字列の正規表現
            /// </summary>
            public static Regex RequireRegex { get; set; }

            /// <summary>
            /// 文字列データ（Shift-JIS）の必須サイズ最大
            /// </summary>
            public static int? RequireSJisByteLengthMax { get; set; }

            /// <summary>
            /// 推奨する文字列の正規表現
            /// </summary>
            public static Regex SafetyRegex { get; set; }
        }

        /// <summary>
        /// メソッドテスト用クラス
        /// </summary>
        public record MethodTestRecord : SingleStringValueRecord<MethodTestRecord>
        {
            protected override bool IsAllowEmpty => true;
            protected override bool IsAllowNewLine => true;

            public MethodTestRecord(string value) : base(value)
            {
            }
        }

        /// <summary>
        /// Equals メソッドテスト用比較対象クラス
        /// </summary>
        public record EqualsTestRecord : SingleStringValueRecord<EqualsTestRecord>
        {
            protected override bool IsAllowEmpty => true;
            protected override bool IsAllowNewLine => true;

            public EqualsTestRecord(string value) : base(value)
            {
            }
        }
    }
}
