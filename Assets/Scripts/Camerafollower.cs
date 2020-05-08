using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Camerafollower : MonoBehaviour
{
    public GameObject player;
    public GameObject previouslyHid;
    public float turnSpeed = 2f;
    public float cameraHeight = 0.5f;
    private Vector3 offset;
    Material wallRenderer;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {


        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = player.transform.position + offset;
        var pos = player.transform.position;
        pos.y += cameraHeight;
        transform.LookAt(pos);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject != previouslyHid)
            {
                previouslyHid.GetComponent<MeshRenderer>().enabled = true;
            }
            if (hit.collider.gameObject.tag != "Player")
            {
                hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                previouslyHid = hit.collider.gameObject;
            }
            
        }

    }

}
