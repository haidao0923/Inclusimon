using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModeSelector : MonoBehaviour
{
    [SerializeField] SimonGame simonGame;
    [SerializeField] Text titleText;
    [SerializeField] Image[] buttonImages;
    [SerializeField] Sprite[] normalSprites, colorblindSprites;
    [SerializeField] GameObject blindModePanel, reviewGameObject;
    [SerializeField] Button startButton, openChangeModeButton;
    [SerializeField] GameObject changeModePanel;
    [SerializeField] Button[] selectModeButton;
    void Start()
    {
        openChangeModeButton.onClick.AddListener(() => OpenModeMenu());
        for (int i = 0; i < selectModeButton.Length; i++) {
            int index = i;
            selectModeButton[i].onClick.AddListener(() => ClickModeButton(index));
        }
    }
    void OpenModeMenu() {
        changeModePanel.SetActive(true);
        StopAllCoroutines();
        Mode.Instance.isChangingMode = true;
        simonGame.Fail();
    }
    void ClickModeButton(int index)
    {
        blindModePanel.SetActive(false);
        reviewGameObject.SetActive(false);
        switch (index) {
        case 0:
            Mode.Instance.modeType = Mode.ModeType.Normal;
            for (int i = 0; i < 4; i++) {
                buttonImages[i].sprite = normalSprites[i];
            }
            break;
        case 1:
            Mode.Instance.modeType = Mode.ModeType.Colorblind;
            for (int i = 0; i < 4; i++) {
                buttonImages[i].sprite = colorblindSprites[i];
            }
            break;
        case 2:
            Mode.Instance.modeType = Mode.ModeType.Blind;
            blindModePanel.SetActive(true);
            break;
        case 3:
            Mode.Instance.modeType = Mode.ModeType.Deaf;
            break;
        case 4:
            Mode.Instance.modeType = Mode.ModeType.SCT;
            break;
        case 5:
            Mode.Instance.modeType = Mode.ModeType.MemoryDeficit;
            reviewGameObject.SetActive(true);
            break;
        }
        changeModePanel.SetActive(false);
        titleText.text = Mode.Instance.modeType + " Mode";
        startButton.interactable = true;
        Mode.Instance.isChangingMode = false;
    }
}
