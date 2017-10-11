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
        STTopologyQuery query = new STTopologyQuery(STTopologyQueryType.PositionsOnFloor) { Floor_MinLength = 0.5f, MinWidth = 0.5f };
        Debug.Log("Query Result: " + query.RunQuery() + " positions");

        foreach(SpatialUnderstandingDllTopology.TopologyResult result in query.Results)
        {
            GameObject newObj = Instantiate(TestPrefab, result.position, Quaternion.LookRotation(result.normal, Vector3.up));
            newObj.GetComponent<MeshRenderer>().material.color = Color.red;
            newObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        STShapeQuery shapeQuery = new STShapeQuery(STShapeQueryType.ShapeBounds, "Square");
        Debug.Log("Shape Query Result: " + shapeQuery.RunQuery() + " positions");

        foreach(SpatialUnderstandingDllShapes.ShapeResult result in shapeQuery.Results)
        {
            GameObject newObj = Instantiate(TestPrefab, result.position, Quaternion.identity);
            newObj.GetComponent<MeshRenderer>().material.color = Color.yellow;
            newObj.transform.localScale = new Vector3(result.halfDims.x, result.halfDims.y, (result.halfDims.x + result.halfDims.y) / 2);
        }
    }
}
