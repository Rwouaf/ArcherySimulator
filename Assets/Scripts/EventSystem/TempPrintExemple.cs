using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPrintExemple : MonoBehaviour
{
    public void MyExempleFun(Component sender, object data)
    {
        Debug.Log("Youpi I received the event");
    }
}
