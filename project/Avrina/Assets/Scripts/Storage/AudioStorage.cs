using System.IO;

public class AudioStorage
{
    private const string filePath = "AudioConfig.txt";

    /// <summary>
    ///  How load is the music
    /// </summary>
    public static float musicVolume = 1f;
    /// <summary>
    ///  Is the music muted
    /// </summary>
    public static bool isMusicMuted = false;
    /// <summary>
    ///  How load are sound effect
    /// </summary>
    public static float soundEffectVolume = 1f;
    /// <summary>
    ///  Are sound effects muted
    /// </summary>
    public static bool areSoundEffectsMuted = false;

    
    /// <summary>
    ///  Will be called at the start of the programm. Reads the audio settings if they exist
    /// </summary>
    static AudioStorage()
    {
        if (!File.Exists(filePath))
        {
            SaveValues();
        }

        ReadValues();
    }

    /// <summary>
    ///  Stores the audio settings to the config
    /// </summary>
    public static void SaveValues()
    {
        string fileContent = "MusicVolume: " + musicVolume.ToString() + "\n";
        fileContent += "IsMusicMuted: " + isMusicMuted.ToString() + "\n";
        fileContent += "SoundEffectVolume: " + soundEffectVolume.ToString() + "\n";
        fileContent += "AreSoundEffectsMuted: " + areSoundEffectsMuted.ToString() + "\n";

        File.WriteAllText(filePath, fileContent);
    }

    /// <summary>
    ///  Reads the audio settings from the config
    /// </summary>
    private static void ReadValues()
    {
        string fileContent = File.ReadAllText(filePath);
        foreach (var variable in fileContent.Split('\n'))
        {
            if (variable.Contains("MusicVolume: "))
            {
                musicVolume = float.Parse(variable.Replace("MusicVolume: ", ""));
            }
            else if (variable.Contains("IsMusicMuted: "))
            {
                isMusicMuted = bool.Parse(variable.Replace("IsMusicMuted: ", ""));
            }
            else if (variable.Contains("SoundEffectVolume: "))
            {
                soundEffectVolume = float.Parse(variable.Replace("SoundEffectVolume: ", ""));
            }
            else if (variable.Contains("AreSoundEffectsMuted: "))
            {
                areSoundEffectsMuted = bool.Parse(variable.Replace("AreSoundEffectsMuted: ", ""));
            }
        }
    }
}
