using UnityEngine;
using UnityARInterface;
using UnityEngine.XR.ARFoundation;

public class PlaceMapOnARPlane : MonoBehaviour
{

	[SerializeField]
	private Transform _mapTransform;


	void Start()
	{
		ARPlaneHandler.returnARPlane += PlaceMap;
		ARPlaneHandler.resetARPlane += ResetPlane;
	}

	void PlaceMap(ARPlane plane)
	{
		if (!_mapTransform.gameObject.activeSelf)
		{
			_mapTransform.gameObject.SetActive(true);
		}
		var pos = plane.center;
		_mapTransform.position = new Vector3(pos.x, pos.y + 0.01f, pos.z);
	}

	void ResetPlane()
	{
		_mapTransform.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		ARPlaneHandler.returnARPlane -= PlaceMap;
	}
}
