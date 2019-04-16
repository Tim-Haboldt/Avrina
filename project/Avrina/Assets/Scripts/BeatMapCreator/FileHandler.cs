using UnityEngine;
using System.IO;

public class FileHandler
{
    /**
     * creates json string from given object and stores string as txt with given fileName
     */
    public static void saveObjectAsJsonString<dataType>(string fileName, dataType data)
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
     * used to load data from an json string and exports it as an object
     * the given string stores the name of the file
     */
    public static dataType loadObjectFromJsonString<dataType>(string fileName)
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
