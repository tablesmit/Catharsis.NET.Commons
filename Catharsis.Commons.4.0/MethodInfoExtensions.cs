﻿using System;
using System.Reflection;

namespace Catharsis.Commons
{
  /// <summary>
  ///   <para>Tests set for class <see cref="MethodInfo"/>.</para>
  /// </summary>
  public static class MethodInfoExtensions
  {
    /// <summary>
    ///   <para>Creates a delegate of the specified type to represent a specified static method.</para>
    /// </summary>
    /// <param name="method">The <see cref="MethodInfo"/> describing the static or instance method the delegate is to represent.</param>
    /// <param name="type">The <see cref="Type"/> of delegate to create.</param>
    /// <returns>A delegate of the specified type to represent the specified static method.</returns>
    /// <exception cref="ArgumentNullException">If either <paramref name="method"/> or <paramref name="type"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="System.Delegate.CreateDelegate(Type, MethodInfo)"/>
    /// <seealso cref="Delegate{T}(MethodInfo)"/>
    public static Delegate Delegate(this MethodInfo method, Type type)
    {
      Assertion.NotNull(method);
      Assertion.NotNull(type);

      return System.Delegate.CreateDelegate(type, method);
    }

    /// <summary>
    ///   <para>Creates a delegate of the specified type to represent a specified static method.</para>
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of delegate to create.</typeparam>
    /// <param name="method">The <see cref="MethodInfo"/> describing the static or instance method the delegate is to represent.</param>
    /// <returns>A delegate of the specified type to represent the specified static method.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="method"/> is a <c>null</c> reference.</exception>
    /// <seealso cref="System.Delegate.CreateDelegate(Type, MethodInfo)"/>
    /// <seealso cref="Delegate(MethodInfo, Type)"/>
    public static Delegate Delegate<T>(this MethodInfo method)
    {
      Assertion.NotNull(method);

      return method.Delegate(typeof(T));
    }
  }
}