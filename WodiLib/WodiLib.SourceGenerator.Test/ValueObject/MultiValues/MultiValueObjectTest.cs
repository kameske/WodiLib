using NUnit.Framework;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.SourceGenerator.Test.ValueObject.MultiValues
{
    public static class MultiValueObjectTests
    {
        [Test]
        public static void ConstructorTest()
        {
            var _ = new MultiValueObject(1, 2);
        }
    }

    [MultiValueObject(CastType = CastType.Explicit)]
    public partial record MultiValueObject
    {
        /// <summary>
        ///     X
        /// </summary>
        public int X { get; init; }

        /// <summary>Y</summary>
        public int Y { get; init; }
    }
}
