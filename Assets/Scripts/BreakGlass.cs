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

        if (Globals.GlassBroke)
        {
            baseCollider.enabled = false;
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || !sprint.atFullSpeed)
        {
            return;
        }

        Globals.GlassBroke = true;
        window.breakWindow();
        baseCollider.enabled = false;
        Destroy(gameObject);
    }
}
