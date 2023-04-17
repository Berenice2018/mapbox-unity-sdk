namespace Mapbox.Examples
{
	using UnityEngine;
	using UnityEngine.XR.ARFoundation;

	public class UpdateMapPosByARPlaneY : MonoBehaviour
	{
		[SerializeField]
		Transform _mapRoot;
		ARPlaneManager _arPlaneManager;


		void Start()
		{
			_arPlaneManager.planesChanged += UpdateMapPosOnY;
		}

		void UpdateMapPosOnY(ARPlanesChangedEventArgs args)
		{
			ARPlane plane = args.updated[0];
			var pos = _mapRoot.position;
			_mapRoot.position = new Vector3(pos.x, plane.center.y + 0.01f, pos.z);
		}
	}
}
