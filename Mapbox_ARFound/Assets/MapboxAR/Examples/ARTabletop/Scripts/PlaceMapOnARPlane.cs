using UnityEngine;
using UnityARInterface;
using UnityEngine.XR.ARFoundation;

public class PlaceMapOnARPlane : MonoBehaviour
{

	[SerializeField]
	private Transform _mapTransform;
	[SerializeField] ARPlaneManager arPlaneManager;

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
		_mapTransform.position = new Vector3(pos.x, pos.y + 0.005f, pos.z);

		if (_mapTransform.name.Contains("Globe"))
		{
			var direction = plane.center - Camera.main.transform.position;
			direction = direction.normalized;
			var inFrontPos = direction * 2.5f;
			_mapTransform.position = new Vector3(inFrontPos.x, pos.y + 0.6f, inFrontPos.z);
		}

		//arPlaneManager.enabled = false;
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
