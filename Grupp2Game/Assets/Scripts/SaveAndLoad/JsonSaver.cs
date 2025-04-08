using System;
using System.IO;
using UnityEngine;

//Author Vidar Edlund
public class JsonSaver : ISaver
{
    private readonly string dirPath = Application.persistentDataPath + "/Saves";
    private string fileName = null;

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "JoeMama";

    public JsonSaver(bool useEncryption)
    {
       // _fileName = fileName;
        this.useEncryption = useEncryption;
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
    public void Save(GameData gameData)
    {
        fileName = gameData.ActiveScene + ".json";
        string filePath = string.Join('/', dirPath, fileName);
        Debug.Log(filePath);
        if (!File.Exists(filePath))
        {
            Debug.Log("No save file found, a new one is created");
            File.Create(filePath).Close();
        }
        try
        {
            //Serialize our C# data Object to a json 
            string dataString = JsonUtility.ToJson(gameData, true);

            //Encrypt data if that option is ticked
            if (useEncryption)
            {
                dataString = EncryptDecrypt(dataString);
            }
            //Write the serialized data to a file

            File.WriteAllText(filePath, dataString);

            //using (FileStream filestream = new FileStream(filePath, FileMode.Create))
            //{
            //    using(StreamWriter writer = new StreamWriter(filestream))
            //    {
            //        writer.WriteLine(dataString);
            //    }
            //}
        }
        catch (Exception e)
        {
            Debug.Log($"An error occurred when trying to save to a file at {filePath}");
            Debug.Log(e.Message);
        }


    }
    public bool FileExists(string file)
    {
        string filePath = string.Join('/', dirPath, file);
        Debug.Log(file);
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            Debug.Log($"File system found no data to load - {file}");
            return false;
        }
    }
    public GameData LoadLatest()
    {
        GetLatestSaveFile();
        string filePath = string.Join('/', dirPath, fileName);

        Debug.Log(filePath);
        GameData gameData = null;

        if (!FileExists(fileName))
        {
            return gameData;
        }
        try
        {
            //Load serialized data from a file
            string dataString = File.ReadAllText(filePath);

            //Encrypt data if that option is ticked
            if (useEncryption)
            {
                dataString = EncryptDecrypt(dataString);
            }

            //Deserialize the data from Json back to a C# object
            gameData = JsonUtility.FromJson<GameData>(dataString);


            //using (FileStream filestream = new FileStream(filePath, FileMode.Open))
            //{
            //    using (StreamReader reader = new StreamReader(filestream))
            //    {
            //        dataString = reader.ReadToEnd();
            //    }
            //}
        }
        catch (Exception e)
        {
            Debug.Log($"An error occurred when trying to load from a file at {filePath}");
            Debug.Log(e.Message);
        }


        return gameData;
    }
    public GameData Load(string currentScene)
    {
        fileName = currentScene + ".json";

        string filePath = string.Join('/', dirPath, fileName);

        Debug.Log(filePath);
        GameData gameData = null;

        if (!FileExists(fileName))
        {
            return gameData;
        }
        try
        {
            //Load serialized data from a file
            string dataString = File.ReadAllText(filePath);

            //Encrypt data if that option is ticked
            if (useEncryption)
            {
                dataString = EncryptDecrypt(dataString);
            }

            //Deserialize the data from Json back to a C# object
            gameData = JsonUtility.FromJson<GameData>(dataString);

            
            //using (FileStream filestream = new FileStream(filePath, FileMode.Open))
            //{
            //    using (StreamReader reader = new StreamReader(filestream))
            //    {
            //        dataString = reader.ReadToEnd();
            //    }
            //}
        }
        catch (Exception e)
        {
            Debug.Log($"An error occurred when trying to load from a file at {filePath}");
            Debug.Log(e.Message);
        }


        return gameData;
    }
    public bool FilesExists()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.json");
        return fileInfo.Length > 0;
    }
    public void DeleteAllFiles()
    {
        if (!Directory.Exists(dirPath))
        {
            Debug.LogWarning("Directory does not exist: " + dirPath);
            return;
        }
        Debug.Log("Deleting files from: " + dirPath);

        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles("*.json");

        for (int i = fileInfo.Length; i > 0; i--)
        {
            try
            {
                File.Delete(fileInfo[i - 1].FullName);
                Debug.Log("Deleted: " + fileInfo[i - 1].FullName);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to delete file: " + fileInfo[i - 1].FullName + " Error: " + ex.Message);
            }
        }

        //foreach (FileInfo file in fileInfo)
        //{
        //    try
        //    {
        //        File.Delete(file.FullName);
        //        Debug.Log("Deleted: " + file.FullName);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Debug.LogError("Failed to delete file: " + file.FullName + " Error: " + ex.Message);
        //    }
        //}

        Debug.Log("All save files successfully deleted.");


        //Maybe use these

        /*
         * if (!Application.isEditor && !Application.isPlaying)
{
    Debug.LogWarning("File deletion is disabled outside of play mode!");
    return;
}

if (!Directory.Exists(dirPath)) return;

if (fileInfo.Length > 0)
{
    Debug.LogWarning("Are you sure you want to delete all saves? Press a button to confirm.");
    return;
}
         */
    }
    public bool DeleteAllFilesConfirmation(bool shouldDelete)
    {
        return shouldDelete;
    }
    private void GetLatestSaveFile()
    {
        if (fileName == null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            FileInfo[] fileInfo = directoryInfo.GetFiles("*.json");
            FileInfo currentFileInfo = null;
            foreach (FileInfo file in fileInfo)
            {
                if (currentFileInfo == null)
                {
                    currentFileInfo = file;
                }
                if (currentFileInfo.LastWriteTime < file.LastWriteTime)
                {
                    currentFileInfo = file;
                }
            }
            if (currentFileInfo != null)
                fileName = currentFileInfo.Name;
        }
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
