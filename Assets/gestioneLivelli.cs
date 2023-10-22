using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gestioneLivelli : MonoBehaviour
{
    public string nomeBundle, nomeAsset;
    public RectTransform barraCaricamento;
    public Text percCaricamneto;
    public float velocitàCamera = 1, posizioneInizialeCamera = 0;
    public Shader acquaShader, terrenoShader;
    public Sprite immagineSfondo;

    void Awake()
    {
        movimento.velocitàCamera = velocitàCamera;
        movimento.posizioneInizialeCamera = posizioneInizialeCamera;
        if (!impostazioni.livelloDiProva)
        {
            if (impostazioni.tipoSfondo == impostazioni.TipiSfondo.dinamico3D)
                StartCoroutine(CaricaLivello3D());
            else
                StartCoroutine(CaricaLivello2D());
        }
        else
            StartCoroutine(CaricaLivello2D());
    }

    IEnumerator CaricaLivello3D()
    {
        AssetBundleCreateRequest bundleLoadRequest;
        bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, nomeBundle));

        Coroutine cor = StartCoroutine(MostraProgresso(bundleLoadRequest, 0));
        yield return bundleLoadRequest;
        StopCoroutine(cor);

        var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        print("iniz");
        var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(nomeAsset);
        yield return assetLoadRequest;
        print("fatto");
        GameObject prefab = (GameObject)assetLoadRequest.asset;
        if (acquaShader != null)
        {
            Transform p = Instantiate(prefab).transform;
            p.GetChild(3).GetChild(1).GetChild(20).GetChild(0).GetComponent<MeshRenderer>().material.shader = acquaShader;
            p.GetChild(2).GetComponent<Terrain>().materialTemplate.shader = terrenoShader;
        }
        else
            Instantiate(prefab);
        myLoadedAssetBundle.Unload(false);
        AsyncOperation loaderScene = SceneManager.LoadSceneAsync(SceneManager.sceneCountInBuildSettings - 2, LoadSceneMode.Additive);
        cor = StartCoroutine(MostraProgresso(loaderScene, 1));
        yield return loaderScene;
        StopCoroutine(cor);
        loaderScene = SceneManager.LoadSceneAsync(SceneManager.sceneCountInBuildSettings - 1, LoadSceneMode.Additive);
        cor = StartCoroutine(MostraProgresso(loaderScene, 1));
        yield return loaderScene;
        StopCoroutine(cor);
        Destroy(gameObject);
    }

    IEnumerator CaricaLivello2D()
    {
        if (impostazioni.tipoSfondo == impostazioni.TipiSfondo.immagineStatica2D && !impostazioni.livelloDiProva)
            impostazioniInGame.sfondo = immagineSfondo;
        AsyncOperation loaderScene = SceneManager.LoadSceneAsync(SceneManager.sceneCountInBuildSettings - 1, LoadSceneMode.Additive);
        Coroutine cor = StartCoroutine(MostraProgresso(loaderScene, 1));
        yield return loaderScene;
        StopCoroutine(cor);
        loaderScene = SceneManager.LoadSceneAsync(SceneManager.sceneCountInBuildSettings - 2, LoadSceneMode.Additive);
        cor = StartCoroutine(MostraProgresso(loaderScene, 1));
        yield return loaderScene;
        StopCoroutine(cor);
        Destroy(gameObject);
    }

    IEnumerator MostraProgresso(AssetBundleCreateRequest car, int n)
    {
        for (; ; )
        {
            barraCaricamento.localScale = new Vector3(car.progress * .8f, 1, 1);
            percCaricamneto.text = (car.progress * .8f * 100).ToString("F0") + "%";
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator MostraProgresso(AsyncOperation car, int n)
    {
        for (; ; )
        {
            barraCaricamento.localScale = new Vector3(car.progress * .2f + .8f, 1, 1);
            percCaricamneto.text = (car.progress * .2f * 100 + 80).ToString("F0") + "%";
            yield return new WaitForSeconds(.1f);
        }
    }
}