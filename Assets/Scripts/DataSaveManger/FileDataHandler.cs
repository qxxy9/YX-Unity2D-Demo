using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = string.Empty;
    private bool encryptData = false;
    private string codeWord = "secretdata";

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool encryptData)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.encryptData = encryptData;
        
    }

    public void Save(GameData _data)
    {
        string fullPath=Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore=JsonUtility.ToJson(_data,true);

            if (encryptData) 
                dataToStore=EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception  ) 
        {
            Debug.Log("error");
        }
    }

    public GameData Load()
    {
        string fullPath=Path.Combine(dataDirPath, dataFileName);
        GameData _data = new GameData();

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                _data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch { }
        }
        else return null;
        return _data;
    }


    public void Delete()
    {
        string fullPath=Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData +=(char)(_data[i]^codeWord[i%codeWord.Length]);
        }
        return modifiedData;
    }

}
