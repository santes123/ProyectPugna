using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class StateByPersistence : MonoBehaviour {
	public UnityEvent OnEqualValue;
	public UnityEvent OnDifferentValue;
	public CheckType CheckType;
	public bool AutoCheck = true;
	public string key;
	public string value;
	string finalKey;

	void Start ()
	{
		if (AutoCheck) {
			Check ();
		}
	}

	public bool Check ()
	{
		switch (CheckType) {
		case CheckType.Key:
			finalKey = key;
			break;
		case CheckType.SceneNameAndKey:
			finalKey = GameData.currentDataStructure + key;
			break;
		case CheckType.SceneNameAndName:
			finalKey = GameData.currentDataStructure + name;
			break;
		}
		string outPutValue;
		if (GameData.Data.StringData.TryGetValue (finalKey, out outPutValue)) {
			if (outPutValue.Equals (value)) {
				OnEqualValue.Invoke ();
				return true;
			} 
			else {
				OnDifferentValue.Invoke ();
				return false;
			}
		}
		OnDifferentValue.Invoke ();
		return false;			
	}
}
public enum CheckType {SceneNameAndKey,Key,SceneNameAndName};
