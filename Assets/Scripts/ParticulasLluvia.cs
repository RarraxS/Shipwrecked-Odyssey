using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParticulasLluvia : MonoBehaviour
{
    private int screenWidth, screenHeight;

    private ParticleSystem parSysLluvia;

    [SerializeField] private Vector2 offsetMultiplier, scaleMultiplier;

    [SerializeField] private GameObject camera;
    private Camera cam;

    void Start()
    {
        parSysLluvia = GetComponent<ParticleSystem>();

        cam = camera.GetComponent<Camera>();

        ActualizarResolucion();

        OnResolutionChange();
    }

    void Update()
    {
        if (screenHeight != Screen.height || screenWidth != Screen.width)
        {
            OnResolutionChange();

            ActualizarResolucion();
        }
    }

    private void ActualizarResolucion()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void OnResolutionChange() 
    {
        float anchoMundo, alturaMundo;

        var shape = parSysLluvia.shape;


        alturaMundo = cam.orthographicSize * 2f;
        anchoMundo = alturaMundo * cam.aspect;

        float medidasFinales = anchoMundo + (alturaMundo * 2);

        shape.position = new Vector3(offsetMultiplier.x, offsetMultiplier.y, shape.position.z);
        shape.scale = new Vector3(medidasFinales, shape.scale.y, shape.scale.z);
    }
}
