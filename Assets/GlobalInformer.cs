using UnityEngine;

public class GlobalInformer : MonoBehaviour
{
    public static GlobalInformer instance;  

    private void Awake()                    //Реализован паттерн синглтон. Проверяем, если 
                                            //информер уже есть на сцене, то грохаем.
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public void PrintMessageToDebug(string message)
    {
        Debug.Log(message);
    }
}
