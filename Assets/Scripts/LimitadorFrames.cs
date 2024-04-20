using UnityEngine;

public class LimitadorFrames : MonoBehaviour
{
    private void Start()
    {
        //Los frames máximos a los que puede llegar a correr el juego
        Application.targetFrameRate = 60;
    }
}
