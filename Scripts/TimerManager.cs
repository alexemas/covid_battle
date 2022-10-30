using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TimerImage;
    [SerializeField]
    private GameObject finalScorePanel;
    private Image _timerImage;
    private Text _timerText;

    protected Canvas _HUD;

    private float _starterTime;
    private float _time;
    private float _timeLeft;
    private float _fastTime;

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartTimer()
    {
        Time.timeScale = 1;
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            UpdateTimeText(_timeLeft);
            yield return null;
        }
    }

    private IEnumerator StarterTimer()
    {
        _fastTime = _starterTime;
        var Imgtimer = Instantiate(TimerImage, new Vector3(0, 0, 0), Quaternion.identity);
        Imgtimer.transform.SetParent(_HUD.transform, false);
        _timerImage = GameObject.Find("TimerImage(Clone)").GetComponent<Image>();
        Time.timeScale = 0;
        while (_fastTime > 0)
        {
            _fastTime -= Time.fixedDeltaTime;
            var normalizedValue = Mathf.Clamp(_fastTime / _starterTime, 0.0f, 1.0f);
            _timerImage.fillAmount = normalizedValue;
            yield return null;
        }
        Destroy(Imgtimer);
        StartCoroutine(StartTimer());
    }

    private void Start()
    {
        var controller = GetComponent<PlayerController>();
        _HUD = GameObject.FindWithTag(TagEnum.HUD.ToString()).GetComponent<Canvas>();
        _timerText = GameObject.Find("Timer").GetComponent<Text>();

        _starterTime = controller.StarterTime;
        _time = controller.Time;
        _timeLeft = _time;
        UpdateTimeText(_timeLeft);
        StartCoroutine(StarterTimer());
    }

    private void UpdateTimeText(float time)
    {
        if (time < 0)
        {
            time = 0;
            finalScorePanel.SetActive(true);
        }

        if (time < 0 && finalScorePanel.activeSelf)
        {
            time = 0;
            Time.timeScale = 0;
        }

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
