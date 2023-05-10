using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGenerator : MonoBehaviour
{
    public GameObject JPPrefab;
    public GameObject HPPrefab;
    public GameObject DPPrefab;
    public GameObject particles;
    public float coilTight = 5;
    public float coilHeight = 5;
    public float rotateSpeed;

    //slow spiral power ups
    public GameObject powerParent1;
    //fast spiral power ups
    public GameObject powerParent2;
    //ring power ups
    public GameObject powerParent3;
    //stack of rings
    public GameObject ringStackParent;




    // Start is called before the first frame update
    void Start()
    {

        //slow spiral
        for (int i = 0; i < 90; i++) 
        {
            GameObject jumpPower = Instantiate(HPPrefab, transform.position, Quaternion.identity);
            transform.Rotate(0, 10f, 0);
            transform.Translate(transform.forward * coilTight);
            transform.Translate(transform.up * coilHeight * 1.2f);

            jumpPower.transform.SetParent(powerParent1.transform);

        }
        rotateSpeed = 10f;
        gameObject.transform.position = new Vector3(0, 0, -10f);

        //fast power
        for (int i = 0; i < 90; i++)
        {
            GameObject jumpPower = Instantiate(DPPrefab, transform.position, Quaternion.identity);
            transform.Rotate(0, -10f, 0);
            transform.Translate(-transform.forward * coilTight*1.5f);
            transform.Translate(transform.up * coilHeight * 1.5f);

            jumpPower.transform.SetParent(powerParent2.transform);

        }
        rotateSpeed = 10f;
        gameObject.transform.position = new Vector3(-15f, -10f, 0);

        //ring
        for (int i = 0; i < 15; i++)
        {
            GameObject jumpPower = Instantiate(JPPrefab, transform.position, Quaternion.identity);
            transform.Rotate(0, 15f, 0);
            transform.Translate(transform.forward * coilTight*2);
            //transform.Translate(transform.up * coilHeight);

            jumpPower.transform.SetParent(powerParent3.transform);

        }

        gameObject.transform.position = new Vector3(0, 15f, 0);
        //stack of rings
        for (int i = 0; i < 20; i++)
        {
            GameObject ringStack = Instantiate(powerParent3, transform.position, Quaternion.identity);
            transform.Translate(transform.up * 8f*i/5f);
            //transform.Translate(transform.up * coilHeight);

            ringStack.transform.SetParent(ringStackParent.transform);

        }
    }

    // Update is called once per frame
    void Update()
    {
        powerParent1.transform.Rotate(0, rotateSpeed *0.5f* Time.deltaTime, 0);
        powerParent2.transform.Rotate(0, rotateSpeed *-1.5f * Time.deltaTime, 0);
        ringStackParent.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
