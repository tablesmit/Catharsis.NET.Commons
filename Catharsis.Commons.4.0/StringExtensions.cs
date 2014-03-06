using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Catharsis.Commons
{
  /// <summary>
  ///   <para>Set of extension methods for class <see cref="string"/>.</para>
  /// </summary>
  /// <seealso cref="string"/>
  public static class StringExtensions
  {
    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    public static byte[] Base64(this string value)
    {
      Assertion.NotNull(value);

      return System.Convert.FromBase64String(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="encoding"></param>
    /// <param name="preamble"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    public static byte[] Bytes(this string value, Encoding encoding = null, bool preamble = true)
    {
      Assertion.NotNull(value);

      var textEncoding = encoding ?? Encoding.UTF8;
      return preamble ? textEncoding.GetPreamble().Join(textEncoding.GetBytes(value)) : textEncoding.GetBytes(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="self"></param>
    /// <param name="other"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    public static int CompareTo(this string self, string other, StringComparison comparison)
    {
      Assertion.NotNull(self);

      return string.Compare(self, other, comparison);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="self"></param>
    /// <param name="other"></param>
    /// <param name="options"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    public static int CompareTo(this string self, string other, CompareOptions options, CultureInfo culture = null)
    {
      Assertion.NotNull(self);

      return string.Compare(self, other, culture ?? CultureInfo.InvariantCulture, options);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="action"></param>
    /// <exception cref="ArgumentNullException">If either <paramref name="value"/> or <paramref name="action"/> is a <c>null</c> reference.</exception>
    public static void EachLine(this string value, Action<string> action)
    {
      Assertion.NotNull(value);
      Assertion.NotNull(action);

      value.Split(new [] { Environment.NewLine }, StringSplitOptions.None).Each(action);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    public static string FormatSelf(this string value, params object[] args)
    {
      Assertion.NotNull(value);

      return string.Format(value, args);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    public static byte[] Hex(this string value)
    {
      Assertion.NotNull(value);

      if (value.Length == 0)
      {
        return Enumerable.Empty<byte>().ToArray();
      }

      var result = new byte[value.Length / 2];
      for (var i = 0; i < value.Length; i += 2)
      {
        result[i / 2] = byte.Parse(value.Substring(i, 2), NumberStyles.HexNumber);
      }
      return result;
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string value)
    {
      return string.IsNullOrEmpty(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="value"/> or <paramref name="pattern"/> is a <c>null</c> reference.</exception>
    public static bool Matches(this string value, string pattern)
    {
      Assertion.NotNull(value);
      Assertion.NotNull(pattern);

      return Regex.IsMatch(value, pattern);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="where"></param>
    /// <param name="replacements"></param>
    /// <exception cref="ArgumentNullException">If either <paramref name="where"/> or <paramref name="replacements"/> is a <c>null</c> reference.</exception>
    public static string Replace(this string where, IEnumerable<KeyValuePair<string, string>> replacements)
    {
      Assertion.NotNull(where);
      Assertion.NotNull(replacements);

      var sb = new StringBuilder(where);
      replacements.Where(replacement => replacement.Value != null).Each(replacement => sb.Replace(replacement.Key, replacement.Value));
      return sb.ToString();
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="where"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="where"/>, <paramref name="from"/> or <paramref name="to"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException"></exception>
    public static string Replace(this string where, IList<string> from, IList<string> to)
    {
      Assertion.NotNull(where);
      Assertion.NotNull(from);
      Assertion.NotNull(to);
      Assertion.True(from.Count == to.Count);

      var sb = new StringBuilder(where);
      for (var i = 0; i < from.Count; i++)
      {
        sb.Replace(from[i], to[i]);
      }
      return sb.ToString();
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="where"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="where"/>, <paramref name="from"/> or <paramref name="to"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException"></exception>
    public static string Replace(this string where, string[] from, string[] to)
    {
      Assertion.NotNull(where);
      Assertion.NotNull(from);
      Assertion.NotNull(to);
      Assertion.True(from.Length == to.Length);

      var sb = new StringBuilder(where);
      for (var i = 0; i < from.Length; i++)
      {
        sb.Replace(from[i], to[i]);
      }
      return sb.ToString();
    }

    /// <summary>
    ///   <para>Converts the specified string representation of a logical value to its <see cref="Boolean"/> equivalent.</para>
    /// </summary>
    /// <param name="boolean">A string containing the value to convert.</param>
    /// <returns><c>true</c> if value is equivalent to <see cref="Boolean.TrueString"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="boolean"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="boolean"/> is <see cref="string.Empty"/> string.</exception>
    /// <seealso cref="bool.Parse(string)"/>
    public static bool ToBoolean(this string boolean)
    {
      Assertion.NotEmpty(boolean);

      return Boolean.Parse(boolean);
    }

    /// <summary>
    ///  <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToBoolean(this string value, out bool result)
    {
      return bool.TryParse(value, out result);
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its <see cref="byte"/> equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing a number to convert. The string is interpreted using the <see cref="NumberStyles.Integer"/> style.</param>
    /// <returns>The <see cref="byte"/> value equivalent to the number contained in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="byte.Parse(string)"/>
    public static byte ToByte(this string value)
    {
      Assertion.NotEmpty(value);

      return Byte.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToByte(this string value, out byte result)
    {
      return byte.TryParse(value, out result);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is <see cref="string.Empty"/> string.</exception>
    public static DateTime ToDateTime(this string value)
    {
      Assertion.NotEmpty(value);

      return DateTime.Parse(value);
    }
    
    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToDateTime(this string value, out DateTime result)
    {
      return DateTime.TryParse(value, out result);
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its <see cref="Decimal"/> equivalent.</para>
    /// </summary>
    /// <param name="value">The string representation of the number to convert.</param>
    /// <returns>The <see cref="Decimal"/> number equivalent to the number contained in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="decimal.Parse(string)"/>
    public static decimal ToDecimal(this string value)
    {
      Assertion.NotEmpty(value);

      return Decimal.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToDecimal(this string value, out decimal result)
    {
      return decimal.TryParse(value, out result);
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its double-precision floating-point number equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing a number to convert.</param>
    /// <returns>A double-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="double.Parse(string)"/>
    public static double ToDouble(this string value)
    {
      Assertion.NotEmpty(value);

      return double.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToDouble(this string value, out double result)
    {
      return double.TryParse(value, out result);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is <see cref="string.Empty"/> string.</exception>
    public static T ToEnum<T>(this string value) where T : struct
    {
      Assertion.True(typeof(T).IsEnum);

      return Enum.Parse(typeof(T), value, true).To<T>();
    }

    /// <summary>
    ///   <para>Converts the specified string representation of a GUID (Globally Unique Identifier) to its <see cref="Guid"/> equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing the GUID value to convert in one of the supported formats.</param>
    /// <returns>The <see cref="Guid"/> value equivalent to the number contained in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="Guid(string)"/>
    public static Guid ToGuid(this string value)
    {
      Assertion.NotEmpty(value);

      return new Guid(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToGuid(this string value, out Guid result)
    {
      try
      {
        result = new Guid(value);
        return true;
      }
      catch
      {
        result = default(Guid);
        return false;
      }
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its 16-bit signed integer equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing a number to convert.</param>
    /// <returns>A 16-bit signed integer equivalent to the number contained in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="short.Parse(string)"/>
    public static short ToInt16(this string value)
    {
      Assertion.NotEmpty(value);

      return Int16.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToInt16(this string value, out short result)
    {
      return short.TryParse(value, out result);
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its 32-bit signed integer equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing a number to convert.</param>
    /// <returns>A 32-bit signed integer equivalent to the number contained in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="int.Parse(string)"/>
    public static int ToInt32(this string value)
    {
      Assertion.NotEmpty(value);

      return Int32.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToInt32(this string value, out int result)
    {
      return int.TryParse(value, out result);
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its 64-bit signed integer equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing a number to convert.</param>
    /// <returns>A 64-bit signed integer equivalent to the number contained in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="long.Parse(string)"/>
    public static long ToInt64(this string value)
    {
      Assertion.NotEmpty(value);

      return Int64.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToInt64(this string value, out long result)
    {
      return long.TryParse(value, out result);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is <see cref="string.Empty"/> string.</exception>
    public static IPAddress ToIpAddress(this string value)
    {
      Assertion.NotEmpty(value);

      return IPAddress.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToIpAddress(this string value, out IPAddress result)
    {
      return IPAddress.TryParse(value, out result);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="pattern"/> is a <c>null</c> reference.</exception>
    public static Regex ToRegex(this string pattern)
    {
      Assertion.NotNull(pattern);

      return new Regex(pattern);
    }

    /// <summary>
    ///   <para>Converts the string representation of a number to its single-precision floating-point number equivalent.</para>
    /// </summary>
    /// <param name="value">A string representing a number to convert.</param>
    /// <returns> single-precision floating-point number equivalent to the numeric value or symbol specified in <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is empty string.</exception>
    /// <seealso cref="Single.Parse(string)"/>
    public static Single ToSingle(this string value)
    {
      Assertion.NotEmpty(value);

      return Single.Parse(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToSingle(this string value, out Single result)
    {
      return Single.TryParse(value, out result);
    }

    /// <summary>
    ///   <para>Converts the specified string representation of a URI to its <see cref="Uri"/> equivalent.</para>
    /// </summary>
    /// <param name="value">A string containing a URI address.</param>
    /// <returns>The <see cref="Uri"/> instance created from the given string address.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is <see cref="string.Empty"/> string.</exception>
    /// <seealso cref="Uri(string)"/>
    public static Uri ToUri(this string value)
    {
      Assertion.NotEmpty(value);

      return new Uri(value);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ToUri(this string value, out Uri result)
    {
      return Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out result);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="marker"></param>
    /// <param name="tokens"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="value"/>, <paramref name="tokens"/> or <paramref name="marker"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="marker"/> is <see cref="string.Empty"/> string.</exception>
    public static string Tokenize(this string value, IDictionary<string, object> tokens, string marker = ":")
    {
      Assertion.NotNull(value);
      Assertion.NotNull(tokens);
      Assertion.NotEmpty(marker);

      return value.Replace(tokens.Keys.Select(token => "{0}{1}".FormatSelf(marker, token)).ToArray(), tokens.Values.Select(token => token.ToString()).ToArray());
    }

    /// <summary>
    ///   <para>Determines whether the specified string is either <see cref="string.Empty"/> or consists only of white-space characters.</para>
    /// </summary>
    /// <param name="value">A string reference.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> parameter is either empty string or consists of white-space characters; otherwise, <c>false</c>.</returns>
    public static bool Whitespace(this string value)
    {
      return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
    }
  }
}