using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class finePartita : MonoBehaviour
{
    [Header("Uova")]
    public Animator[] uovaAn;
    public GameObject[] uovaRotte;
    public Sprite[] uovaRotteSp;
    public int uova;
    public int[] rotturaUova = new int[] { 0, 0, 0 };
    public int[] tocchiMassimiUova;

    public void FinePartita()
    {
        gameObject.SetActive(true);
        uova = 3;
        StartCoroutine(MostraUovo(uova));
    }

    IEnumerator MostraUovo(int index)
    {
        for (int i = 0; i < index; i++)
        {
            yield return new WaitForSeconds(.5f);
            uovaAn[i].gameObject.SetActive(true);
            //uovaAn[i].SetTrigger("vai");
        }
    }

    public void ApriUovo(int index)
    {
        rotturaUova[index]++;
        if (tocchiMassimiUova[index] != rotturaUova[index])
        {
            uovaAn[index].GetComponent<Image>().sprite = uovaRotteSp[rotturaUova[index]];
            uovaAn[index].SetTrigger("suc");
        }
        else
        {
            uovaRotte[index].SetActive(true);
            uovaAn[index].gameObject.SetActive(false);
        }
    }
}