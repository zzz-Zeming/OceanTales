using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl playerControl;
    private Animator animator;
    private Collider2D cld;
    public GameObject HookPre;
    private GameObject currentHook;
    Transform FirePoint;

    public bool OnBoat = false;
    public bool Row = false;
    public bool Rowing = false;
    public bool Fishing = false;
    public float throwForceX = 2f;
    public float throwForceY = 1f;
    public bool GetOut = false;
    public bool canGetOut = false;
    public bool GetBack = false;

    public AudioSource audioSource;
    public AudioClip sand;
    public AudioClip wood;
    public AudioClip water;
    private float audioTime = 10f;
    public bool isLand = true;
    private bool isSand = true;

    private float FishNum = 0;
    private float FishMax = 2;
    public bool GetBoat = false;
    public Text lv1;
    public Text lv2;
    public Text lv3;
    private int LV = 1;
    public UnityEngine.UI.Image exp;

    private void Awake()
    {
        playerControl = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cld = GetComponent<Collider2D>();
        FirePoint = this.transform.Find("FirePoint");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0f && !Fishing)
        {
            transform.Translate(Vector2.right * horizontal * Time.deltaTime);
            GetComponent<SpriteRenderer>().flipX = horizontal > 0 ? false : true;
            if (!Row)
            {
                animator.SetBool("IsWalk", true);
            }
            else
            {
                Rowing = true;
            }

            audioTime += Time.deltaTime;
            if (isLand)
            {
                if (isSand)
                {
                    if (audioTime > 0.7f)
                    {
                        audioSource.PlayOneShot(sand);
                        audioTime = 0f;
                    }
                }
                else
                {
                    if (audioTime > 1f)
                    {
                        audioSource.PlayOneShot(wood);
                        audioTime = 0f;
                    }
                }
            }
            else
            {
                if (audioTime > 2f)
                {
                    audioSource.PlayOneShot(water);
                    audioTime = 0f;
                }
            }
        }
        else
        {
            animator.SetBool("IsWalk", false);
            Rowing = false;
            audioSource.Stop();
        }
        StartRowing();

        if (Input.GetKeyDown(KeyCode.Space) && !Row)
        {
            if (!Fishing)
            {
                CastHook();
            }
            else
            {
                ReelIn();
            }
        }

        if (canGetOut && GetOut && Input.GetKeyDown(KeyCode.F))
        {
            GetOut = false;
            StatusTable.instance.SaveCurrentSceneData();
            SceneManager.LoadScene("Ocean");
        }
        else if (GetBack && Input.GetKeyDown(KeyCode.F))
        {
            GetBack = false;
            StatusTable.instance.SaveCurrentSceneData();
            SceneManager.LoadScene("Land");
        }

        FishNum = StatusTable.instance.caughtFishIDs.Count;
        exp.fillAmount = FishNum / FishMax;
        if (FishNum >= FishMax && LV == 1)
        {
            GetBoat = true;
            lv1.gameObject.SetActive(false);
            lv2.gameObject.SetActive(true);
            LV += 1;
            FishMax = 3f;
        }
        else if (FishNum >= FishMax && LV == 2)
        {
            canGetOut = true;
            lv2.gameObject.SetActive(false);
            lv3.gameObject.SetActive(true);
            LV += 1;
        }
    }

    public void GetOnBoat()
    {
        if (GetBoat)
        {
            if (!OnBoat)
            {
                this.transform.position = new Vector3(this.transform.position.x, -0.7f, this.transform.position.z);
                OnBoat = true;
                isLand = false;
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, -0.25f, this.transform.position.z);
                Row = false;
                OnBoat = false;
                isLand = true;
            }
        }
    }
    
    void StartRowing()
    {
        if (OnBoat)
        {
            if (Row)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 2.6f);
                //cld.offset = new Vector2(0f, 0.07f);
                animator.SetBool("OnBoat", true);
                if (Rowing)
                {
                    animator.SetBool("IsRow", true);
                }
                else
                {
                    animator.SetBool("IsRow", false);
                }
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 2.8f);
                //cld.offset = new Vector2(0f, 0.18f);
                animator.SetBool("IsRow", false);
                animator.SetBool("OnBoat", false);
            }
        }
    }

    void CastHook()
    {
        Fishing = true;
        throwForceX = 2f;
        currentHook = Instantiate(HookPre, FirePoint.position + new Vector3(0f, 0f, -2f), Quaternion.identity);
        Rigidbody2D rb = currentHook.GetComponent<Rigidbody2D>();
        throwForceX = GetComponent<SpriteRenderer>().flipX ? -throwForceX : throwForceX;
        rb.AddForce(new Vector2(throwForceX, throwForceY), ForceMode2D.Impulse);
        animator.SetTrigger("Fishing");
    }

    void ReelIn()
    {
        Fishing = false;
        animator.SetTrigger("GetFish");
        if (currentHook != null)
        {
            Destroy(currentHook);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "DB_Ocean")
        {
            GetOut = true;
        }
        else if (collision.transform.tag == "DB_Home")
        {
            GetBack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "DB_Ocean")
        {
            GetOut = false;
        }
        else if (collision.transform.tag == "DB_Home")
        {
            GetBack = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "wood")
        {
            isSand = false;
        }
        else if (collision.collider.tag == "sand")
        {
            isSand = true;
        }
    }
}