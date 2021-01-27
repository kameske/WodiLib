using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ControlStandardKeySetTest
    {
        private static readonly object[] TestCaseSource =
        {
            new object[] {new[] {false, false, false, false, false, false, false}, (byte) 0},
            new object[] {new[] {true, false, false, false, false, false, false}, (byte) 1},
            new object[] {new[] {false, true, false, false, false, false, false}, (byte) 2},
            new object[] {new[] {true, true, false, false, false, false, false}, (byte) 3},
            new object[] {new[] {false, false, true, false, false, false, false}, (byte) 4},
            new object[] {new[] {true, false, true, false, false, false, false}, (byte) 5},
            new object[] {new[] {false, true, true, false, false, false, false}, (byte) 6},
            new object[] {new[] {true, true, true, false, false, false, false}, (byte) 7},
            new object[] {new[] {false, false, false, true, false, false, false}, (byte) 16},
            new object[] {new[] {true, false, false, true, false, false, false}, (byte) 17},
            new object[] {new[] {false, true, false, true, false, false, false}, (byte) 18},
            new object[] {new[] {true, true, false, true, false, false, false}, (byte) 19},
            new object[] {new[] {false, false, true, true, false, false, false}, (byte) 20},
            new object[] {new[] {true, false, true, true, false, false, false}, (byte) 21},
            new object[] {new[] {false, true, true, true, false, false, false}, (byte) 22},
            new object[] {new[] {true, true, true, true, false, false, false}, (byte) 23},
            new object[] {new[] {false, false, false, false, true, false, false}, (byte) 32},
            new object[] {new[] {true, false, false, false, true, false, false}, (byte) 33},
            new object[] {new[] {false, true, false, false, true, false, false}, (byte) 34},
            new object[] {new[] {true, true, false, false, true, false, false}, (byte) 35},
            new object[] {new[] {false, false, true, false, true, false, false}, (byte) 36},
            new object[] {new[] {true, false, true, false, true, false, false}, (byte) 37},
            new object[] {new[] {false, true, true, false, true, false, false}, (byte) 38},
            new object[] {new[] {true, true, true, false, true, false, false}, (byte) 39},
            new object[] {new[] {false, false, false, true, true, false, false}, (byte) 48},
            new object[] {new[] {true, false, false, true, true, false, false}, (byte) 49},
            new object[] {new[] {false, true, false, true, true, false, false}, (byte) 50},
            new object[] {new[] {true, true, false, true, true, false, false}, (byte) 51},
            new object[] {new[] {false, false, true, true, true, false, false}, (byte) 52},
            new object[] {new[] {true, false, true, true, true, false, false}, (byte) 53},
            new object[] {new[] {false, true, true, true, true, false, false}, (byte) 54},
            new object[] {new[] {true, true, true, true, true, false, false}, (byte) 55},
            new object[] {new[] {false, false, false, false, false, true, false}, (byte) 64},
            new object[] {new[] {true, false, false, false, false, true, false}, (byte) 65},
            new object[] {new[] {false, true, false, false, false, true, false}, (byte) 66},
            new object[] {new[] {true, true, false, false, false, true, false}, (byte) 67},
            new object[] {new[] {false, false, true, false, false, true, false}, (byte) 68},
            new object[] {new[] {true, false, true, false, false, true, false}, (byte) 69},
            new object[] {new[] {false, true, true, false, false, true, false}, (byte) 70},
            new object[] {new[] {true, true, true, false, false, true, false}, (byte) 71},
            new object[] {new[] {false, false, false, true, false, true, false}, (byte) 80},
            new object[] {new[] {true, false, false, true, false, true, false}, (byte) 81},
            new object[] {new[] {false, true, false, true, false, true, false}, (byte) 82},
            new object[] {new[] {true, true, false, true, false, true, false}, (byte) 83},
            new object[] {new[] {false, false, true, true, false, true, false}, (byte) 84},
            new object[] {new[] {true, false, true, true, false, true, false}, (byte) 85},
            new object[] {new[] {false, true, true, true, false, true, false}, (byte) 86},
            new object[] {new[] {true, true, true, true, false, true, false}, (byte) 87},
            new object[] {new[] {false, false, false, false, true, true, false}, (byte) 96},
            new object[] {new[] {true, false, false, false, true, true, false}, (byte) 97},
            new object[] {new[] {false, true, false, false, true, true, false}, (byte) 98},
            new object[] {new[] {true, true, false, false, true, true, false}, (byte) 99},
            new object[] {new[] {false, false, true, false, true, true, false}, (byte) 100},
            new object[] {new[] {true, false, true, false, true, true, false}, (byte) 101},
            new object[] {new[] {false, true, true, false, true, true, false}, (byte) 102},
            new object[] {new[] {true, true, true, false, true, true, false}, (byte) 103},
            new object[] {new[] {false, false, false, true, true, true, false}, (byte) 112},
            new object[] {new[] {true, false, false, true, true, true, false}, (byte) 113},
            new object[] {new[] {false, true, false, true, true, true, false}, (byte) 114},
            new object[] {new[] {true, true, false, true, true, true, false}, (byte) 115},
            new object[] {new[] {false, false, true, true, true, true, false}, (byte) 116},
            new object[] {new[] {true, false, true, true, true, true, false}, (byte) 117},
            new object[] {new[] {false, true, true, true, true, true, false}, (byte) 118},
            new object[] {new[] {true, true, true, true, true, true, false}, (byte) 119},
            new object[] {new[] {false, false, false, false, false, false, true}, (byte) 128},
            new object[] {new[] {true, false, false, false, false, false, true}, (byte) 129},
            new object[] {new[] {false, true, false, false, false, false, true}, (byte) 130},
            new object[] {new[] {true, true, false, false, false, false, true}, (byte) 131},
            new object[] {new[] {false, false, true, false, false, false, true}, (byte) 132},
            new object[] {new[] {true, false, true, false, false, false, true}, (byte) 133},
            new object[] {new[] {false, true, true, false, false, false, true}, (byte) 134},
            new object[] {new[] {true, true, true, false, false, false, true}, (byte) 135},
            new object[] {new[] {false, false, false, true, false, false, true}, (byte) 144},
            new object[] {new[] {true, false, false, true, false, false, true}, (byte) 145},
            new object[] {new[] {false, true, false, true, false, false, true}, (byte) 146},
            new object[] {new[] {true, true, false, true, false, false, true}, (byte) 147},
            new object[] {new[] {false, false, true, true, false, false, true}, (byte) 148},
            new object[] {new[] {true, false, true, true, false, false, true}, (byte) 149},
            new object[] {new[] {false, true, true, true, false, false, true}, (byte) 150},
            new object[] {new[] {true, true, true, true, false, false, true}, (byte) 151},
            new object[] {new[] {false, false, false, false, true, false, true}, (byte) 160},
            new object[] {new[] {true, false, false, false, true, false, true}, (byte) 161},
            new object[] {new[] {false, true, false, false, true, false, true}, (byte) 162},
            new object[] {new[] {true, true, false, false, true, false, true}, (byte) 163},
            new object[] {new[] {false, false, true, false, true, false, true}, (byte) 164},
            new object[] {new[] {true, false, true, false, true, false, true}, (byte) 165},
            new object[] {new[] {false, true, true, false, true, false, true}, (byte) 166},
            new object[] {new[] {true, true, true, false, true, false, true}, (byte) 167},
            new object[] {new[] {false, false, false, true, true, false, true}, (byte) 176},
            new object[] {new[] {true, false, false, true, true, false, true}, (byte) 177},
            new object[] {new[] {false, true, false, true, true, false, true}, (byte) 178},
            new object[] {new[] {true, true, false, true, true, false, true}, (byte) 179},
            new object[] {new[] {false, false, true, true, true, false, true}, (byte) 180},
            new object[] {new[] {true, false, true, true, true, false, true}, (byte) 181},
            new object[] {new[] {false, true, true, true, true, false, true}, (byte) 182},
            new object[] {new[] {true, true, true, true, true, false, true}, (byte) 183},
            new object[] {new[] {false, false, false, false, false, true, true}, (byte) 192},
            new object[] {new[] {true, false, false, false, false, true, true}, (byte) 193},
            new object[] {new[] {false, true, false, false, false, true, true}, (byte) 194},
            new object[] {new[] {true, true, false, false, false, true, true}, (byte) 195},
            new object[] {new[] {false, false, true, false, false, true, true}, (byte) 196},
            new object[] {new[] {true, false, true, false, false, true, true}, (byte) 197},
            new object[] {new[] {false, true, true, false, false, true, true}, (byte) 198},
            new object[] {new[] {true, true, true, false, false, true, true}, (byte) 199},
            new object[] {new[] {false, false, false, true, false, true, true}, (byte) 208},
            new object[] {new[] {true, false, false, true, false, true, true}, (byte) 209},
            new object[] {new[] {false, true, false, true, false, true, true}, (byte) 210},
            new object[] {new[] {true, true, false, true, false, true, true}, (byte) 211},
            new object[] {new[] {false, false, true, true, false, true, true}, (byte) 212},
            new object[] {new[] {true, false, true, true, false, true, true}, (byte) 213},
            new object[] {new[] {false, true, true, true, false, true, true}, (byte) 214},
            new object[] {new[] {true, true, true, true, false, true, true}, (byte) 215},
            new object[] {new[] {false, false, false, false, true, true, true}, (byte) 224},
            new object[] {new[] {true, false, false, false, true, true, true}, (byte) 225},
            new object[] {new[] {false, true, false, false, true, true, true}, (byte) 226},
            new object[] {new[] {true, true, false, false, true, true, true}, (byte) 227},
            new object[] {new[] {false, false, true, false, true, true, true}, (byte) 228},
            new object[] {new[] {true, false, true, false, true, true, true}, (byte) 229},
            new object[] {new[] {false, true, true, false, true, true, true}, (byte) 230},
            new object[] {new[] {true, true, true, false, true, true, true}, (byte) 231},
            new object[] {new[] {false, false, false, true, true, true, true}, (byte) 240},
            new object[] {new[] {true, false, false, true, true, true, true}, (byte) 241},
            new object[] {new[] {false, true, false, true, true, true, true}, (byte) 242},
            new object[] {new[] {true, true, false, true, true, true, true}, (byte) 243},
            new object[] {new[] {false, false, true, true, true, true, true}, (byte) 244},
            new object[] {new[] {true, false, true, true, true, true, true}, (byte) 245},
            new object[] {new[] {false, true, true, true, true, true, true}, (byte) 246},
            new object[] {new[] {true, true, true, true, true, true, true}, (byte) 247}
        };

        [TestCaseSource(nameof(TestCaseSource))]
        public static void CreateTest(bool[] answers, byte b)
        {
            var instance = new ControlStandardKeySet(b);

            Assert.AreEqual(instance.Ok, answers[0]);
            Assert.AreEqual(instance.Cancel, answers[1]);
            Assert.AreEqual(instance.Sub, answers[2]);
            Assert.AreEqual(instance.Down, answers[3]);
            Assert.AreEqual(instance.Left, answers[4]);
            Assert.AreEqual(instance.Right, answers[5]);
            Assert.AreEqual(instance.Up, answers[6]);
        }

        [TestCaseSource(nameof(TestCaseSource))]
        public static void ToByteTest(bool[] flags, byte answer)
        {
            var instance = new ControlStandardKeySet
            {
                Ok = flags[0],
                Cancel = flags[1],
                Sub = flags[2],
                Down = flags[3],
                Left = flags[4],
                Right = flags[5],
                Up = flags[6]
            };

            Assert.AreEqual(instance.ToByte(), answer);
        }
    }
}
