using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject[] imageObjects;
    [Header("Colors")]
    public Color selectMateral;
    public Color normalMaterial;
    public Color transMaterial;

    private RawImage[] objects;

    private void Awake()
    {
        objects = new RawImage[imageObjects.Length];
        for(int i = 0; i < imageObjects.Length; i++)
        {
            objects[i] = imageObjects[i].GetComponent<RawImage>();
        }
        InitImages();
    }

    public void InitImages()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].color = transMaterial;
        }
    }

    public void EnableImage(int index)
    {
        InitImages();
        objects[index].color = normalMaterial;
    }

    public void SelectImage(int index)
    {
        InitImages();
        objects[index].color = selectMateral;
    }
}
