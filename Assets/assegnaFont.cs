using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class assegnaFont : MonoBehaviour
{
    public Font font;
    public bool assegna;
    
    void Update()
    {
        if (assegna)
        {
            Text[] testi = Resources.FindObjectsOfTypeAll<Text>();
            foreach (Text t in testi)
                t.font = font;
            /*TMP_Text[] tmp = Resources.FindObjectsOfTypeAll<TMP_Text>();
            foreach (TMP_Text t in tmp)
                t.font = font;*/
            assegna = false;
        }
    }
}
