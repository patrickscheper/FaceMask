using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Texture2D currentTexture;
    public string currentPath;

    public Transform mainUITransform;

    public PictureWindowManager windowManager;

    public delegate void CameraSavedImage();
    public event CameraSavedImage OnCameraSavedImage;

    public void TakePicture()
    {
        StartCoroutine(SaveImage());
    }

    public IEnumerator SaveImage()
    {
        mainUITransform.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        currentTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        currentTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        currentTexture.Apply();

        string path = Path.Combine(Application.temporaryCachePath, "shared image.png");
        File.WriteAllBytes(path, currentTexture.EncodeToPNG());

        yield return new WaitForEndOfFrame();

        windowManager.screenshotImage.texture = currentTexture;
        currentPath = path;

        OnCameraSavedImage?.Invoke();
    }
}
