using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public static List<Uccello> listaUccelli;
    public List<Uccello> _listaUccelli;

    [Header("Pannelli")]
    public RectTransform mainRT;
    public RectTransform setPartitaRT;
    public RectTransform potenziamentiRT;
    public RectTransform impostazioniRT;

    [Header("Elementi")]
    public RectTransform elencoUccelliRT;

    public int bacche;

    public UnityEngine.EventSystems.EventSystem eventSystem;
    public static List<Color> coloriPannelli;
    public Camera cam;

    public static int livello = 1;

    float interpolazioneNonLineare(float t)
    {
        return 3 * Mathf.Pow(t, 4) - 2 * Mathf.Pow(t, 5);
    }

    void Awake()
    {
        listaUccelli = _listaUccelli;
    }

    void Start()
    {
        Camera.main.backgroundColor = coloriPannelli[0];
        ImpostaUccelli(elencoUccelliRT);
    }

    void ImpostaUccelli(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            t.GetChild(i).GetComponent<Image>().sprite = listaUccelli[i].corpoIm;
            t.GetChild(i).GetChild(0).GetComponent<Image>().sprite = listaUccelli[i].occhiIm;
            t.GetChild(i).GetChild(1).GetComponent<Image>().sprite = listaUccelli[i].alaDIm;
            t.GetChild(i).GetChild(2).GetComponent<Image>().sprite = listaUccelli[i].alaSIm;
        }
    }

    public void AttivaDisattivaOggetto(GameObject oggetto)
    {
        oggetto.SetActive(!oggetto.activeInHierarchy);
    }

    public void TransizioneMainSetPartitaPan(int n)
    {
        string st = n.ToString();
        int verso = System.Convert.ToInt32(st[0] + "");
        int ordine = System.Convert.ToInt32(st[1] + "");
        Color colore = coloriPannelli[ordine - 1];
        StartCoroutine(Transizione(mainRT, setPartitaRT, verso, ordine, colore));
    }

    public void TransizioneMainPotenziamenti(int n)
    {
        string st = n.ToString();
        int verso = System.Convert.ToInt32(st[0] + "");
        int ordine = System.Convert.ToInt32(st[1] + "");
        Color colore = coloriPannelli[System.Convert.ToInt32(st[2] + "")];
        StartCoroutine(Transizione(mainRT, potenziamentiRT, verso, ordine, colore));
    }

    public void TransizioneMainImpostazioni(int n)
    {
        string st = n.ToString();
        int verso = System.Convert.ToInt32(st[0] + "");
        int ordine = System.Convert.ToInt32(st[1] + "");
        Color colore = coloriPannelli[System.Convert.ToInt32(st[2] + "")];
        StartCoroutine(Transizione(mainRT, impostazioniRT, verso, ordine, colore));
    }

    IEnumerator Transizione(RectTransform pannello1, RectTransform pannello2, int verso, int ordine, Color coloreFinale)
    {
        MMVibrationManager.Soft();
        static int valoreCorretto (int v)
        {
            if (v == 1)
                return -1;
            else
                return 1;
        }

        ordine = valoreCorretto(ordine);
        verso = valoreCorretto(verso);
        
        if (ordine == -1)
        {
            RectTransform a = pannello1;
            pannello1 = pannello2;
            pannello2 = a;
        }
        Color colIniz = cam.backgroundColor;
        eventSystem.gameObject.SetActive(false);
        pannello2.gameObject.SetActive(true);
        float posIniz = (pannello1.rect.height + pannello1.rect.height / 5) * verso;
        for (float i = 0; i < 1.1; i += .05f)
        {
            float t = interpolazioneNonLineare(i);
            if (verso == 1)
            {
                pannello2.anchoredPosition = new Vector3(0, Mathf.Lerp(-posIniz, 0, t), 0);
                pannello1.anchoredPosition = new Vector3(0, Mathf.Lerp(0, posIniz, t), 0);
            }
            else
            {
                pannello2.anchoredPosition = new Vector3(0, Mathf.Lerp(-posIniz, 0, t), 0);
                pannello1.anchoredPosition = new Vector3(0, Mathf.Lerp(0, posIniz, t), 0);
            }
            cam.backgroundColor = Color.Lerp(colIniz, coloreFinale, t);
            yield return new WaitForSeconds(.032f);
        }
        pannello1.gameObject.SetActive(false);
        eventSystem.gameObject.SetActive(true);
    }

    public void AcquistaUccello(int n)
    {
        MMVibrationManager.Successo();
        bacche -= listaUccelli[n].prezzo;
    }

    public void CambiaScena(int n)
    {
        MMVibrationManager.Successo();
        UnityEngine.SceneManagement.SceneManager.LoadScene(n);
    }
}

[System.Serializable]
public class Uccello
{
    public Sprite corpoIm;
    public Sprite occhiIm;
    public Sprite alaDIm;
    public Sprite alaSIm;
    public Color colorePrincipale;
    public int prezzo;
}