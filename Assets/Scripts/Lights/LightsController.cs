using UnityEngine;

public class LightsController : MonoBehaviour
{
    [SerializeField] private TimeSheet timeSheet;
    private LightsModel lightsModel;

    [SerializeField] private ScriptableEventSingleParameterFloat timeBetweenMinutesUpdateEvent;
    [SerializeField] private ScriptableEventSingleParameterBool askForTimeContainer;

    //Yo llamo a un tio para qyue me llame con otra info de vuelta

    private float timeBetweenMinuteChange;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
