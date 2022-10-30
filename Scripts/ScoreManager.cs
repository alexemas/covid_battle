using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreLabel;
    
    private int _score = 0;

    private void Update()
    {
        scoreLabel.text = string.Format("{0}", _score.ToString());
    }
    
    public void AddScore(int score = 1)
    {;
        _score += score;
    }
}