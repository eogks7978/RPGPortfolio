using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파일을 기기에 생성 및 삭제하기 위한 클래스
/// </summary>
public class FileManager : MonoBehaviour
{
    public static Action OnDataDeleted;

    public static bool DeleteFilesInFolder(string folderName)
    {
        string folderPath = Application.persistentDataPath + "/" + folderName;

        string[] files = Directory.GetFiles(folderPath);

        // 파일이 있으면 삭제
        foreach (string file in files)
        {
            try
            {
                File.Delete(file); // 파일 삭제
                Debug.Log("삭제된 파일: " + file);
            }
            catch (IOException e)
            {
                Debug.LogError("파일 삭제 실패: " + file + " Error: " + e.Message);
                return false;
            }
        }
        OnDataDeleted?.Invoke();
        return true;
    }

    public static bool CreateFolder(string folderName)
    {
        string folderPath = Application.persistentDataPath + "/" + folderName;
        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return true;
        }
        catch (IOException e)
        {
            Debug.LogError("폴더 생성 실패: " + e.Message);
            return false;
        }
    }
}
