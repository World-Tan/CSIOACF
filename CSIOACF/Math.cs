﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSIOACF
{
	public class Math
	{
        private const int NumTerms = 20; // Number of terms in the Taylor series

        public static float Sin(float angle)
        {
            angle %= (2 * MathF.PI); // Ensure angle is within one full revolution (2π radians)

            float result = angle;
            float power = angle * angle * angle;
            float factorial = 6;

            for (int i = 1; i <= NumTerms; i++)
            {
                if (i % 2 != 0)
                {
                    result -= power / factorial;
                }
                else
                {
                    result += power / factorial;
                }

                power *= angle * angle;
                factorial *= (2 * i + 1) * (2 * i);
            }

            return result;
        }

        public static float Cos(float angle)
        {
            angle %= (2 * MathF.PI); // Ensure angle is within one full revolution (2π radians)

            float result = 1; // The first term of the Taylor series is 1 (cos(0) = 1)
            float power = angle * angle;
            float factorial = 2;

            for (int i = 1; i <= NumTerms; i++)
            {
                if (i % 2 == 0)
                {
                    result += power / factorial;
                }
                else
                {
                    result -= power / factorial;
                }

                power *= angle * angle;
                factorial *= (2 * i + 1) * (2 * i);
            }

            return result;
        }

        public static float Tan(float angle)
        {
            // To calculate tan(x), use sin(x) / cos(x)
            return Sin(angle) / Cos(angle);
        }

        public static float Asin(float value)
        {
            // Ensure the input value is within the range [-1, 1]
            value = MathF.Max(MathF.Min(value, 1), -1);

            float result = value;
            float power = value * value * value;
            float factorial = 6;

            for (int i = 1; i <= NumTerms; i++)
            {
                int denominator = 2 * i + 1;
                if (i % 2 != 0)
                {
                    result += power / factorial;
                }
                else
                {
                    result -= power / factorial;
                }

                power *= value * value;
                factorial *= denominator * (denominator + 1);
            }

            return result;
        }

        public static float Acos(float value)
        {
            // To calculate acos(x), use π/2 - asin(x)
            return MathF.PI / 2 - Asin(value);
        }

        public static float Atan(float value)
        {
            float result = value;
            float power = value * value;
            float sign = 1;

            for (int i = 1; i <= NumTerms; i++)
            {
                sign = -sign;
                result += sign * power / (2 * i + 1);
                power *= value * value;
            }

            return result;
        }

        public static float Atan2(float y, float x)
        {
            // Special cases: atan2(0, 0) and atan2(y, 0) for non-zero y
            if (x == 0)
            {
                if (y > 0)
                {
                    return MathF.PI / 2; // 90 degrees
                }
                else if (y < 0)
                {
                    return -MathF.PI / 2; // -90 degrees
                }
                else
                {
                    return 0; // Undefined, return 0
                }
            }

            float atanValue = MathF.Atan(y / x);

            // Adjust the angle based on the quadrant
            if (x > 0)
            {
                return atanValue;
            }
            else if (x < 0)
            {
                if (y >= 0)
                {
                    return atanValue + MathF.PI; // Add 180 degrees for the 2nd and 3rd quadrants
                }
                else
                {
                    return atanValue - MathF.PI; // Subtract 180 degrees for the 4th quadrant
                }
            }
            else // x == 0
            {
                return y > 0 ? MathF.PI / 2 : -MathF.PI / 2; // 90 or -90 degrees
            }
        }

        public static float Sinh(float angle)
        {
            float result = angle;
            float power = angle * angle * angle;
            float factorial = 6;

            for (int i = 1; i <= NumTerms; i++)
            {
                result += power / factorial;

                power *= angle * angle;
                factorial *= (2 * i + 1) * (2 * i);
            }

            return result;
        }

        public static float Cosh(float angle)
        {
            float result = 1; // The first term of the Taylor series is 1 (cosh(0) = 1)
            float power = angle * angle;
            float factorial = 2;

            for (int i = 1; i <= NumTerms; i++)
            {
                result += power / factorial;

                power *= angle * angle;
                factorial *= (2 * i) * (2 * i - 1);
            }

            return result;
        }

        public static float Tanh(float angle)
        {
            float sinhValue = Sinh(angle);
            float coshValue = Cosh(angle);

            return sinhValue / coshValue;
        }

        public static float Exp(float x)
        {
            float result = 1;
            float power = x;
            float factorial = 1;

            for (int i = 1; i <= NumTerms; i++)
            {
                result += power / factorial;

                power *= x;
                factorial *= i;
            }

            return result;
        }

        public static float Log(float x)
        {
            if (x <= 0)
            {
                throw new ArgumentException("Input must be greater than 0.");
            }

            if (x == 1)
            {
                return 0; // log(1) = 0
            }

            if (x < 0)
            {
                throw new ArgumentException("Input must be greater than 0.");
            }

            float result = 0;
            float term = (x - 1) / (x + 1);
            float power = term * term;

            for (int i = 1; i <= NumTerms; i += 2)
            {
                result += term / i;
                term *= power;
            }

            return 2 * result;
        }

        public static float Log10(float x)
        {
            // Use the identity: log10(x) = log(x) / log(10)
            return Log(x) / Log(10);
        }

        public static float Pow(float x, float y)
        {
            // If the base is negative and the exponent is not an integer,
            // the result may be a complex number, so we handle these cases separately.
            if (x < 0 && !IsInteger(y))
            {
                throw new ArgumentException("Base must be non-negative for non-integer exponents.");
            }

            // Calculate the natural logarithm of the base using the custom Log function
            float logX = Log(x);

            // Use the identity: x^y = exp(y * log(x))
            return Exp(y * logX);
        }

        private static bool IsInteger(float value)
        {
            return Abs(value % 1) < float.Epsilon;
        }

        public static float Sqrt(float N)
        {
            if (N < 0) throw new ArgumentException("N can't be negative.");
            float guess = N/2, prev = 0;
            while (Abs(prev - guess) > 0.00000001f)
            {
                prev = guess;
                guess = ((N / guess) + guess) / 2;
            }

            return guess;
        }

        public static double Ceil(float x)
		{
			if (x < 0) return -Floor(-x);

            double frac = x - (int)x;
			return frac == 0 ? x : (int)x + 1;
		}

		public static double Floor(float x)
		{
            if (x < 0) return -Ceil(-x);

			return (int)x;
		}

		public static unsafe double Abs(double x)
		{
			return x < 0 ? -x : x;
		}

		public static unsafe double LdExp(double x, double y)
		{
            return x * CSIOACF.Math.Pow(2, (float)y);
		}

		public static unsafe double FrExp(double x, int *y)
        {
            int j = 0, i = 1;
            while (i < x) { i *= 2; j++; }

            *y = j;
            return x / i;
        }

		public static unsafe double ModF(double x, double *ip)
		{
            *ip = (int)x;
			return x - *ip;
		}

		public static double FMod(double x, double y)
		{
			return x % y;
		}
	}
}
