using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
	public Light sun;
	public float secondsPerDay = 60f;
	public float timeStep = 1f;
	public float currentTimeOfDay = 0f;
	public float transitionTime = 0.05f;
	private float sunInitialIntensity;
	public GameObject lights;
	public GameObject spawner;
	private SpawnSystem spawn;
	public GameObject timer;
	private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
		sunInitialIntensity = 1;
		spawn = spawner.GetComponent<SpawnSystem>();
		timerText = timer.GetComponent<Text>();
		spawn.startNewWave();
	}

    // Update is called once per frame
    void Update()
    {
		UpdateSun();
		currentTimeOfDay += (Time.deltaTime / secondsPerDay) * timeStep;

		if (currentTimeOfDay >= 1)
		{
			spawn.startNewWave();
			currentTimeOfDay = 0;
		}

		timerText.text = "Only " + Mathf.Round((24 - (currentTimeOfDay * 24))) + " hours untill new arsonists arrive. Keep your 5G tower alive!";
	}

	void UpdateSun()
	{
		Vector3 v = new Vector3(60, (currentTimeOfDay * 360f) -180f, 0);
		sun.transform.localRotation = Quaternion.Euler(v);
		float intensityMultiplier = 1;
		if (currentTimeOfDay <= (0.25f - transitionTime) || currentTimeOfDay >= (0.73f + transitionTime))
		{
			intensityMultiplier = 0;
		}
		else if (currentTimeOfDay <= 0.25f)
		{
			this.lights.SetActive(false);
			intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - (0.25f - transitionTime)) * (1 / transitionTime));
		}
		else if (currentTimeOfDay >= 0.73f)
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / transitionTime)));
		}
		sun.intensity = sunInitialIntensity * intensityMultiplier;
		if (sunInitialIntensity * intensityMultiplier > 0.25f)
		{
			this.lights.SetActive(false);
		} else
		{
			this.lights.SetActive(true);
		}
	}
}
