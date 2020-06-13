using System;

namespace My_Utils
{
    /// <summary>
    /// A struct representation of a fraction.
    /// </summary>
    public struct Fraction
    {
        /// <summary>
        /// Numerator
        /// </summary>
        public readonly int numerator;

        /// <summary>
        /// Denominator
        /// </summary>
        public readonly int denominator;

        /// <summary>
        /// True if fraction is already reduced.
        /// </summary>
        private readonly bool isReduced;

        public Fraction(int numerator, int denominator, bool storeReduced = false)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }

            isReduced = storeReduced;
            this.numerator = numerator;
            this.denominator = denominator;
            if (isReduced)
            {
                this = Reduced;
            }
        }

        public Fraction(float numerator, float denominator, bool storeReduced = false)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }

            int maxDecimalPlaces = MyMath.MaxDecimalPlaces(numerator, denominator);

            isReduced = storeReduced;
            this.numerator = (int)(numerator * Math.Pow(10, maxDecimalPlaces));
            this.denominator = (int)(denominator * Math.Pow(10, maxDecimalPlaces));
            if (isReduced)
            {
                this = Reduced;
            }
        }

        /// <summary>
        /// Return te reduced value of the fraction
        /// </summary>
        public Fraction Reduced
        {
            get
            {
                if (isReduced)
                {
                    return this;
                }
                else
                {
                    int gretestCommonDivisor = MyMath.GreatestCommonDivisor(numerator, denominator);
                    return new Fraction(numerator / gretestCommonDivisor, denominator / gretestCommonDivisor);
                }
            }
        }

        public static Fraction operator +(Fraction a)
        {
            return a;
        }

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.numerator, a.denominator);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.denominator + b.numerator * a.denominator, a.denominator * b.numerator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return a + (-b);
        }

        public static Fraction operator *(Fraction a, int b)
        {
            return new Fraction(a.numerator * b, a.denominator * b);
        }

        public static Fraction operator *(Fraction a, float b)
        {
            return new Fraction(a.numerator * b, a.denominator * b);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.numerator * b.numerator, a.denominator * b.denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.numerator == 0)
            {
                throw new DivideByZeroException();
            }
            return new Fraction(a.numerator * b.denominator, a.denominator * b.numerator);
        }

        public override string ToString()
        {
            return $"{numerator}/{denominator}";
        }

        /// <summary>
        /// Return the fraction as an array -> { numerator, denominator }.
        /// </summary>
        public int[] ToArray()
        {
            return new int[] { numerator, denominator };
        }
    }
}