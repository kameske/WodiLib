using NUnit.Framework;
using WodiLib.SourceGenerator.ValueObject.Attributes;

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

    [MultiValueObject]
    public partial record MultiValueObject
    {
        /// <summary>X</summary>
        public int X { get; }

        /// <summary>Y</summary>
        public int Y { get; }

        /// <summary>Z</summary>
        public int Z { get; init; } = 0;
    }
}
