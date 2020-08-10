using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewinder : MonoBehaviour
{
    public bool isRewinding;
    public bool active;

    private float blinkTime = 2.0f;

    [SerializeField]
    GameObject blinkingLight;
    [SerializeField]
    GameObject doneLight;

    public GameObject vhsTape;

    // Start is called before the first frame update
    void Start()
    {
        isRewinding = false;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewinding)
        {
            doneLight.SetActive(false);
            StartBlinking();
        }
        else
        {
            blinkTime = 2;
            blinkingLight.SetActive(false);
            doneLight.SetActive(true);
        }
    }

    public void StartBlinking()
    {
        if (blinkTime > 1)
        {
            blinkingLight.SetActive(true);
        }
        else
        {
            blinkingLight.SetActive(false);
        }
        if (blinkTime <= 0)
        {
            blinkTime = 2;
        }
        blinkTime -= Time.deltaTime;
    }
}
