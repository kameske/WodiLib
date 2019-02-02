using System;
using NUnit.Framework;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class NormalOrFreePositionTest
    {
        [TestCase(false, false)]
        [TestCase(true, false)]
        public static void NormalPositionAccessorTest(bool isFreePosition, bool isError)
        {
            var instance = new NormalOrFreePosition();
            var errorOccured = false;

            {
                // getter
                {
                    // X
                    try
                    {
                        var _ = instance.NormalPositionX;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Y
                    errorOccured = false;
                    try
                    {
                        var _ = instance.NormalPositionY;
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
                    // X
                    errorOccured = false;
                    try
                    {
                        instance.NormalPositionX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Y
                    errorOccured = false;
                    try
                    {
                        instance.NormalPositionY = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
            }
        }

        [TestCase(true, false)]
        [TestCase(false, false)]
        public static void FreePositionAccessorTest(bool isFreePosition, bool isError)
        {
            var instance = new NormalOrFreePosition();
            var errorOccured = false;

            {
                // getter
                {
                    // Left-Up X
                    try
                    {
                        var _ = instance.FreePositionLeftUpX;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Up Y
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionLeftUpY;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Down X
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionLeftDownX;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Down Y
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionLeftDownY;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Up X
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionRightUpX;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Up Y
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionRightUpY;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Down X
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionRightDownX;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Down Y
                    errorOccured = false;
                    try
                    {
                        var _ = instance.FreePositionRightDownY;
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
                    // Left-Up X
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftUpX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Up Y
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftUpY = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Down X
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftDownX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Down Y
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftDownY = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Up X
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftUpX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Up Y
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftUpY = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Down X
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftDownX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Left-Down Y
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionLeftDownY = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Up X
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionRightUpX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Up Y
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionRightUpY = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Down X
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionRightDownX = 0;
                    }
                    catch (Exception)
                    {
                        errorOccured = true;
                    }

                    Assert.AreEqual(errorOccured, isError);
                }
                {
                    // Right-Down Y
                    errorOccured = false;
                    try
                    {
                        instance.FreePositionRightDownY = 0;
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