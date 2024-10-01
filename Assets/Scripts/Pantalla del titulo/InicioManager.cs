using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicioManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void LinkButton (string link)
    {
        Application.OpenURL(link);
    }
}
