using TMPro;
using UnityEngine;

public class HudHora : MonoBehaviour
{
    [SerializeField] TMP_Text textHora, textDia;

    
    [SerializeField] int dia, hora, minutos;
    [SerializeField] float temporizadorPasoDelTiempo;


    string estacion;
    
    int opcEstacion = 1;
    


    [SerializeField] private int numEstacion = 1;
    public static Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        //Programar las animaciones
        //Programar el actualizar el dia
        //Programar el actualizar la hora
        //Programar el actualizar el HUD
    }

    //void Animaciones()
    //{
    //    //Actualiza el animator de las estaciones con el sprite de la estación correspondiente 
    //    animator.SetInteger("numEstacion", numEstacion);

    //    //Cuando se llega al día 31 se cambia a la siguiente estación
    //    if (dia > 30)
    //    {
    //        numEstacion++;
    //    }

    //    if (numEstacion == 5)
    //    {
    //        numEstacion = 1;
    //    }
    //}

    //void ActualizarHora()
    //{
    //    textHora.text = hora.ToString("0") + ":" + minutos.ToString("00");
    //    //Paso del tiempo
    //    if (GameManager.Instance.menuAbierto == false)
    //        temporizadorTiempo -= Time.deltaTime;

    //    if (temporizadorTiempo <= 0)
    //    {
    //        temporizadorTiempo = 7f;
    //        minutos += 10;
    //    }

    //    if (minutos >= 60)
    //    {
    //        minutos -= 60;
    //        hora++;
    //    }

    //    if (hora >= 24)
    //    {
    //        hora -= 24;
    //        ObserverManager.Instance.NotifyObserver("dia completado");
    //    }
    //}

    //void ActualizarDia()
    //{
    //    textDia.text = dia.ToString("0") + ". " + estacion;

    //    if (dia > 30)
    //    {
    //        dia = 1;
    //        opcEstacion++;
    //    }

    //    if (dia >= 30 && estacion == "Inv.")
    //        opcEstacion = 1;

    //    switch (opcEstacion)
    //    {
    //        case 1:
    //            estacion = "Prim.";
    //            break;

    //        case 2:
    //            estacion = "Ver.";
    //            break;

    //        case 3:
    //            estacion = "Oto.";
    //            break;

    //        case 4:
    //            estacion = "Inv.";
    //            break;
    //    }
    //}
}