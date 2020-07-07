using UnityEngine;

/// <summary>
/// My way of finding singleton instances of important classes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour
{
	public static T GetInstance() {
		return GameObject.Find(typeof(T).Name).GetComponent<T>();
	}
}
