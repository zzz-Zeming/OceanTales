using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusTable : MonoBehaviour
{
    [System.Serializable]
    public class PlayerState
    {
        public Vector3 position;
        public bool OnBoat;
        public bool Row;
        public bool Rowing;
        public bool Fishing;
        public bool canGetOut;
        public bool GetBoat;
        public bool isLand;
    }

    [System.Serializable]
    public class BoatState
    {
        public Vector3 position;
    }

    [System.Serializable]
    public class CameraState
    {
        public Vector3 position;
    }

    [System.Serializable]
    public class SceneData
    {
        public string sceneName;
        public PlayerState playerState = new PlayerState(); // 这里改成了单个对象而不是List，因为通常单人游戏只有一个玩家喵
        public BoatState boatState = new BoatState();
        public CameraState cameraState = new CameraState();
        public bool hasSavedData = false; // 标记该场景是否存过档
    }

    public static StatusTable instance;
    private Dictionary<string, SceneData> sceneDataDic = new Dictionary<string, SceneData>();

    // 在 StatusTable 类里面添加 喵
    public List<string> caughtFishIDs = new List<string>(); // 存储钓到的鱼的名字或ID

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保证切换场景时不被销毁 喵
            SceneManager.sceneLoaded += OnSceneLoaded; // 只保留加载事件
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- 核心修改：手动保存方法 ---
    // 请在执行 SceneManager.LoadScene 之前调用这个方法 喵！
    public void SaveCurrentSceneData()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneData data = new SceneData { sceneName = currentSceneName };

        // 1. 获取玩家 喵
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerControl pc = playerObj.GetComponent<PlayerControl>();
            if (pc != null)
            {
                data.playerState.position = playerObj.transform.position;
                data.playerState.OnBoat = pc.OnBoat;
                data.playerState.Row = pc.Row;
                data.playerState.Rowing = pc.Rowing;
                data.playerState.Fishing = pc.Fishing;
                data.playerState.canGetOut = pc.canGetOut;
                data.playerState.GetBoat = pc.GetBoat;
                data.playerState.isLand = pc.isLand;
            }
        }

        // 2. 获取船只 喵
        GameObject boatObj = GameObject.FindGameObjectWithTag("Boat");
        if (boatObj != null)
        {
            data.boatState.position = boatObj.transform.position;
        }

        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj != null)
        {
            data.cameraState.position = cameraObj.transform.position;
        }

        data.hasSavedData = true;
        sceneDataDic[currentSceneName] = data; // 存入字典

        Debug.Log($"保存成功喵！场景: {currentSceneName} | 玩家位置: {data.playerState.position}");
    }

    // --- 加载场景时自动调用 ---
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // 如果字典里没有这个场景的数据，说明是第一次进入，直接跳过 喵
        if (!sceneDataDic.ContainsKey(sceneName)) return;

        SceneData data = sceneDataDic[sceneName];
        if (!data.hasSavedData) return;

        Debug.Log($"开始加载数据喵... 场景: {sceneName}");

        // 1. 恢复船只位置 (必须先恢复船，因为玩家可能要站在船上) 喵
        GameObject boatObj = GameObject.FindGameObjectWithTag("Boat");
        if (boatObj != null)
        {
            boatObj.transform.position = data.boatState.position;
        }

        // 2. 恢复玩家状态 喵
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // 恢复物理位置
            playerObj.transform.position = data.playerState.position;

            PlayerControl pc = playerObj.GetComponent<PlayerControl>();
            if (pc != null)
            {
                pc.OnBoat = data.playerState.OnBoat;
                pc.Row = data.playerState.Row;
                pc.Rowing = data.playerState.Rowing;
                pc.Fishing = data.playerState.Fishing;
                pc.canGetOut = data.playerState.canGetOut;
                pc.GetBoat = data.playerState.GetBoat;
                pc.isLand = data.playerState.isLand;
            }
        }

        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj != null)
        {
            cameraObj.transform.position = data.cameraState.position;
        }
    }

    public void AddFish(string fishName)
    {
        caughtFishIDs.Add(fishName);
        // 这里以后可以扩展保存到硬盘的功能 喵
    }
}