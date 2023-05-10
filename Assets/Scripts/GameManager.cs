using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int jumpCharge;
    public float hoverCharge;
    public float hoverTimer;
    public int dashCharge;

    public TMP_Text JCText;
    public TMP_Text DCText;
    public TMP_Text rotateHint;
    public TMP_Text driftHint;
    public TMP_Text hoverHint;
    public TMP_Text dashHint;
    public Image hoverHealthImage;
    public Image hoverHealthShell;

    public Animator startAnimator;
    public Animator hintAnimator;

    PlayerController playerController;
    
    public GameObject pcObj;

    public Rigidbody playerRB;

    bool gameStart = false;
 

    // Start is called before the first frame update
    void Start()
    {

        playerController = pcObj.GetComponent<PlayerController>();
        playerRB = pcObj.GetComponent<Rigidbody>();

        jumpCharge = 3;
        hoverCharge = 10f;
        hoverTimer = 0;
        dashCharge = 3;

        rotateHint.gameObject.SetActive(false);
        driftHint.gameObject.SetActive(false);
        hoverHint.gameObject.SetActive(false);
        dashHint.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        JCText.text = jumpCharge.ToString();
        DCText.text = dashCharge.ToString();

        if (Time.timeSinceLevelLoad >= 1.5f && gameStart == false)
        {
            StartLaunch();
        }

        if (playerController.jumpCount == 2)
        {
            ShowDriftHints();
        }

        if (playerController.jumpCount == 3)
        {
            ShowHoverHint();
        }

        if (dashCharge <= 0)
        {
            dashCharge = 0;
        }

        if (playerController.jumpCount == 4)
        {
            ShowDashHint();
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("YouWin");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void StartLaunch()
    {
        rotateHint.gameObject.SetActive(true);
        
        playerRB.AddForce(0, 0, 300);
        playerRB.useGravity = true;
        gameStart = true;
    }
    public void ShowDriftHints()
    {
        driftHint.gameObject.SetActive(true);
        hintAnimator.SetBool("driftHint", true);
    }

    public void ShowHoverHint()
    {
        hoverHint.gameObject.SetActive(true);
    }

    public void ShowDashHint()
    {
        dashHint.gameObject.SetActive(true);
    }

    public void DecreaseHoverCharge()
    {
        
        //hoverHealthImage.gameObject.SetActive(true);
        hoverHealthImage.fillAmount = hoverCharge/ 10f;

    }

}
