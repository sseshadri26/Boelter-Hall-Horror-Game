using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSize : MonoBehaviour
{
    //create slider for xScale and zScale
    [SerializeField]
    float xSize = 1.0f;
    [SerializeField]
    float zSize = 1.0f;

    [SerializeField]
    bool useFeet;

    //take in the 2 portal colliders and the 2 portal screens
    [SerializeField]
    BoxCollider Portal1Collider;
    [SerializeField]
    BoxCollider Portal2Collider;

    [SerializeField]
    GameObject Portal1Screen;
    [SerializeField]
    GameObject Portal2Screen;

    // Start is called before the first frame update

    void Start()
    {

    }


    private void OnValidate()
    {
        //log that this has been called
        //Debug.Log("PortalSize OnValidate");

        //scale xSize to meters or feet
        float scaledXSize =
            useFeet ? xSize * 0.3048f : xSize;
        float scaledZSize =
            useFeet ? zSize * 0.3048f : zSize;

        //scale portal1screen and portal2screen by xsize and zsize
        Portal1Screen.transform.localScale = new Vector3(scaledXSize, Portal1Screen.transform.localScale.y, scaledZSize);
        Portal2Screen.transform.localScale = new Vector3(scaledXSize, Portal2Screen.transform.localScale.y, scaledZSize);

        Portal1Collider.size = new Vector3(scaledXSize, Portal1Collider.size.y, scaledZSize);
        Portal2Collider.size = new Vector3(scaledXSize, Portal2Collider.size.y, scaledZSize);

    }
}
