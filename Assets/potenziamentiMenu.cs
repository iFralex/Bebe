using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class potenziamentiMenu : partitaManager
{
    public RectTransform dettagliPan, dettagliCampoPan, sacchettoPan, powerUpPan;
    public Button aggiungiCuoreBt;
    public Image[] copertureIm;

    public override void Start()//
    {
        if (PlayerPrefs.HasKey("numero cuori"))
            if (PlayerPrefs.GetInt("numero cuori") >= 9)
                Destroy(aggiungiCuoreBt.gameObject);
        //PlayerPrefs.DeleteAll();
        /*PlayerPrefs.DeleteKey("tempoPowerUp) Rallenta tempo (Power up): (durata) Durata");
        PlayerPrefs.DeleteKey("dannoPowerUp) Scudo (Power up): (quantità) Quantità");
        PlayerPrefs.DeleteKey("tempoPowerUp) Rallenta tempo (Power up): (quantità) Quantità");
        PlayerPrefs.DeleteKey("dannoPowerUp) Scudo (Power up): (durata) Durata");
        PlayerPrefs.DeleteKey("dannoPowerUp) Scudo (Power up): (potenza) Protezione");*/
        CreaLista();
        base.AggiornaTipoCuori(true);
        base.AggiornaDimensioneVita(false, 3);
        ImpostaPulsanti();
        ImpostaDimensioni();
        gameObject.SetActive(false);
    }

    void ImpostaPulsanti(int eliminati = 0)
    {
        riquadroVita.GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        int n = -1;
        for (int i = 0; i < riquadroVita.childCount; i++)
        {
            if (i != 0 && i <= eliminati)
                continue;
            n++;
            int m = n;
            riquadroVita.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelezionaCuore(m));
        }
    }

    public void ImpostaDimensioni()
    {
        float alt = 100;
        for (int i = 0; i < sacchettoPan.parent.childCount; i++)
            if (sacchettoPan.parent.GetChild(i).gameObject.activeInHierarchy && !sacchettoPan.parent.GetChild(i).GetComponent<LayoutElement>())
                alt += sacchettoPan.parent.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 90;
        sacchettoPan.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(sacchettoPan.parent.GetComponent<RectTransform>().sizeDelta.x, alt);
    }

    public void CreaLista()
    {
        bool Cè(string s)
        {
            return PlayerPrefs.HasKey(s);
        }

        void Salva(string s, object value)
        {
            if (value is int)
                PlayerPrefs.SetInt(s, (int)value);
            else if (value is float)
                PlayerPrefs.SetFloat(s, (float)value);
            else if (value is string)
                PlayerPrefs.SetString(s, (string)value);
        }

        int RicavaInt(string s)
        {
            return PlayerPrefs.GetInt(s);
        }

        int num = 0;
        if (Cè("numero cuori"))
            num = RicavaInt("numero cuori");
        DettagliPotenziamento cuore = DettagliPotenziamento.Clona(DettagliPotenziamento.listaElementi[0]);
        if (Time.time < .5f)
        for (int i = num; i > 1; i--)
        {
            cuore = DettagliPotenziamento.Clona(cuore);
            cuore.nome = "Cuore " + (i).ToString();
            DettagliPotenziamento.listaElementi.Insert(1, cuore);
        }
        cuori.Clear();
        for (int i = 0; i < DettagliPotenziamento.listaElementi.Count; i++)
        {
            DettagliPotenziamento elemento = DettagliPotenziamento.listaElementi[i];
            for (int o = 0; o < elemento.listaCampi.Count; o++)
            {
                string stringa = elemento.tipo + ") " + elemento.nome + ": (" + elemento.listaCampi[o].tipoSpecifico + ") " + elemento.listaCampi[o].nome;
                if (Cè(stringa))
                    elemento.listaCampi[o].livello = RicavaInt(stringa);
            }
            if (elemento.tipo == DettagliPotenziamento.Tipo.cuore)
                cuori.Add(new Cuore() { livello = elemento.livello - 1 });
            else if (elemento.tipo == DettagliPotenziamento.Tipo.sacchetto)
                riquadroVita.parent.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[0].valore.ToString();
            else
                powerUpPan.GetChild((int)elemento.tipo - 2).GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[elemento.tipo != DettagliPotenziamento.Tipo.salutePowerUp ? 2 : 1].valore.ToString();
        }
        SalvaVariabiliStatiche();
        //foreach (DettagliPotenziamento c in DettagliPotenziamento.listaElementi)
        //print(JsonUtility.ToJson(c));
    }

    public void SalvaVariabiliStatiche()
    {
        variabili.quantitàPowerUps = new List<int>() { (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.dannoPowerUp).listaCampi[2].valore, (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.tempoPowerUp).listaCampi[2].valore, (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.salutePowerUp).listaCampi[1].valore };
        variabili.tempiPowerUps = new List<int>() { (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.dannoPowerUp).listaCampi[1].valore, (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.tempoPowerUp).listaCampi[1].valore };
        variabili.potenzePowerUps = new List<int>() { (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.dannoPowerUp).listaCampi[0].valore, (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.tempoPowerUp).listaCampi[0].valore, (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.salutePowerUp).listaCampi[0].valore };
        baccheMassime = (int)DettagliPotenziamento.OttieniDettagliPerTipo(DettagliPotenziamento.Tipo.sacchetto).listaCampi[0].valore;
    }

    public void DisattivaTutteSelezioni()
    {
        for (int i = 0; i < riquadroVita.childCount; i++)
            riquadroVita.GetChild(i).GetComponent<Image>().enabled = false;
        barraBacche.GetComponent<Image>().enabled = false;
        for (int i = 0; i < powerUpPan.childCount; i++)
            powerUpPan.GetChild(i).GetComponent<Image>().enabled = false;
    }

    public void SelezionaCuore(int n)
    {
        bool attivo = !riquadroVita.GetChild(n).GetComponent<Image>().enabled;
        dettagliCampoPan.gameObject.SetActive(false);
        dettagliPan.gameObject.SetActive(attivo);
        if (attivo)
        {
            DisattivaTutteSelezioni();
            riquadroVita.GetChild(n).GetComponent<Image>().enabled = true;
        }
        else
        {
            riquadroVita.GetChild(n).GetComponent<Image>().enabled = false;
            return;
        }
        MoreMountains.NiceVibrations.MMVibrationManager.Selezione();
        DettagliPotenziamento elemento = DettagliPotenziamento.OttieniListaDettagliPerTipo(DettagliPotenziamento.Tipo.cuore)[n];
        dettagliPan.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.nome;
        dettagliPan.GetChild(0).GetChild(1).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(elemento.percentualeLivello / 100f, 1, 1);
        dettagliPan.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = elemento.livello.ToString();
        for (int i = 0; i < dettagliPan.GetChild(0).GetChild(2).childCount; i++)
            Destroy(dettagliPan.GetChild(0).GetChild(2).GetChild(i).gameObject);
        RectTransform cop = Instantiate(riquadroVita.GetChild(n).GetChild(0).gameObject, dettagliPan.GetChild(0).GetChild(2)).GetComponent<RectTransform>();
        cop.anchoredPosition = Vector2.zero;
        Destroy(cop.GetComponent<Button>());
        dettagliPan.GetChild(1).GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        for (int i = dettagliPan.childCount - 1; i >= 2; i--)
            DestroyImmediate(dettagliPan.GetChild(i).gameObject);
        for (int i = 0; i < elemento.listaCampi.Count - 1; i++)
        {
            int x = i;
            Instantiate(dettagliPan.GetChild(1).gameObject).GetComponent<Button>().onClick.AddListener(() => SelezionaCampo(0, elemento, x + 1));
            dettagliPan.GetChild(i + 1).GetComponent<Image>().color = Color.black;
        }
        dettagliPan.GetChild(1).GetComponent<Button>().onClick.AddListener(() => SelezionaCampo(0, elemento, n));
        dettagliPan.GetChild(1).GetComponent<Image>().color = Color.black;
        for (int i = 0; i < elemento.listaCampi.Count; i++)
        {
            dettagliPan.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[i].nome;
            dettagliPan.GetChild(i + 1).GetChild(1).GetComponent<Text>().text = elemento.listaCampi[i].valore.ToString();
        }
        RectTransform immagine = dettagliPan.GetChild(0).GetChild(2).GetComponent<RectTransform>();
        dettagliPan.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(immagine.sizeDelta.x, immagine.rect.width * 350 / 384f);
        float alt = 0;
        for (int i = 0; i < dettagliPan.GetChild(0).childCount; i++)
            alt += dettagliPan.GetChild(0).GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
        dettagliPan.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(dettagliPan.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, alt);
        alt = 50;
        for (int i = 0; i < dettagliPan.childCount; i++)
            alt += dettagliPan.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
        dettagliPan.sizeDelta = new Vector2(dettagliPan.sizeDelta.x, alt);
    }

    public void SelezionaCampo(int indexCamp, DettagliPotenziamento elemento, int indexCuore = -1)
    {
        Elemento campo = elemento.listaCampi[indexCamp];
        for (int i = 1; i < dettagliPan.childCount; i++)
            dettagliPan.GetChild(i).GetComponent<Image>().color = Color.black;
        dettagliPan.GetChild(indexCamp + 1).GetComponent<Image>().color = Color.green;
        dettagliCampoPan.gameObject.SetActive(true);
        dettagliCampoPan.GetChild(0).GetComponent<Text>().text = campo.nome;
        dettagliCampoPan.GetChild(2).GetComponent<Text>().text = campo.descrizione;
        dettagliCampoPan.GetChild(3).GetComponent<Text>().text = campo.valore.ToString();
        if (campo.livello < campo.prezzi.Count - 1)
        {
            dettagliCampoPan.GetChild(4).gameObject.SetActive(true);
            dettagliCampoPan.GetChild(4).GetChild(1).GetComponent<Text>().text = campo.prezzi[campo.livello + 1].ToString();
            dettagliCampoPan.GetChild(4).GetChild(2).GetComponent<Text>().text = campo.valori[campo.livello + 1].ToString();
            dettagliCampoPan.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
            if (campo.tipo == DettagliPotenziamento.Tipo.cuore)
                dettagliCampoPan.GetChild(4).GetComponent<Button>().onClick.AddListener(() => PotenziaElemento(indexCuore, 0));
            else if (campo.tipo == DettagliPotenziamento.Tipo.sacchetto)
                dettagliCampoPan.GetChild(4).GetComponent<Button>().onClick.AddListener(() => PotenziaElemento(_elemento: elemento, _campo: elemento.listaCampi[0]));
            else
                dettagliCampoPan.GetChild(4).GetComponent<Button>().onClick.AddListener(() => PotenziaElemento(_elemento: elemento, _campo: elemento.listaCampi[indexCamp]));
        }
        else
            dettagliCampoPan.GetChild(4).gameObject.SetActive(false);
        MoreMountains.NiceVibrations.MMVibrationManager.Soft();
    }

    public void AcquistaCuore()
    {
        if (riquadroVita.childCount >= 9)
            return;
        if (PlayerPrefs.HasKey("numero cuori"))
            PlayerPrefs.SetInt("numero cuori", PlayerPrefs.GetInt("numero cuori") + 1);
        else
            PlayerPrefs.SetInt("numero cuori", 2);
        DettagliPotenziamento cuore = DettagliPotenziamento.Clona(DettagliPotenziamento.listaElementi[0]);
        cuore.listaCampi[0].livello = 0;
        cuore.nome = "Cuore " + (riquadroVita.childCount + 1);
        cuori.Add(new Cuore() { livello = cuore.livello });
        DettagliPotenziamento.listaElementi.Insert(riquadroVita.childCount, cuore);
        int n = riquadroVita.childCount;
        Instantiate(riquadroVita.GetChild(0).gameObject, riquadroVita).transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => SelezionaCuore(n));
        riquadroVita.GetChild(riquadroVita.childCount - 1).GetChild(0).GetComponent<Image>().sprite = listaCuoriIm[0];
        riquadroVita.GetChild(riquadroVita.childCount - 1).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[0], 0, 1);
        riquadroVita.GetChild(riquadroVita.childCount - 1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[0], 0, 1);
        riquadroVita.GetChild(riquadroVita.childCount - 1).GetComponent<Image>().enabled = false;
        base.AggiornaDimensioneVita(false, 3);
        ImpostaDimensioni();
        if (riquadroVita.childCount >= 9)
            Destroy(aggiungiCuoreBt.gameObject);
        MoreMountains.NiceVibrations.MMVibrationManager.Successo();
    }

    public void PotenziaElemento(int indexDet = -1, int indexCamp = -1, DettagliPotenziamento _elemento = null, Elemento _campo = null)
    {
        DettagliPotenziamento elemento;
        Elemento campo;
        if (indexDet == -1)
        {
            elemento = _elemento;
            campo = _campo;
        }
        else
        {
            elemento = DettagliPotenziamento.listaElementi[indexDet];
            campo = elemento.listaCampi[indexCamp];
        }
        if (campo.livello >= 4)
            return;
        StartCoroutine(Copertura());
        string s = elemento.tipo + ") " + elemento.nome + ": (" + campo.tipoSpecifico + ") " + campo.nome;
        if (PlayerPrefs.HasKey(s))
            PlayerPrefs.SetInt(s, PlayerPrefs.GetInt(s) + 1);
        else
            PlayerPrefs.SetInt(s, 1);
        campo.livello++;
        if (elemento.tipo == DettagliPotenziamento.Tipo.cuore)
        {
            riquadroVita.GetChild(indexDet).GetChild(0).GetComponent<Image>().sprite = listaCuoriIm[campo.livello];
            riquadroVita.GetChild(indexDet).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[campo.livello], 0, 1);
            riquadroVita.GetChild(indexDet).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, Cuore.gColoreLivello[campo.livello], 0, 1);
            riquadroVita.GetChild(indexDet).GetComponent<Image>().enabled = false;
            SelezionaCuore(indexDet);
            SelezionaCampo(indexCamp, elemento, indexDet);
        }
        else if (elemento.tipo == DettagliPotenziamento.Tipo.sacchetto)
        {
            barraBacche.GetComponent<Image>().enabled = false;
            SelezionaSacchetto();
            SelezionaCampo(0, elemento);
        }
        else
        {
            powerUpPan.GetChild((int)campo.tipo - 2).GetComponent<Image>().enabled = false;
            SelezionaPowerUp((int)campo.tipo - 2);
            SelezionaCampo((int)campo.tipoSpecifico, elemento);
        }
        SalvaVariabiliStatiche();
        MoreMountains.NiceVibrations.MMVibrationManager.Successo();
    }

    public void SelezionaSacchetto()
    {
        bool attivo = !barraBacche.GetComponent<Image>().enabled;
        dettagliCampoPan.gameObject.SetActive(false);
        dettagliPan.gameObject.SetActive(attivo);

        if (attivo)
        {
            DisattivaTutteSelezioni();
            barraBacche.GetComponent<Image>().enabled = true;
        }
        else
        {
            barraBacche.GetComponent<Image>().enabled = false;
            return;
        }
        MoreMountains.NiceVibrations.MMVibrationManager.Selezione();
        DettagliPotenziamento elemento = DettagliPotenziamento.OttieniListaDettagliPerTipo(DettagliPotenziamento.Tipo.sacchetto)[0];
        dettagliPan.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.nome;
        dettagliPan.GetChild(0).GetChild(1).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(elemento.percentualeLivello / 100f, 1, 1);
        dettagliPan.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = elemento.livello.ToString();
        riquadroVita.parent.GetChild(2).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[0].valore.ToString();
        for (int i = 0; i < dettagliPan.GetChild(0).GetChild(2).childCount; i++)
            Destroy(dettagliPan.GetChild(0).GetChild(2).GetChild(i).gameObject);
        RectTransform cop = Instantiate(barraBacche.GetChild(0).gameObject, dettagliPan.GetChild(0).GetChild(2)).GetComponent<RectTransform>();
        cop.anchoredPosition = Vector2.zero;
        Destroy(cop.GetComponent<Button>());
        cop.GetChild(1).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[0].valore.ToString();
        dettagliPan.GetChild(1).GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        for (int i = 0; i < elemento.listaCampi.Count - 1; i++)
        {
            int x = i;
            Instantiate(dettagliPan.GetChild(1).gameObject).GetComponent<Button>().onClick.AddListener(() => SelezionaCampo(0, elemento));
            dettagliPan.GetChild(i + 1).GetComponent<Image>().color = Color.black;
        }
        dettagliPan.GetChild(1).GetComponent<Button>().onClick.AddListener(() => SelezionaCampo(0, elemento));
        dettagliPan.GetChild(1).GetComponent<Image>().color = Color.black;
        for (int i = 0; i < elemento.listaCampi.Count; i++)
        {
            dettagliPan.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[i].nome;
            dettagliPan.GetChild(i + 1).GetChild(1).GetComponent<Text>().text = elemento.listaCampi[i].valore.ToString();
        }
        RectTransform immagine = dettagliPan.GetChild(0).GetChild(2).GetComponent<RectTransform>();
        dettagliPan.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(immagine.sizeDelta.x, immagine.rect.width * 200 / 650f);
        float alt = 0;
        for (int i = 0; i < dettagliPan.GetChild(0).childCount; i++)
            alt += dettagliPan.GetChild(0).GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
        dettagliPan.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(dettagliPan.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, alt);
        alt = 50;
        for (int i = 0; i < dettagliPan.childCount; i++)
            alt += dettagliPan.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
        dettagliPan.sizeDelta = new Vector2(dettagliPan.sizeDelta.x, alt);
    }

    public void SelezionaPowerUp(int n)
    {
        bool attivo = !powerUpPan.GetChild(n).GetComponent<Image>().enabled;
        dettagliCampoPan.gameObject.SetActive(false);
        dettagliPan.gameObject.SetActive(attivo);

        if (attivo)
        {
            DisattivaTutteSelezioni();
            powerUpPan.GetChild(n).GetComponent<Image>().enabled = true;
        }
        else
        {
            powerUpPan.GetChild(n).GetComponent<Image>().enabled = false;
            return;
        }
        MoreMountains.NiceVibrations.MMVibrationManager.Selezione();
        DettagliPotenziamento elemento = DettagliPotenziamento.OttieniListaDettagliPerTipo((DettagliPotenziamento.Tipo)n + 2)[0];
        dettagliPan.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.nome;
        dettagliPan.GetChild(0).GetChild(1).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(elemento.percentualeLivello / 100f, 1, 1);
        dettagliPan.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = elemento.livello.ToString();
        powerUpPan.GetChild(n).GetChild(0).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[n != 2 ? 2 : 1].valore.ToString();
        for (int i = 0; i < dettagliPan.GetChild(0).GetChild(2).childCount; i++)
            Destroy(dettagliPan.GetChild(0).GetChild(2).GetChild(i).gameObject);
        RectTransform cop = Instantiate(powerUpPan.GetChild(n).GetChild(0).gameObject, dettagliPan.GetChild(0).GetChild(2)).GetComponent<RectTransform>();
        cop.anchoredPosition = Vector2.zero;
        Destroy(cop.GetComponent<Button>());
        cop.GetChild(0).GetComponent<Text>().text = elemento.listaCampi[elemento.tipo != DettagliPotenziamento.Tipo.salutePowerUp ? 2 : 1].valore.ToString();
        dettagliPan.GetChild(1).GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        for (int i = dettagliPan.childCount - 1; i >= 2; i--)
            DestroyImmediate(dettagliPan.GetChild(i).gameObject);
        for (int i = dettagliPan.childCount - 1; i >= 2; i--)
            DestroyImmediate(dettagliPan.GetChild(i).gameObject);
        for (int i = 0; i < elemento.listaCampi.Count - 1; i++)
        {
            int x = i;
            Instantiate(dettagliPan.GetChild(1).gameObject, dettagliPan).GetComponent<Button>().onClick.AddListener(() => SelezionaCampo(x + 1, elemento));
            dettagliPan.GetChild(i + 1).GetComponent<Image>().color = Color.black;
        }
        dettagliPan.GetChild(1).GetComponent<Button>().onClick.AddListener(() => SelezionaCampo(0, elemento));
        dettagliPan.GetChild(1).GetComponent<Image>().color = Color.black;
        for (int i = 0; i < elemento.listaCampi.Count; i++)
        {
            dettagliPan.GetChild(i + 1).GetChild(0).GetComponent<Text>().text = elemento.listaCampi[i].nome;
            dettagliPan.GetChild(i + 1).GetChild(1).GetComponent<Text>().text = elemento.listaCampi[i].valore.ToString();
        }
        RectTransform immagine = dettagliPan.GetChild(0).GetChild(2).GetComponent<RectTransform>();
        dettagliPan.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(immagine.sizeDelta.x, immagine.rect.width);
        float alt = 0;
        for (int i = 0; i < dettagliPan.GetChild(0).childCount; i++)
            alt += dettagliPan.GetChild(0).GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
        dettagliPan.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(dettagliPan.GetChild(0).GetComponent<RectTransform>().sizeDelta.x, alt);
        alt = 50;
        for (int i = 0; i < dettagliPan.childCount; i++)
            alt += dettagliPan.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
        dettagliPan.sizeDelta = new Vector2(dettagliPan.sizeDelta.x, alt);
    }

    IEnumerator Copertura()
    {
        foreach (Image a in copertureIm)
        {
            a.gameObject.SetActive(true);
            a.color = Color.white;
        }
        for (float i = 0; i < 1f; i += .1f)
        {
            foreach (Image a in copertureIm)
                a.color = new Color(1, 1, 1, 1f - i);
            yield return new WaitForSeconds(.05f);
        }
        foreach (Image a in copertureIm)
            a.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class DettagliPotenziamento
{
    public enum Tipo { cuore, sacchetto, dannoPowerUp, tempoPowerUp, salutePowerUp }
    public Tipo tipo;
    public string nome;
    public string descrizione;
    public Sprite immagine;
    public int livello
    {
        get
        {
            if (listaCampi.Count == 1)
                return listaCampi[0].livello + 1;
            int tot = 0;
            for (int o = 0; o < listaCampi.Count; o++)
                for (int i = listaCampi[o].livello; i >= 0; i--)
                    tot += listaCampi[o].prezzi[i];

            int a = 0;
            for (int o = 0; o < 5; o++)
            {
                for (int i = 0; i < listaCampi.Count; i++)
                    a += listaCampi[i].prezzi[o];
                if (tot < a)
                    return o;
            }
            return 5;
        }
    }

    public int percentualeLivello
    {
        get
        {
            if (listaCampi.Count == 1)
                return 150;
            int tot = 0;
            for (int o = 0; o < listaCampi.Count; o++)
                for (int i = listaCampi[o].livello; i >= 0; i--)
                    tot += listaCampi[o].prezzi[i];

            int a = 0;
            int l = 0;
            for (int o = 0; o < 5; o++)
            {
                for (int i = 0; i < listaCampi.Count; i++)
                    a += listaCampi[i].prezzi[o];
                if (tot <= a)
                {
                    for (int i = 0; i < listaCampi.Count; i++)
                        if (o > 0)
                            l += listaCampi[i].prezzi[o - 1];
                    break;
                }
            }
            int t = a - l;
            int p = tot - l;
            if (p == 0)
                p = 105;
            return p * 100 / t;
        }
    }
    public List<Elemento> listaCampi;

    public static List<DettagliPotenziamento> listaElementi;

    public static DettagliPotenziamento cercaPerNome(string n)
    {
        foreach (DettagliPotenziamento a in listaElementi)
            if (a.nome == n)
                return a;
        return null;
    }

    public static List<DettagliPotenziamento> OttieniListaDettagliPerTipo(Tipo t)
    {
        List<DettagliPotenziamento> l = new List<DettagliPotenziamento>();
        foreach (DettagliPotenziamento a in listaElementi)
            if (a.tipo == t)
                l.Add(a);
        return l;
    }

    public static DettagliPotenziamento Clona(DettagliPotenziamento elem)
    {
        List<Elemento> campi = new List<Elemento>();
        for (int i = 0; i < elem.listaCampi.Count; i++)
            campi.Add(Elemento.Clona(elem.listaCampi[i]));
        return new DettagliPotenziamento() { tipo = elem.tipo, nome = elem.nome, descrizione = elem.descrizione, immagine = elem.immagine, listaCampi = campi};
    }

    public static DettagliPotenziamento OttieniDettagliPerTipo(DettagliPotenziamento.Tipo t)
    {
        foreach (DettagliPotenziamento a in listaElementi)
            if (a.tipo == t)
                return a;
        return null;
    }
}

[System.Serializable]
public class Elemento
{
    public enum TipoSpecifico { potenza, durata, quantità }
    public TipoSpecifico tipoSpecifico;
    public DettagliPotenziamento.Tipo tipo;
    public string nome;
    public string descrizione;
    public float valore
    {
        get { return valori[livello]; }
    }
    public int prezzo
    {
        get { return prezzi[livello]; }
    }
    public int livello;
    public Sprite immagine;
    public List<int> prezzi
    {
        get { return PrezzoDaTipo(tipo, tipoSpecifico); }
    }

    public static List<int> PrezzoDaTipo(DettagliPotenziamento.Tipo t, TipoSpecifico ts = TipoSpecifico.potenza)
    {
        switch (t)
        {
            case DettagliPotenziamento.Tipo.cuore:
                return ListinoPrezzi.cuori;
            case DettagliPotenziamento.Tipo.sacchetto:
                return ListinoPrezzi.sacchetto;
        }

        if (ts == TipoSpecifico.durata)
            switch (t)
            {
                case DettagliPotenziamento.Tipo.dannoPowerUp:
                    return ListinoPrezzi.durataDannoPowerUp;
                case DettagliPotenziamento.Tipo.tempoPowerUp:
                    return ListinoPrezzi.durataTempoPowerUp;
                case DettagliPotenziamento.Tipo.salutePowerUp:
                    return ListinoPrezzi.durataSalutePowerUp;
            }
        else if (ts == TipoSpecifico.potenza)
            switch (t)
            {
                case DettagliPotenziamento.Tipo.dannoPowerUp:
                    return ListinoPrezzi.potenzaDannoPowerUp;
                case DettagliPotenziamento.Tipo.tempoPowerUp:
                    return ListinoPrezzi.potenzaTempoPowerUp;
                case DettagliPotenziamento.Tipo.salutePowerUp:
                    return ListinoPrezzi.potenzaSalutePowerUp;
            }
        if (ts == TipoSpecifico.quantità)
            switch (t)
            {
                case DettagliPotenziamento.Tipo.dannoPowerUp:
                    return ListinoPrezzi.quantitàDannoPowerUp;
                case DettagliPotenziamento.Tipo.tempoPowerUp:
                    return ListinoPrezzi.quantitàTempoPowerUp;
                case DettagliPotenziamento.Tipo.salutePowerUp:
                    return ListinoPrezzi.quantitàSalutePowerUp;
            }
        return null;
    }

    public List<int> valori
    {
        get { return ValoreDaTipo(tipo, tipoSpecifico); }
    }

    public static List<int> ValoreDaTipo(DettagliPotenziamento.Tipo t, TipoSpecifico ts = TipoSpecifico.potenza)
    {
        switch (t)
        {
            case DettagliPotenziamento.Tipo.cuore:
                return ListinoValori.cuori;
            case DettagliPotenziamento.Tipo.sacchetto:
                return ListinoValori.sacchetto;
        }

        if (ts == TipoSpecifico.durata)
            switch (t)
            {
                case DettagliPotenziamento.Tipo.dannoPowerUp:
                    return ListinoValori.durataDannoPowerUp;
                case DettagliPotenziamento.Tipo.tempoPowerUp:
                    return ListinoValori.durataTempoPowerUp;
                case DettagliPotenziamento.Tipo.salutePowerUp:
                    return ListinoValori.durataSalutePowerUp;
            }
        else if (ts == TipoSpecifico.potenza)
            switch (t)
            {
                case DettagliPotenziamento.Tipo.dannoPowerUp:
                    return ListinoValori.potenzaDannoPowerUp;
                case DettagliPotenziamento.Tipo.tempoPowerUp:
                    return ListinoValori.potenzaTempoPowerUp;
                case DettagliPotenziamento.Tipo.salutePowerUp:
                    return ListinoValori.potenzaSalutePowerUp;
            }
        else if (ts == TipoSpecifico.quantità)
            switch (t)
            {
                case DettagliPotenziamento.Tipo.dannoPowerUp:
                    return ListinoValori.quantitàDannoPowerUp;
                case DettagliPotenziamento.Tipo.tempoPowerUp:
                    return ListinoValori.quantitàTempoPowerUp;
                case DettagliPotenziamento.Tipo.salutePowerUp:
                    return ListinoValori.quantitàSalutePowerUp;
            }
        return null;
    }

    public static Elemento Clona(Elemento campo)
    {
        return new Elemento() { tipoSpecifico = campo.tipoSpecifico, tipo = campo.tipo, nome = campo.nome, descrizione = campo.descrizione, immagine = campo.immagine, livello = campo.livello };
    }
}

public class ListinoPrezzi
{
    public static List<int> cuori = new List<int>{ 10, 35, 60, 100, 250};
    public static List<int> sacchetto = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> durataSalutePowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> durataDannoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> durataTempoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> potenzaSalutePowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> potenzaDannoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> potenzaTempoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> quantitàSalutePowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> quantitàDannoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> quantitàTempoPowerUp = new List<int> { 10, 35, 60, 100, 250 };

    public static int Totale(int c, int livello = -1)
    {
        int t = 0;
        if (livello == -1)
        for (int i = 0; i < 5; i++)
            t += cuori[i] * c + sacchetto[i] + durataSalutePowerUp[i] + durataDannoPowerUp[i] + durataTempoPowerUp[i] + potenzaSalutePowerUp[i] + potenzaDannoPowerUp[i] + potenzaTempoPowerUp[i];
        else
            t += cuori[livello] * c + sacchetto[livello] + durataSalutePowerUp[livello] + durataDannoPowerUp[livello] + durataTempoPowerUp[livello] + potenzaSalutePowerUp[livello] + potenzaDannoPowerUp[livello] + potenzaTempoPowerUp[livello];
        return t;
    }
}

public class ListinoValori
{
    public static List<int> cuori = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> sacchetto = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> durataSalutePowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> durataDannoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> durataTempoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> potenzaSalutePowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> potenzaDannoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> potenzaTempoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> quantitàSalutePowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> quantitàDannoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
    public static List<int> quantitàTempoPowerUp = new List<int> { 10, 35, 60, 100, 250 };
}