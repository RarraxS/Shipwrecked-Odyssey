using TMPro;
using System.Collections;
using UnityEngine;

public class DescripcionesController : MonoBehaviour
{
    public GameObject panelDescripciones;
    public TMP_Text textDescripciones;
    public RectTransform descripcionesTransform;
    [SerializeField] private Vector3 distancia;

    private static DescripcionesController instance;
    public static DescripcionesController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        descripcionesTransform = panelDescripciones.GetComponent<RectTransform>();
        textDescripciones = textDescripciones.GetComponent<TMP_Text>();
    }


    void Start()
    {
        
    }

    void Update()
    {
        if (panelDescripciones.activeInHierarchy == true)
        {
            descripcionesTransform.position = Input.mousePosition + distancia;
        }
    }
}
