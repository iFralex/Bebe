using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using Lofelt.NiceVibrations;

public class powerUp : MonoBehaviour
{
    public enum PowerUps { diminuisciDanno, rallentaTempo, riempiSalute }
    public Image pannello;
    public SpriteRenderer scudoIm;
    public partitaManager pm;
    public List<int> rimanenti;
    public List<int> tempi;
    public List<int> potenze;
    public List<AudioClip> powerUpSuoni;
    public AudioClip saluteSuono;
    public RectTransform area;
    float sec;
    public impostazioniInGame impostazioniPan;

    void Start()
    {
        rimanenti.Clear();
        foreach (int n in variabili.quantitàPowerUps)
        {
            float n1 = 1;
            if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n1 = 1.5f;
            else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n1 = 2.5f;
            rimanenti.Add((int)(n * n1));
        }
        tempi.Clear();
        foreach (int n in variabili.tempiPowerUps)
        {
            int n1 = 1;
            if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n1 = 2;
            else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n1 = 3;
            tempi.Add(n * n1);
        }
        potenze.Clear();
        foreach (int n in variabili.potenzePowerUps)
        {
            float n1 = 1;
            if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n1 = 2;
            else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n1 = 4;
            potenze.Add((int)(n * n1));
        }
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = rimanenti[i].ToString();
    }

    void AttivaTriggerEvent(bool attivo)
    {
        UnityEngine.EventSystems.EventTrigger[] a = GetComponentsInChildren<UnityEngine.EventSystems.EventTrigger>();
        foreach (UnityEngine.EventSystems.EventTrigger b in a)
            b.enabled = attivo;
    }

    public void RallentaTempo()
    {
        StartCoroutine(RallentaTempoCor());
    }

