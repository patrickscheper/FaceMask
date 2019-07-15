using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class ScanningAnimation : MonoBehaviour
{
    private RectTransform rectTransform;

    public Vector2 maximizedSize;
    public RectTransform maximizedTarget;

    public Vector2 minimizedSize;
    public RectTransform minimizedTarget;

    private bool firstTime = true;
    public bool isTracking = false;

    public Image icon;

    public float animationTime = 1;
    public AnimationCurve animationCurve;

    public Animator animator;

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    private void Start()
    {
        icon = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        animator.SetBool("IsTracking", isTracking);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void MoveIcon(bool tracking)
    {
        if (tracking == isTracking && !firstTime)
            return;

        if (firstTime)
            firstTime = false;

        isTracking = tracking;
        animator.SetBool("IsTracking", isTracking);
        StopCoroutine("MoveAndResizeIcon");
        StartCoroutine(MoveAndResizeIcon(animationTime));
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = isTracking ? minimizedTarget.anchoredPosition : maximizedTarget.anchoredPosition;
        rectTransform.sizeDelta = isTracking ? minimizedSize : maximizedSize;
    }

    public IEnumerator MoveAndResizeIcon(float time)
    {
        Vector2 currentSizeDelta = rectTransform.sizeDelta;
        Vector2 currentPosition = rectTransform.anchoredPosition;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            rectTransform.sizeDelta = Vector2.Lerp(currentSizeDelta, isTracking ? minimizedSize : maximizedSize,
                animationCurve.Evaluate(elapsedTime / time));

            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, isTracking ?
                minimizedTarget.anchoredPosition : maximizedTarget.anchoredPosition,
                animationCurve.Evaluate(elapsedTime / time));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        rectTransform.anchoredPosition = isTracking ? minimizedTarget.anchoredPosition : maximizedTarget.anchoredPosition;
    }
}
