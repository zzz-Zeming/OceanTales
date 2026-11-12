using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_RoomControl : MonoBehaviour
{
    public GameObject Fish1Pre;
    public GameObject Fish2Pre;
    public GameObject Fish3Pre;
    public GameObject Fish4Pre;
    public GameObject Fish5Pre;
    public GameObject Fish6Pre;
    public GameObject Fish7Pre;
    public GameObject Fish8Pre;
    private bool Over = false;

    private void Start()
    {
        Over = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (StatusTable.instance.caughtFishIDs != null && !Over)
        {
            foreach (string t in StatusTable.instance.caughtFishIDs)
            {
                if (t == "Fish1")
                {
                    Instantiate(Fish1Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish2")
                {
                    Instantiate(Fish2Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish3")
                {
                    Instantiate(Fish3Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish4")
                {
                    Instantiate(Fish4Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish5")
                {
                    Instantiate(Fish5Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish6")
                {
                    Instantiate(Fish6Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish7")
                {
                    Instantiate(Fish7Pre, transform.position, Quaternion.identity);
                }
                else if (t == "Fish8")
                {
                    Instantiate(Fish8Pre, transform.position, Quaternion.identity);
                }
            }
            Over = true;
        }
    }
}
