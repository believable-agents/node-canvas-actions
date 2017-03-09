using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Sense))]
public class LookAtPro : MonoBehaviour {

	public float lookSmoother = 3f;

	Sense _personalSpace;
	Animator _animator;
	Transform _transform;

	float lookWeight;

	// Use this for initialization
	void Start () {
		_personalSpace = GetComponentsInChildren<Sense> ().FirstOrDefault (w => w.SenseType == SenseType.Vision);

		if (_personalSpace == null) {
			Debug.LogWarning(string.Format("Agent '{0}' cannot do 'LookAtPro' as it has no 'Vision' sense.", gameObject.name));
		}

		_animator =  GetComponent<Animator> ();
		_transform = transform;
	}


	void OnAnimatorIK(int layer)
	{
		if (_personalSpace == null) {
			return;
		}

		if (_personalSpace.PlayerDetected != null || _personalSpace.OtherDetected != null) {
			Vector3 vect;

			if (_personalSpace.PlayerDetected != null) {
				vect = _personalSpace.PlayerDetected.position;
				vect.y += 1.5f;
			} else {  
				vect = _personalSpace.OtherDetected.position;
			}

			_animator.SetLookAtPosition (vect);
			_animator.SetLookAtWeight (lookWeight);

			lookWeight = Mathf.Lerp(lookWeight, 1f, Time.deltaTime * lookSmoother);
		} else {
			if (_animator == null || _transform == null) return;

			_animator.SetLookAtPosition(_transform.position + _transform.forward);
			lookWeight = Mathf.Lerp(lookWeight, 0f, Time.deltaTime * lookSmoother);
		}

	}
}
