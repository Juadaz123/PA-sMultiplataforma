using System;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
public class ButtonAnimator : MonoBehaviour
{
    [Header("Animation Parameters")]
    [SerializeField] private float animationDuration = 1f;
    [HideInInspector] public Vector2 originalPosition;
    
    private RectTransform _buttonRectTransform;

    private void Awake()
    {
        _buttonRectTransform  = transform as RectTransform;
        originalPosition = _buttonRectTransform.anchoredPosition;
    }

    public async void MoveX(float valueX)
    {
        await(AMove(Vector2.left * valueX, animationDuration));
    }

    public async Task AMove(Vector2 target, float duration)
    {
        float t = 0f;
        Vector2 initialPosition = _buttonRectTransform.anchoredPosition;
        while (t < duration)
        {
            t += Time.deltaTime;
            _buttonRectTransform.anchoredPosition = Vector2.Lerp(initialPosition, target, t / duration);
            await Task.Yield();
        }
        _buttonRectTransform.anchoredPosition = target; // por si acaso
    }

}
