using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace Catharsis.Commons
{
  /// <summary>
  ///   <para>Tests set for class <see cref="ObjectExtensions"/>.</para>
  /// </summary>
  public sealed class ObjectExtensionsTests
  {
    private sealed class DumpTestObject
    {
      public object Property { get; set; }
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.As{T}(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void As_Method()
    {
      object subject = null;
      Assert.True(subject.As<object>() == null);

      Assert.True(subject.As<string>() == null);

      subject = new object();
      Assert.True(ReferenceEquals(subject.As<object>(), subject));

      var date = DateTime.UtcNow;
      Assert.True(date.As<string>() == default(string));

      Assert.True(date.As<DateTime>() == date);
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtendedExtensions.Binary(object)"/></description></item>
    ///     <item><description><see cref="ObjectExtendedExtensions.Binary(object, Stream, bool)"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void Binary_Methods()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtendedExtensions.Binary(null));
      Assert.Throws<ArgumentNullException>(() => ObjectExtendedExtensions.Binary(null, Stream.Null));
      Assert.Throws<ArgumentNullException>(() => new object().Binary(null));

      var subject = new object();
      var serialized = new object().Binary();
      Assert.True(serialized.Length > 0);
      Assert.False(new object().Binary().Binary().SequenceEqual(new object().Binary()));
      Assert.False(new object().Binary().SequenceEqual(string.Empty.Binary()));
      Assert.True(subject.Binary().SequenceEqual(subject.Binary()));
      Assert.True(Guid.Empty.ToString().Binary().SequenceEqual(Guid.Empty.ToString().Binary()));

      new MemoryStream().With(stream =>
      {
        Assert.True(ReferenceEquals(subject.Binary(stream), subject));
        Assert.True(stream.ToArray().SequenceEqual(serialized));
        Assert.True(stream.CanWrite);
      });
      new MemoryStream().With(stream =>
      {
        Assert.True(ReferenceEquals(subject.Binary(stream, true), subject));
        Assert.True(stream.ToArray().SequenceEqual(serialized));
        Assert.Throws<ObjectDisposedException>(() => stream.ReadByte());
      });
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.Dump(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void Dump_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Dump(null));

      Assert.True(new object().Dump() == "[]");

      var subject = new DumpTestObject();
      Assert.True(subject.Dump() == "[Property:\"\"]");
      subject.Property = Guid.Empty;
      Assert.True(subject.Dump() == "[Property:\"{0}\"]".FormatValue(Guid.Empty));
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtensions.Equality{T}(T, T, string[])"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.Equality{T}(T, T, Expression{Func{T, object}}[])"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void Equality_Methods()
    {
      Assert.True(ObjectExtensions.Equality<object>(null, null, (string[]) null));
      Assert.True(ObjectExtensions.Equality(null, null, (Expression<Func<object, object>>[]) null));
      Assert.False(ObjectExtensions.Equality(null, new object(), (string[])null));
      Assert.False(ObjectExtensions.Equality(null, new object(), (Expression<Func<object, object>>[])null));
      Assert.False(new object().Equality(null, (string[])null));
      Assert.False(new object().Equality(null, (Expression<Func<object, object>>[]) null));
      Assert.False(new object().Equality(string.Empty, (string[]) null));
      Assert.False(new object().Equality(string.Empty, (Expression<Func<object, object>>[])null));
      Assert.False(new object().Equality(new object(), (string[])null));
      Assert.False(new object().Equality(new object(), (Expression<Func<object, object>>[])null));

      var subject = new object();
      Assert.True(subject.Equality(subject, (string[])null));
      Assert.True(subject.Equality(subject, (Expression<Func<object, object>>[]) null));
      Assert.True(Guid.Empty.ToString().Equality(Guid.Empty.ToString(), (string[]) null));
      Assert.True(Guid.Empty.ToString().Equality(Guid.Empty.ToString(), (Expression<Func<object, object>>[]) null));
      Assert.True(Guid.NewGuid().ToString().Equality(Guid.NewGuid().ToString(), new[] { "Length" }));
      Assert.True(Guid.NewGuid().ToString().Equality(Guid.NewGuid().ToString(), x => x.Length));
      Assert.False("first".Equality("second", "Length"));
      Assert.False("first".Equality("second", x => x.Length));
      Assert.True("text".Equality("text", "property"));
      Assert.False("first".Equality("second", "property"));
      
      var testSubject = new TestObject();
      Assert.True(testSubject.Equality(testSubject, (string[]) null));
      Assert.True(testSubject.Equality(testSubject, (Expression<Func<TestObject, object>>[]) null));
      Assert.True(new TestObject().Equality(new TestObject(), (string[])null));
      Assert.True(new TestObject().Equality(new TestObject(), (Expression<Func<TestObject, object>>[])null));
      Assert.True(new TestObject { PublicProperty = "property" }.Equality(new TestObject { PublicProperty = "property" }, (string[]) null));
      Assert.True(new TestObject { PublicProperty = "property" }.Equality(new TestObject { PublicProperty = "property" }, (Expression<Func<TestObject, object>>[]) null));
      Assert.False(new TestObject { PublicProperty = "property" }.Equality(new TestObject(), (string[])null));
      Assert.False(new TestObject { PublicProperty = "property" }.Equality(new TestObject(), (Expression<Func<TestObject, object>>[])null));
      Assert.False(new TestObject { PublicProperty = "first" }.Equality(new TestObject { PublicProperty = "second" }, (string[])null));
      Assert.False(new TestObject { PublicProperty = "first" }.Equality(new TestObject { PublicProperty = "second" }, (Expression<Func<TestObject, object>>[])null));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.False(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void False_Method()
    {
      Assert.False(ObjectExtensions.True(null));
      Assert.False(false.True());
      Assert.False(byte.MinValue.True());
      Assert.False(char.MinValue.True());
      Assert.False(decimal.Zero.True());
      Assert.False(((double)0).True());
      Assert.False(((short)0).True());
      Assert.False(((int)0).True());
      Assert.False(((long)0).True());
      Assert.False(((sbyte)0).True());
      Assert.False(((Single)0).True());
      Assert.False(string.Empty.True());
      Assert.False(((ushort)0).True());
      Assert.False(((uint)0).True());
      Assert.False(((ulong)0).True());
      Assert.False(Enumerable.Empty<object>().True());
      Assert.False(new object[] { }.True());
      Assert.False(Regex.Match("first", "second").True());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.Field(object, string)"/> method.</para>
    /// </summary>
    [Fact]
    public void Field_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Field(null, "field"));
      Assert.Throws<ArgumentNullException>(() => new object().Field(null));
      Assert.Throws<ArgumentException>(() => new object().Field(string.Empty));

      Assert.True(new object().Field("field") == null);

      var subject = new TestObject { PublicField = "value" };
      Assert.True(subject.Field("PublicField").To<string>() == "value");
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.Finalize{T}(T, bool)"/> method.</para>
    /// </summary>
    [Fact]
    public void Finalize_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Finalize<object>(null));

      var subject = new object();
      Assert.True(ReferenceEquals(subject.Finalize(), subject));
      Assert.True(ReferenceEquals(subject.Finalize(false), subject));
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtensions.GetHashCode{T}(T, IEnumerable{string})"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.GetHashCode{T}(T, Expression{Func{T, object}}[])"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void GetHashCode_Methods()
    {
      Assert.True(ObjectExtensions.GetHashCode<object>(null, (string[]) null) == 0);
      Assert.True(ObjectExtensions.GetHashCode<object>(null, Enumerable.Empty<string>().ToArray()) == 0);
      Assert.True(ObjectExtensions.GetHashCode(null, (Expression<Func<object, object>>[]) null) == 0);
      Assert.True(ObjectExtensions.GetHashCode(null, Enumerable.Empty<Expression<Func<object, object>>>().ToArray()) == 0);

      Assert.True(new object().GetHashCode((string[])null) != 0);
      Assert.True(new object().GetHashCode((Expression<Func<object, object>>[]) null) != 0);

      Assert.True(new object().GetHashCode((string[])null) != new object().GetHashCode((string[])null));
      Assert.True(new object().GetHashCode((Expression<Func<object, object>>[]) null) != new object().GetHashCode((Expression<Func<object, object>>[]) null));

      var subject = new object();
      Assert.True(subject.GetHashCode((string[])null) == subject.GetHashCode((string[])null));
      Assert.True(subject.GetHashCode((Expression<Func<object, object>>[]) null) == subject.GetHashCode((Expression<Func<object, object>>[]) null));
      Assert.True(subject.GetHashCode((string[])null) == subject.GetHashCode());
      Assert.True(subject.GetHashCode((Expression<Func<object, object>>[])null) == subject.GetHashCode());
      Assert.True(string.Empty.GetHashCode((string[]) null) != new object().GetHashCode((string[]) null));
      Assert.True(string.Empty.GetHashCode((string[]) null) == string.Empty.GetHashCode((string[]) null));

      Assert.True(Guid.NewGuid().ToString().GetHashCode(new[] { "Length" }) == Guid.NewGuid().ToString().GetHashCode(new[] { "Length" }));
      Assert.True(Guid.NewGuid().ToString().GetHashCode(new[] { "Length" }) == Guid.NewGuid().ToString().GetHashCode(x => x.Length));
      Assert.True(Guid.NewGuid().ToString().GetHashCode(x => x.Length) == Guid.NewGuid().ToString().GetHashCode(x => x.Length));

      var testObject = new TestObject();
      Assert.True(testObject.GetHashCode((string[])null) == testObject.GetHashCode((string[])null));
      Assert.True(testObject.GetHashCode(new[] { "PublicProperty" }) == testObject.GetHashCode(new[] { "PublicProperty" }));
      Assert.True(testObject.GetHashCode(x => x.PublicProperty) == testObject.GetHashCode(new[] { "PublicProperty" }));
      Assert.True(testObject.GetHashCode(x => x.PublicProperty) == testObject.GetHashCode(x => x.PublicProperty));
      Assert.True(testObject.GetHashCode(new[] { "PublicProperty" }) == testObject.GetHashCode(new[] { "ProtectedProperty" }));
      Assert.True(testObject.GetHashCode(x => x.PublicProperty) == testObject.GetHashCode(new[] { "ProtectedProperty" }));
      Assert.True(testObject.GetHashCode(new[] { "invalid" }) == testObject.GetHashCode(new[] { "invalid" }));
      Assert.True(testObject.GetHashCode(new[] { "invalid_1" }) == testObject.GetHashCode(new[] { "invalid_2" }));
      testObject.PublicProperty = "property";
      Assert.True(new TestObject { PublicProperty = "property" }.GetHashCode((string[])null) == new TestObject { PublicProperty = "property" }.GetHashCode((string[]) null));
      Assert.True(new TestObject { PublicProperty = "first" }.GetHashCode((string[])null) != new TestObject { PublicProperty = "second" }.GetHashCode((string[])null));
      Assert.True(testObject.GetHashCode(new[] { "PublicProperty" }) == testObject.GetHashCode(new[] { "PublicProperty" }));
      Assert.True(testObject.GetHashCode(x => x.PublicProperty) == testObject.GetHashCode(new[] { "PublicProperty" }));
      Assert.True(testObject.GetHashCode(x => x.PublicProperty) == testObject.GetHashCode(x => x.PublicProperty));
      Assert.True(testObject.GetHashCode(new[] { "PublicProperty" }) != testObject.GetHashCode(new[] { "ProtectedProperty" }));
      Assert.True(testObject.GetHashCode(x => x.PublicProperty) != testObject.GetHashCode(new[] { "ProtectedProperty" }));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.HasField(object, string)"/> method.</para>
    /// </summary>
    [Fact]
    public void HasField_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.HasField(null, "name"));
      Assert.Throws<ArgumentNullException>(() => new object().HasField(null));
      Assert.Throws<ArgumentException>(() => new object().HasField(string.Empty));

      Assert.False(new object().HasField("field"));
      
      var subject = new TestObject();
      Assert.True(subject.HasField("PublicStaticField"));
      Assert.True(subject.HasField("ProtectedStaticField"));
      Assert.True(subject.HasField("PrivateStaticField"));
      Assert.True(subject.HasField("PublicField"));
      Assert.True(subject.HasField("ProtectedField"));
      Assert.True(subject.HasField("PrivateField"));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.HasMethod(object, string)"/> method.</para>
    /// </summary>
    [Fact]
    public void HasMethod_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.HasMethod(null, "name"));
      Assert.Throws<ArgumentNullException>(() => new object().HasMethod(null));
      Assert.Throws<ArgumentException>(() => new object().HasMethod(string.Empty));

      Assert.False(new object().HasMethod("method"));

      var subject = new TestObject();
      Assert.True(subject.HasMethod("PublicStaticMethod"));
      Assert.True(subject.HasMethod("ProtectedStaticMethod"));
      Assert.True(subject.HasMethod("PrivateStaticMethod"));
      Assert.True(subject.HasMethod("PublicMethod"));
      Assert.True(subject.HasMethod("ProtectedMethod"));
      Assert.True(subject.HasMethod("PrivateMethod"));
    }
    
    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.HasProperty(object, string)"/> method.</para>
    /// </summary>
    [Fact]
    public void HasProperty_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.HasProperty(null, "name"));
      Assert.Throws<ArgumentNullException>(() => new object().HasProperty(null));
      Assert.Throws<ArgumentException>(() => new object().HasProperty(string.Empty));

      Assert.False(new object().HasProperty("property"));

      var subject = new TestObject();
      Assert.True(subject.HasProperty("PublicStaticProperty"));
      Assert.True(subject.HasProperty("ProtectedStaticProperty"));
      Assert.True(subject.HasProperty("PrivateStaticProperty"));
      Assert.True(subject.HasProperty("PublicProperty"));
      Assert.True(subject.HasProperty("ProtectedProperty"));
      Assert.True(subject.HasProperty("PrivateProperty"));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.InvokeMethod(object, string, object[])"/> method.</para>
    /// </summary>
    [Fact]
    public void InvokeMethod_Methods()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.InvokeMethod(null, string.Empty));
      Assert.Throws<ArgumentNullException>(() => new object().InvokeMethod(null));
      Assert.Throws<ArgumentException>(() => new object().InvokeMethod(string.Empty));
      Assert.Throws<TargetParameterCountException>(() => new object().InvokeMethod("ToString", new object()));
      Assert.Throws<AmbiguousMatchException>(() => string.Empty.InvokeMethod("ToString").To<string>() == string.Empty);

      Assert.True(new object().InvokeMethod("method") == null);
      Assert.True((bool) string.Empty.InvokeMethod("Contains", string.Empty));
      //Assert.False(new object().InvokeMethod("ReferenceEquals", new object()).To<bool>());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.Is{T}(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void Is_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Is<object>(null));

      Assert.True(new object().Is<object>());
      Assert.False(new object().Is<string>());
      Assert.True(string.Empty.Is<IEnumerable<char>>());
    }
    
    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.IsNumeric(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void IsNumeric_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.IsNumeric(null));

      Assert.True(byte.MinValue.IsNumeric());
      Assert.True(decimal.MinValue.IsNumeric());
      Assert.True(double.MinValue.IsNumeric());
      Assert.True(short.MinValue.IsNumeric());
      Assert.True(int.MinValue.IsNumeric());
      Assert.True(sbyte.MinValue.IsNumeric());
      Assert.True(Single.MinValue.IsNumeric());
      Assert.True(ushort.MinValue.IsNumeric());
      Assert.True(uint.MinValue.IsNumeric());
      Assert.True(ulong.MinValue.IsNumeric());
      Assert.False(new object().IsNumeric());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.Member{T,R}(T, Expression{Func{T,R}})"/> method.</para>
    /// </summary>
    [Fact]
    public void Member_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Member(null, Enumerable.Empty<Expression<Func<object, object>>>().FirstOrDefault()));
      Assert.Throws<ArgumentNullException>(() => new object().Member<object, object>(null));

      var text = Guid.NewGuid().ToString();
      Assert.True(text.Member(x => x.Length) == text.Length);
      Assert.True(text.Member(x => x.ToString()) == text);
      Assert.True(DateTime.UtcNow.Member(x => x.Ticks <= DateTime.UtcNow.Ticks));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.Property(object, string)"/> method.</para>
    /// </summary>
    [Fact]
    public void Property_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Property(null, "property"));
      Assert.Throws<ArgumentNullException>(() => new object().Property(null));
      Assert.Throws<ArgumentException>(() => new object().Property(string.Empty));

      Assert.True(new object().Property("property") == null);

      var subject = new TestObject { PublicProperty = "value" };
      Assert.True(subject.Property("PublicProperty").To<string>() == "value");
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.SetProperty{T}(T, string, object)"/> method.</para>
    /// </summary>
    [Fact]
    public void SetProperty_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.SetProperty<object>(null, "property", new object()));
      Assert.Throws<ArgumentNullException>(() => new object().SetProperty(null, new object()));
      Assert.Throws<ArgumentException>(() => new object().SetProperty(string.Empty, new object()));
      
      var subject = new TestObject();
      var property = Guid.NewGuid().ToString();
      Assert.True(ReferenceEquals(subject.SetProperty("PublicProperty", null), subject));
      Assert.Throws<ArgumentException>(() => subject.SetProperty("ReadOnlyProperty", property));
      
      subject.SetProperty("PublicStaticProperty", property);
      Assert.True(subject.Property("PublicStaticProperty").Equals(property));
      
      subject.SetProperty("ProtectedStaticProperty", property);
      Assert.True(subject.Property("ProtectedStaticProperty").Equals(property));

      subject.SetProperty("PrivateStaticProperty", property);
      Assert.True(subject.Property("PrivateStaticProperty").Equals(property));

      subject.SetProperty("PublicProperty", property);
      Assert.True(subject.Property("PublicProperty").Equals(property));

      subject.SetProperty("ProtectedProperty", property);
      Assert.True(subject.Property("ProtectedProperty").Equals(property));

      subject.SetProperty("PrivateProperty", property);
      Assert.True(subject.Property("PrivateProperty").Equals(property));
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtensions.SetProperties{T}(T, IDictionary{string, object})"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.SetProperties{T}(T, object)"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void SetProperties_Methods()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.SetProperties<object>(null, new Dictionary<string, object>()));
      Assert.Throws<ArgumentNullException>(() => new object().SetProperties((IDictionary<string, object>) null));
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.SetProperties<object>(null, (object) null));
      Assert.Throws<ArgumentNullException>(() => new object().SetProperties((object) null));

      var subject = new TestObject();
      var property = Guid.NewGuid().ToString();
      
      Assert.Throws<ArgumentException>(() => subject.SetProperties(new Dictionary<string, object>().AddNext("ReadOnlyProperty", property)));
      Assert.True(ReferenceEquals(subject.SetProperties(new Dictionary<string, object>().AddNext("PublicProperty", property).AddNext("property", new object())), subject));
      Assert.True(subject.Property("PublicProperty").Equals(property));

      Assert.Throws<ArgumentException>(() => subject.SetProperties(new { ReadOnlyProperty = property }));
      Assert.True(ReferenceEquals(subject.SetProperties(new { PublicProperty = property, property = new object() }), subject));
      Assert.True(subject.Property("PublicProperty").Equals(property));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.To{T}(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void To_Method()
    {
      object subject = null;
      Assert.True(subject.To<object>() == null);
      Assert.True(subject.To<string>() == null);
      
      subject = new object();
      Assert.True(ReferenceEquals(subject.To<object>(), subject));
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtensions.ToString(object, IEnumerable{string})"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.ToString{T}(T, Expression{Func{T, object}}[])"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void ToString_Method()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.ToString(null, Enumerable.Empty<string>().ToArray()));
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.ToString(null, Enumerable.Empty<Expression<Func<object, object>>>().ToArray()));
      
      Assert.True(new object().ToString(new [] {"property"}) == "[]", new object().ToString(new [] {"property"}));
      Assert.True(new object().ToString((string[]) null) == "[]");
      Assert.True(new object().ToString(Enumerable.Empty<string>().ToArray()) == "[]");
      Assert.True(new object().ToString((Expression<Func<object, object>>[]) null) == "[]");
      Assert.True(new object().ToString(Enumerable.Empty<Expression<Func<object, object>>>().ToArray()) == "[]");

      var date = DateTime.UtcNow;
      Assert.True(date.ToString(new [] {"Ticks"}) == "[Ticks:\"{0}\"]".FormatValue(date.Ticks), date.ToString(new [] {"Ticks"}));
      Assert.True(date.ToString(x => x.Ticks) == "[Ticks:\"{0}\"]".FormatValue(date.Ticks), date.ToString(x => x.Ticks));
      Assert.True(date.ToString(new [] {"Day", "Month", "Year"}) == "[Day:\"{0}\", Month:\"{1}\", Year:\"{2}\"]".FormatValue(date.Day, date.Month, date.Year));
      Assert.True(date.ToString(x => x.Day, x => x.Month, x => x.Year) == "[Day:\"{0}\", Month:\"{1}\", Year:\"{2}\"]".FormatValue(date.Day, date.Month, date.Year));
      Assert.True(date.ToString(new [] {"Today"}) == "[Today:\"{0}\"]".FormatValue(DateTime.Today));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="ObjectExtensions.True(object)"/> method.</para>
    /// </summary>
    [Fact]
    public void True_Method()
    {
      Assert.True(true.True());
      Assert.True(((byte) 1).True());
      Assert.True(char.MaxValue.True());
      Assert.True(decimal.One.True());
      Assert.True(double.Epsilon.True());
      Assert.True(((short) 1).True());
      Assert.True(((int) 1).True());
      Assert.True(((long) 1).True());
      Assert.True(((sbyte) 1).True());
      Assert.True(Single.Epsilon.True());
      Assert.True("string".True());
      Assert.True(((ushort) 1).True());
      Assert.True(((uint) 1).True());
      Assert.True(((ulong) 1).True());
      Assert.True(new [] { new object() }.True());
      Assert.True(Regex.Match("value", "value").True());
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtensions.With{T}(T, Action{T})"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.With{T, RESULT}(T, Func{T, RESULT})"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void With_Methods()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.With<object>(null, subject => {}));
      Assert.Throws<ArgumentNullException>(() => new object().With(null));
      
      var text = Guid.NewGuid().ToString();
      Assert.Throws<ObjectDisposedException>(() => new StringReader(text).With(disposable =>
      {
        disposable.With(x => Assert.True(x.ReadToEnd() == text));
      }).Read());

      var list = new List<string>().With(x => x.Add(text));
      Assert.True(list.Count == 1);
      Assert.True(list[0] == text);

      Assert.True(new object().With(x => text) == text);
    }

    /// <summary>
    ///   <para>Performs testing of following methods :</para>
    ///   <list type="bullet">
    ///     <item><description><see cref="ObjectExtensions.Xml{T}(T, Type[])"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.Xml{T}(T, Stream, Encoding, Type[])"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.Xml{T}(T, TextWriter, Type[])"/></description></item>
    ///     <item><description><see cref="ObjectExtensions.Xml{T}(T, XmlWriter, Type[])"/></description></item>
    ///   </list>
    /// </summary>
    [Fact]
    public void Xml_Methods()
    {
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Xml<object>(null));
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Xml<object>(null, Stream.Null));
      Assert.Throws<ArgumentNullException>(() => new object().Xml((Stream)null));
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Xml<object>(null, TextWriter.Null));
      Assert.Throws<ArgumentNullException>(() => new object().Xml((TextWriter)null));
      Assert.Throws<ArgumentNullException>(() => ObjectExtensions.Xml<object>(null, XmlWriter.Create(Stream.Null)));
      Assert.Throws<ArgumentNullException>(() => new object().Xml((XmlWriter)null));

      var subject = Guid.NewGuid().ToString();
      
      var xml = subject.Xml();
      var stringWriter = new StringWriter();
      stringWriter.XmlWriter().Write(writer =>
      {
        new XmlSerializer(subject.GetType()).Serialize(writer, subject);
        Assert.True(stringWriter.ToString() == xml);
      });
      Assert.True(subject.Xml((Type[]) null) == xml);
      Assert.True(subject.Xml(Enumerable.Empty<Type>().ToArray()) == xml);
      Assert.True(subject.Xml((Type[]) null) == xml);

      new MemoryStream().With(stream =>
      {
        Assert.True(ReferenceEquals(subject.Xml(stream, Encoding.Unicode), subject));
        Assert.True(stream.Rewind().Text() == xml);
        Assert.True(stream.CanWrite);
      });

      new StringWriter().With(writer =>
      {
        Assert.True(ReferenceEquals(subject.Xml(writer), subject));
        Assert.True(writer.ToString() == xml);
        writer.WriteLine();
      });

      stringWriter = new StringWriter();
      stringWriter.XmlWriter().Write(writer =>
      {
        Assert.True(ReferenceEquals(subject.Xml(writer), subject));
        Assert.True(stringWriter.ToString() == xml);
        stringWriter.WriteLine();
      });
    }
  }
}