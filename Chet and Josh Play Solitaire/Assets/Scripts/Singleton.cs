using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implmentation of the singleton pattern using <see href="https://blog.mzikmund.com/2019/01/a-modern-singleton-in-unity/">this implmentation</see>.
/// </summary>
/// <typeparam name="T">Any MonoBehaviour to be made to follow the singleton pattern.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => LazyInstance.Value;

    private static readonly System.Lazy<T> LazyInstance = new System.Lazy<T>(CreateSingleton);

    /// <summary>
    /// Creates the singleton object
    /// </summary>
    /// <returns>The MonoBehaviour that is a singleton.</returns>
    private static T CreateSingleton ()
    {
        var ownerObject = new GameObject($"{typeof(T).Name} [singleton]");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }
}
