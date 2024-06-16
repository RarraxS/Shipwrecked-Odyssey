using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UIElements;
using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour
{
    [SerializeField] private int puntosDeVida;
    public string herramientaNecesaria;
    [SerializeField] private int nivelMinimoDeHerramienta;


    private void Start()
    {
        
    }
    
    private void Update()
    {
        
    }


    public void Golpear () 
    {
        //GolpearHerramienta();
        //GolpearMano();


        if (Toolbar.Instance.herramientaSeleccionada.item != null)
        {
            if (Toolbar.Instance.herramientaSeleccionada.item.herramienta == herramientaNecesaria || 
                herramientaNecesaria == "" && Toolbar.Instance.herramientaSeleccionada.item.herramienta != "")
            {
                Debug.Log("Herramienta");
            }
        }

        //Si se golpea con la mano u otro ojeto se llama a esta funcion que comprobara si esa
        //herramienta es la adecuada para ese tipo de objeto y en caso de ser asi le resta una
        //cantidad de puntos de vida indicados establecidos en la propia herramienta
        else if (herramientaNecesaria == "")
        {
            Debug.Log("No herramienta");
        }

        else
            return;



        //if ((Toolbar.Instance.herramientaSeleccionada.item == null ||
        //    Toolbar.Instance.herramientaSeleccionada.item.herramienta == "") &&
        //    herramientaNecesaria == "")
        //    GolpearMano();

        //Si se golpea con una herramienta se llama a esta funcion que comprobara si esa herramienta
        //es la adecuada para ese tipo de objeto y en caso de ser asi le resta una cantidad de puntos
        //de vida indicados establecidos en la propia herramienta
        //else if (Toolbar.Instance.herramientaSeleccionada.item.herramienta != "" &&
        //    Toolbar.Instance.herramientaSeleccionada.item != null)
        //    GolpearHerramienta();

        //else
        //    return;
    }

    public void GolpearHerramienta() 
    {
        if ((herramientaNecesaria == Toolbar.Instance.herramientaSeleccionada.item.herramienta
            && Toolbar.Instance.herramientaSeleccionada.item.nivel >= nivelMinimoDeHerramienta) || 
            herramientaNecesaria == "")
        {
            Debug.Log("Aqui estoy pegando con una HERRAMIENTA");
            puntosDeVida -= Toolbar.Instance.herramientaSeleccionada.item.damageHerramienta;

            if (puntosDeVida <= 0)
            {
                OcultarObjeto();
            }
        }
    }

    public void GolpearMano()
    {
        Debug.Log("Aqui estoy pegando con la MANO");
        puntosDeVida -= 1;

        if (puntosDeVida <= 0)
        {
            OcultarObjeto();
        }
    }
    private void OcultarObjeto()
    {
        //Debug.Log(this.name + " tiene " + puntosDeVida + " puntos de vida");
    }
}
