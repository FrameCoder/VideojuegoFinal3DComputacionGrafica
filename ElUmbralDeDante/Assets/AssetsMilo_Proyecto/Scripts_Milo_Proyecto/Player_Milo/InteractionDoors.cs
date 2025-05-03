using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDoors : MonoBehaviour, IInteractuable
{
    public void ActivarObjeto()
    {
        Destroy(gameObject);
    }
}
