using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalVisibility : MonoBehaviour
{
    [SerializeField]
    int numberOfHorizontalRaycasts, numberOfVerticalRaycasts;

    //2d array of Vector3 points matching the above variables
    public Vector3[,,] raycastPoints;
    public bool?[,,] raycastPointsSuccess;


    public GameObject player;

    [SerializeReference]
    public bool playerIsInFrontOfPortal;
    [SerializeReference]
    public bool isVisible;

    [SerializeReference]
    private MeshRenderer portalRenderer;

    public BoxCollider col;


    Vector3 P000;
    Vector3 P001;

    Vector3 P100;
    Vector3 P101;

    Vector3 basisX, basisY, basisZ;

    // Start is called before the first frame update
    void Start()
    {

        //if not set in editor, set player automatically
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //get coordinates of the basically 2d boxcollider
        Transform trans = col.transform;
        Vector3 min = col.center - col.size * 0.5f;
        Vector3 max = col.center + col.size * 0.5f;
        P000 = trans.TransformPoint(new Vector3(min.x, min.y, min.z));
        P001 = trans.TransformPoint(new Vector3(min.x, min.y, max.z));
        //P010 = trans.TransformPoint(new Vector3(min.x, max.y, min.z));
        //P011 = trans.TransformPoint(new Vector3(min.x, max.y, max.z));
        P100 = trans.TransformPoint(new Vector3(max.x, min.y, min.z));
        P101 = trans.TransformPoint(new Vector3(max.x, min.y, max.z));
        //P110 = trans.TransformPoint(new Vector3(max.x, max.y, min.z));
        //P111 = trans.TransformPoint(new Vector3(max.x, max.y, max.z));

        //Generate the 2 basis vectors for our grid.

        basisX = P101 - P100; //yellow - green
        basisY = P000 - P100; //red - green

        basisZ = Vector3.Normalize(Vector3.Cross(basisX, basisY)) * 0.8f;

        // green     red
        // yellow    blue

        // P100     P000
        // P101     P001

        raycastPoints = new Vector3[numberOfHorizontalRaycasts, numberOfVerticalRaycasts, 2];
        raycastPointsSuccess = new bool?[numberOfHorizontalRaycasts, numberOfVerticalRaycasts, 2];

        for (int i = 0; i < numberOfHorizontalRaycasts; i++)
        {
            for (int j = 0; j < numberOfVerticalRaycasts; j++)
            {
                raycastPoints[i, j, 1] = P100 + basisX * i / (numberOfHorizontalRaycasts - 1) + basisY * j / (numberOfVerticalRaycasts - 1);
                raycastPoints[i, j, 0] = P100 + basisX * i / (numberOfHorizontalRaycasts - 1) + basisY * j / (numberOfVerticalRaycasts - 1) - basisZ;

            }
        }











    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.DrawSphere(P000, 0.1f);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(P001, 0.1f);

        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(P100, 0.1f);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(P101, 0.1f);

        //Gizmos.color = Color.magenta;
        //Gizmos.DrawSphere(testPoint, 0.1f);



        //draw sphere for every point in raycastPoints
        if (raycastPoints != null)
        {
            //Draw the first sphere
            //Gizmos.DrawSphere(raycastPoints[0, 0], 0.1f);
            for (int i = 0; i < numberOfHorizontalRaycasts; i++)
            {
                for (int j = 0; j < numberOfVerticalRaycasts; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        //change color to green if the correspoinding success is true
                        if (raycastPointsSuccess[i, j, k] == true)
                        {
                            Gizmos.color = Color.green;
                        }
                        else if (raycastPointsSuccess[i, j, k] == null)
                        {
                            Gizmos.color = Color.yellow;
                        }
                        else
                        {
                            Gizmos.color = Color.red;
                        }
                        Gizmos.DrawSphere(raycastPoints[i, j, k], 0.1f);
                    }


                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check which side of the object the player is on
        Vector3 portalToPlayer = player.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
        playerIsInFrontOfPortal = dotProduct > 0;

        //make every value in raycastPointsSuccess null
        for (int i = 0; i < numberOfHorizontalRaycasts; i++)
        {
            for (int j = 0; j < numberOfVerticalRaycasts; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    raycastPointsSuccess[i, j, k] = null;
                }
            }
        }

        //isVisible = portalRenderer.isVisible && RaycastVisibility();
        isVisible = portalRenderer.isVisible;

    }

    bool RaycastVisibility()
    {

        //loop through each point in raycastPoints again, and check their location on the screen. if they are on the screen, then initialize a raycast.

        //bool isVisible = false;
        for (int i = 0; i < numberOfHorizontalRaycasts; i++)
        {
            for (int j = 0; j < numberOfVerticalRaycasts; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    raycastPointsSuccess[i, j, k] = false;
                    //eliminate raycasts currently not on screen using WorldToScreenPoint
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(raycastPoints[i, j, k]);

                    //if the point is on the screen, then raycast
                    //check if x, y or z are negative, or if x or y are greater than screen.height and width
                    if (screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height && screenPoint.z > 0)
                    {
                        //raycast from the point raycastPoints[i, j] to the camera

                        //get direction from the point to the camera
                        Vector3 direction = Camera.main.transform.position - raycastPoints[i, j, k];


                        if (Physics.Raycast(raycastPoints[i, j, k] + Vector3.Normalize(direction) * 0.001f, direction, out RaycastHit hit, 100))
                        {
                            //if the raycast hits the player, then return true
                            if (hit.collider.gameObject == player)
                            {
                                Debug.DrawRay(raycastPoints[i, j, k], direction * hit.distance, Color.yellow);
                                raycastPointsSuccess[i, j, k] = true;
                                //isVisible = true;
                                return true;
                            }

                        }
                    }
                }

            }
        }


        //return isVisible;
        //if any of the remaining raycasts hit the camera uninterrupted, then return true. else return false



        return false;



    }


}
