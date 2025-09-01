using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

/// <summary>
/// 데이터를 총괄하는 매니저
/// </summary>
[Serializable]
public class DataManager : MonoBehaviour
{
    #region singleton
    private static DataManager instance;
    public static DataManager Instance => instance;
    #endregion

    [SerializeField] private TableDataTable tableDataTable;
    public TableDataTable TableDataTable { get => tableDataTable; }

    public const string DefaultDataFolderName = "DataFolder";
    public const string DefaultPlayerDataFileName = "PlayerData";
    public const string DefaultInventoryDataFileName = "InventoryData";
    public const string DefaultSkillDataFileName = "SkillData";
    public const string DefaultMapDataPath = "Assets/05_Data/Data_MapData";
    public const string DefaultIllustratedGuideData = "IllustratedGuideData";
    public const string DefaultTutorialProgressData = "TutorialProgressData";
    public const string DefaultOptionData = "OptionData";

    public int SelectedSaveSlot = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }

    // Json으로 변환 및 저장
    public static void SaveData(object obj, string fileName)
    {
#if UNITY_EDITOR
        // 가독성 높지만 용량 큼. Editor용.
        var data = JsonConvert.SerializeObject(obj);
#else
        var data = JsonConvert.SerializeObject(obj);
#endif

        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", data);
    }

    public static void SaveData(object obj, string fileName, string path)
    {
#if UNITY_EDITOR
        // 가독성 높지만 용량 큼. Editor용.
        var data = JsonConvert.SerializeObject(obj);
#else
        var data = JsonConvert.SerializeObject(obj);
#endif

        File.WriteAllText(path + "/" + fileName + ".json", data);
    }

    // data file을 Json으로 변환
    public static T LoadData<T>(string fileName)
    {
        string data = "";

        try
        {
            data = File.ReadAllText(Application.persistentDataPath + "/" + fileName + ".json");
        }
        catch (Exception)
        {
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(data);
    }

    public static T LoadData<T>(string fileName, string path)
    {
        string data = "";

        try
        {
            data = File.ReadAllText(path + "/" + fileName + ".json");
        }
        catch (Exception)
        {
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(data);
    }

    public static T ParseData<T> (string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public async Task<object> LoadAddressableObjectAsync(string key)
    {
        // Addressable 자산을 비동기적으로 로드
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<object>(key);

        // 로드가 완료될 때까지 기다림
        await handle.Task;

        // 로드 결과가 성공적인지 확인
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 성공적으로 로드된 자산을 반환
            return handle.Result;
        }
        else
        {
            // 실패 시 null 반환
            Debug.LogError($"Failed to load Addressable asset with key: {key}");
            return null;
        }
    }

    public static object LoadAddressableObject(string key)
    {
        // Addressable 자산을 동기적으로 로드
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<object>(key);

        // 비동기 작업이 완료될 때까지 기다림
        handle.WaitForCompletion();

        // 로드 결과가 성공적인지 확인
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 성공적으로 로드된 자산을 반환
            return handle.Result;
        }
        else
        {
            // 실패 시 null 반환
            Debug.LogError($"Failed to load Addressable asset with key: {key}");
            return null;
        }
    }
}

/// <summary>
/// 유틸 클래스
/// </summary>
[Serializable]
public class Vector2Wrapper
{
    public float X;
    public float Y;

    public Vector2Wrapper(Vector2 value)
    {
        X = value.x;
        Y = value.y;
    }
}

/// <summary>
/// 유틸 클래스
/// </summary>
[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}