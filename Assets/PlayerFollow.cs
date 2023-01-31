using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    // make the camera follow the player object
    public Transform player;
    public Vector3 offset;
    private void Start()
    {
        offset = transform.position - player.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
