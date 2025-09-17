using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsText;
    public float updateInterval = 0.5f; // Actualizar cada 0.5 segundos
    private float accum = 0f;
    private int frames = 0;
    private float timeLeft;
    private float currentFPS;
    private float averageFPS;
    private float minFPS = Mathf.Infinity;
    private float maxFPS = 0;
    private List<float> fpsList = new List<float>();
    private int averageFrameCount = 60; // Número de frames para promediar

    void Start()
    {
        timeLeft = updateInterval;
        if (fpsText == null)
        {
            Debug.LogError("FPSCounter: No se asignó el componente TextMeshProUGUI.");
        }
    }

    void Update()
    {
        // Cálculo de FPS actual
        currentFPS = 1.0f / Time.unscaledDeltaTime;
        accum += currentFPS;
        frames++;
        fpsList.Add(currentFPS);

        // Actualizar mínimos y máximos
        if (currentFPS < minFPS) minFPS = currentFPS;
        if (currentFPS > maxFPS) maxFPS = currentFPS;

        // Promedio de FPS en los últimos 'averageFrameCount' frames
        if (fpsList.Count > averageFrameCount)
        {
            fpsList.RemoveAt(0);
        }
        float sum = 0;
        foreach (float fps in fpsList)
        {
            sum += fps;
        }
        averageFPS = sum / fpsList.Count;

        // Actualizar texto cada 'updateInterval' segundos
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.0f)
        {
            string fpsDisplay = $"FPS Actual: {Mathf.RoundToInt(currentFPS)}\n" +
                                $"Promedio: {Mathf.RoundToInt(averageFPS)}\n" +
                                $"Mínimo: {Mathf.RoundToInt(minFPS)}\n" +
                                $"Máximo: {Mathf.RoundToInt(maxFPS)}";
            fpsText.text = fpsDisplay;
            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}