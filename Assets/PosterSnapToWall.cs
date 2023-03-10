using UnityEngine;

//#if UNITY_EDITOR
using UnityEditor;
//#endif

[ExecuteInEditMode]
public class PosterSnapToWall : MonoBehaviour
{
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private bool overlapAllowed = false;
    [SerializeField] private int raycastRows = 3;
    [SerializeField] private int raycastColumns = 3;
    [SerializeField] private float distanceFromWall = 0.01f;

    [SerializeField]
    FilePicker fp;

    private Vector3 oldPos;

    private void Awake()
    {
        // Get all colliders attached to this poster (including children)
        oldPos = Vector3.up;
    }

    private void Update()
    {
        //return;
        ////#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            return;
        }
        
        //if (oldPos != gameObject.transform.position)
        //{
        //    gameObject.transform.position = gameObject.transform.position - gameObject.transform.forward * 0.1f;
        //}
        //oldPos = gameObject.transform.position;
        //return;

        // Only perform raycast in edit mode
        Vector3 center = transform.position;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        // Calculate the size of the grid based on the number of rows, columns, and spacing
        float gridWidth = fp.quadWidth;
        float gridHeight = fp.quadHeight;
        Vector3 topLeft = center - (right * (gridWidth / 2f)) + (up * (gridHeight / 2f));

        
        float closestDistance = float.PositiveInfinity;
        Vector3 normal = Vector3.forward;
        
        // Loop over each position in the grid and create a raycast
        for (int x = 0; x < raycastColumns; x++)
        {
            for (int y = 0; y < raycastRows; y++)
            {
                Vector3 position = topLeft + (right * (x * gridWidth/(raycastColumns-1))) - (up * (y * gridHeight/(raycastRows-1))) + transform.forward* 0.01f;
                Ray ray = new Ray(position, forward);

                // visualize the raycast
                Debug.DrawRay(position, forward);

                if (Physics.Raycast(ray, out RaycastHit hit, snapDistance))
                {
                    //Debug.Log(hit.distance);
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        normal = hit.normal;
                        
                    }
                    // Snap to the wall if hit
                }
            }
        }
        if (closestDistance <= snapDistance)
            SnapToWall(closestDistance, normal);
    }

    private void SnapToWall(float distance, Vector3 normal)
    {
        transform.SetPositionAndRotation(transform.position + Vector3.forward * (distance - distanceFromWall), Quaternion.FromToRotation(-Vector3.forward, normal));
    }
}
