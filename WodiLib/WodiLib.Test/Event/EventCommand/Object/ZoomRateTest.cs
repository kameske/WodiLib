using System;
using NUnit.Framework;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ZoomRateTest
    {
        private static readonly object[] SameRateAccessorTestCaseSource =
        {
            new object[] {ZoomRateType.Normal, false},
            new object[] {ZoomRateType.OnlyDepth, true},
            new object[] {ZoomRateType.OnlyWidth, true},
            new object[] {ZoomRateType.Different, true},
            new object[] {ZoomRateType.Same, false},
        };

        [TestCaseSource(nameof(SameRateAccessorTestCaseSource))]
        public static void SameRateAccessorTest(ZoomRateType zoomRateType, bool isError)
        {
            var instance = new ZoomRate {ZoomRateType = zoomRateType};
            var errorOccured = false;

            {
                // getter
                {
                    // Rate
                    try
                    {
                        var _ = instance.Rate;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
            }
            {
                // setter
                {
                    // Rate
                    errorOccured = false;
                    try
                    {
                        instance.Rate = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
            }
        }

        private static readonly object[] DifferenceRateAccessorTestCaseSource =
        {
            new object[] {ZoomRateType.Normal, true},
            new object[] {ZoomRateType.OnlyDepth, false},
            new object[] {ZoomRateType.OnlyWidth, false},
            new object[] {ZoomRateType.Different, false},
            new object[] {ZoomRateType.Same, true},
        };

        [TestCaseSource(nameof(DifferenceRateAccessorTestCaseSource))]
        public static void DifferenceRateAccessorTest(ZoomRateType zoomRateType, bool isError)
        {
            var instance = new ZoomRate {ZoomRateType = zoomRateType};
            var errorOccured = false;

            {
                // getter
                {
                    // RateWidth
                    try
                    {
                        var _ = instance.RateWidth;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // RateHeight
                    errorOccured = false;
                    try
                    {
                        var _ = instance.RateHeight;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
            }
            {
                // setter
                {
                    // RateWidth
                    errorOccured = false;
                    try
                    {
                        instance.RateWidth = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // RateHeight
                    errorOccured = false;
                    try
                    {
                        instance.RateHeight = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
            }
        }
    }
}