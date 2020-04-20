using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
	public CameraCut cut;
	public Transform playingCameraTarget;
	public Transform deadTarget;
	public GameObject menu;
	public GameObject ui;
	public DayNightCycle dayNight;
	public GameObject fog;
	public GameObject menuMusic;
	public GameObject gameMusic;
	public GameObject towerCool;
	public GameObject towerBroken;
	private RadioTower tower;
	public CameraFollow follow;
	public CameraCut deadCut;
	public GameObject gameOver;

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
		tower = FindObjectOfType<RadioTower>();
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
					dayNight.enabled = true;
					fog.SetActive(true);
					state = State.playing;
					menuMusic.SetActive(false);
					gameMusic.SetActive(true);
				}
				break;
			}
			case (State.playing):
			{
				if (tower.health < 10)
				{
					towerCool.SetActive(false);
					towerBroken.SetActive(true);
				}

				if (tower.health == 0)
				{ 
					state = State.dead;
					ui.SetActive(false);
					dayNight.currentTimeOfDay = 0;
					dayNight.enabled = false;
					fog.SetActive(false);
					follow.enabled = false;
					deadCut.enabled = true;
					cut.target = deadTarget;
					gameOver.SetActive(true);
				}
				break;
			}
			case (State.dead):
			{
				if (Input.GetKeyDown("escape"))
				{
					Application.Quit();
				}

				if (Input.GetKeyDown("space"))
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
				break;
			}
		}	
    }
}
