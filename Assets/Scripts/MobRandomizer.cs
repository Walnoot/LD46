using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRandomizer : MonoBehaviour
{
	public GameObject hair1;
	public GameObject hair2;
	public GameObject hair3;
	public GameObject hair4;
	public GameObject character;

	// Start is called before the first frame update
	void Start()
    {
		Debug.Log("Going to randomize a mob");
		List<GameObject> hair = new List<GameObject>() {
			hair1, hair2, hair3, hair4
		};

		List<Color> hairColors = new List<Color> {
			Color.red,
			Color.gray,
			Color.black
		};

		List<Color> shirtColors = new List<Color>
		{
			Color.blue,
			Color.green,
			Color.red,
			Color.yellow
		};

		GameObject activeHair = hair[UnityEngine.Random.Range(0, hair.Count)];
		activeHair.SetActive(true);
		activeHair.GetComponent<MeshRenderer>().material.color = hairColors[UnityEngine.Random.Range(0, hairColors.Count)];

		character.GetComponent<SkinnedMeshRenderer>().materials[0].color = shirtColors[UnityEngine.Random.Range(0, shirtColors.Count)];

	}
}
