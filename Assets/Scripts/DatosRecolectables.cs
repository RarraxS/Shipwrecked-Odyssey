using UnityEngine;


[CreateAssetMenu (menuName = "Datos/Datos objeto decolectable")]
public class DatosRecolectables : ScriptableObject
{
    public Sprite sprite;
    public string herramienta;
    public int nivelMinimoDeLaHerramienta;
    public int puntosDeVida;

    public GameObject[] objetoRecolectable;
    public int[] cantidadBase;
    public int[] cantidadPrimerAumento;
    public int[] cantidadSegundoAumento;
    public int[] probabilidadDelPrimerAumento;
    public int[] probabilidadDelSegundoAumento;
}
