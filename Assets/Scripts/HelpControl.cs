using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpControl : MonoBehaviour
{
    public GameObject help;
    public GameObject bag;
    private bool isOpen = false;

    public GameObject fish1;
    public GameObject fish1_detail;
    public GameObject fish2;
    public GameObject fish2_detail;
    public GameObject fish3;
    public GameObject fish3_detail;
    public GameObject fish4;
    public GameObject fish4_detail;
    public GameObject fish5;
    public GameObject fish5_detail;
    public GameObject fish6;
    public GameObject fish6_detail;
    public GameObject fish7;
    public GameObject fish7_detail;
    public GameObject fish8;
    public GameObject fish8_detail;
    private bool isOpen_f = false;
    private bool isOpen_fd = false;

    public GameObject end;
    private bool End = false;
    List<string> FishIDs = new List<string>();

    private void Update()
    {
        FishIDs = StatusTable.instance.caughtFishIDs.Distinct().ToList();
        if (FishIDs.Count == 8 && !End)
        {
            end.SetActive(true);
            End = true;
        }
    }

    public void Setting()
    {
        if (!isOpen)
        {
            help.SetActive(true);
            isOpen = true;
        }
        else
        {
            help.SetActive(false);
            isOpen = false;
        }
    }

    public void Bag()
    {
        if (StatusTable.instance.caughtFishIDs != null)
        {
            foreach (string t in StatusTable.instance.caughtFishIDs)
            {
                if (t == "Fish1")
                {
                    fish1.SetActive(true);
                }
                else if (t == "Fish2")
                {
                    fish2.SetActive(true);
                }
                else if (t == "Fish3")
                {
                    fish3.SetActive(true);
                }
                else if (t == "Fish4")
                {
                    fish4.SetActive(true);
                }
                else if (t == "Fish5")
                {
                    fish5.SetActive(true);
                }
                else if (t == "Fish6")
                {
                    fish6.SetActive(true);
                }
                else if (t == "Fish7")
                {
                    fish7.SetActive(true);
                }
                else if (t == "Fish8")
                {
                    fish8.SetActive(true);
                }
            }
        }
        if (!isOpen_f)
        {
            bag.SetActive(true);
            isOpen_f = true;
        }
        else
        {
            bag.SetActive(false);
            isOpen_f = false;
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Home");
    }

    public void fish01()
    {
        if (!isOpen_fd)
        {
            fish1_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish1_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish02()
    {
        if (!isOpen_fd)
        {
            fish2_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish2_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish03()
    {
        if (!isOpen_fd)
        {
            fish3_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish3_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish04()
    {
        if (!isOpen_fd)
        {
            fish4_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish4_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish05()
    {
        if (!isOpen_fd)
        {
            fish5_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish5_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish06()
    {
        if (!isOpen_fd)
        {
            fish6_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish6_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish07()
    {
        if (!isOpen_fd)
        {
            fish7_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish7_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void fish08()
    {
        if (!isOpen_fd)
        {
            fish8_detail.SetActive(true);
            isOpen_fd = true;
        }
        else
        {
            fish8_detail.SetActive(false);
            isOpen_fd = false;
        }
    }

    public void end_back()
    {
        end.SetActive(false);
    }
}
