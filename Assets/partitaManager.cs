using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class partitaManager : MonoBehaviour
{
    [Header("Player")]
    public movimento player;
    public SpriteRenderer occhiAperti;
    public SpriteRenderer beccoAperto;

    [Header("Vita")]
    public float vita = 100;
    public float vitaMassima;
    public static List<Cuore> cuori = new List<Cuore>();
    public static List<Sprite> listaCuoriIm;
    public float scudo = 1;
    public RectTransform riquadroVita, riq;
    public Camera cameraSfondo;
    public Image dannoIm;

    [Header("Bacche")]
    public RectTransform barraBacche;
    public int bacche;
    public static int baccheMassime;

    [Header("PowerUp")]
    public RectTransform riquadroPowerUp;

    [Header("Verso")]
    public List<AudioClip> versi;
    public AudioSource versoSource;

    [Header("Gruppi UI")]
    public RectTransform[] riquadroLati;
    public Image sfondoIm;

    public virtual void Start()
    {
        GetComponent<Camera>().enabled = true;
        vitaMassima = vita = Cuore.VitaMassima(cuori);
        AggiornaDimensioniLati();
        AggiornaTipoCuori();
        AggiornaDimensioneVita();
        AggiornaDimensioneBacche();
        AggiornaDimensionePowerUp();
        StartCoroutine(Versi());
        StartCoroutine(SbattiOcchi());
    }

    public virtual void AggiungiBacche(int n)
    {
        if (bacche + n <= baccheMassime)
        {
            bacche += n;
            barraBacche.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3((float)bacche / baccheMassime, 1, 1);
            barraBacche.GetComponentInChildren<Text>().text = bacche.ToString();
            if (!dannoIm.gameObject.activeInHierarchy)
                StartCoroutine(MostraBacche());
            Lofelt.NiceVibrations.HapticPatterns.PlayConstant(.5f, .33f * n, .5f);
        }
    }

    public virtual void AggiornaDimensioniLati()
    {
        foreach (RectTransform l in riquadroLati)
            l.sizeDelta = new Vector2(riq.rect.size.x * ((1f - cameraSfondo.rect.width) / 2f), 0);
    }

    public virtual void AggiornaDimensioneVita(bool ridimRiquad = true, int l = 0, int celleEliminate = 0)
    {
        if (ridimRiquad)
            riquadroVita.sizeDelta = new Vector2(riq.rect.size.x * ((1f - cameraSfondo.rect.width) / 2f), 0);
        int cellePerRiga = 1;
        float larg = riquadroVita.sizeDelta.x;
        cellePerRiga = (int)Mathf.Round((larg - 50) / 175f);
        float colonne = (riquadroVita.childCount - celleEliminate) / (float)cellePerRiga;
        if (colonne > (int)colonne)
            colonne++;
        if (l == 0)
            l = Mathf.Min(cellePerRiga, riquadroVita.childCount);
        riquadroVita.sizeDelta = new Vector3((l * 175) + 25, ((int)colonne * 175) + 25);
    }

    public virtual void AggiornaDimensioneBacche()
    {
        barraBacche.sizeDelta = new Vector2(riq.rect.size.x * ((1f - cameraSfondo.rect.width) / 2.2f), barraBacche.sizeDelta.y);
    }

    public virtual void AggiornaDimensionePowerUp()
    {
        riquadroPowerUp.sizeDelta = new Vector2(riq.rect.size.x * ((1f - cameraSfondo.rect.width) / 2.2f), riquadroPowerUp.sizeDelta.y);
    }

    public virtual void AggiornaTipoCuori(bool figlioUlteriore = false)
    {
        for (int i = 0; i < cuori.Count - 1; i++)
            Instantiate(riquadroVita.GetChild(0).gameObject, riquadroVita);
        
        for (int i = 0; i < cuori.Count; i++)
        {
            if (!figlioUlteriore)
            {
                riquadroVita.GetChild(i).GetComponent<Image>().sprite = listaCuoriIm[cuori[i].livello];
                riquadroVita.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[cuori[i].livello], 0, 1);
                riquadroVita.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[cuori[i].livello], 0, 1);
            }
            else
            {
                riquadroVita.GetChild(i).GetChild(0).GetComponent<Image>().sprite = listaCuoriIm[cuori[i].livello];
                riquadroVita.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[cuori[i].livello], 0, 1);
                riquadroVita.GetChild(i).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[cuori[i].livello], 0, 1);
            }
        }
    }

    public virtual void ImpostaPosizioneCuori()
    {
        for (int i = 0; i < cuori.Count; i++)
            riquadroVita.GetChild(i).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void Danno(float danno)
    {
        vita -= danno / scudo;
        Vector3 posFin = new Vector3(85, -91, 0);
        Vector3 posIni = Vector3.zero;
        float _vita = vita;
        int i = 0;
        while (_vita > 0)
        {
            if (_vita - cuori[i].vitaCuore() > 0)
                _vita -= cuori[i].vitaCuore();
            else
                break;
            i++;
        }
        riquadroVita.GetChild(i).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(posIni, posFin, 1 - _vita / cuori[i].vitaCuore());
        
        for (int o = i + 1; o < cuori.Count; o++)
            riquadroVita.GetChild(o).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = posFin;
        if (impostazioniInGame.cinguettaSeSubisceDanno)
        {
            versoSource.clip = versi[Random.Range(0, versi.Count)];
            versoSource.Play();
        }
        if (!dannoIm.gameObject.activeInHierarchy && danno != 0)
            StartCoroutine(MostraDanno());
        if (danno != 0)
            MoreMountains.NiceVibrations.MMVibrationManager.Pesante();
    }

    IEnumerator MostraDanno()
    {
        dannoIm.gameObject.SetActive(true);
        float alfaMassimo = Mathf.Lerp(0.1f, 1f, 1f - (vita / vitaMassima));
        float i = 0;
        while (i < 10)
        {
            dannoIm.color = new Color(1, 0, 0, Mathf.Lerp(0, alfaMassimo, i / 10));
            i++;
            yield return new WaitForSeconds(.01f);
        }

        i = 0;
        while (i < 10)
        {
            dannoIm.color = new Color(1, 0, 0, Mathf.Lerp(alfaMassimo, 0, i / 10));
            i++;
            yield return new WaitForSeconds(.03f);
        }
        dannoIm.gameObject.SetActive(false);
    }

    IEnumerator MostraBacche()
    {
        dannoIm.gameObject.SetActive(true);
        float alfaMassimo = .8f;
        float i = 0;
        while (i < 10)
        {
            dannoIm.color = new Color(1, 0, 1, Mathf.Lerp(0, alfaMassimo, i / 10));
            i++;
            yield return new WaitForSeconds(.01f);
        }

        i = 0;
        while (i < 10)
        {
            dannoIm.color = new Color(1, 0, 1, Mathf.Lerp(alfaMassimo, 0, i / 10));
            i++;
            yield return new WaitForSeconds(.03f);
        }
        dannoIm.gameObject.SetActive(false);
    }

    public virtual IEnumerator Versi()
    {
        for (; ; )
        {
            float aspetta = 3;
            switch (impostazioniInGame.frequenzaCinguettii)
            {
                case impostazioni.FrequenzaCinguettii.sempre:
                    aspetta = Random.Range(1f, 5f);
                    break;
                case impostazioni.FrequenzaCinguettii.spesso:
                    aspetta = Random.Range(2f, 7f);
                    break;
                case impostazioni.FrequenzaCinguettii.aVolte:
                    aspetta = Random.Range(4f, 11f);
                    break;
            }
            yield return new WaitForSeconds(aspetta);
            if (impostazioniInGame.frequenzaCinguettii == impostazioni.FrequenzaCinguettii.mai)
                continue;
            versoSource.clip = versi[Random.Range(0, versi.Count)];
            versoSource.panStereo = player.transform.position.x / 6f;
            versoSource.pitch = Random.Range(.7f, 1.3f);
            versoSource.Play();
            beccoAperto.gameObject.SetActive(true);
            yield return new WaitForSeconds(versoSource.clip.length);
            beccoAperto.gameObject.SetActive(false);
        }
    }

    public virtual IEnumerator SbattiOcchi()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(Random.Range(.5f, 3f));
            occhiAperti.gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(.05f, .2f));
            occhiAperti.gameObject.SetActive(true);
        }
    }
}

[System.Serializable]
public class Cuore
{
    public int livello;
    public static List<float> gColoreLivello = new List<float>() { 1, .75f, .5f, .25f, 0 };

    public float vitaCuore()
    {
        return Elemento.ValoreDaTipo(DettagliPotenziamento.Tipo.cuore)[livello];
    }

    public static float VitaMassima(List<Cuore> cuori)
    {
        float _vita = 0;
        for (int i = 0; i < cuori.Count; i++)
            _vita += Elemento.ValoreDaTipo(DettagliPotenziamento.Tipo.cuore)[cuori[i].livello];
        return _vita;
    }
}