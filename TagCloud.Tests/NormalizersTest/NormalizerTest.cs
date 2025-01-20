using FileSenderRailway;
using FluentAssertions;
using TagCloud.Normalizers;

namespace TagCloud.Tests.NormalizersTest
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
        [TestCase(1.1)]
        public void Normalize_ThrowsException_WithInvalidMinCoefficient(
            double minCoefficient)
        {
            var expected = Result.Fail<Dictionary<string, double>>(
                "Минимальный коэффициент нормализации должен быть в диапазоне от 0 до 1");
            var actual = normalizer.Normalize(values, minCoefficient, defaultDecimalPlaces);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Normalize_ThrowsException_WithEmptyValues()
        {
            var expected = Result.Fail<Dictionary<string, double>>(
                "Словарь значений не может быть пустым");
            var actual = normalizer.Normalize(
                new Dictionary<string, uint>(), defaultMinCoefficient, defaultDecimalPlaces);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Normalize_ThrowsException_WithValuesAsNull()
        {
            var expected = Result.Fail<Dictionary<string, double>>(
                "Словарь значений не может быть пустым");
            var actual = normalizer.Normalize(null!, defaultMinCoefficient, defaultDecimalPlaces);
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Normalize_ThrowsException_WithInvalidDecimalPlaces()
        {
            var expected = Result.Fail<Dictionary<string, double>>(
                "Количество знаков после запятой не может быть отрицательным");
            var actual = normalizer.Normalize(values, defaultMinCoefficient, -1);
            actual.Should().BeEquivalentTo(expected);
        }

        [TestCase(4)]
        [TestCase(2)]
        public void Normalize_CalculatesСorrectly(int decimalPlaces)
        {
            var dict = new Dictionary<string, double>();
            foreach (var pair in expectedResult)
            {
                dict[pair.Key] = Math.Round(pair.Value, decimalPlaces);
            }
            var expected = dict.AsResult();
            var actual = normalizer.Normalize(values, defaultMinCoefficient, decimalPlaces);
            actual.Value.Should().BeEquivalentTo(expected.Value);
        }
    }
}
