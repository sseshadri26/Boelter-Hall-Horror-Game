using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    private SprintAccelerationTuning sprint;
    private BreakableWindow window;
    
    public BoxCollider baseCollider;

    private void Start()
    {
        sprint = FindObjectOfType<SprintAccelerationTuning>();
        window = transform.parent.GetComponent<BreakableWindow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || !sprint.atFullSpeed)
        {
            return;
        }

        window.breakWindow();
        baseCollider.enabled = false;
        Destroy(gameObject);
    }
}
