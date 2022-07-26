using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalVisibility : MonoBehaviour
{
    [SerializeField]
    public int numberOfHozizontalRaycasts, numberOfVerticalRaycasts;


    public GameObject player;

    [SerializeReference]
    public bool playerIsInFrontOfPortal;
    [SerializeReference]
    public bool isVisible;

    [SerializeReference]
    private MeshRenderer portalRenderer;

    public BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        //if not set in editor, set player automatically
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //get coordinates of the basically 2d boxcollider


        var trans = col.transform;
        var min = col.center - col.size * 0.5f;
        var max = col.center + col.size * 0.5f;
        var P000 = trans.TransformPoint(new Vector3(min.x, min.y, min.z));
        var P001 = trans.TransformPoint(new Vector3(min.x, min.y, max.z));
        var P010 = trans.TransformPoint(new Vector3(min.x, max.y, min.z));
        var P011 = trans.TransformPoint(new Vector3(min.x, max.y, max.z));
        var P100 = trans.TransformPoint(new Vector3(max.x, min.y, min.z));
        var P101 = trans.TransformPoint(new Vector3(max.x, min.y, max.z));
        var P110 = trans.TransformPoint(new Vector3(max.x, max.y, min.z));
        var P111 = trans.TransformPoint(new Vector3(max.x, max.y, max.z));

        //print all these to the log
        Debug.Log("P000 = " + P000 + ", P001 = " +


    }

    // Update is called once per frame
    void Update()
    {
        //check which side of the object the player is on
        Vector3 portalToPlayer = player.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
        playerIsInFrontOfPortal = dotProduct > 0;


        //isVisible = portalRenderer.isVisible && RaycastVisibility();
        isVisible = portalRenderer.isVisible;


    }

    bool RaycastVisibility()
    {


        //create grid of points on the front face of the portal
        //eliminate raycasts currently not on screen using WorldToScreenPoint

        //if any of the remaining raycasts hit the camera uninterrupted, then return true. else return false



        return true;



    }


}
