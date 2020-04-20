using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
	public CameraCut cut;
	public Transform playingCameraTarget;
	public GameObject menu;
	public GameObject ui;
	public DayNightCycle dayNight;

	enum State {
		menu,
		playing,
		dead
	}
	State state;

    // Start is called before the first frame update
    void Start()
    {
		state = State.menu;
    }

    // Update is called once per frame
    void Update()
    {
		switch (state)
		{
			case (State.menu):
			{
				if (Input.anyKey)
				{
					menu.SetActive(false);
					ui.SetActive(true);
					cut.target = playingCameraTarget;
					dayNight.active = true;
					dayNight.doSpawn();
					state = State.playing;
				}
				break;
			}
		}	
    }
}
