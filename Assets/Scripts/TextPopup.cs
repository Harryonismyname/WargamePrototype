using PolearmStudios.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour, IPooledObject
{
    [SerializeField] float lifespan = 5;
    TextMeshPro _textMeshPro;
    Timer _timer;
    Vector3 cameraDir;
    bool isSpawned;

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
        _timer.Update(Time.deltaTime);
        _textMeshPro.alpha = _timer.TimeElapsed / (_timer.Duration - (lifespan * .9f));
        cameraDir = (Camera.main.transform.position - transform.position).normalized;
        transform.position += cameraDir * Time.deltaTime;
        transform.forward = cameraDir;
    }
}

public interface IPooledObject
{
    public void OnObjectSpawn();
}