using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class CommonEventStrArgListTest
    {
        private static readonly object[] AccessorTestCaseSource =
        {
            new object[] {0, 10},
            new object[] {1, 10},
            new object[] {2, 10},
            new object[] {3, 10},
            new object[] {0, "10"},
            new object[] {1, "10"},
            new object[] {2, "10"},
            new object[] {3, "10"}
        };

        [TestCaseSource(nameof(AccessorTestCaseSource))]
        public static void AccessorTest(int index, object value)
        {
            IntOrStr obj;
            switch (value)
            {
                case int i:
                    obj = i;
                    break;
                case string s:
                    obj = s;
                    break;
                default:
                    Assert.Fail();
                    return;
            }

            var instance = new CommonEventStrArgList {[index] = obj};
            for (var idx = 0; idx < 4; idx++)
            {
                if (idx != index) continue;

                var resultInstance = instance[index];
                switch (value)
                {
                    case int i2:
                        Assert.AreEqual(resultInstance.InstanceIntOrStrType, IntOrStrType.Int);
                        Assert.AreEqual(resultInstance.ToInt(), i2);
                        break;
                    case string s2:
                        Assert.AreEqual(resultInstance.InstanceIntOrStrType, IntOrStrType.Str);
                        Assert.AreEqual(resultInstance.ToStr(), s2);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        private static readonly object[] ReferenceFlgTestCaseSource =
        {
            new object[] {new[] {false, false, false, false}, (byte) 0},
            new object[] {new[] {true, false, false, false}, (byte) 16},
            new object[] {new[] {false, true, false, false}, (byte) 32},
            new object[] {new[] {true, true, false, false}, (byte) 48},
            new object[] {new[] {false, false, true, false}, (byte) 64},
            new object[] {new[] {true, false, true, false}, (byte) 80},
            new object[] {new[] {false, true, true, false}, (byte) 96},
            new object[] {new[] {true, true, true, false}, (byte) 112},
            new object[] {new[] {false, false, false, true}, (byte) 128},
            new object[] {new[] {true, false, false, true}, (byte) 144},
            new object[] {new[] {false, true, false, true}, (byte) 160},
            new object[] {new[] {true, true, false, true}, (byte) 176},
            new object[] {new[] {false, false, true, true}, (byte) 192},
            new object[] {new[] {true, false, true, true}, (byte) 208},
            new object[] {new[] {false, true, true, true}, (byte) 224},
            new object[] {new[] {true, true, true, true}, (byte) 240}
        };

        [TestCaseSource(nameof(ReferenceFlgTestCaseSource))]
        public void ReferenceFlgGetTest(bool[] strFlgs, byte answer)
        {
            var instance = new CommonEventStrArgList();
            for (var i = 0; i < 4; i++)
                if (strFlgs[i])
                    instance[i] = "test";
                else
                    instance[i] = 10;
            Assert.AreEqual(instance.ReferenceFlg, answer);
        }

        [TestCaseSource(nameof(ReferenceFlgTestCaseSource))]
        public void ReferenceFlgSetTest(bool[] strFlgs, byte flg)
        {
            var instance = new CommonEventStrArgList {ReferenceFlg = flg};
            for (var i = 0; i < 4; i++)
                Assert.AreEqual(instance[i].InstanceIntOrStrType, strFlgs[i] ? IntOrStrType.Str : IntOrStrType.Int);
        }
    }
}
