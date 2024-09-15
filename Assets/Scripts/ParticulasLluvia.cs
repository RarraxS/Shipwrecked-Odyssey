using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParticulasLluvia : MonoBehaviour
{
    private int screenWidth, screenHeight;

    [SerializeField] private GameObject lluvia;
    private ParticleSystem parSysLluvia;

    [SerializeField] private Vector2 offsetMultiplier, scaleMultiplier;

    void Start()
    {
        parSysLluvia = lluvia.GetComponent<ParticleSystem>();

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
        var shape = parSysLluvia.shape;
        
        shape.position = new Vector3(screenWidth * offsetMultiplier.x, screenHeight * offsetMultiplier.y, shape.position.z);

        if (screenWidth != Screen.width)
        {
            shape.scale = new Vector3(screenWidth * scaleMultiplier.x, shape.scale.y, shape.scale.z);
        }

        else
        {
            shape.scale = new Vector3(1f / (screenHeight * scaleMultiplier.y), shape.scale.y, shape.scale.z);
        }
    }
}
