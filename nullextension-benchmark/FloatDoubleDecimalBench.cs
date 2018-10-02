using BenchmarkDotNet.Attributes;
using Xunit;

namespace nullextension_benchmark
{
    public class FloatDoubleDecimalBench
    {
        // getting overflowexceptions with more iterations
        const int maxStepsDecimalCanTake = 26;

        [Benchmark(Baseline = true)]
        public float NeperFloat()
        {
            return ApproximateEulerFloat(maxStepsDecimalCanTake);
        }

        [Benchmark]
        public double NeperDouble()
        {
            return ApproximateEulerDouble(maxStepsDecimalCanTake);
        }

        [Benchmark]
        public decimal NeperDecimal()
        {
            return ApproximateEulerDecimal(maxStepsDecimalCanTake);
        }

        public float ApproximateEulerFloat(int n)
        {
            float kFactorial = 1;

            if (n <= 1)
                return 2;
            int i = 1;
            float eSum = 1.0F + 1.0F / kFactorial;

            while(n-- > 0)
            {
                kFactorial *= ++i;
                eSum += 1.0F / kFactorial;
            }

            return eSum;
        }

        public double ApproximateEulerDouble(int n)
        {
            double kFactorial = 1;

            if (n <= 1)
                return 2;
            int i = 1;
            double eSum = 1.0 + 1.0 / kFactorial;

            while (n-- > 0)
            {
                kFactorial *= ++i;
                eSum += 1.0 / kFactorial;
            }

            return eSum;
        }

        public decimal ApproximateEulerDecimal(int n)
        {
            decimal kFactorial = 1;

            if (n <= 1)
                return 2;
            int i = 1;
            decimal eSum = 1.0m + 1.0m / kFactorial;

            while (n-- > 0)
            {
                kFactorial *= ++i;
                eSum += 1.0m / kFactorial;
            }

            return eSum;
        }

        [Theory,
            InlineData(1, 2, 1),
            InlineData(2, 2.7, 1),
            InlineData(3, 2.7, 1),
            InlineData(4, 2.72, 2),
            InlineData(5, 2.718, 3),
            InlineData(6, 2.71828, 4),
            InlineData(6000, 2.71828182845904523536028747135266249775724709369995, 6)]
        void FloatTest(int steps, float expected, int prec)
        {
            var result = ApproximateEulerFloat(steps);

            Assert.Equal((double)expected, (double)result, prec);
        }

        [Theory,
            InlineData(1, 2, 1),
            InlineData(2, 2.7, 1),
            InlineData(3, 2.7, 1),
            InlineData(4, 2.72, 2),
            InlineData(5, 2.718, 3),
            InlineData(6, 2.71828, 4),
            InlineData(6000, 2.71828182845904523536028747135266249775724709369995, 13)]
        void DoubleTest(int steps, double expected, int prec)
        {
            var result = ApproximateEulerDouble(steps);

            Assert.Equal(expected, result, prec);
        }


        const decimal neper50Cut = 2.71828182845904523536028747135266249775724709369995m;

        [Theory,
            InlineData(1, 2, 1),
            InlineData(2, 2.7, 1),
            InlineData(3, 2.7, 1),
            InlineData(4, 2.72, 2),
            InlineData(5, 2.718, 3),
            InlineData(6, 2.71828, 4),
            InlineData(26, 2.71828182845904523536028747135266249775724709369995, 13)]
        void DecimalTest(int steps, decimal expected, int prec)
        {
            var result = ApproximateEulerDecimal(steps);

            Assert.Equal(expected, result, prec);
        }
    }
}
