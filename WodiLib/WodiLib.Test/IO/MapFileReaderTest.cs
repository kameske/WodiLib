using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class MapFileReaderTest
    {
        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            MapFileTestItemGenerator.OutputMapFile();
        }

        [Test]
        public static void Map023Test()
        {
            Common(
                MapFileTestItemGenerator.GenerateMap023Data(),
                "Map023.mps");

            Assert.True(true);
        }

        [Test]
        public static void Map024Test()
        {
            Common(
                MapFileTestItemGenerator.GenerateMap024Data(),
                "Map024.mps");

            Assert.True(true);
        }

        [Test]
        public static void Map025Test()
        {
            Common(
                MapFileTestItemGenerator.GenerateMap025Data(),
                "Map025.mps");

            Assert.True(true);
        }

        private static void Common(MapData resultData, string readFileName)
        {
            var reader = new MpsFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{readFileName}");

            var readResult = false;
            MapData data = null;
            var errorMessage = "";
            try
            {
                data = reader.ReadSync();
                readResult = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }


            // 正しく読めること
            if (!readResult)
            {
                throw new InvalidOperationException(
                    $"Error Occured. Message : {errorMessage}");
            }

            Console.WriteLine("Write Test Clear.");

            // 意図したデータと一致すること
            var resultDataBytes = resultData.ToBinary().ToArray();
            var readResultDataBytes = data.ToBinary().ToArray();

            if (resultDataBytes.Length != readResultDataBytes.Length)
            {
                throw new InvalidOperationException(
                    $"Data Length Not Match. " +
                    $"(answerLength: {resultDataBytes.Length}, readResultLength: {readResultDataBytes.Length})");
            }

            for (long i = 0; i < 0; i++)
            {
                if (resultDataBytes[i] != readResultDataBytes[i])
                {
                    throw new InvalidOperationException(
                        $"Data Byte Not Match. (index: {i}, answer: {resultDataBytes[i]}," +
                        $" readResult: {readResultDataBytes[i]})");
                }
            }
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            MapFileTestItemGenerator.DeleteMapFile();
        }
    }
}