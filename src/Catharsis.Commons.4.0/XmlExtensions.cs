﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Catharsis.Commons
{
  /// <summary>
  ///   <para>Set of extensions methods for class <see cref="object"/>.</para>
  /// </summary>
  /// <seealso cref="object"/>
  public static class XmlExtensions
  {
    /// <summary>
    ///   <para>Deserializes XML contents of stream into object of specified type.</para>
    /// </summary>
    /// <typeparam name="T">Type of object which is to be the result of deserialization process.</typeparam>
    /// <param name="self">Stream of XML data for deserialization.</param>
    /// <param name="close">Whether to automatically close <paramref name="self"/> after deserialization process or leave it intact.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for deserialization purposes.</param>
    /// <returns>Deserialized XML contents of source <paramref name="self"/> as the object (or objects graph with a root element) of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlSerializer"/>
    public static T AsXml<T>(this Stream self, bool close = false, params Type[] types)
    {
      Assertion.NotNull(self);

      var serializer = types != null ? new XmlSerializer(typeof(T), types) : new XmlSerializer(typeof(T));
      try
      {
        return serializer.Deserialize(self).To<T>();
      }
      finally
      {
        if (close)
        {
          self.Dispose();
        }
      }
    }

    /// <summary>
    ///   <para>Deserializes XML string text into object of specified type.</para>
    /// </summary>
    /// <typeparam name="T">Type of object which is to be the result of deserialization process.</typeparam>
    /// <param name="self">XML data for deserialization.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for deserialization purposes.</param>
    /// <returns>Deserialized XML contents of <paramref name="self"/> string as the object (or objects graph with a root element) of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="self"/> is <see cref="string.Empty"/> string.</exception>
    /// <seealso cref="XmlSerializer"/>
    public static T AsXml<T>(this string self, params Type[] types)
    {
      Assertion.NotEmpty(self);

      return new StringReader(self).AsXml<T>(true, types);
    }

    /// <summary>
    ///   <para>Deserializes XML contents from <see cref="TextReader"/> into object of specified type.</para>
    /// </summary>
    /// <typeparam name="T">Type of object which is to be the result of deserialization process.</typeparam>
    /// <param name="self"><see cref="TextReader"/> which is used as a source for XML data.</param>
    /// <param name="close">Whether to automatically close <paramref name="self"/> after deserialization process or leave it intact.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for deserialization purposes.</param>
    /// <returns>Deserialized XML contents of source <paramref name="self"/> as the object (or objects graph with a root element) of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    public static T AsXml<T>(this TextReader self, bool close = false, params Type[] types)
    {
      Assertion.NotNull(self);

      var serializer = types != null ? new XmlSerializer(typeof(T), types) : new XmlSerializer(typeof(T));
      try
      {
        return serializer.Deserialize(self).To<T>();
      }
      finally
      {
        if (close)
        {
          self.Dispose();
        }
      }
    }

    /// <summary>
    ///   <para>Deserializes XML data from <see cref="XmlReader"/> into object of specified type.</para>
    /// </summary>
    /// <param name="self"><see cref="XmlReader"/> used for retrieving XML data for deserialization.</param>
    /// <param name="type">Type of object which is to be the result of deserialization process.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for deserialization purposes.</param>
    /// <returns>Deserialized XML contents from a <paramref name="self"/> as the object (or objects graph with a root element) of <paramref name="type"/>.</returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="self"/> or <paramref name="type"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlSerializer"/>
    /// <seealso cref="Deserialize{T}(XmlReader, IEnumerable{Type})"/>
    public static object Deserialize(this XmlReader self, Type type, IEnumerable<Type> types = null)
    {
      Assertion.NotNull(self);
      Assertion.NotNull(type);

      return (types != null ? new XmlSerializer(type, types.ToArray()) : new XmlSerializer(type)).Deserialize(self);
    }

    /// <summary>
    ///   <para>Deserializes XML data from <see cref="XmlReader"/> into object of specified type.</para>
    /// </summary>
    /// <typeparam name="T">Type of object which is to be the result of deserialization process.</typeparam>
    /// <param name="self"><see cref="XmlReader"/> used for retrieving XML data for deserialization.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for deserialization purposes.</param>
    /// <returns>Deserialized XML contents from a <paramref name="self"/> as the object (or objects graph with a root element) of <paramref name="self"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlSerializer"/>
    /// <seealso cref="Deserialize(XmlReader, Type, IEnumerable{Type})"/>
    public static T Deserialize<T>(this XmlReader self, IEnumerable<Type> types = null)
    {
      Assertion.NotNull(self);

      return self.Deserialize(typeof(T), types).As<T>();
    }

    /// <summary>
    ///   <para>Translates specified <see cref="XElement"/> into a dictionary.</para>
    ///   <para>Attributes in XML document are translated to string keys and values, nodes become dictionaries with string keys themselves.</para>
    /// </summary>
    /// <param name="self"><see cref="XElement"/>, whose structure is to be converted to <see cref="IDictionary{string, object}"/> instance.</param>
    /// <returns>Dictionary that follows the structure of <see cref="XElement"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XDocumentXmlExtensions.Dictionary(XDocument)"/>
    public static IDictionary<string, object> Dictionary(this XElement self)
    {
      Assertion.NotNull(self);

      var result = new Dictionary<string, object>();
      DictionaryXml(self, result);
      return result;
    }

    /// <summary>
    ///   <para>Translates specified <see cref="XDocument"/> into a dictionary.</para>
    ///   <para>Attributes in XML document are translated to string keys and values, nodes become dictionaries with string keys themselves.</para>
    /// </summary>
    /// <param name="self"><see cref="XDocument"/>, whose structure is to be converted to <see cref="IDictionary{string, object}"/> instance.</param>
    /// <returns>Dictionary that follows the structure of <see cref="XDocument"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XElementExtensions.Dictionary(XElement)"/>
    public static IDictionary<string, object> Dictionary(this XDocument self)
    {
      Assertion.NotNull(self);

      return self.Root.Dictionary();
    }

    /// <summary>
    ///   <para>Translates XML content from specified <see cref="XmlReader"/> into a dictionary.</para>
    ///   <para>Attributes in XML document are translated to string keys and values, nodes become dictionaries with string keys themselves.</para>
    /// </summary>
    /// <param name="self"><see cref="XmlReader"/> used for reading of XML data which is to be converted to <see cref="IDictionary{string, object}"/> instance.</param>
    /// <returns>Dictionary that follows the structure of XML data from <see cref="XmlReader"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="self"/> is a <c>null</c> reference.</exception>
    public static IDictionary<string, object> Dictionary(this XmlReader self)
    {
      Assertion.NotNull(self);

      return XDocument.Load(self).Dictionary();
    }

    /// <summary>
    ///   <para>Serializes given object or graph into XML string.</para>
    /// </summary>
    /// <typeparam name="T">Type of object to be serialized.</typeparam>
    /// <param name="self">Object (or objects graph with a root element) to be serialized.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for serialization purposes.</param>
    /// <returns>Serialized XML contents of <paramref name="self"/> instance.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <seealso cref="XmlSerializer"/>
    /// <seealso cref="ToXml{T}(T, Stream, Encoding, Type[])"/>
    /// <seealso cref="ToXml{T}(T, TextWriter, Type[])"/>
    /// <seealso cref="ToXml{T}(T, XmlWriter, Type[])"/>
    public static string ToXml<T>(this T self, params Type[] types)
    {
      Assertion.NotNull(self);

      using (var writer = new StringWriter())
      {
        self.ToXml(writer, types);
        return writer.ToString();
      }
    }

    /// <summary>
    ///   <para>Serializes given object or graph and writes XML content into specified <see cref="Stream"/>.</para>
    /// </summary>
    /// <typeparam name="T">Type of object to be serialized.</typeparam>
    /// <param name="self">Object (or objects graph with a root element) to be serialized.</param>
    /// <param name="destination">Destination stream to which serialized XML data is to be written.</param>
    /// <param name="encoding">Encoding to be used for transformation between bytes and characters when writing to a <paramref name="destination"/> stream. If not specified, default encoding will be used.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for serialization purposes.</param>
    /// <returns>Back reference to the currently serialized object.</returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="destination"/> or <paramref name="types"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlSerializer"/>
    /// <seealso cref="ToXml{T}(T, Type[])"/>
    /// <seealso cref="ToXml{T}(T, TextWriter, Type[])"/>
    /// <seealso cref="ToXml{T}(T, XmlWriter, Type[])"/>
    public static T ToXml<T>(this T self, Stream destination, Encoding encoding = null, params Type[] types)
    {
      Assertion.NotNull(self);
      Assertion.NotNull(destination);

      destination.XmlWriter(encoding: encoding).Write(writer => self.ToXml(writer, types));
      return self;
    }

    /// <summary>
    ///   <para>Serializes given object or graph and writes XML content using specified <see cref="TextWriter"/>.</para>
    /// </summary>
    /// <typeparam name="T">Type of object to be serialized.</typeparam>
    /// <param name="self">Object (or objects graph with a root element) to be serialized.</param>
    /// <param name="writer"><see cref="TextWriter"/> to be used for writing XML content into its underlying destination.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for serialization purposes.</param>
    /// <returns>Back reference to the currently serialized object.</returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="self"/> or <paramref name="writer"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlSerializer"/>
    /// <seealso cref="ToXml{T}(T, Type[])"/>
    /// <seealso cref="ToXml{T}(T, Stream, Encoding, Type[])"/>
    /// <seealso cref="ToXml{T}(T, XmlWriter, Type[])"/>
    public static T ToXml<T>(this T self, TextWriter writer, params Type[] types)
    {
      Assertion.NotNull(self);
      Assertion.NotNull(writer);

      var serializer = types != null ? new XmlSerializer(typeof(T), types) : new XmlSerializer(typeof(T));
      writer.XmlWriter(encoding: Encoding.UTF8).Write(xmlWriter => serializer.Serialize(xmlWriter, self));
      return self;
    }

    /// <summary>
    ///   <para>Serializes given object or graph and writes XML content using specified <see cref="XmlWriter"/>.</para>
    /// </summary>
    /// <typeparam name="T">Type of object to be serialized.</typeparam>
    /// <param name="self">Object (or objects graph with a root element) to be serialized.</param>
    /// <param name="writer"><see cref="XmlWriter"/> to be used for writing XML content into its underlying destination.</param>
    /// <param name="types">Additional types to be used by <see cref="XmlSerializer"/> for serialization purposes.</param>
    /// <returns>Back reference to the currently serialized object.</returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="self"/> or <paramref name="writer"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="XmlSerializer"/>
    /// <seealso cref="ToXml{T}(T, Type[])"/>
    /// <seealso cref="ToXml{T}(T, Stream, Encoding, Type[])"/>
    /// <seealso cref="ToXml{T}(T, TextWriter, Type[])"/>
    public static T ToXml<T>(this T self, XmlWriter writer, params Type[] types)
    {
      Assertion.NotNull(self);
      Assertion.NotNull(writer);

      var serializer = types != null ? new XmlSerializer(typeof(T), types) : new XmlSerializer(typeof(T));
      serializer.Serialize(writer, self);
      return self;
    }

    /// <summary>
    ///   <para>Calls specified delegate action in a context of target <see cref="XmlWriter"/>, automatically closing it after the call.</para>
    /// </summary>
    /// <typeparam name="WRITER">Type of <see cref="XmlWriter"/> implementation.</typeparam>
    /// <param name="self"><see cref="XmlWriter"/> instance, in context of which <paramref name="action"/> method is to be called. It will be closed automatically no matter whether <paramref name="action"/> method call succeeds or fails.</param>
    /// <param name="action">Delegate that represents a method to be called.</param>
    /// <returns>Back reference to the current target <see cref="XmlWriter"/>.</returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="self"/> or <paramref name="action"/> is a <c>null</c> reference.</exception>
    public static WRITER Write<WRITER>(this WRITER self, Action<WRITER> action) where WRITER : XmlWriter
    {
      Assertion.NotNull(self);
      Assertion.NotNull(action);

      try
      {
        action(self);
      }
      finally
      {
        self.Flush();
      }

      return self;
    }

    private static void DictionaryXml(XElement xml, IDictionary<string, object> dictionary)
    {
      if (xml.FirstNode == null)
      {
        dictionary[xml.Name.LocalName] = null;
      }
      else if (xml.FirstNode.NodeType == XmlNodeType.CDATA)
      {
        dictionary[xml.Name.LocalName] = ((XCData) xml.FirstNode).Value;
      }
      else if (xml.FirstNode.NodeType == XmlNodeType.Text)
      {
        dictionary[xml.Name.LocalName] = ((XText) xml.FirstNode).Value;
      }
      else
      {
        var childDictionary = new Dictionary<string, object>();
        dictionary[xml.Name.LocalName] = childDictionary;
        foreach (var childAttribute in xml.Attributes())
        {
          childDictionary[childAttribute.Name.LocalName] = childAttribute.Value;
        }
        foreach (var element in xml.Elements())
        {
          DictionaryXml(element, childDictionary);
        }
      }
    }
  }
}