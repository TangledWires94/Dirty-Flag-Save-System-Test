using System.IO;
using UnityEngine;

//Functions for reading and saving data to/from file
public static class FileIO
{
    //Save data of any type to provided file path, open reference -> write data -> close reference
    public static void SaveJsonData<T>(T Data, string filePath)
    {
        string jsonString = JsonUtility.ToJson(Data);
        StreamWriter writer = new StreamWriter(filePath);
        try
        {
            writer.Write(jsonString);
        }
        catch(IOException exception)
        {
            Debug.LogFormat("FileIO error trying to save at {0}.\nMessage : {1}", filePath, exception.Message);
        }
        writer.Dispose();
    }

    //Read data of any type from the specified file path, open reference -> read data -> close reference
    public static T LoadJsonData<T>(string filePath, bool createIfNotFound = false)
    {
        string jsonString = "";
        try
        {
            StreamReader reader = new StreamReader(filePath);
            jsonString = reader.ReadToEnd();
            reader.Dispose();
        }
        catch(FileNotFoundException)
        {
            Debug.LogFormat("File Not Found! : {0}", filePath);
            T defaultData = default(T);
            if (createIfNotFound)
            {
                Debug.Log("Creating new default data file");
                SaveJsonData<T>(defaultData, filePath);
            }
            return defaultData;
        }

        T data = JsonUtility.FromJson<T>(jsonString);
        return data;
    }

    //Check to see if the requested folder exists, if not create a new folder at that directory and print a warning to the console, returns true if the folder had to be created
    public static bool CheckForFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            return false;
        } 
        else
        {
            Directory.CreateDirectory(folderPath);
            Debug.LogFormat("Created missing folder at {0}", folderPath);
        }
        return true;
    }

}
