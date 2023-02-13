using System.Collections.Generic;
using UnityEngine;

public class EnemyPointersPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform _enemyPointerTemplate;

    List<RectTransform> _pointers = new();

    int _pointerIndex;

    public void AddPointer(Vector3 viewPortPosition, float scale)
    {
        var pointer = GetPointer();
        pointer.gameObject.SetActive(true);
        pointer.anchorMin = viewPortPosition;
        pointer.anchorMax = viewPortPosition;
        pointer.rotation = Quaternion.LookRotation(Vector3.forward, viewPortPosition - Vector3.one * 0.5f);
        pointer.localScale = Vector3.one * scale;
    }

    private RectTransform GetPointer()
    {
        if (_pointerIndex < _pointers.Count)
        {
            return _pointers[_pointerIndex++];
        }

        var pointer = Instantiate(_enemyPointerTemplate, transform);
        pointer.gameObject.SetActive(true);
        _pointers.Add(pointer);
        _pointerIndex++;
        return pointer;
    }

    public void ResetPointers()
    {
        _pointerIndex = 0;
    }

    public void HideUnusedPointers()
    {
        for (int i = _pointerIndex; i < _pointers.Count; i++)
        {
            _pointers[i].anchorMin = -1000 * Vector2.one;
            _pointers[i].anchorMax = -1000 * Vector2.one;
        }
    }
}