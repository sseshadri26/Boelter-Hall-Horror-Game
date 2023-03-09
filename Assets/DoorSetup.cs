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
        Door1,
        Door2,
        Door2b
    };

    [SerializeField]
    private DoorNumber doorNumber;


    private void Start()
    {
        // Find all the TextMeshPro components in the children of this object and store them in the _texts array.
        _texts = GetComponentsInChildren<TextMeshPro>();

        // Set the initial door to be displayed.
    }

    private void Update()
    {
        if (_texts.Length == 0)
        {
            _texts = GetComponentsInChildren<TextMeshPro>();
        }
        // Update the text of all the TextMeshPro components with the value of _num.
        foreach (TextMeshPro text in _texts)
        {
            text.text = _num;
        }

        // deactivate all the gameobjects in _doors execept the one with index of _selectedDoorIndex
        if (_doors==null)
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

    }   
}