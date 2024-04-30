using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMasher
{
    private ArrayList _keys;
    private KeyCode _currentKey;
    private float _currentNewKeyIndex = 30;
    ButtonMashBattle _parent;

    public ButtonMasher(KeyCode key1, KeyCode key2, KeyCode key3, KeyCode key4, ButtonMashBattle parent)
    {
        _keys = new ArrayList();

        _keys.Add(key1);
        _keys.Add(key2);
        _keys.Add(key3);
        _keys.Add(key4);

        _currentKey = key1;
        _parent = parent;
    }

    public void NewUpdate()
    {
        if (_currentNewKeyIndex !=  _parent.newKeyIndex)
        {
            _currentKey = (KeyCode)_keys[_parent.newKeyIndex];
        }

        _currentNewKeyIndex = _parent.newKeyIndex;
    }

    public KeyCode GetCurrentKey()
    {
        return _currentKey;
    }
}
