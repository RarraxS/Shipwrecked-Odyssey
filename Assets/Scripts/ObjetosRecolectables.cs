using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour
{
    [SerializeField] private string herramientaNecesaria;
    private bool hayObjeto;

    private void Start()
    {
        
    }
    
    private void Update()
    {
        if (this.gameObject.activeSelf)
            hayObjeto = true;

        else
            hayObjeto= false;
    }

    public void GolpearHerramienta() 
    {
        if (herramientaNecesaria == Toolbar.Instance.herramientaSeleccionada.item.herramienta || 
            herramientaNecesaria == "")
        {
            Debug.Log("Aqui estoy pegando con una HERRAMIENTA");
        }
    }

    public void GolpearMano()
    {
        Debug.Log("Aqui estoy pegando con la MANO");
    }
}
