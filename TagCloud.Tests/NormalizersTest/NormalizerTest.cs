using FileSenderRailway;
using FluentAssertions;
using TagCloud.Normalizers;

namespace TagCloud.Tests.NormalaizersTest
{
    [TestFixture]
    internal class NormalizerTest
    {
        private readonly Normalizer normalizer = new Normalizer();
        private readonly Dictionary<string, uint> values = new Dictionary<string, uint>
        {
            { "one", 14 },
            { "two", 15 },
            { "three", 23 },
            { "four", 32 },
        };
        private readonly Dictionary<string, double> expectedResult = new Dictionary<string, double>
        {
            { "one", 0.25 },
            { "two",0.29166666666666669 },
            { "three", 0.625 },
            { "four", 1.0 },

        };
        private readonly int defaultDecimalPlaces = 4;
        private readonly double defaultMinCoefficient = 0.25;

        [TestCase(-0.1)]
        public void Normalize_ThrowsArgumentException_WithMinCoefficientLessThanZero(
            double minCoefficient)
        {
            var expected = Result.Fail<Dictionary<string, double>>("Минимальный коэффициент нормализации не может быть меньше 0");
            var actual = normalizer.Normalize(values, minCoefficient, defaultDecimalPlaces);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(0.25, 4)]
        [TestCase(0.25, 2)]
        public void Normalize_CalculatesСorrectly(double minCoefficient, int decimalPlaces)
        {
            var dict = new Dictionary<string, double>();
            foreach (var pair in expectedResult)
            {
                dict[pair.Key] = Math.Round(pair.Value, decimalPlaces);
            }
            var expected = dict.AsResult();
            var actual = normalizer.Normalize(values, minCoefficient, decimalPlaces);
            actual.IsSuccess.Should().Be(expected.IsSuccess);
            actual.Error.Should().Be(expected.Error);
            actual.Value.Should().BeEquivalentTo(expected.Value);
        }
    }
}
