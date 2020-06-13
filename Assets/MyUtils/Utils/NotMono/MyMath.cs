using UnityEngine;
using System;
using System.Collections.Generic;

namespace My_Utils
{
    

    /// <summary>
    /// Useful math functions.
    /// </summary>
    public static class MyMath
    {
        /// <summary>
        /// Return the absolute value of a Vector2
        /// </summary>
        public static Vector2 Abs(Vector2 vector2)
        {
            return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
        }

        /// <summary>
        /// Return true if a value is between 'min' and 'max'
        /// </summary>
        public static bool IsBetween(float value, float min, float max)
        {
            float realMin = min < max ? min : max;
            float realMax = min > max ? min : max;

            return (realMin <= value) && (value <= realMax);
        }

        /// <summary>
        ///  Return true or false based on a probability of "0.0 to 1.0"
        /// </summary>
        public static bool Chance(float probability)
        {
            int randint = MyRandom.Next(0, 101);

            return (probability * 100) >= randint;
        }

        public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection);
            return lineStart + (closestPoint * lineDirection);
        }

        public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 fullDirection = lineEnd - lineStart;
            Vector3 lineDirection = Vector3.Normalize(fullDirection);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection);
            return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
        }

        //Bounce
        public static float Bounce(float x)
        {
            return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
        }

        public static Vector2 Bounce(Vector2 vec)
        {
            return new Vector2(Bounce(vec.x), Bounce(vec.y));
        }

        public static Vector3 Bounce(Vector3 vec)
        {
            return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
        }


        /// <summary>
        /// Test for value that is near specified float (due to floating point inprecision).
        /// </summary>
        public static bool Approx(float val, float about, float range)
        {
            return ((Mathf.Abs(val - about) < range));
        }


        /// <summary>
        /// Test if a Vector3 is close to another Vector3(due to floating point inprecision).
        /// Compares the square of the distance to the square of the range as this avoids calculating a square root which is 
        /// much slower than squaring the range.
        /// </summary>
        public static bool Approx(Vector3 vec1, Vector3 vec2, float range)
        {
            return ((vec1 - vec2).sqrMagnitude < range * range);
        }


        /// <summary>
        /// Return an array with the first n primes, starting by a custom value.
        /// </summary>
        /// <param name="length">How many primes</param>
        /// <param name="start">The number that will start counting to get primes.</param>
        public static int[] GetPrimesByLength(int length, int start = 2)
        {
            int[] primes = new int[length];

            if (start < 2)
                start = 2;

            int atualNumber = start;
            int primesQtt = 0;
            while (primesQtt < length)
            {
                if (atualNumber.IsPrime())
                {
                    primes[primesQtt] = atualNumber;
                    primesQtt++;
                }

                atualNumber++;
            }

            return primes;
        }


        /// <summary>
        /// Return an array with all primes between two numbers. (start <= prime <= end)
        /// </summary>
        /// <param name="start">The number to start searching primes.</param>
        /// <param name="end">The number to end searching primes.</param>
        public static int[] GetPrimesBetween(int start, int end)
        {
            List<int> primes = new List<int>();

            for (int i = start; i <= end; i++)
            {
                if (i.IsPrime())
                {
                    primes.Add(i);
                }
            }

            return primes.ToArray();
        }


        /// <summary>
        /// Return the max quantity decimal places between the numbers
        /// </summary>
        /// <param name="x">Any float number</param>
        /// <param name="y">Any float number</param>
        public static int MaxDecimalPlaces(float x, float y)
        {
            int xDecimalPlaces = x.DecimalPlaces();
            int yDecimalPlaces = y.DecimalPlaces();
            int maxDecimalPlaces = Math.Max(xDecimalPlaces, yDecimalPlaces);
            return maxDecimalPlaces;
        }

        /// <summary>
        /// Return the max quantity of decimal places between an array of numbers
        /// </summary>
        /// <param name="numbers">Any float array</param>
        public static int MaxDecimalPlaces(float[] numbers)
        {
            int maxDecimalPlaces = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                int atualDecimalPlaces = numbers[i].DecimalPlaces();
                if (atualDecimalPlaces > maxDecimalPlaces)
                {
                    maxDecimalPlaces = atualDecimalPlaces;
                }
            }

            return maxDecimalPlaces;
        }


        /// <summary>
        /// Return the gratest common divisor between two numbers.
        /// </summary>
        public static int GreatestCommonDivisor(int n1, int n2)
        {
            int[] array = new int[] { n1, n2 };
            return array.GreatestCommonDivisor();
        }


        /// <summary>
        /// Return the reduced value of a fraction.
        /// </summary>
        /// <param name="fraction">Must be a int[2] -> { numerator, denominator }</param>
        /// <returns></returns>
        public static int[] ReduceFraction(int[] fraction)
        {
            Fraction newFraction = new Fraction(fraction[0], fraction[1]);
            return newFraction.Reduced.ToArray();
        }


        /// <summary>
        /// Clamp a int between a min and a max.
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;

            return value;
        }


        /// <summary>
        /// Clamp a float between a min and a max.
        /// </summary>
        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;

            return value;
        }


        public static Vector2 Sqrt(Vector2 vector2)
        {
            return new Vector2(Mathf.Sqrt(vector2.x), Mathf.Sqrt(vector2.y));
        }
    }
}
