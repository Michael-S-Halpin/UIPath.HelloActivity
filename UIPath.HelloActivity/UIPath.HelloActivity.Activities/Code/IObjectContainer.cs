using System;
using System.Collections.Generic;

namespace UiPath.HelloActivity.Activities.Code;

/* NOTE:
 * If you setup you solution to compile multiple scoped activity projects you may encounter
 * 'The type 'IObjectContain' exists in both...this...project and...that..project' error.
 * To resolve this error you can move IObjectContainer.cs and ObjectContainer.cs our of this
 * shared project and into each of your scoped activity projects and rename them accordingly.
 *
 * If someone can find out how to resolve this error and keep these classes in the shared
 * library that would be even better.
 */
/// <summary>
/// A simple container for objects meant to be shared across the application space.
/// </summary>
public interface IObjectContainer
{
    /// <summary>
    /// Stores a reference to an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectToAdd"></param>
    void Add<T>(T objectToAdd) where T : class;
    /// <summary>
    /// Gets the object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Get<T>() where T : class;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    IEnumerable<object> Where(Func<object, bool> filter);
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    void Remove<T>() where T : class;
    /// <summary>
    /// Determines whether this instance contains the object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>
    ///   <c>true</c> if this instance contains object; otherwise, <c>false</c>.
    /// </returns>
    bool Contains<T>() where T : class;
    /// <summary>
    /// Clears this instance.
    /// </summary>
    void Clear();
}