using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DescripcionesController : MonoBehaviour
{
    public GameObject panelDescripciones;
    public TMP_Text textNombre, textDescripciones;
    private RectTransform descripcionesTransform;
    [SerializeField] private Vector3 distancia;

    private void Awake()
    {
        descripcionesTransform = panelDescripciones.GetComponent<RectTransform>();
        textDescripciones = textDescripciones.GetComponent<TMP_Text>();
        textNombre = textNombre.GetComponent<TMP_Text>();
    }
    void Update()
    {
        //Este script lo posee el objeto del inventario para que se puedan actualizar 
        //los valores cuando este desactivado el objeto de las descripciones. 
        //Se encarga de que cuando el cursor está sobre el objeto "inventario"
        //el objeto de las descripciones actualiza su posición, y cuando está fuera de 
        //este objeto "inventario" las descripciones se desactivan. La activacion la 
        //realizan los propios botones del inventario.

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            panelDescripciones.SetActive(false);
        }

        if (panelDescripciones.activeInHierarchy == true)
        {
            descripcionesTransform.position = Input.mousePosition + distancia;
        }
    }
}