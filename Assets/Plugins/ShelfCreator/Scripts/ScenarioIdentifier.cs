using UnityEngine;

public class ScenarioIdentifier : MonoBehaviour
{
	public int ourScenario;
	public GameObject ourGameObject;

	void Start()
	{
		ourGameObject = gameObject;
	}
}
