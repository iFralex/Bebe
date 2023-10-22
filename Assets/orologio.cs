using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class orologio : MonoBehaviour
{
    public int numeroGiri;
    public RectTransform riquadroOrol, parteDestra, parteSinistra, lancetta, copertura, parteFinale;
    public Text tempo;

    void Start()
    {
        StartCoroutine(Inizia(new List<Color>() { Color.black, new Color(0, .7f, 0, 1), Color.yellow, Color.red }));
    }

    IEnumerator Inizia(List<Color> colori)
    {
        for (int i = 1; i < colori.Count; i++)
        {
            riquadroOrol.GetComponent<Image>().color = colori[i - 1];
            parteDestra.GetComponent<Image>().color = colori[i];
            parteSinistra.GetComponent<Image>().color = colori[i];
            parteFinale.GetComponent<Image>().color = colori[i];
            copertura.GetComponent<Image>().color = colori[i - 1];

            parteDestra.sizeDelta = Vector2.zero;
            parteSinistra.sizeDelta = Vector2.zero;
            copertura.gameObject.SetActive(true);

            float num = 0;
            for (; num < 1;)
            {
                num += .00333333f;
                parteDestra.sizeDelta = Vector2.Lerp(Vector2.zero, new Vector2(riquadroOrol.rect.width / 2, riquadroOrol.rect.width / 2.5f), num);
                lancetta.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, -90, num)));
                string s = "";
                if ((num * 15) < 10)
                    s = "0";
                tempo.text = (i - 1).ToString() + ":" + s + ((int)(num * 15)).ToString();
                yield return new WaitForSeconds(.025f);
            }
            parteDestra.sizeDelta = new Vector2(riquadroOrol.rect.width, riquadroOrol.rect.width / 2f);

            num = 0;
            for (; num < 1;)
            {
                num += .00333333f;
                lancetta.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(-90, -180, num)));
                tempo.text = (i - 1).ToString() + ":" + ((int)(num * 15) + 15).ToString();
                yield return new WaitForSeconds(.025f);
            }
            parteDestra.sizeDelta = Vector2.one * riquadroOrol.rect.width;
            copertura.gameObject.SetActive(false);

            num = 0;
            for (; num < 1;)
            {
                num += .00333333f;
                parteSinistra.sizeDelta = Vector2.Lerp(Vector2.zero, new Vector2(riquadroOrol.rect.width / 2, riquadroOrol.rect.width / 2.5f), num);
                lancetta.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(180, 90, num)));
                tempo.text = (i - 1).ToString() + ":" + ((int)(num * 15) + 30).ToString();
                yield return new WaitForSeconds(.025f);
            }
            parteSinistra.sizeDelta = new Vector2(riquadroOrol.rect.width, riquadroOrol.rect.width / 2f);

            num = 0;
            for (; num < 1;)
            {
                num += .00333333f;
                lancetta.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(90, 0, num)));
                tempo.text = (i - 1).ToString() + ":" + ((int)(num * 15) + 45).ToString();
                yield return new WaitForSeconds(.025f);
            }
        }
        tempo.text = (colori.Count - 1).ToString() + ":00";
    }
}