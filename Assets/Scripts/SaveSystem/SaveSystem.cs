using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
	public static void SaveGame(GameData gameData)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/save1.castlefile";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, gameData);
		stream.Close();
	}

	public static GameData LoadGame()
	{
		string path = Application.persistentDataPath + "/save1.castlefile";
		Debug.Log(path);
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			GameData gameData = formatter.Deserialize(stream) as GameData;
			stream.Close();

			return gameData;
		}
		else
		{
			Debug.LogError($"Load error: no file in path {path}");
			return null;
		}
		
	}
}
