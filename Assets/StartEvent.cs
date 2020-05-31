using UnityEngine;

public class StartEvent : MonoBehaviour
{
    void Start()
    {
        GlobalInformer.instance.PrintMessageToDebug("StartEvent!");
    }
}
