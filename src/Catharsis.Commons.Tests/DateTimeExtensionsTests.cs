using System;
using System.Globalization;
using Xunit;

namespace Catharsis.Commons
{
  /// <summary>
  ///   <para>Tests set for class <see cref="DateTimeExtensions"/>.</para>
  /// </summary>
  public sealed class DateTimeExtensionsTests
  {
    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.EndOfDay(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void EndOfDay_Method()
    {
      var now = DateTime.UtcNow;
      var endOfDay = now.EndOfDay();

      Assert.True(endOfDay >= now);
      Assert.Equal(now.Kind, endOfDay.Kind);
      Assert.Equal(now.Year, endOfDay.Year);
      Assert.Equal(now.Month, endOfDay.Month);
      Assert.Equal(now.Day, endOfDay.Day);
      Assert.Equal(23, endOfDay.Hour);
      Assert.Equal(59, endOfDay.Minute);
      Assert.Equal(59, endOfDay.Second);
      Assert.Equal(0, endOfDay.Millisecond);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.EndOfMonth(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void EndOfMonth_Method()
    {
      var now = DateTime.UtcNow;
      var endOfMonth = now.EndOfMonth();

      Assert.True(endOfMonth >= now);
      Assert.Equal(now.Kind, endOfMonth.Kind);
      Assert.Equal(now.Year, endOfMonth.Year);
      Assert.Equal(now.Month, endOfMonth.Month);
      Assert.Equal(DateTime.DaysInMonth(now.Year, now.Month), endOfMonth.Day);
      Assert.Equal(23, endOfMonth.Hour);
      Assert.Equal(59, endOfMonth.Minute);
      Assert.Equal(59, endOfMonth.Second);
      Assert.Equal(0, endOfMonth.Millisecond);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.EndOfYear(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void EndOfYear_Method()
    {
      var now = DateTime.UtcNow;
      var endOfYear = now.EndOfYear();

      Assert.True(endOfYear >= now);
      Assert.Equal(now.Kind, endOfYear.Kind);
      Assert.Equal(now.Year, endOfYear.Year);
      Assert.Equal(12, endOfYear.Month);
      Assert.Equal(31, endOfYear.Day);
      Assert.Equal(23, endOfYear.Hour);
      Assert.Equal(59, endOfYear.Minute);
      Assert.Equal(59, endOfYear.Second);
      Assert.Equal(0, endOfYear.Millisecond);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.ISO8601(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void ISO8601_Method()
    {
      var time = DateTime.Today;
      Assert.True(DateTime.ParseExact(time.ISO8601(), "o", CultureInfo.InvariantCulture).Equals(time));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.IsSameDate(DateTime, DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void IsSameDate_Method()
    {
      var date = new DateTime(2000, 2, 1);

      Assert.True(date.IsSameDate(new DateTime(2000, 2, 1)));
      Assert.False(date.IsSameDate(new DateTime(2000, 2, 2)));
      Assert.False(date.IsSameDate(new DateTime(2000, 3, 1)));
      Assert.False(date.IsSameDate(new DateTime(2001, 2, 1)));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.IsSameTime(DateTime, DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void IsSameTime_Method()
    {
      var time = new DateTime(2000, 2, 1, 12, 2, 1);

      Assert.True(time.IsSameTime(new DateTime(2000, 1, 1, 12, 2, 1)));
      Assert.False(time.IsSameTime(new DateTime(2000, 1, 1, 12, 2, 2)));
      Assert.False(time.IsSameTime(new DateTime(2000, 1, 1, 12, 3, 1)));
      Assert.False(time.IsSameTime(new DateTime(2000, 1, 1, 13, 2, 1)));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.NextDay(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void NextDay_Method()
    {
      var now = DateTime.UtcNow;
      Assert.Equal(now.AddDays(1), now.NextDay());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.NextMonth(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void NextMonth_Method()
    {
      var now = DateTime.UtcNow;
      Assert.Equal(now.AddMonths(1), now.NextMonth());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.NextYear(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void NextYear_Method()
    {
      var now = DateTime.UtcNow;
      Assert.Equal(now.AddYears(1), now.NextYear());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.PreviousDay(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void PreviousDay_Method()
    {
      var now = DateTime.UtcNow;
      Assert.Equal(now.AddDays(-1), now.PreviousDay());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.PreviousMonth(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void PreviousMonth_Method()
    {
      var now = DateTime.UtcNow;
      Assert.Equal(now.AddMonths(-1), now.PreviousMonth());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.PreviousYear(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void PreviousYear_Method()
    {
      var now = DateTime.UtcNow;
      Assert.Equal(now.AddYears(-1), now.PreviousYear());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.RFC1121(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void RFC_Method()
    {
      var time = DateTime.Today;
      Assert.True(DateTime.ParseExact(time.RFC1121(), new DateTimeFormatInfo().RFC1123Pattern, CultureInfo.InvariantCulture).Equals(time));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.StartOfDay(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void StartOfDay_Method()
    {
      var now = DateTime.UtcNow;
      var startOfDay = now.StartOfDay();

      Assert.True(startOfDay <= now);
      Assert.Equal(now.Kind, startOfDay.Kind);
      Assert.Equal(now.Year, startOfDay.Year);
      Assert.Equal(now.Month, startOfDay.Month);
      Assert.Equal(now.Day, startOfDay.Day);
      Assert.Equal(0, startOfDay.Hour);
      Assert.Equal(0, startOfDay.Minute);
      Assert.Equal(0, startOfDay.Second);
      Assert.Equal(0, startOfDay.Millisecond);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.StartOfMonth(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void StartOfMonth_Method()
    {
      var now = DateTime.UtcNow;
      var startOfMonth = now.StartOfMonth();

      Assert.True(startOfMonth <= now);
      Assert.Equal(now.Kind, startOfMonth.Kind);
      Assert.Equal(now.Year, startOfMonth.Year);
      Assert.Equal(now.Month, startOfMonth.Month);
      Assert.Equal(1, startOfMonth.Day);
      Assert.Equal(0, startOfMonth.Hour);
      Assert.Equal(0, startOfMonth.Minute);
      Assert.Equal(0, startOfMonth.Second);
      Assert.Equal(0, startOfMonth.Millisecond);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.StartOfYear(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void StartOfYear_Method()
    {
      var now = DateTime.UtcNow;
      var startOfYear = now.StartOfYear();

      Assert.True(startOfYear <= now);
      Assert.Equal(now.Kind, startOfYear.Kind);
      Assert.Equal(now.Year, startOfYear.Year);
      Assert.Equal(1, startOfYear.Month);
      Assert.Equal(1, startOfYear.Day);
      Assert.Equal(0, startOfYear.Hour);
      Assert.Equal(0, startOfYear.Minute);
      Assert.Equal(0, startOfYear.Second);
      Assert.Equal(0, startOfYear.Millisecond);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Friday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Friday_Method()
    {
      Assert.True(new DateTime(2014, 1, 3).Friday());
      Assert.False(new DateTime(2014, 1, 4).Friday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Monday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Monday_Method()
    {
      Assert.True(new DateTime(2013, 12, 30).Monday());
      Assert.False(new DateTime(2013, 12, 31).Monday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Saturday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Saturday_Method()
    {
      Assert.True(new DateTime(2014, 1, 4).Saturday());
      Assert.False(new DateTime(2014, 1, 5).Saturday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Sunday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Sunday_Method()
    {
      Assert.True(new DateTime(2014, 1, 5).Sunday());
      Assert.False(new DateTime(2014, 1, 6).Sunday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Thursday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Thursday_Method()
    {
      Assert.True(new DateTime(2014, 1, 2).Thursday());
      Assert.False(new DateTime(2014, 1, 3).Thursday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Tuesday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Tuesday_Method()
    {
      Assert.True(new DateTime(2013, 12, 31).Tuesday());
      Assert.False(new DateTime(2014, 1, 1).Tuesday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.Wednesday(DateTime)"/> method.</para>
    /// </summary>
    [Fact]
    public void Wednesday_Method()
    {
      Assert.True(new DateTime(2014, 1, 1).Wednesday());
      Assert.False(new DateTime(2014, 1, 2).Wednesday());
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.UpTo(DateTime, DateTime, Action)"/> method.</para>
    /// </summary>
    [Fact]
    public void UpTo_Method()
    {
      var counter = 0;
      new DateTime(1999, 1, 1).UpTo(new DateTime(1998, 12, 31), () => counter++);
      Assert.Equal(0, counter);
      new DateTime(1999, 1, 1).UpTo(new DateTime(1999, 1, 1), () => counter++);
      Assert.Equal(0, counter);
      new DateTime(1999, 1, 1).UpTo(new DateTime(1999, 1, 3), () => counter++);
      Assert.Equal(2, counter);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.DownTo(DateTime, DateTime, Action)"/> method.</para>
    /// </summary>
    [Fact]
    public void DownTo_Method()
    {
      var counter = 0;
      new DateTime(1999, 1, 1).DownTo(new DateTime(1999, 1, 2), () => counter++);
      Assert.Equal(0, counter);
      new DateTime(1999, 1, 1).DownTo(new DateTime(1999, 1, 1), () => counter++);
      Assert.Equal(0, counter);
      new DateTime(1999, 1, 1).DownTo(new DateTime(1998, 12, 30), () => counter++);
      Assert.Equal(2, counter);
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.FromNow(TimeSpan)"/> method.</para>
    /// </summary>
    [Fact]
    public void FromNow_Method()
    {
      var timespan = new TimeSpan(1, 0, 0, 0);
      Assert.True(timespan.FromNow().IsSameDate(DateTime.Now.Add(timespan)));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.FromNowUtc(TimeSpan)"/> method.</para>
    /// </summary>
    [Fact]
    public void FromNowUtc_Method()
    {
      var timespan = new TimeSpan(1, 0, 0, 0);
      Assert.True(timespan.FromNowUtc().IsSameDate(DateTime.UtcNow.Add(timespan)));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.BeforeNow(TimeSpan)"/> method.</para>
    /// </summary>
    [Fact]
    public void BeforeNow_Method()
    {
      var timespan = new TimeSpan(1, 0, 0, 0);
      Assert.True(timespan.BeforeNow().IsSameDate(DateTime.Now.Subtract(timespan)));
    }

    /// <summary>
    ///   <para>Performs testing of <see cref="DateTimeExtensions.BeforeNowUtc(TimeSpan)"/> method.</para>
    /// </summary>
    [Fact]
    public void BeforeNowUtc_Method()
    {
      var timespan = new TimeSpan(1, 0, 0, 0);
      Assert.True(timespan.BeforeNowUtc().IsSameDate(DateTime.UtcNow.Subtract(timespan)));
    }
  }
}