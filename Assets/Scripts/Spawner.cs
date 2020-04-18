using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
		
	float SPACING_ROW = 2.0F;	
	float SPACING_COLUMN = 2.0F;	

	void Start()
	{

	}

    // Update is called once per frame
	void Update()
	{

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
				Vector3 pos = startPos + new Vector3(r * SPACING_ROW, 0f, c * SPACING_COLUMN);
				Instantiate(template, pos, Quaternion.identity);
				spawned ++;
			}
		}

		return spawned;
	}
}
