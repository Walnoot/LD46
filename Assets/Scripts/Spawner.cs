using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
		
	float SPACING_ROW = 5.0F;	
	float SPACING_COLUMN = 5.0F;	
	Queue<GameObject> queueTemplate = new Queue<GameObject>();
	Queue<Vector3> queuePos = new Queue<Vector3>();
	private float timeout;

	void Start()
	{

	}

    // Update is called once per frame
	void Update()
	{
		timeout -= Time.deltaTime;
		if(timeout <= 0 && queueTemplate.Count != 0){
			var template = queueTemplate.Dequeue();
			var pos = queuePos.Dequeue();
			Instantiate(template, pos, Quaternion.identity);
			timeout = 0.20f;
		}
	}

	public int spawnRectangle(int rows, int columns, int max, GameObject template) {
		Vector3 startPos = transform.position;
		int spawned = 0;
		for (int r = 0; r < rows; r++) 
		{
			for (int c = 0; c < columns; c++)
			{
				if(spawned >= max){
					break;
				}
				Vector3 pos = startPos + new Vector3(r * SPACING_ROW, 1f, c * SPACING_COLUMN);
				queueTemplate.Enqueue(template);
				queuePos.Enqueue(pos);
				spawned ++;
			}
		}

		return spawned;
	}
}
