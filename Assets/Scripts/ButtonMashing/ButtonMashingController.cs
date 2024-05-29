using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMashingController : MonoBehaviour
{
    private ButtonMashBattle _buttonmashBattle;
    public Canvas _canvas;
    [SerializeField] private RectTransform _mashUI;
    [SerializeField] private Image[] images;

    private bool _startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        _mashUI = GameObject.Find("Middle").GetComponent<RectTransform>();
        _canvas = GameObject.Find("ButtonMashUI").GetComponent<Canvas>();
        _canvas.enabled = true;
        _buttonmashBattle = new ButtonMashBattle();
        images[0] = GameObject.Find("W").GetComponent<Image>();
        images[1] = GameObject.Find("A").GetComponent<Image>();
        images[2] = GameObject.Find("S").GetComponent<Image>();
        images[3] = GameObject.Find("D").GetComponent<Image>();
        images[4] = GameObject.Find("I").GetComponent<Image>();
        images[5] = GameObject.Find("J").GetComponent<Image>();
        images[6] = GameObject.Find("K").GetComponent<Image>();
        images[7] = GameObject.Find("L").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _mashUI.localPosition = new Vector3(_buttonmashBattle._currentBattleStatus * 27.5f, _mashUI.localPosition.y, _mashUI.localPosition.z);
        //if (Input.GetMouseButtonDown(1)) StartMashing();

        if (_startGame)
        {
            _buttonmashBattle.Update();

            _startGame = !_buttonmashBattle.IsGameDone();

            switch (_buttonmashBattle.GetCurrentKey1())
            {
                case KeyCode.W:
                    images[0].enabled = true;
                    images[1].enabled = false;
                    images[2].enabled = false;
                    images[3].enabled = false;
                    break;
                case KeyCode.A:
                    images[0].enabled = false;
                    images[1].enabled = true;
                    images[2].enabled = false;
                    images[3].enabled = false;
                    break;
                case KeyCode.S:
                    images[0].enabled = false;
                    images[1].enabled = false;
                    images[2].enabled = true;
                    images[3].enabled = false;
                    break;
                case KeyCode.D:
                    images[0].enabled = false;
                    images[1].enabled = false;
                    images[2].enabled = false;
                    images[3].enabled = true;
                    break;
            }
            switch (_buttonmashBattle.GetCurrentKey2())
            {
                case KeyCode.UpArrow:
                    images[4].enabled = true;
                    images[5].enabled = false;
                    images[6].enabled = false;
                    images[7].enabled = false;
                    break;
                case KeyCode.LeftArrow:
                    images[4].enabled = false;
                    images[5].enabled = true;
                    images[6].enabled = false;
                    images[7].enabled = false;
                    break;
                case KeyCode.DownArrow:
                    images[4].enabled = false;
                    images[5].enabled = false;
                    images[6].enabled = true;
                    images[7].enabled = false;
                    break;
                case KeyCode.RightArrow:
                    images[4].enabled = false;
                    images[5].enabled = false;
                    images[6].enabled = false;
                    images[7].enabled = true;
                    break;
            }


            if (_buttonmashBattle.IsGameDone())
            {
                Debug.Log("winner: " + _buttonmashBattle.GetGameWinner());
                foreach (Image img in  images)
                {
                    img.enabled = false;
                }
            }
        }
    }

    public void StartMashing()
    {
        _startGame = true;
    }

    public bool ButtonmashingIsDone()
    {
        return _buttonmashBattle.IsGameDone();
    }

    public int GetWinner()
    {
        return _buttonmashBattle.GetGameWinner();

    }
}
