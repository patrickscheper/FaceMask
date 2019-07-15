using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureWindowManager : MonoBehaviour
{
    public RectTransform shareWindowTransform;
    public RawImage screenshotImage;

    public ScanningAnimation scanningAnimation;
    public CameraManager cameraManager;

    public Button[] buttons;

    public float animationTime;
    public AnimationCurve animationCurve;

    void Start()
    {
        shareWindowTransform.sizeDelta = new Vector2(1080, 1920);
        shareWindowTransform.anchoredPosition = new Vector2(0, -Screen.height);
        ActivateButtons(false);
    }

    private void OnEnable()
    {
        cameraManager.OnCameraSavedImage += ShowWindow;
    }

    private void OnDisable()
    {
        cameraManager.OnCameraSavedImage -= ShowWindow;
    }

    public void Share()
    {
        new NativeShare().AddFile(cameraManager.currentPath).SetSubject("Face Mask Application").SetText("").Share();
    }

    public void ShowWindow()
    {
        StartCoroutine(MoveWindow(true, animationTime));
    }

    public void CloseWindow()
    {
        ActivateButtons(false);
        cameraManager.mainUITransform.gameObject.SetActive(true);
        scanningAnimation.ResetPosition();
        StartCoroutine(MoveWindow(false, animationTime));
    }

    public IEnumerator MoveWindow(bool up, float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            shareWindowTransform.anchoredPosition = new Vector2(0, Mathf.Lerp(up ? -Screen.height : 0,
                up ? 0 : -Screen.height, animationCurve.Evaluate(elapsedTime / time)));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        shareWindowTransform.anchoredPosition = new Vector2(0, up ? 0 : -Screen.height);

        if (up)
            ActivateButtons(true);
    }

    private void ActivateButtons(bool active)
    {
        foreach (Button button in buttons)
            button.interactable = active;
    }
}
