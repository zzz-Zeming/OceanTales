using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackControl : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("Land");
    }
}
