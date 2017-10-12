using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolkitExample : MonoBehaviour, IInputClickHandler
{

    public SpatialToolkit SpatialToolkit;
    public GameObject TestPrefab;

    bool _scanCompleteFired = false;


	void Start () {
        SpatialToolkit = FindObjectOfType<SpatialToolkit>();
        SpatialToolkit.StartScanning();
    }
	
    public void OnScanComplete()
    {
        PlaceObjects();
        InputManager.Instance.PushFallbackInputHandler(gameObject);
        _scanCompleteFired = true;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if(_scanCompleteFired)
        {
            ResetObjects();
            PlaceObjects();
        }
    }

    void ResetObjects()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("TestCube"))
        {
            Destroy(g);
        }
    }

    void PlaceObjects()
    {
        /*STTopologyQuery query = new STTopologyQuery(STTopologyQueryType.PositionsOnFloor) { Floor_MinLength = 0.5f, MinWidth = 0.5f };
        Debug.Log("Query Result: " + query.RunQuery() + " positions");

        foreach (SpatialUnderstandingDllTopology.TopologyResult result in query.Results)
        {
            GameObject newObj = Instantiate(TestPrefab, result.position, Quaternion.LookRotation(result.normal, Vector3.up));
            newObj.GetComponent<MeshRenderer>().material.color = Color.red;
            newObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }*/

        STShapeQuery shapeQuery = new STShapeQuery(STShapeQueryType.ShapeBounds, "Square");
        Debug.Log("Shape Query Result: " + shapeQuery.RunQuery() + " positions");

        foreach (SpatialUnderstandingDllShapes.ShapeResult result in shapeQuery.Results)
        {
            GameObject newObj = Instantiate(TestPrefab, result.position, Quaternion.identity);
            newObj.GetComponent<MeshRenderer>().material.color = Color.yellow;
            newObj.transform.localScale = new Vector3(result.halfDims.x, result.halfDims.y, (result.halfDims.x + result.halfDims.y) / 2);
        }

        STPlacementQuery placementQuery = new STPlacementQuery("ExampleQuery", STPlacementType.Shape) { TargetShape = SpatialToolkit.GetShape("Square"), TargetShapeComponent = SpatialToolkit.GetShape("Square").GetComponent("Main") };
        TestPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        placementQuery.Rules.Add(new STPlacementRule()
        {
            Type = SpatialUnderstandingDllObjectPlacement.ObjectPlacementRule.ObjectPlacementRuleType.Rule_AwayFromOtherObjects,
            MinDistance = 0.5f,
        });
        placementQuery.Place(TestPrefab, new Vector3(0.25f, 0.25f, 0.25f));

        STPlacementQuery floorQuery = new STPlacementQuery("FloorQuery", STPlacementType.FloorAndCeiling);
        floorQuery.Rules.Add(placementQuery.Rules[0]);

        for (int i = 0; i < 10; i++)
        {
            floorQuery.Place(TestPrefab, new Vector3(0.25f, 0.25f, 0.25f), new Vector3(0.25f, 0.25f, 0.25f));
        }
    }
}
