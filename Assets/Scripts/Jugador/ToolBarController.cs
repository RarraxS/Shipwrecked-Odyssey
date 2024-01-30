using System;
using UnityEngine;

public class ToolBarController : MonoBehaviour
{
    [SerializeField] private int tamañoToolbar;
    private int herramientaSeleccionada;

    public Action<int> onChange;

    private void Update()
    {
        float delta = Input.mouseScrollDelta.y;
        if(delta != 0)
        {
            if(delta > 0)
            {
                herramientaSeleccionada += 1;
                herramientaSeleccionada = (herramientaSeleccionada >= tamañoToolbar ? 0 : herramientaSeleccionada);
            }
            else
            {
                herramientaSeleccionada -= 1;
                herramientaSeleccionada = (herramientaSeleccionada < 0 ? tamañoToolbar - 1 : herramientaSeleccionada);
            }
            onChange?.Invoke(herramientaSeleccionada);
        }
    }

    internal void Set(int id)
    {
        herramientaSeleccionada = id;
    }
}
