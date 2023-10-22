using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventoColore : MonoBehaviour
{
    [System.Serializable]
    public class Evento : UnityEngine.Events.UnityEvent<Color> { }
    public Evento evento = new Evento();
}