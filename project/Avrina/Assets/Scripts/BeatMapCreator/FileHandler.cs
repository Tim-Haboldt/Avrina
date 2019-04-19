using UnityEngine;
using System.IO;

public class FileHandler
{
    /**
     * Creates json string from given object and stores string as txt file with given file name
     */
    public static void SaveObjectAsJsonString<dataType>(string fileName, dataType data)
    {
        // create path
        string dataPath = Path.Combine(Application.persistentDataPath, fileName);
        // create json string from object
        string jsonString = JsonUtility.ToJson(data);
        // save data object
        using (StreamWriter streamWriter = File.CreateText(dataPath))
        {
            streamWriter.Write(jsonString);
        }
    }

    /**
     * Used to load data from an json string and exports it as an object
     * The given string stores the name of the file
     */
    public static dataType LoadObjectFromJsonString<dataType>(string fileName)
    {
        // create path from given fileName
        string dataPath = Path.Combine(Application.persistentDataPath, fileName);
        // load data
        using (StreamReader streamReader = File.OpenText(dataPath))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<dataType>(jsonString);
        }
    }
}
