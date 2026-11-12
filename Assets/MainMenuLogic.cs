using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public void StartGame()
    {
        // 记得先把 StatusTable 清空或者初始化，防止读到旧数据
        // StatusTable.instance.ResetData(); (如果你有写重置方法的话)

        SceneManager.LoadScene("Land"); // 你的渔夫场景名字
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏喵！");
    }
}