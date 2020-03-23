using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDelete : MonoBehaviour
{
    void DeleteAll()
    {
        Destroy(this);
        Destroy(this.gameObject.GetComponent<Animator>());
    }
}
