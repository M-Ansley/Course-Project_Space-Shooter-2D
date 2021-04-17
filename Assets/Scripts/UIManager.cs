using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textBox = null;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _liveSprites; // 2 = 2 lives, 1 = 1 life, 0 = no life

    private int _maxLives = 3;
    private int _lives = 3;
    private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
        UpdatePoints(0);
    }


    // *************************************************************************************
    // POINTS

    private void UpdatePoints(int increment)
    {
        _score += increment;
        textBox.text = "Score: " + _score.ToString();
    }

    private void EnemyDestroyed(int score)
    {
        UpdatePoints(score);
    }

    // *************************************************************************************
    // LIVES

    private void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    private void RestoreHealth()
    {
        if (_lives < _maxLives)
        {
            _lives++;
            UpdateLives(_lives);
            print("Should restore health");
        }
    }

    private void PlayerDamaged()
    {
        _lives--;
        UpdateLives(_lives);
    }

    private void PlayerDied()
    {
        _lives--;
        UpdateLives(_lives);
    }

    private void ListenToEvents()
    {
        GameEvents.current.playerDamaged += PlayerDamaged;
        GameEvents.current.playerDied += PlayerDied;
        GameEvents.current.playerKill.AddListener(EnemyDestroyed);
        GameEvents.current.restoreHealth += RestoreHealth;
    }
}
