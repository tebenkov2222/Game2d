using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSprite : MonoBehaviour
{
    public Image Load;
    public void SetLoad(float proc)
    {
        Load.fillAmount = proc / 1000;
    }

}
