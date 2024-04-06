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
