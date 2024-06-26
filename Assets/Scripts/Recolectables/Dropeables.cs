using UnityEngine;

[CreateAssetMenu(menuName ="Datos/Dropeable")]
public class Dropeables : ScriptableObject
{
    public GameObject recolectable;
    public int[] cantidad;
    public int[] probabilidad;
}
