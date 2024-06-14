using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosRecolectables : MonoBehaviour
{
    private bool hayObjeto;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (this.gameObject.activeSelf)
            hayObjeto = true;

        else
            hayObjeto= false;
    }
}
