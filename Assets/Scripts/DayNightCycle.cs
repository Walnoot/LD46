using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
	public Light sun;
	public float secondsPerDay = 60f;
	public float timeStep = 1f;
	public float currentTimeOfDay = 0f;
	public float transitionTime = 0.05f;
	private float sunInitialIntensity;

    // Start is called before the first frame update
    void Start()
    {
		sunInitialIntensity = sun.intensity;
    }

    // Update is called once per frame
    void Update()
    {
		UpdateSun();
		currentTimeOfDay += (Time.deltaTime / secondsPerDay) * timeStep;

		if (currentTimeOfDay >= 1)
		{
			currentTimeOfDay = 0;
		}
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
			intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - (0.25f - transitionTime)) * (1 / transitionTime));
		}
		else if (currentTimeOfDay >= 0.73f)
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / transitionTime)));
		}
		sun.intensity = sunInitialIntensity * intensityMultiplier;
	}
}
