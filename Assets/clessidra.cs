using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

public class clessidra : MonoBehaviour
{
	[SerializeField] Image fillTopImage;
	[SerializeField] Image fillBottomImage;
	[SerializeField] Text roundText;
	[SerializeField] Image sandDotsImage;
	[SerializeField] RectTransform sandPyramidRect;

	//Events
	[HideInInspector] public UnityAction onAllRoundsCompleted;
	[HideInInspector] public UnityAction<int> onRoundStart;
	[HideInInspector] public UnityAction<int> onRoundEnd;

	[Space(30f)]
	public float roundDuration = 10f;
	public int totalRounds = 3;

	float defaultSandPyramidYPos, tempoRimanente;
	int currentRound = 0;

	void Awake()
	{
		defaultSandPyramidYPos = sandPyramidRect.anchoredPosition.y;
		sandDotsImage.DOFade(0f, 0f);
		roundDuration += 1.3f;
	}

	public void Begin(float tempo)
	{
		roundDuration = tempo + 1.3f;
		roundText.DOFade(0f, 0f);
		transform
			.DORotate(Vector3.zero, .8f, RotateMode.FastBeyond360)
			.From(Vector3.forward * 180f)
			.SetEase(Ease.InOutBack)
			.SetUpdate(true);
		tempoRimanente = roundDuration;
		StartCoroutine(ScalaTempo());
		GetComponent<AudioSource>().Play();
		++currentRound;

		//start event
		if (onRoundStart != null)
			onRoundStart.Invoke(currentRound);


		sandDotsImage.DOFade(1f, .8f).SetUpdate(true);
		sandDotsImage.material.DOOffset(Vector2.down * -roundDuration, roundDuration).From(Vector2.zero).SetEase(Ease.Linear).SetUpdate(true);

		//Scale Pyramid
		sandPyramidRect.DOScaleY(1f, roundDuration / 3f).From(0f).SetUpdate(true);
		sandPyramidRect.DOScaleY(0f, roundDuration / 1.5f).SetDelay(roundDuration / 3f).SetEase(Ease.Linear).SetUpdate(true);

		sandPyramidRect.anchoredPosition = Vector2.up * defaultSandPyramidYPos;
		sandPyramidRect.DOAnchorPosY(0f, roundDuration).SetEase(Ease.Linear).SetUpdate(true);

		ResetClock();

		roundText.DOFade(1f, .8f).SetUpdate(true);

		fillTopImage
			.DOFillAmount(0, roundDuration)
			.SetEase(Ease.Linear)
			.OnUpdate(OnTimeUpdate)
			.SetUpdate(true);
	}

	void OnTimeUpdate()
	{
		fillBottomImage.fillAmount = 1f - fillTopImage.fillAmount;
	}

	public void ResetClock()
	{
		transform.rotation = Quaternion.Euler(Vector3.zero);
		fillTopImage.fillAmount = 1f;
		fillBottomImage.fillAmount = 0f;
	}

	IEnumerator ScalaTempo()
    {
		GetComponent<CanvasGroup>().alpha = 1;
		tempoRimanente -= 1.3f;
		for (; tempoRimanente > 0.09; )
        {
			tempoRimanente -= .1f;
			roundText.text = tempoRimanente.ToString("F1");
			yield return new WaitForSecondsRealtime(.1f);
        }
		roundText.DOFade(0f, .5f);
		GetComponent<CanvasGroup>().alpha = .7f;
	}
}
