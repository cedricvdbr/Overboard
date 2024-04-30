using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField]
    private Image _currentImage;

    [SerializeField]
    private Sprite[] Images;

    private int _currentImageIndex;

    // Start is called before the first frame update
    void Start()
    {
        _currentImage = GetComponent<Image>();
    }

    public void SetCurrentImage(int indexNr)
    {
        _currentImageIndex = indexNr;
    }

    private void ChangeImage()
    {
        _currentImage.sprite = Images[_currentImageIndex];
    }

}
