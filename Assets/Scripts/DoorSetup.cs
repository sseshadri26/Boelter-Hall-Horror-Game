using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class DoorSetup : MonoBehaviour
{
    // Array to hold all the TextMeshPro components found in the children of this object.
    private TextMeshPro[] _texts;
    [SerializeField] private GameObject[] _doors;
    [SerializeField] private string _num;


    enum DoorNumber // your custom enumeration
    {
        Door1_RoundHandle,
        Door2_RoundHandle_Chalkboard,
        Door2b_ComplexHandleChalkboard,
        Glass_Door,
        Double_Door_No_Frame,
        Double_Door_Frame,
        Giant_Door,
        Small_Big_Door,
        Door1_Backface_Culled,

        Stair_Door
    };

    [SerializeField]
    private DoorNumber doorNumber;


    private void Start()
    {
        // Find all the TextMeshPro components in the children of this object and store them in the _texts array.
        _texts = GetComponentsInChildren<TextMeshPro>();
    }

    private void Update()
    {
        _texts = GetComponentsInChildren<TextMeshPro>();

        // Update the text of all the TextMeshPro components with the value of _num.
        foreach (TextMeshPro text in _texts)
        {
            text.text = _num;
        }

        // deactivate all the gameobjects in _doors execept the one with index of _selectedDoorIndex
        if (_doors == null)
        {
            return;
        }
        for (int i = 0; i < _doors.Length; i++)
        {
            if (i == (int)doorNumber)
            {
                _doors[i].SetActive(true);
            }
            else
            {
                _doors[i].SetActive(false);
            }
        }

        // Disable this script so it's not calling Update every frame
        if (Application.isPlaying)
            this.enabled = false;
    }
}