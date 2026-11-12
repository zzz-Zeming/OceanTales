using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointControl : MonoBehaviour
{
    public GameObject FishPre;
    public int MaxNum;
    public int TurnNum;
    private int Num;
    public float WaitTime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Num = 0;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Num < MaxNum && timer > WaitTime)
        {
            for (int i = 0; i < TurnNum; i++)
            {
                Instantiate(FishPre, transform.position, Quaternion.identity);
            }
            Num += TurnNum;
            timer = 0f;
        }
    }
}
