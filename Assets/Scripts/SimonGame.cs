 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonGame : MonoBehaviour
{
    [SerializeField] Button startButton, reviewButton;
    [SerializeField] Button[] buttons;
    [SerializeField] Image[] buttonImages;
    [SerializeField] Animator[] buttonAnimators;
    [SerializeField] AudioSource[] buttonSounds;
    [SerializeField] AudioSource loseSound;
    [SerializeField] Text scoreText, highscoreText;
    List<int> patternArray = new List<int>();
    List<int> inputArray = new List<int>();
    int lastLightUpIndex;
    int currentIndex;


    void Start()
    {
        scoreText.text = string.Format("Score:\n{0}", ScoreManager.Instance.score);
        Debug.Log((int) Mode.Instance.modeType);
        highscoreText.text = string.Format("Highscore:\n{0}", ScoreManager.Instance.highscore[(int) Mode.Instance.modeType]);
        startButton.onClick.AddListener(() => ClickStartButton());
        reviewButton.onClick.AddListener(() => StartCoroutine(ClickReviewButton()));
        for (int i = 0; i < buttons.Length; i++) {
            int index = i;
            buttons[i].onClick.AddListener(() => StartCoroutine(ClickButton(index)));
        }
    }

    void Update() {
        if (Mode.Instance.modeType == Mode.ModeType.Blind) {
            BlindModeClickButton();
        }
    }

    void ClickStartButton() {
        StartCoroutine(GetPattern(ScoreManager.Instance.score));
        startButton.interactable = false;
    }

    IEnumerator ClickReviewButton() {
        SetInteractableAllButtons(false);
        for (int i = 0; i < patternArray.Count && !Mode.Instance.isChangingMode; i++) {
            yield return LightUpButton(patternArray[i], 1);
        }
        if (!Mode.Instance.isChangingMode) {
            SetInteractableAllButtons(true);
        }
    }

    IEnumerator GetPattern(int amount) {
        patternArray.Clear();
        inputArray.Clear();
        Debug.Log("GetPattern" + currentIndex);
        currentIndex = -1;
        SetInteractableAllButtons(false);
        yield return new WaitForSeconds(3);
        for (int i = -1; i < amount && !Mode.Instance.isChangingMode; i++) {
            yield return LightUpRandomButton();
        }
        if (!Mode.Instance.isChangingMode) {
            SetInteractableAllButtons(true);
        }
    }

    public void SetInteractableAllButtons(bool boolean) {
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = boolean;
        }
        reviewButton.interactable = boolean;
    }

    IEnumerator ClickButton(int index) {
        Debug.Log(currentIndex);
        inputArray.Add(index);
        currentIndex += 1;
        if (inputArray[currentIndex] != patternArray[currentIndex]) {
            Fail();
        } else if (currentIndex + 1 == patternArray.Count) {
            Success();
        }
        float speed = 1;
        if (Mode.Instance.modeType == Mode.ModeType.SCT) {
            speed = 0.5f;
        }
        yield return LightUpButton(index, speed);
    }

    void BlindModeClickButton() {
        bool buttonsInteractable = buttons[0].interactable;
        if (Input.GetKeyDown(KeyCode.Q)) {
            buttonSounds[0].Play();
        } else if (Input.GetKeyDown(KeyCode.W)) {
            buttonSounds[1].Play();
        } else if (Input.GetKeyDown(KeyCode.E)) {
            buttonSounds[2].Play();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            buttonSounds[3].Play();
        } else if (buttonsInteractable && Input.GetKeyDown(KeyCode.A)) {
            StartCoroutine(ClickButton(0));
        } else if (buttonsInteractable && Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(ClickButton(1));
        } else if (buttonsInteractable && Input.GetKeyDown(KeyCode.D)) {
            StartCoroutine(ClickButton(2));
        } else if (buttonsInteractable && Input.GetKeyDown(KeyCode.F)) {
            StartCoroutine(ClickButton(3));
        } else if (Input.GetKeyDown(KeyCode.G) && startButton.interactable == true) {
            ClickStartButton();
        }
    }

    public void Fail() {
        loseSound.Play();
        ScoreManager.Instance.score = 0;
        scoreText.text = string.Format("Score:\n{0}", ScoreManager.Instance.score);
        SetInteractableAllButtons(false);
        startButton.interactable = true;
    }
    private void Success() {
        ScoreManager.Instance.score += 1;
        if (ScoreManager.Instance.score > ScoreManager.Instance.highscore[(int) Mode.Instance.modeType]) {
            ScoreManager.Instance.highscore[(int) Mode.Instance.modeType] = ScoreManager.Instance.score;
        }
        scoreText.text = string.Format("Score:\n{0}", ScoreManager.Instance.score);
        highscoreText.text = string.Format("Highscore:\n{0}", ScoreManager.Instance.highscore[(int) Mode.Instance.modeType]);
        StartCoroutine(GetPattern(ScoreManager.Instance.score));
    }

    IEnumerator LightUpRandomButton() {
        int random = Random.Range(0, 4);
        patternArray.Add(random);
        float speed = 1;
        if (Mode.Instance.modeType == Mode.ModeType.SCT) {
            speed = 0.5f;
        }
        yield return LightUpButton(random, speed);
    }

    IEnumerator LightUpButton(int index, float speed) {
        buttonAnimators[lastLightUpIndex].Rebind();
        buttonAnimators[index].speed = speed;
        //Mode.Instance.GetAnimationStringForCurrentMode(Mode.Instance.modeType)
        buttonAnimators[index].Play("Normal_Light_Up");
        if (Mode.Instance.modeType != Mode.ModeType.Deaf) {
            buttonSounds[index].Play();
        }
        lastLightUpIndex = index;
        yield return new WaitForSeconds(1.5f / speed);
    }
}
