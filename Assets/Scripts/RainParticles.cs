using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RainParticles : MonoBehaviour
{
    private int screenWidth, screenHeight;

    [SerializeField] private GameObject rainObject;
    private ParticleSystem rainParSys;

    [SerializeField] private Vector2 offsetMultiplier, scaleMultiplier;

    [SerializeField] private GameObject camera;
    private Camera cam;

    void Start()
    {
        rainParSys = rainObject.GetComponent<ParticleSystem>();

        cam = camera.GetComponent<Camera>();

        UpdateResolution();

        OnResolutionChange();
    }

    void Update()
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            OnResolutionChange();

            UpdateResolution();
        }
    }

    private void UpdateResolution()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void OnResolutionChange() 
    {
        float worldWidth, worldHeight;

        worldHeight = cam.orthographicSize * 2f;
        worldWidth = worldHeight * cam.aspect;

        float finalMeasurement = worldWidth + (worldHeight * 2);

        var shape = rainParSys.shape;

        shape.position = new Vector3(offsetMultiplier.x, offsetMultiplier.y, shape.position.z);
        shape.scale = new Vector3(finalMeasurement, shape.scale.y, shape.scale.z);

        var emision = rainParSys.emission;

        emision.rateOverTime = shape.scale.x / 2;
    }
}