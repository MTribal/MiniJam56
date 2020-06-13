using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// Type of chars.
    /// </summary>
    
    [Flags]
    public enum CharType
    {
        None = 0,
        Alphabetic = 1,
        Numeric = 2,
        Special = 4
    }

    public delegate char GetChar();

    /// <summary>
    /// A proper and easy way to get random numbers.
    /// </summary>
    public static class MyRandom
    {
        public static System.Random random = new System.Random();


        /// <summary>
        /// Returns a random value where [min <= x < max].
        /// </summary>
        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }


        /// <summary>
        /// Return a value between 0 and 1.
        /// </summary>
        /// <returns></returns>
        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Returns a random value where [min <= x < max].
        /// </summary>
        public static float NextFloat(float min, float max)
        {
            return MyMath.Clamp((float)random.NextDouble() * max, min, max);
        }


        /// <summary>
        /// Return a random Vector2, with a specif range, [min <= value < max]
        /// </summary>
        public static Vector2 NextVector2(float min, float max)
        {
            return new Vector2(NextFloat(min, max), NextFloat(min, max));
        }


        /// <summary>
        /// Return a random Vector3, with a specif range, [min <= value < max]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 NextVector3(float min, float max)
        {
            return new Vector3(NextFloat(min, max), NextFloat(min, max), NextFloat(min, max));
        }


        /// <summary>
        /// Has a specif chance of return true. 
        /// </summary>
        /// <param name="chance">The chance of return true, represented as a fraction. Ex: 1/4 => 25% </param>
        public static bool Probality(Fraction chance)
        {
            return Next(1, chance.denominator + 1) <= chance.numerator;
        }


        /// <summary>
        /// Has a specif chance of return true.
        /// </summary>
        /// <param name="chancePercentage">The chance of return true, represented as percentage.</param>
        public static bool Probality(float chancePercentage)
        {
            Fraction chance = new Fraction(chancePercentage, 100);

            return Next(1, chance.denominator + 1) <= chance.numerator;
        }


        /// <summary>
        /// Retrun a random string.
        /// </summary>
        /// <param name="stringLength">The length of the string.</param>
        /// <param name="permittedChars">The type of chars that is permitted. (Can use flags)</param>
        public static string GetRandomString(int stringLength, CharType permittedChars)
        {
            if (permittedChars == 0)
            {
                throw new ArgumentException("Argument can't be None. " + nameof(permittedChars));
            }

            bool allowAlphaChars = (permittedChars & CharType.Alphabetic) == CharType.Alphabetic;
            bool allowNumChars = (permittedChars & CharType.Numeric) == CharType.Numeric;
            bool allowSpecialChars = (permittedChars & CharType.Special) == CharType.Special;

            List<GetChar> possibleFunctions = new List<GetChar>();
            if (allowAlphaChars)
            {
                possibleFunctions.Add(GetRandomAlphabeticChar);
            }
            if (allowNumChars)
            {
                possibleFunctions.Add(GetRandomNumericChar);
            }
            if (allowSpecialChars)
            {
                possibleFunctions.Add(GetRandomSpecialChar);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < stringLength; i++)
            {
                char randomChar = possibleFunctions.ChoiceAnyOne().Invoke();
                sb.Append(randomChar);
            }

            return sb.ToString();
        }

        public static char GetRandomAlphabeticChar()
        {
            int randomIndex = Next(0, 2);
            if (randomIndex == 0)
            {
                return (char)Next(65, 91);
            }
            else
            {
                return (char)Next(97, 122);
            }
        }

        public static char GetRandomNumericChar()
        {
            return (char)Next(48, 58);
        }

        public static char GetRandomSpecialChar()
        {
            int randomIndex = new int[] { 0, 1, 2, 3, 4 }.ChoiceAnyOne();
            if (randomIndex == 0)
            {
                return (char)Next(33, 48);
            }
            else if (randomIndex == 1)
            {
                return (char)Next(58, 65);
            }
            else if (randomIndex == 2)
            {
                return (char)Next(58, 65);
            }
            else if (randomIndex == 3)
            {
                return (char)Next(91, 97);
            }
            else
            {
                return (char)Next(123, 127);
            }
        }

        public static Color GetRandomColor()
        {
            return new Color(NextFloat(0, 1), NextFloat(0, 1), NextFloat(0, 1));
        }
    }
}
