using UnityEngine;
using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

namespace UnityARInterface
{
	public class ARPlaneHandler : MonoBehaviour
	{
		public static Action resetARPlane;
		public static Action<ARPlane> returnARPlane;

		ARPlaneManager _arPlaneManager;

		private TrackableId _planeId;
		private ARPlane _cachedARPlane;

		void OnEnable()
		{
			//debug.text += "\n***** On Enable()";
			_arPlaneManager = GetComponent<ARPlaneManager>();
			_arPlaneManager.planesChanged += UpdateARPlane;
		}


		void UpdateARPlane(ARPlanesChangedEventArgs args)
		{
			ARPlane arPlane;

			for(int i=0; i < args.added.Count; ++i)
			{
				arPlane = args.added[i];
				if (_planeId == null)
					_planeId = arPlane.trackableId;

				if (arPlane.trackableId == _planeId)
					_cachedARPlane = args.added[i];
			}
			{
				
			}
			returnARPlane(_cachedARPlane);
		}
	}
}
