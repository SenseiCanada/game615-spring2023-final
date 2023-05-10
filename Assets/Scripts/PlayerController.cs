using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Rigidbody playerRB;
    public Rigidbody playerCubeRB;
    public float jumpForce;
    public float dashForce;
    public float rotateSpeed = 5f;
    public float moveSpeed = 1f;
    public float groundDistance;
    public float dashDistance;
    public bool controlsEnabled = false;
    public bool powerCollide = false;
    public bool dashEnabled;

    GameManager gm;
    public Animator menuAnimator;
    public Animator warnAnimator;
    public Animator playerAnim;
    public float warnTimer = 100f;
    
    
    public GameObject mainCamera;
    public GameObject ground;
    public Vector2 playerStartXZ;
    public Vector2 PlayerXZ;
    public float DashStartTime;

    public int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        jumpForce = 1000f;
        dashForce = 3000f;
        GameObject gmObj = GameObject.Find("GameManager");
        gm = gmObj.GetComponent<GameManager>();
        
        dashEnabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        
        

        groundDistance = Vector3.Distance(gameObject.transform.position, ground.transform.position);
        

        //conditional controls
        if (controlsEnabled == true)
        {
            enableControls();
            powerCollide= true;
        }

        //tank controls
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        gameObject.transform.Translate(transform.forward * vAxis * moveSpeed * Time.deltaTime, Space.World);
        gameObject.transform.Rotate(0, rotateSpeed * Time.deltaTime * hAxis, 0);
        if(hAxis !=0 && gm.rotateHint == true)
        {
            gm.rotateHint.gameObject.SetActive(false);
        }
        if (vAxis != 0 && gm.hintAnimator.GetBool("driftHint") == true)
        {
            gm.hintAnimator.SetBool("driftHint", false);
        }

        //jump warning
        if (groundDistance <= 50f && warnAnimator.GetBool("needWarning") == false)
        {
            warnAnimator.SetBool("needWarning", true);
        }
        else if (groundDistance <= warnTimer && controlsEnabled == false)
        {
            jumpWarn();
        }
       
        if(!dashEnabled)
        {
            playerRB.drag = 0;
        }
        if (dashEnabled == true)
        {
            float timeElapsed = Time.time - DashStartTime;
            Debug.Log(timeElapsed);
            if (timeElapsed >= .5f)
            {
                dashEnabled = false;
            }

        }

    }

    void enableControls()
    {

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && gm.jumpCharge > 0)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, 0);
            playerRB.AddForce(Vector3.up * jumpForce);
            playerAnim.StopPlayback();
            playerAnim.Play("jumpAnim");
            gm.jumpCharge -= 1;
            warnAnimator.SetBool("needWarning", false);
            jumpCount++;
        }

        //drop
        if (Input.GetMouseButtonDown(0) && playerRB.velocity.y > 0)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, -0.1f);
        }

        //hover
        if (Input.GetKey(KeyCode.LeftShift) && gm.hoverCharge > 0)
        {
            playerRB.useGravity = false;
            playerRB.velocity = new Vector3(0, 0, 0);
            gm.hoverTimer += Time.deltaTime;
            gm.hoverCharge = gm.hoverCharge - gm.hoverTimer;
            gm.DecreaseHoverCharge();
            

        }
        else 
        {
            gm.hoverTimer = 0;
            playerRB.useGravity = true;
            //gm.hoverHealthShell.gameObject.SetActive(false);
            //gm.hoverHealthImage.gameObject.SetActive(false);
            gm.hoverHint.gameObject.SetActive(false);

        }

        //dash
        if (Input.GetKeyDown(KeyCode.V) && gm.dashCharge > 0)
        {
            dashEnabled = true;
            playerAnim.Play("jumpAnim");
            DashStartTime = Time.time;
            playerStartXZ = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
            playerRB.velocity = new Vector3(0, 0, 0);
            playerRB.AddForce(mainCamera.transform.forward * dashForce);
            playerRB.AddForce(mainCamera.transform.up * dashForce);
            gm.dashCharge--;
            ArrestDash();
        }
        else
        {
            gm.dashHint.gameObject.SetActive(false);
        }
        
    }
    
    void jumpWarn()
    {
        warnAnimator.SetBool("needWarning", true);
        controlsEnabled = true;
        menuAnimator.SetBool("powerUnlocked", true);
    }

    void ArrestDash()
    {
        dashDistance = Vector2.Distance(PlayerXZ, playerStartXZ);
        //gm.dashHint.gameObject.SetActive(false);

        if (dashDistance >= 10f)
        {
            playerRB.drag = 5f;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("jumpPower") && powerCollide == true)
        {
            gm.jumpCharge += 1;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("hoverPower") && powerCollide == true)
        {
            gm.hoverCharge = 10;
            Destroy(other.gameObject);
            if (gm.hoverHealthImage.fillAmount <= 10f)
            {
                gm.hoverHealthImage.fillAmount = 10f;
            }
        }

        if (other.CompareTag("towerTop"))
        {
            gm.LoadScene();
        }

        if (other.CompareTag("ground"))
        {
            gm.LoadGameOver();
        }

        if (other.CompareTag("wall"))
        {
            playerRB.velocity = new Vector3(0, playerRB.velocity.y, 0);
        }
        if (other.CompareTag("dashPower") && powerCollide == true)
        {
            gm.dashCharge += 3;
            Destroy(other.gameObject);
        }
    }
}   
