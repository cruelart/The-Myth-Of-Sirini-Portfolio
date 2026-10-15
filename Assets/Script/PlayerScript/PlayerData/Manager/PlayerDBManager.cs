using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerDBManager : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "playerDB.json"); // 저장경로는 playerDB.json 파일

    //싱글톤 사용으로 PlayerToTalManager가 이 인스턴스를 통해 다른 매니저에게 분배할 예정 ㅇㅇ
    public static PlayerDBManager instance { get; private set; }

    public PlayerStatData playerDB { get; private set; } // 플레이중인 플레이어의 데이터
    //private Dictionary<int, QuestProgressData> questProgressDataTable = new(); // 퀘스트 ID별로 플레이어의 진행현황을 저장하는 해시테이블

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this; 
        playerDB = Load();

        DontDestroyOnLoad(gameObject); // 씬 전환때 오브젝트 파괴 방지
    }

    private void Update()
    {
        //Debug.Log("PlayerDBManager에서 보여주는 maxHp는 " + instance.playerDB.MaxHp);
    }

    public void Save(PlayerStatData _data)
    {
        string json = JsonUtility.ToJson(_data, true); // 데이터를 json문자열로 바꾸고
        File.WriteAllText(SavePath, json); // 파일에 덮어씌움
    }

    public PlayerStatData Load()
    {
        if(File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath); // 저장된 파일경로에서 파일 읽고
            PlayerStatData data = JsonUtility.FromJson<PlayerStatData>(json); // json을 PlayerDB클래스에 맞게 변환
            return data;
        }
        Debug.Log("PlayerDBManager에서 제이슨파일을 찾지 못해서 새롭게 PlayeDB를 생성합니다");
        return new PlayerStatData(); // 파일 없으면 기본값으로 세팅
    }

    public void DeleteSaveFile()
    {
        if(File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
    }

}
