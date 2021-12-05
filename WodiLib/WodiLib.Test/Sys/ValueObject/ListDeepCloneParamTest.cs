using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ListDeepCloneParamTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(null, false)]
        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        public static void LengthTest(int? length, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ListDeepCloneParam<string>
                {
                    Length = length
                };
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] DeepCloneWithTestCaseSource =
        {
            new object[] { null, false },
            new object[] { Array.Empty<KeyValuePair<int, string>>(), false },
            new object[] { new KeyValuePair<int, string>[] { new(-1, "string") }, false },
            new object[] { new KeyValuePair<int, string>[] { new(0, "string") }, false },
            new object[] { new KeyValuePair<int, string>[] { new(4, "string") }, false },
            new object[] { new KeyValuePair<int, string>[] { new(5, "string") }, false },
            new object[]
            {
                new KeyValuePair<int, string>[] { new(-1, "string"), new(0, "string") }, false
            },
            new object[]
            {
                new KeyValuePair<int, string>[] { new(0, "string"), new(4, "string") }, false
            },
            new object[] { new KeyValuePair<int, string>[] { new(1, null) }, true },
            new object[] { new KeyValuePair<int, string>[] { new(3, "string"), new(2, null) }, true }
        };

        [TestCaseSource(nameof(DeepCloneWithTestCaseSource))]
        public static void ValuesTest(IEnumerable<KeyValuePair<int, string>> values, bool isError)
        {
            var setValues = values is null
                ? null
                : new Dictionary<int, string>();
            values?.ForEach(pair => setValues[pair.Key] = pair.Value);

            var errorOccured = false;
            try
            {
                var _ = new ListDeepCloneParam<string>
                {
                    Values = setValues
                };
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}
