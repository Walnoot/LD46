using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelathBar : MonoBehaviour
{

	private RadioTower tower;
	// Start is called before the first frame update
	void Start()
    {
		tower = FindObjectOfType<RadioTower>();
	}

    // Update is called once per frame
    void Update()
    {
		Vector3 scale = new Vector3(0.422f * tower.health * 0.01f, 0.32428f, 1);
		this.transform.localScale = scale;
	}
}
