using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmearEffect : MonoBehaviour
{
	Queue<Vector3> _recentPositions = new Queue<Vector3>();

	[SerializeField]
	float _timeLag = 0;
	[SerializeField] 
	float _minMagnitude = 1;
	private float _timer;

	Material _smearMat = null;
	public Material smearMat
	{
		get
		{
			if (!_smearMat)
				_smearMat = GetComponent<Renderer>().material;
			

			if (!_smearMat.HasProperty("Vector3_LastPos"))
				_smearMat.shader = Shader.Find("Shader Graphs/SmearShader");

			return _smearMat;
		}
	}

	void LateUpdate()
	{
		_timer += Time.deltaTime;
		if (_recentPositions.Count > 0)
		{
			if (_timer > _timeLag)
			{
				var previousPos = _recentPositions.Dequeue();
				var sqrMagnitude = Vector3.SqrMagnitude(transform.position - previousPos);
				smearMat.SetVector("Vector3_LastPos", sqrMagnitude > _minMagnitude ? previousPos : transform.position);
			}
		}				

		smearMat.SetVector("Vector3_CurrentPos", transform.position);
		_recentPositions.Enqueue(transform.position);
	}
}
