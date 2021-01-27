using System;
using Commons;
using NUnit.Framework;
using WodiLib.Project;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Project.ValueObject
{
    [TestFixture]
    public class EventCommandSentenceInfoTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ConstructorTestCaseSource =
        {
            new object[] {null, null, true},
            new object[] {EventCommandColorSet.Black, null, true},
            new object[] {null, new EventCommandSentence("Text"), true},
            new object[] {EventCommandColorSet.Green, new EventCommandSentence("Text"), false},
        };

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(EventCommandColorSet colorSet,
            EventCommandSentence sentence, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new EventCommandSentenceInfo(colorSet, sentence);
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