    IEnumerator RallentaTempoCor()
    {
        MMVibrationManager.Successo();
        HapticPatterns.PlayConstant(.25f, .2f, tempi[(int)PowerUps.rallentaTempo] + 1);
        GetComponent<AudioSource>().clip = powerUpSuoni[0];
        GetComponent<AudioSource>().Play();
        rimanenti[(int)PowerUps.rallentaTempo] -= 1;
        transform.GetChild((int)PowerUps.rallentaTempo).localScale *= 1.5f;
        transform.GetChild((int)PowerUps.rallentaTempo).GetChild(0).GetComponent<Text>().text = rimanenti[(int)PowerUps.rallentaTempo].ToString();
        if (rimanenti[(int)PowerUps.rallentaTempo] == 0)
        {
            Destroy(transform.GetChild((int)PowerUps.rallentaTempo).GetComponent<UnityEngine.EventSystems.EventTrigger>());
            Color r = transform.GetChild((int)PowerUps.rallentaTempo).GetComponent<Image>().color;
            transform.GetChild((int)PowerUps.rallentaTempo).GetComponent<Image>().color = new Color(r.r, r.g, r.b, .5f);
        }
        transform.parent.GetComponentInChildren<clessidra>().Begin(tempi[(int)PowerUps.rallentaTempo]);
        AttivaTriggerEvent(false);
        GetComponent<CanvasGroup>().alpha = .5f;
        pannello.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            pannello.color = new Color(0f, .5f, 1, Mathf.Lerp(0f, .5f, i / 10f));
            yield return new WaitForSecondsRealtime(.1f);
        }
        pm.transform.GetChild(1).GetComponent<AudioSource>().pitch = .7f;
        pm.transform.GetChild(1).GetComponent<AudioLowPassFilter>().enabled = true;
        Time.timeScale = potenze[(int)PowerUps.rallentaTempo] / 100f;
        yield return new WaitForSecondsRealtime(tempi[(int)PowerUps.rallentaTempo]);
        pm.transform.GetChild(1).GetComponent<AudioSource>().pitch = 1f;
        pm.transform.GetChild(1).GetComponent<AudioLowPassFilter>().enabled = false;
        for (int i = 0; i < 10; i++)
        {
            pannello.color = new Color(0f, .5f, 1, Mathf.Lerp(.5f, 0f, i / 10f));
            yield return new WaitForSecondsRealtime(.1f);
        }
        Time.timeScale = 1f;
        pannello.gameObject.SetActive(false);
        transform.GetChild((int)PowerUps.rallentaTempo).localScale /= 1.5f;
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<AudioSource>().clip = powerUpSuoni[1];
        GetComponent<AudioSource>().Play();
        AttivaTriggerEvent(true);
        MMVibrationManager.Avviso();
    }

    public void DiminuisciDanno()
    {
        StartCoroutine(DiminuisciDannoCor());
    }

    IEnumerator DiminuisciDannoCor()
    {
        MMVibrationManager.Successo();
        HapticPatterns.PlayConstant(.25f, .2f, tempi[(int)PowerUps.diminuisciDanno] + 1);
        GetComponent<AudioSource>().clip = powerUpSuoni[0];
        GetComponent<AudioSource>().Play();
        rimanenti[(int)PowerUps.diminuisciDanno] -= 1;
        transform.GetChild((int)PowerUps.diminuisciDanno).localScale *= 1.5f;
        transform.GetChild((int)PowerUps.diminuisciDanno).GetChild(0).GetComponent<Text>().text = rimanenti[(int)PowerUps.diminuisciDanno].ToString();
        if (rimanenti[(int)PowerUps.diminuisciDanno] == 0)
        {
            Destroy(transform.GetChild((int)PowerUps.diminuisciDanno).GetComponent<UnityEngine.EventSystems.EventTrigger>());
            Color r = transform.GetChild((int)PowerUps.diminuisciDanno).GetComponent<Image>().color;
            transform.GetChild((int)PowerUps.diminuisciDanno).GetComponent<Image>().color = new Color(r.r, r.g, r.b, .5f);
        }
        AttivaTriggerEvent(false);
        transform.parent.GetComponentInChildren<clessidra>().Begin(tempi[(int)PowerUps.diminuisciDanno]);
        GetComponent<CanvasGroup>().alpha = .5f;
        scudoIm.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            scudoIm.color = new Color(1, 1, 0, Mathf.Lerp(0f, 1f, i / 10f));
            yield return new WaitForSecondsRealtime(.1f);
        }
        pm.scudo *= potenze[(int)PowerUps.diminuisciDanno];
        yield return new WaitForSeconds(tempi[(int)PowerUps.diminuisciDanno]);
        for (int i = 0; i < 10; i++)
        {
            scudoIm.color = new Color(1, 1, 0, Mathf.Lerp(1f, 0f, i / 10f));
            yield return new WaitForSecondsRealtime(.1f);
        }
        pm.scudo /= potenze[(int)PowerUps.diminuisciDanno];
        scudoIm.gameObject.SetActive(false);
        transform.GetChild((int)PowerUps.diminuisciDanno).localScale /= 1.5f;
        GetComponent<CanvasGroup>().alpha = 1;
        AttivaTriggerEvent(true);
        GetComponent<AudioSource>().clip = powerUpSuoni[1];
        GetComponent<AudioSource>().Play();
        MMVibrationManager.Avviso();
    }

    public void RiempiSalute()
    {
        StartCoroutine(RiempiSaluteCor());
    }

    IEnumerator RiempiSaluteCor()
    {
        MMVibrationManager.Successo();
        GetComponent<AudioSource>().clip = saluteSuono;
        GetComponent<AudioSource>().Play();
        rimanenti[(int)PowerUps.riempiSalute] -= 1;
        transform.GetChild((int)PowerUps.riempiSalute).localScale *= 1.5f;
        transform.GetChild((int)PowerUps.riempiSalute).GetChild(0).GetComponent<Text>().text = rimanenti[(int)PowerUps.riempiSalute].ToString();
        if (rimanenti[(int)PowerUps.riempiSalute] == 0)
        {
            Destroy(transform.GetChild((int)PowerUps.riempiSalute).GetComponent<UnityEngine.EventSystems.EventTrigger>());
            Color r = transform.GetChild((int)PowerUps.riempiSalute).GetComponent<Image>().color;
            transform.GetChild((int)PowerUps.riempiSalute).GetComponent<Image>().color = new Color(r.r, r.g, r.b, .5f);
        }
        AttivaTriggerEvent(false);
        GetComponent<CanvasGroup>().alpha = .5f;
        pannello.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            pannello.color = new Color(0, 1, 0, Mathf.Lerp(0f, .8f, i / 10f));
            yield return new WaitForSecondsRealtime(.1f);
        }
        pm.vita = Mathf.Min(pm.vitaMassima, pm.vita + pm.vitaMassima / potenze[(int)PowerUps.riempiSalute]);
        pm.ImpostaPosizioneCuori();
        pm.Danno(0);
        for (int i = 0; i < 10; i++)
        {
            pannello.color = new Color(0, 1, 0, Mathf.Lerp(.8f, 0f, i / 10f));
            yield return new WaitForSecondsRealtime(.1f);
        }
        AttivaTriggerEvent(true);
        pannello.gameObject.SetActive(false);
        transform.GetChild((int)PowerUps.riempiSalute).localScale /= 1.5f;
        GetComponent<CanvasGroup>().alpha = 1;
        MMVibrationManager.Avviso();
    }

    public void Premuto(int n)
    {
        sec = Time.time;
        pm.player.input = false;
        StartCoroutine(Illumina(transform.GetChild(n).GetChild(1).gameObject.GetComponent<Image>(), 0, 1));
    }

    public void FinePremuto(int n)
    {
        if (Time.time - sec >= impostazioniInGame.tempoPremutoPowerUp)
            switch (n)
            {
                case 1:
                    RallentaTempo();
                    break;
                case 0:
                    DiminuisciDanno();
                    break;
                case 2:
                    RiempiSalute();
                    break;
                case 3:
                    impostazioniPan.ApriImpostazioni(true);
                    break;
            }
        StartCoroutine(Illumina(transform.GetChild(n).GetChild(1).gameObject.GetComponent<Image>(), 1, 0));
        StartCoroutine(ReattivaInput());
    }

    public void CancellaPremuto(int n)
    {
        sec = Time.time + 100;
    }

    IEnumerator Illumina(Image img, float ini, float fin)
    {
        for (float i = 0; i < 1; i += .1f)
        {
            yield return new WaitForSecondsRealtime(.01f);
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(ini, fin, i));
        }
    }

    IEnumerator ReattivaInput()
    {
        yield return new WaitForSeconds(.1f);
        pm.player.input = true;
    }
}