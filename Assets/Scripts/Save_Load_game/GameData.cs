using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameData {
	public static GamePersistenData Data;
	static IPersistenceManager DataManager;
	static bool initialized;

	public static string currentDataStructure {
		get{
			return UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name + "_";
		}
	}

	public static void Init () //llamar al metodo init para empezar
	{
		if (!initialized) {
			initialized = true;
			//*Temporal*//
			//Cambiar por guardado remoto
			DataManager = new LocalBinarySave ();
			//
			if (Data == null)
				LoadOrCreate ();
		}
	}

	static void LoadOrCreate ()
	{
		Data = DataManager.Load<GamePersistenData> ();
		//Debug.Log("loading data..");
		if (Data == null) {
			Data = new GamePersistenData ();
			Save ();
		}			
	}

	public static void ResetData ()
	{
		Data = new GamePersistenData ();
		Save ();
	}

	public static void Save ()
	{
		//Debug.Log("guardando...");
		Debug.Log("gurdando papasito...");
		DataManager.Save (Data);
	}

	public static void AddValue (string key, string value)
	{
		if (!Data.StringData.ContainsKey (key)) {
			Data.StringData.Add (key, value);
		} 
		else {
			Data.StringData [key] += value;
		}
		Save ();
	}
}

[System.Serializable]
public class GamePersistenData
{
	public GamePersistenData ()
	{
		PlayerData = new GameState ();
		StringData = new Dictionary<string, string>();
	}
	public GameState PlayerData;
	public Dictionary<string,string> StringData;
}

public interface IPersistenceManager
{
	void Save(object data);
	T Load<T>();
}

public class LocalBinarySave : IPersistenceManager
{
	const string fileName = "/GameData";
	public void Save (object data)
	{
		BinaryFormatter bf = new BinaryFormatter();
		Stream stream = new FileStream(Application.persistentDataPath+fileName, FileMode.Create);
		bf.Serialize(stream,data);
		stream.Close();
	}

	public T Load<T> ()
	{
		try{
			if (File.Exists (Application.persistentDataPath + fileName)) {
				BinaryFormatter bf = new BinaryFormatter();
				Stream stream = new FileStream(Application.persistentDataPath+fileName, FileMode.Open);
				T t = (T)bf.Deserialize(stream);
				stream.Close();
				return t;	
			}	
		}
		catch(System.Exception e) {
			Debug.Log (e.Message);
		}
		return default(T);
	}	
}
