using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolkitExample : MonoBehaviour {

    public SpatialToolkit SpatialToolkit;
    public GameObject TestPrefab;


	void Start () {
        SpatialToolkit = FindObjectOfType<SpatialToolkit>();
        SpatialToolkit.StartScanning();
	}
	
    public void OnScanComplete()
    {
        STTopologyQuery query = new STTopologyQuery(STTopologyQueryType.PositionsOnFloor) { Floor_MinLength = 0.25f, MinWidth = 0.25f };
        Debug.Log("Query Result: " + query.RunQuery() + " positions");

        foreach(SpatialUnderstandingDllTopology.TopologyResult result in query.Results)
        {
            GameObject newObj = Instantiate(TestPrefab, result.position, Quaternion.LookRotation(result.normal, Vector3.up));
            newObj.transform.localScale = new Vector3(result.width * 0.8f, (result.length + result.width) / 2f, result.length * 0.8f);
        }
    }
}
