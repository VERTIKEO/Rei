using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtRotation : MonoBehaviour
{
    public float thoughtTime = 5f;
    float remainingThoughtTime;
    
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        remainingThoughtTime = thoughtTime;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
        gameObject.transform.position = player.transform.position + Vector3.up * 2f;
        if (remainingThoughtTime >= 0)
        {
            remainingThoughtTime -= Time.deltaTime;
        }
        if (remainingThoughtTime <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
