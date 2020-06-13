using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

namespace My_Utils
{
    public static class Extensions
    {
        #region Min & Max

        /// <summary>
        /// Return the smallest value of an collection.
        /// </summary>
        public static int Min(this IReadOnlyList<int> numbers)
        {
            int min = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (i == 0)
                {
                    min = numbers[i];
                }
                else
                {
                    if (numbers[i] < min)
                    {
                        min = numbers[i];
                    }
                }
            }

            return min;
        }


        /// <summary>
        /// Return the greatest value of an collection.
        /// </summary>
        public static int Max(this IReadOnlyList<int> numbers)
        {
            int min = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (i == 0)
                {
                    min = numbers[i];
                }
                else
                {
                    if (numbers[i] > min)
                    {
                        min = numbers[i];
                    }
                }
            }

            return min;
        }


        /// <summary>
        /// Return the smallest value of an collection.
        /// </summary>
        public static float Min(this IReadOnlyList<float> numbers)
        {
            float min = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (i == 0)
                {
                    min = numbers[i];
                }
                else
                {
                    if (numbers[i] < min)
                    {
                        min = numbers[i];
                    }
                }
            }

            return min;
        }


        /// <summary>
        /// Return the greatest value of an collection.
        /// </summary>
        public static float Max(this IReadOnlyList<float> numbers)
        {
            float min = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                if (i == 0)
                {
                    min = numbers[i];
                }
                else
                {
                    if (numbers[i] > min)
                    {
                        min = numbers[i];
                    }
                }
            }

            return min;
        }

        #endregion


        /// <summary>
        /// Convert a float2 colletion into a Vector3.
        /// </summary>
        public static Vector2 ToVector2(this float2 myFloat2)
        {
            return new Vector2(myFloat2.x, myFloat2.y);
        }


        /// <summary>
        /// Convert a float colletion into a Vector3.
        /// </summary>
        public static Vector2 ToVector2(this IReadOnlyList<float> collection)
        {
            if (collection.Count == 2 || collection.Count == 3)
            {
                return new Vector2(collection[0], collection[1]);
            }
            else
            {
                Debug.LogError("Tried to convert float collection into Vector3, but collection count not supported. " + collection.Count);
                return Vector3.zero;
            }
        }


        /// <summary>
        /// Convert a float collection into a Vector2.
        /// </summary>
        public static Vector3 ToVector3(this IReadOnlyList<float> collection)
        {
            if (collection.Count == 2)
            {
                return new Vector3(collection[0], collection[1], 0);
            }
            else if (collection.Count == 3)
            {
                return new Vector3(collection[0], collection[1], collection[2]);
            }
            else
            {
                Debug.LogError("Tried to convert float collection into Vector3, but collection count not supported. " + collection.Count);
                return Vector3.zero;
            }
        }


        /// <summary>
        /// Return if a collection has a item that matches a predicate.
        /// </summary>
        public static bool Has<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            foreach (T item in collection)
            {
                if (predicate.Invoke(item))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Return this Vector2 as an direction indicator. Example: Vector2(-4.5f, 7.8) => new Vector2(-1, 1);
        /// </summary>
        public static Vector2 AsDirection(this Vector2 vector2)
        {
            float xPos, yPos;
            if (MyMath.Approx(vector2.x, 0, 0.05f))
                xPos = 0;
            else if (vector2.x > 0)
                xPos = 1;
            else
                xPos = -1;

            if (MyMath.Approx(vector2.y, 0, 0.05f))
                yPos = 0;
            else if (vector2.y > 0)
                yPos = 1;
            else
                yPos = -1;

            return new Vector2(xPos, yPos);
        }


        /// <summary>
        /// Return this Vector2 as an direction indicator and normalized. Example: Vector2(-4.5f, 7.8) => new Vector2(-1, 1).normalized;
        /// </summary>
        public static Vector2 AsDirectionNormalized(this Vector2 vector2)
        {
            return AsDirection(vector2).normalized;
        }


        /// <summary>
        /// Return the axis that a Vector2 is pointing to. Ex: Vector2(-3.8, 0) => Axis.X;
        /// </summary>
        public static Axis ToAxis(this Vector2 vector2)
        {
            if (vector2.x != 0 && vector2.y != 0)
                return Axis.XandY;
            else if (vector2.x != 0)
                return Axis.X;
            else if (vector2.y != 0)
                return Axis.Y;
            else
                return Axis.None;
        }


        /// <summary>
        /// Convert a Vector2 into a float[2]
        /// </summary>
        public static float[] ToFloatArray(this Vector2 vector2)
        {
            return new float[]
            {
                vector2.x,
                vector2.y
            };
        }


        /// <summary>
        /// Convert a Vector3 into a float[3]
        /// </summary>
        public static float[] ToFloatArray(this Vector3 vector3)
        {
            return new float[]
            {
                vector3.x,
                vector3.y,
                vector3.z
            };
        }


        /// <summary>
        /// Convert a int[2] into a int2.
        /// </summary>
        public static int2 ToInt2(this int[] intArray)
        {
            if (intArray == null)
            {
                return new int2();
            }
            else
            {
            return new int2(intArray[0], intArray[1]);
            }
        }


        /// <summary>
        /// Convert a int2 into a int[2].
        /// </summary>
        public static int[] ToIntArray(this int2 myInt2)
        {
            return new int[] { myInt2.x, myInt2.y };
        }


        /// <summary>
        /// Convert a float collection into a Color.
        /// </summary>
        public static Color ToColor(this IReadOnlyList<float> collection)
        {
            if (collection.Count == 3)
            {
                return new Color(collection[0], collection[1], collection[2]);
            }
            else if (collection.Count == 4)
            {
                return new Color(collection[0], collection[1], collection[2], collection[3]);
            }
            else
            {
                Debug.LogError("Tried to convert float collection into Color, but collection count not supported. " + collection.Count);
                return new Color();
            }
        }


        /// <summary>
        /// Convert a Color into a floar array. (With alpha)
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float[] ToFloatArray(this Color color)
        {
            return new float[]
            {
                color.r,
                color.g,
                color.b,
                color.a
            };
        }


        /// <summary>
        /// Transform a Vector2 collection into a Vector3 array.
        /// </summary>
        public static Vector3[] ToVector3Array(this IReadOnlyList<Vector2> vector2Collection)
        {
            Vector3[] vector3Array = new Vector3[vector2Collection.Count];
            for (int i = 0; i < vector2Collection.Count; i++)
            {
                vector3Array[i] = vector2Collection[i];
            }
            return vector3Array;
        }


        /// <summary>
        /// Transform a Vector3 collection into a Vector2 array.
        /// </summary>
        public static Vector2[] ToVector2Array(this IReadOnlyList<Vector3> vector3Collection)
        {
            Vector2[] vector2Array = new Vector2[vector3Collection.Count];
            for (int i = 0; i < vector3Collection.Count; i++)
            {
                vector2Array[i] = vector3Collection[i];
            }
            return vector2Array;
        }


        public static void ForEach<T>(this IReadOnlyList<T> list, Action<T> function)
        {
            foreach (T element in list)
            {
                function(element);
            }
        }


        /// <summary>
        /// Return the integer part of a float. "2.29 => 2"
        /// </summary>
        public static float IntegerPart(this float floatNumber, bool absValue = false)
        {
            if (absValue)
            {
                return Math.Abs(floatNumber - DecimalPart(floatNumber));
            }
            else
            {
                return floatNumber - DecimalPart(floatNumber);
            }
        }


        /// <summary>
        /// Return the decimal part of a float. "2.29 => 0.29"
        /// </summary>
        public static float DecimalPart(this float floatNumber, bool absValue = false)
        {
            if (absValue)
            {
                return Math.Abs(floatNumber - (int)floatNumber);
            }
            else
            {
                return floatNumber - (int)floatNumber;
            }
        }

        /// <summary>
        /// Return the quantity of decimal places a float number has.
        /// </summary>
        public static int DecimalPlaces(this float floatNumber)
        {
            int decimalPlaces = 0;
            while (floatNumber.DecimalPart() != 0)
            {
                floatNumber *= 10;
                decimalPlaces++;
            }

            return decimalPlaces;
        }


        public static bool NearlyEqual(this float floatNumber, float otherFloat, float epsilon = 0.001f)
        {
            float absA = Math.Abs(floatNumber);
            float absB = Math.Abs(otherFloat);
            float diff = absA - absB;

            if (floatNumber == otherFloat)
            {
                return true;
            }
            else
            {
                return diff < epsilon;
            }
        }


        /// <summary>
        /// Return true if a number is prime.
        /// </summary>
        public static bool IsPrime(this int number)
        {
            int max = number / 2;
            for (int i = 2; i <= max; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Return the product of all values of this collection.
        /// </summary>
        public static int Product(this IReadOnlyList<int> numbers)
        {
            if (numbers.Count > 0)
            {
                int mult = 1;
                for (int i = 0; i < numbers.Count; i++)
                {
                    mult *= numbers[i];
                }
                return mult;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Return the greteast common divisor between all values of a collection.
        /// </summary>
        /// <param name="numbers">The numbers that you want to compare divisors.</param>
        public static int GreatestCommonDivisor(this IReadOnlyList<int> numbers)
        {
            int minNumber = numbers.Min();
            int[] primes = MyMath.GetPrimesBetween(2, minNumber);

            List<int> commonDivisors = new List<int>();
            for (int i = 0; i < primes.Length; i++)
            {
                if (numbers.IsDivisibleBy(primes[i]))
                {
                    commonDivisors.Add(primes[i]);
                }
            }

            if (commonDivisors.Count > 0)
            {
                return commonDivisors.Product();
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Return the product of all values of this collection.
        /// </summary>
        public static float Mult(this IReadOnlyList<float> numbers)
        {
            if (numbers.Count > 0)
            {
                float mult = 1;
                for (int i = 0; i < numbers.Count; i++)
                {
                    mult *= numbers[i];
                }
                return mult;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Return true if all values is divisible by 'divisor' parameter.
        /// </summary>
        /// <param name="numbers">The int array that you want to check.</param>
        /// <param name="divisor">The divisor that you want to check.</param>
        public static bool IsDivisibleBy(this IReadOnlyList<int> numbers, int divisor)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] % divisor != 0)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Choice and return a random value of the collection.
        /// </summary>
        public static T ChoiceAnyOne<T>(this IReadOnlyList<T> collection)
        {
            int index = MyRandom.Next(0, collection.Count);
            return collection[index];
        }


        /// <summary>
        /// Shuffle the collection.
        /// </summary>
        public static void Shuffle<T>(this IList<T> collection)
        {
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                int randomIndex = MyRandom.Next(0, i + 1);

                T objInPosI = collection[i];

                collection[i] = collection[randomIndex];
                collection[randomIndex] = objInPosI;
            }
        }


        /// <summary>
        /// Returns the index of a value.
        /// </summary>
        public static int IndexOf<T>(this IReadOnlyList<T> collection, T item)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }


        /// <summary>
        /// Return a collection filled with a value.
        /// </summary>
        public static void Fill<T>(this IList<T> collection, T value)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i] = value;
            }
        }


        /// <summary>
        /// Clone a list<T> if T is ICloneable.
        /// </summary>
        public static IList<T> CLone<T>(this IList<T> collection) where T : ICloneable
        {
            return collection.Select(item => (T)item.Clone()).ToList();
        }


        /// <summary>
        /// Transform a int[] into a enum[].
        /// </summary>
        /// <typeparam name="T">The enum that you are trying to convert.</typeparam>
        /// <param name="intList">List of integers to convert into an enum.</param>
        /// <returns>Returns a enum of the values contained in the int[] </returns>
        public static T[] ToEnum<T>(this IReadOnlyList<int> intArray) where T : Enum
        {
            return intArray.Cast<T>().ToArray();
        }


        /// <summary>
        /// Return true if the dict already contains a value.
        /// </summary>
        public static bool Contains<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value)
        {
            foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
            {
                if (keyValuePair.Value.Equals(value))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Return all enum values of a type T contained in a bitmask.
        /// </summary>
        public static T[] ToEnum<T>(this int bitMask) where T : Enum
        {
            List<int> selectedValues = new List<int>();

            if (bitMask < 0)
            {
                int lenght = Enum.GetValues(typeof(T)).Length;

                int powOfTwo = 1;
                for (int i = 0; i < lenght; i++)
                {
                    selectedValues.Add(powOfTwo);
                    powOfTwo *= 2;
                }
            }
            else
            {
                for (int powOfTwo = 1; powOfTwo <= bitMask; powOfTwo *= 2)
                {
                    if ((bitMask & powOfTwo) == powOfTwo)
                    {
                        selectedValues.Add(powOfTwo);
                    }
                }
            }

            if (selectedValues.Count == 0)
            {
                selectedValues.Add(0);
            }

            return selectedValues.ToEnum<T>();
        }


        /// <summary>
        /// Return all enums cointained in a bitmask enum, or a normal enum.
        /// </summary>
        public static T[] Decrypt<T>(this Enum enumValue) where T : Enum
        {
            int bitMask = int.Parse(enumValue.ToString());
            return bitMask.ToEnum<T>();
        }


        /// <summary>
        /// Add all elements of another list to this list without adding equal elements.
        /// </summary>
        /// <param name="listToAdd">The list that you want to add all the elements.</param>
        public static void AddRangeWithoutRepeat<T>(this List<T> list, List<T> listToAdd)
        {
            for (int i = 0; i < listToAdd.Count; i++)
            {
                if (!list.Contains(listToAdd[i]))
                {
                    list.Add(listToAdd[i]);
                }
            }
        }

        /// <summary>
        /// Get all renderers (recursively) of a gameObject and returns as GenericRenderers.
        /// </summary>
        public static List<GenericRenderer> GetAllRenderers(this Transform transform, RendererType rendererType)
        {
            List<GenericRenderer> genericRenders = new List<GenericRenderer>();

            if (rendererType < 0)
            {
                genericRenders.Add(new GenericRenderer(
                                                        transform.GetComponent<SpriteRenderer>(),
                                                        transform.GetComponent<ColorGroup>(),
                                                        transform.GetComponent<AlphaGroup>(),
                                                        transform.GetComponent<Tilemap>(),
                                                        transform.GetComponent<Image>(),
                                                        transform.GetComponent<RawImage>(),
                                                        transform.GetComponent<Text>(),
                                                        transform.GetComponent<TextMeshProUGUI>(),
                                                        transform.GetComponent<CanvasGroup>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        child.GetComponent<SpriteRenderer>(),
                                                        child.GetComponent<ColorGroup>(),
                                                        child.GetComponent<AlphaGroup>(),
                                                        child.GetComponent<Tilemap>(),
                                                        child.GetComponent<Image>(),
                                                        child.GetComponent<RawImage>(),
                                                        child.GetComponent<Text>(),
                                                        child.GetComponent<TextMeshProUGUI>(),
                                                        child.GetComponent<CanvasGroup>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.SpriteRenderer)
            {
                genericRenders.Add(new GenericRenderer(
                                                        transform.GetComponent<SpriteRenderer>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        child.GetComponent<SpriteRenderer>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.ColorGroup)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        transform.GetComponent<ColorGroup>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        child.GetComponent<ColorGroup>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.AlphaGroup)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        transform.GetComponent<AlphaGroup>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        child.GetComponent<AlphaGroup>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.Tilemap)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        transform.GetComponent<Tilemap>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        child.GetComponent<Tilemap>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.Image)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        transform.GetComponent<Image>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        child.GetComponent<Image>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.RawImage)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        transform.GetComponent<RawImage>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        child.GetComponent<RawImage>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.Text)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        transform.GetComponent<Text>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        child.GetComponent<Text>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.TmProText)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        transform.GetComponent<TextMeshProUGUI>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        child.GetComponent<TextMeshProUGUI>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }
            else if (rendererType == RendererType.CanvasGroup)
            {
                genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        transform.GetComponent<CanvasGroup>()
                                                        ));

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    genericRenders.Add(new GenericRenderer(
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        child.GetComponent<CanvasGroup>()
                                                        ));

                    if (child.childCount > 0)
                    {
                        List<GenericRenderer> childComponents = GetAllRenderers(child, rendererType);

                        genericRenders.AddRange(childComponents);
                    }
                }
            }

            return genericRenders;
        }


        /// <summary>
        /// Get all renderers (recursively) of a gameObject of specific types.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rendererTypes"></param>
        /// <returns></returns>
        public static List<GenericRenderer> GetAllRenderers(this Transform transform, RendererType[] rendererTypes)
        {
            bool spriteRend = rendererTypes.Contains(RendererType.SpriteRenderer);
            bool colorGroupRend = rendererTypes.Contains(RendererType.ColorGroup);
            bool alphaGroupRend = rendererTypes.Contains(RendererType.AlphaGroup);
            bool tilemapRend = rendererTypes.Contains(RendererType.Tilemap);
            bool imageRend = rendererTypes.Contains(RendererType.Image);
            bool rawImageRend = rendererTypes.Contains(RendererType.RawImage);
            bool textRend = rendererTypes.Contains(RendererType.Text);
            bool tmTextRend = rendererTypes.Contains(RendererType.TmProText);
            bool canvasGroupRend = rendererTypes.Contains(RendererType.CanvasGroup);

            List<GenericRenderer> genericRenders = new List<GenericRenderer>();

            SpriteRenderer spriteRenderer = spriteRend ? transform.GetComponent<SpriteRenderer>() : null;
            ColorGroup colorGroup = colorGroupRend ? transform.GetComponent<ColorGroup>() : null;
            AlphaGroup alphaGroup = alphaGroupRend ? transform.GetComponent<AlphaGroup>() : null;
            Tilemap tilemap = tilemapRend ? transform.GetComponent<Tilemap>() : null;
            Image image = imageRend ? transform.GetComponent<Image>() : null;
            RawImage rawImage = rawImageRend ? transform.GetComponent<RawImage>() : null;
            Text text = textRend ? transform.GetComponent<Text>() : null;
            TextMeshProUGUI tmText = tmTextRend ? transform.GetComponent<TextMeshProUGUI>() : null;
            CanvasGroup canvasGroup = canvasGroupRend ? transform.GetComponent<CanvasGroup>() : null;

            genericRenders.Add(new GenericRenderer(
                                                    spriteRenderer,
                                                    colorGroup,
                                                    alphaGroup,
                                                    tilemap,
                                                    image,
                                                    rawImage,
                                                    text,
                                                    tmText,
                                                    canvasGroup));

            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer spriteRenderer2 = spriteRend ? transform.GetComponent<SpriteRenderer>() : null;
                ColorGroup colorGroup2 = colorGroupRend ? transform.GetComponent<ColorGroup>() : null;
                AlphaGroup alphaGroup2 = alphaGroupRend ? transform.GetComponent<AlphaGroup>() : null;
                Tilemap tilemap2 = tilemapRend ? transform.GetComponent<Tilemap>() : null;
                Image image2 = imageRend ? transform.GetComponent<Image>() : null;
                RawImage rawImage2 = rawImageRend ? transform.GetComponent<RawImage>() : null;
                Text text2 = textRend ? transform.GetComponent<Text>() : null;
                TextMeshProUGUI tmText2 = tmTextRend ? transform.GetComponent<TextMeshProUGUI>() : null;
                CanvasGroup canvasGroup2 = canvasGroupRend ? transform.GetComponent<CanvasGroup>() : null;

                Transform child = transform.GetChild(i);

                genericRenders.Add(new GenericRenderer(
                                                    spriteRenderer2,
                                                    colorGroup2,
                                                    alphaGroup2,
                                                    tilemap2,
                                                    image2,
                                                    rawImage2,
                                                    text2,
                                                    tmText2,
                                                    canvasGroup2
                                                    ));

                if (child.childCount > 0)
                {
                    List<GenericRenderer> childComponents = child.GetAllRenderers(rendererTypes);

                    genericRenders.AddRange(childComponents);
                }
            }


            return genericRenders;
        }


        /// <summary>
        /// Get all renderers (recursively) in a gameObject in a rendererType bitmask.
        /// </summary>
        /// <param name="rendererTypeBitMask">The bitmask of the renderer types that you want to get.</param>
        public static List<GenericRenderer> GetAllRenderers(this Transform transform, int rendererTypeBitMask)
        {
            RendererType[] selectedEnums = rendererTypeBitMask.ToEnum<RendererType>();

            return transform.GetAllRenderers(selectedEnums);
        }


        /// <summary>
        /// Get all components of a certain type.
        /// </summary>
        public static List<T> GetAllComponentsOfType<T>(this Transform transform) where T : class
        {
            List<T> components = new List<T>();

            components.Add(transform.GetComponent<T>());

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                components.Add(childTransform.GetComponent<T>());

                if (childTransform.childCount > 0)
                {
                    List<T> childComponents = childTransform.GetAllComponentsOfType<T>();

                    components.AddRange(childComponents);
                }
            }

            return components; // Need remove nulls later
        }


        /// <summary>
        /// Get the first specific component in all recursive parents of a gameObject.
        /// </summary>
        public static T GetComponentInParents<T>(this Transform transform) where T : Component
        {
            T component = transform.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            else
            {
                if (transform.parent != null)
                {
                    return GetComponentInParents<T>(transform.parent);
                }
                else
                {
                    return default;
                }
            }
        }


        /// <summary>
        /// Get all components of a type in all recursive parents of a gameObject.
        /// </summary>
        public static List<T> GetAllComponentsInParents<T>(this Transform transform) where T : Component
        {
            List<T> components = new List<T>();

            T component = transform.GetComponent<T>();
            if (component != null)
            {
                components.Add(component);
            }
            if (transform.parent != null)
            {
                components.AddRange(transform.parent.GetAllComponentsInParents<T>());
            }

            return components;
        }


        /// <summary>
        /// Convert a world position into a canvas position
        /// </summary>
        /// <param name="wordlPos">The world position that you want to convert.</param>
        /// <param name="canvas">The canvas that you are using.</param>
        /// <param name="camera">The camera that you will be used to convert. (By default is Camera.main) </param>
        public static Vector2 ToCanvasSpace(this Vector2 wordlPos, Canvas canvas, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            var viewport_position = camera.WorldToViewportPoint(wordlPos);
            var canvas_rect = canvas.GetComponent<RectTransform>();

            return new Vector2((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                               (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f));
        }


        /// <summary>
        /// Convert a world position into a canvas position
        /// </summary>
        /// <param name="wordlPos">The world position that you want to convert.</param>
        /// <param name="canvas">The canvas that you are using.</param>
        /// <param name="camera">The camera that you will be used to convert. (By default is Camera.main) </param>
        public static Vector3 ToCanvasSpace(this Vector3 wordlPos, Canvas canvas, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            var viewport_position = camera.WorldToViewportPoint(wordlPos);
            var canvas_rect = canvas.GetComponent<RectTransform>();

            return new Vector3((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f),
                               (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f),
                               0);
        }

        /// <summary>
        /// Return true if an layerMask contain another layer inside it.
        /// Use to know if a layerMask contain a collision object layer.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="otherLayer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask layerMask, int otherLayer)
        {
            return (layerMask.value & 1 << otherLayer) != 0;
        }


        /// <summary>
        /// Return true if an int bitmask contains a enum value inside it.
        /// </summary>
        public static bool Contains(this int number, int enumValue)
        {
            return (number & enumValue) == enumValue;
        }


        /// <summary>
        /// Return true if the distance between two positions is less than some distance.
        /// </summary>
        /// <param name="posToCompare">The position you want to compare with.</param>
        /// <param name="distance">The maximum distance to return true.</param>
        /// <returns></returns>
        public static bool IsCloseTo(this Vector2 vector2, Vector2 posToCompare, float distance)
        {
            return Mathf.Abs((vector2 - posToCompare).sqrMagnitude) <= distance * distance;
        }


        /// <summary>
        /// Return true if the distance between two positions is less than some distance.
        /// </summary>
        /// <param name="posToCompare">The position you want to compare with.</param>
        /// <param name="distance">The maximum distance to return true.</param>
        /// <returns></returns>
        public static bool IsCloseTo(this Vector3 vector3, Vector3 posToCompare, float distance)
        {
            return (vector3 - posToCompare).sqrMagnitude < distance * distance;
        }


        /// <summary>
        /// Return the approximate magnitude value of a vector with a small error margin (linear scaling, so don't use for values biiger than (1, 1)), but faster than normal magnitude.
        /// </summary>
        public static float FastMagnitude(this Vector2 vector2)
        {
            // Magnitude ~= Alpha * Max(x, y) + Beta * Min(x, y)
            // In this case Alpha = 0.947543636291f and Beta = 0.392485425092f

            float absX = Math.Abs(vector2.x);
            float absY = Math.Abs(vector2.y);

            return 0.947543636291f * Math.Max(absX, absY) + 0.392485425092f + Math.Min(absX, absY);
        }


        public static float ManhattanDistanceTo(this Vector2 vector2, Vector2 otherVector2)
        {
            return Math.Abs(vector2.x - otherVector2.x) + Math.Abs(vector2.y - otherVector2.y);
        }
    }
}
