using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{



    // Start is called before the first frame update
    [Range(30, 60)] public int FPSLimit = 60;
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;
    [SerializeField] private TextMeshProUGUI UiFps;
    void Awake()
    {
        Application.targetFrameRate = FPSLimit;
        frameDeltaTimeArray = new float[50];
    }

    void OnValidate()
    {
        Application.targetFrameRate = FPSLimit;
    }

    private void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
        UiFps.text = Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach(float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }


}
