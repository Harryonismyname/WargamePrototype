using PolearmStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour, IPooledObject
{
    [SerializeField] float lifespan = 5;
    [SerializeField] float fadeSpeed = 1.5f;
    TextMeshPro _textMeshPro;
    Timer _timer;
    Vector3 cameraDir;
    bool isSpawned;
    float delta;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _timer = new Timer.Builder(lifespan).WithAutoReset().Build();
        _timer.OnComplete += ResetTimer;
    }

    private void OnDestroy()
    {
        _timer.OnComplete -= ResetTimer;
    }

    public void OnObjectSpawn()
    {
        ResetTimer();
        StartPath();
        isSpawned = true;
    }

    public void SetText(string text)
    {
        _textMeshPro.text = text;
    }

    private void StartPath()
    {
        _timer.Start();
    }

    private void ResetTimer()
    {
        _timer.Reset();
        _textMeshPro.alpha = 0;
        isSpawned = false;
    }

    private void Update()
    {
        if (!isSpawned) return;

        delta = (_timer.Duration - (_timer.TimeElapsed * fadeSpeed)) / _timer.Duration;
        _textMeshPro.alpha = delta;
        cameraDir = (Camera.main.transform.position - transform.position).normalized;
        transform.position += cameraDir * Time.deltaTime;
        transform.forward = cameraDir;
        transform.up = Camera.main.transform.up;
        _timer.Update(Time.deltaTime);
    }
}

public interface IPooledObject
{
    public void OnObjectSpawn();
}