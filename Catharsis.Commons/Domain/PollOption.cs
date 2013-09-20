﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Catharsis.Commons.Extensions;

namespace Catharsis.Commons.Domain
{
  /// <summary>
  ///   <para></para>
  /// </summary>
  [EqualsAndHashCode("Text")]
  public class PollOption : EntityBase, IEquatable<PollOption>, ITextable
  {
    private string text;

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is <see cref="string.Empty"/> string.</exception>
    public string Text
    {
      get { return this.text; }
      set
      {
        Assertion.NotEmpty(value);

        this.text = value;
      }
    }

    /// <summary>
    ///   <para>Creates new poll option.</para>
    /// </summary>
    public PollOption()
    {
    }

    /// <summary>
    ///   <para>Creates new poll option with specified properties values.</para>
    /// </summary>
    /// <param name="properties">Named collection of properties to set on poll option after its creation.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="properties"/> is a <c>null</c> reference.</exception>
    public PollOption(IDictionary<string, object> properties) : base(properties)
    {
    }

    /// <summary>
    ///   <para>Creates new poll option.</para>
    /// </summary>
    /// <param name="id">Unique identifier of poll option.</param>
    /// <param name="text">Option's content text.</param>
    /// <exception cref="ArgumentNullException">If either <paramref name="id"/> or <paramref name="text"/> is a <c>null</c> reference.</exception>
    /// <exception cref="ArgumentException">If either <paramref name="id"/> or <paramref name="text"/> is <see cref="string.Empty"/> string.</exception>
    public PollOption(string id, string text) : base(id)
    {
      this.Text = text;
    }

    /// <summary>
    ///   <para>Creates new poll option from its XML representation.</para>
    /// </summary>
    /// <param name="xml"><see cref="XElement"/> object, representing instance of <see cref="PollOption"/> type.</param>
    /// <returns>Recreated poll option object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="xml"/> is a <c>null</c> reference.</exception>
    public static PollOption Xml(XElement xml)
    {
      Assertion.NotNull(xml);

      return new PollOption((string) xml.Element("Id"), (string) xml.Element("Text"));
    }

    /// <summary>
    ///   <para>Transforms current object to XML representation.</para>
    /// </summary>
    /// <returns><see cref="XElement"/> object, representing current <see cref="PollOption"/>.</returns>
    public override XElement Xml()
    {
      return base.Xml().AddContent(
        new XElement("Text", this.Text));
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PollOption other)
    {
      return base.Equals(other);
    }
  }
}