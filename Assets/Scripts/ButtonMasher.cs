using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMasher
{
    private ArrayList _keys;
    private KeyCode _currentKey;

    private float _keyFrameTimer;

    [SerializeField]
    private float SecondsBeforeRenew;

    public ButtonMasher(KeyCode key1, KeyCode key2, KeyCode key3, KeyCode key4)
    {
        _keys = new ArrayList();

        _keys.Add(key1);
        _keys.Add(key2);
        _keys.Add(key3);
        _keys.Add(key4);

        _currentKey = key1;
    }

    public void Update()
    {
        _keyFrameTimer += Time.deltaTime;

        if (_keyFrameTimer >= SecondsBeforeRenew)
        {
            int newKeyIndex = Random.Range(0, _keys.Count);
            _currentKey = (KeyCode)_keys[newKeyIndex];
        }
    }

    public KeyCode GetCurrentKey()
    {
        return _currentKey;
    }
}
