using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STPlacedObject : MonoBehaviour {

    public string PlacedObjectName { get; set; }

    void OnDestroy()
    {
        if (!String.IsNullOrEmpty(PlacedObjectName))
            SpatialUnderstandingDllObjectPlacement.Solver_RemoveObject(PlacedObjectName);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
