using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour
{

    [SerializeField]
    private Camera mainCamera;

    private void ScaleObjects()
    {
        Vector2 deviceResolution = new Vector2(Screen.width, Screen.height);
        float aspectRatio = Screen.width / Screen.height;

        mainCamera.aspect = aspectRatio;



    }

    // Start is called before the first frame update
    void Start()
    {
        ScaleObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
