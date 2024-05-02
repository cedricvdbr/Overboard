using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelController : MonoBehaviour
{

    private GameObject _currentPanel;

    [SerializeField]
    private GameObject[] Panels;

    [HideInInspector]
    public int _currentPanelIndex;
    private int _previousPanelIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        _currentPanel = Panels[0];
    }

    void Update()
    {
        if (_currentPanelIndex != _previousPanelIndex)
        {
            Debug.Log("Changing panel: " + _currentPanelIndex);
            ChangePanel();
            _previousPanelIndex = _currentPanelIndex;
        }
    }

    public void SetCurrentPanel(int indexChange)
    {
        _currentPanelIndex += indexChange;
        if (_currentPanelIndex >= Panels.Length) _currentPanelIndex = 0;
        if (_currentPanelIndex < 0) _currentPanelIndex = Panels.Length - 1;
    }

    private void ChangePanel()
    {
        _currentPanel = Panels[_currentPanelIndex];
        Panels[_currentPanelIndex].SetActive(true);
        for (int i = 0; i < Panels.Length; i++)
        {
            if (i == _currentPanelIndex) continue;
            GameObject panel = Panels[i];
            panel.SetActive(false);
        }
    }
}
