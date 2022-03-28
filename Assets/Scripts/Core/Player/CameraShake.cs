using UnityEngine;

namespace Core.Player
{
	public class CameraShake : MonoBehaviour
	{
		public static CameraShake Instance;
	
		[SerializeField] private Transform camTransform;
		[SerializeField] private float shakeDuration = 0f;
		[SerializeField] private float shakeAmount = 0.7f;
		[SerializeField] private float decreaseFactor = 1.0f;

		private Vector3 originalPos;
	
		void Awake()
		{
			Instance = this;
			if (camTransform == null)
			{
				camTransform = GetComponent(typeof(Transform)) as Transform;
			}
		}
		public static void Shake(float time = 0.1f)
		{
			Instance.shakeDuration = time;
		}
		void OnEnable()
		{
			originalPos = camTransform.localPosition;
		}

		void Update()
		{
			if (shakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;
				camTransform.localPosition = originalPos;
			}
		}
	}
}
