using UnityEngine;

namespace Core.PlayerScripts
{
	public class CameraShake : MonoBehaviour
	{
		public static CameraShake Instance;
	
		[SerializeField] private Transform camTransform;
		[SerializeField] private float shakeDuration;
		[SerializeField] private float shakeAmount = 0.7f;
		[SerializeField] private float decreaseFactor = 1.0f;

		private Vector3 originalPos;
	
		void Awake()
		{
			Instance = this;
			if (camTransform == null)
			{
				camTransform = transform;
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
				camTransform.localPosition = originalPos + Random.insideUnitSphere * Mathf.CeilToInt(Time.deltaTime) * shakeAmount;
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
