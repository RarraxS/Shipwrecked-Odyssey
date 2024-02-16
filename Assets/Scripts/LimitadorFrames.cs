using UnityEngine;

public class LimitadorFrames : MonoBehaviour
{
    private void Start()
    {
        //Los frames máximos a los que puede llegar el juego
        Application.targetFrameRate = 60;
    }
}
