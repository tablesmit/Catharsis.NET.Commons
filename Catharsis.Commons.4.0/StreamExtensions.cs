﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Catharsis.Commons
{
  /// <summary>
  ///   <para>Set of extension methods for class <see cref="Stream"/>.</para>
  /// </summary>
  /// <seealso cref="Stream"/>
  public static class StreamExtensions
  {
    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="BinaryReader"/>
    /// <seealso cref="BinaryWriter(Stream, Encoding)"/>
    public static BinaryReader BinaryReader(this Stream stream, Encoding encoding = null)
    {
      Assertion.NotNull(stream);

      return encoding != null ? new BinaryReader(stream, encoding) : new BinaryReader(stream);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="BinaryWriter"/>
    /// <seealso cref="BinaryReader(Stream, Encoding)"/>
    public static BinaryWriter BinaryWriter(this Stream stream, Encoding encoding = null)
    {
      Assertion.NotNull(stream);

      return encoding != null ? new BinaryWriter(stream, encoding) : new BinaryWriter(stream);
    }

    /// <summary>
    ///   <para>Read the content of this <see cref="Stream"/> and return it as a <see cref="byte"/> array. The input is closed before this method returns.</para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="close"></param>
    /// <returns>The <see cref="byte"/> array from that <paramref name="stream"/></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    public static byte[] Bytes(this Stream stream, bool close = false)
    {
      Assertion.NotNull(stream);

      var destination = new MemoryStream();
      try
      {
        const int bufferSize = 4096;
        var buffer = new byte[bufferSize];
        int count;
        while ((count = stream.Read(buffer, 0, bufferSize)) > 0)
        {
          destination.Write(buffer, 0, count);
        }
      }
      finally
      {
        destination.Close();
        if (close)
        {
          stream.Close();
        }
      }
      return destination.ToArray();
    }
    
    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    public static T Rewind<T>(this T stream) where T : Stream
    {
      Assertion.NotNull(stream);

      stream.Seek(0, SeekOrigin.Begin);
      return stream;
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="close"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    public static string Text(this Stream stream, bool close = false, Encoding encoding = null)
    {
      Assertion.NotNull(stream);

      return stream.CanRead ? stream.TextReader(encoding).Text(close) : string.Empty;
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="TextReader"/>
    /// <seealso cref="TextWriter(Stream, Encoding)"/>
    public static TextReader TextReader(this Stream stream, Encoding encoding = null)
    {
      Assertion.NotNull(stream);

      return encoding != null ? new StreamReader(stream, encoding) : new StreamReader(stream, Encoding.UTF8);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="TextWriter"/>
    /// <seealso cref="TextReader(Stream, Encoding)"/>
    public static TextWriter TextWriter(this Stream stream, Encoding encoding = null)
    {
      Assertion.NotNull(stream);

      return encoding != null ? new StreamWriter(stream, encoding) : new StreamWriter(stream, Encoding.UTF8);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="stream"/> or <paramref name="bytes"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="Write{T}(T, Stream)"/>
    /// <seealso cref="Write{T}(T, string, Encoding)"/>
    public static T Write<T>(this T stream, byte[] bytes) where T : Stream
    {
      Assertion.NotNull(stream);
      Assertion.NotNull(bytes);

      if (stream.CanWrite && bytes.Length > 0)
      {
        stream.Write(bytes, 0, bytes.Length);
      }

      return stream;
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="to"/> or <paramref name="from"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="Write{T}(T, byte[])"/>
    /// <seealso cref="Write{T}(T, string, Encoding)"/>
    public static T Write<T>(this T to, Stream from) where T : Stream
    {
      Assertion.NotNull(to);
      Assertion.NotNull(from);

      if (to.CanWrite && from.CanRead)
      {
        const int bufferSize = 4096;
        var buffer = new byte[bufferSize];
        int count;
        while ((count = from.Read(buffer, 0, bufferSize)) > 0)
        {
          to.Write(buffer, 0, count);
        }
      }

      return to;
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <param name="text"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="stream"/> or <paramref name="text"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="Write{T}(T, byte[])"/>
    /// <seealso cref="Write{T}(T, Stream)"/>
    public static T Write<T>(this T stream, string text, Encoding encoding = null) where T : Stream
    {
      Assertion.NotNull(stream);
      Assertion.NotNull(text);

      if (stream.CanWrite && text.Length > 0)
      {
        var writer = stream.TextWriter(encoding);
        writer.Write(text);
        writer.Flush();
      }

      return stream;
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="close"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XDocument"/>
    public static XDocument XDocument(this Stream stream, bool close = false)
    {
      Assertion.NotNull(stream);

      return System.Xml.XmlReader.Create(stream, new XmlReaderSettings { CloseInput = close }).Read(System.Xml.Linq.XDocument.Load);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="close"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlReader"/>
    /// <seealso cref="XmlWriter(Stream, bool, Encoding)"/>
    public static XmlReader XmlReader(this Stream stream, bool close = false)
    {
      Assertion.NotNull(stream);

      return System.Xml.XmlReader.Create(stream, new XmlReaderSettings { CloseInput = close, IgnoreComments = true, IgnoreWhitespace = true });
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="close"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlWriter"/>
    /// <seealso cref="XmlReader(Stream, bool)"/>
    public static XmlWriter XmlWriter(this Stream stream, bool close = false, Encoding encoding = null)
    {
      Assertion.NotNull(stream);

      var settings = new XmlWriterSettings { CloseOutput = close };
      if (encoding != null)
      {
        settings.Encoding = encoding;
      }
      return System.Xml.XmlWriter.Create(stream, settings);
    }
  }
}