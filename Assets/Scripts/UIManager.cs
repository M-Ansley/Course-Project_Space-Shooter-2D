using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable 0649
public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _fadePanel;

    [SerializeField]
    private TextMeshProUGUI _waveDisplay = null;

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
    // WAVES
    public IEnumerator DisplayWave(int waveNumber, float displayDuration)
    {
        _waveDisplay.text = "WAVE " + waveNumber;
        yield return new WaitForSecondsRealtime(displayDuration);
        _waveDisplay.text = "";
    }


    // *************************************************************************************
    // OUTRO
    public IEnumerator Outro()
    {
        _waveDisplay.text = "YOU WON";
        yield return new WaitForSecondsRealtime(3f);

        float elapsedTime = 0;
        float fadeDuration = 5f;

        Color startColor = _fadePanel.color;

        while (_fadePanel.color.a != 1)
        {
            float alphaVal = Mathf.Lerp(0, 1, (elapsedTime / fadeDuration));
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alphaVal);
            _fadePanel.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3f);

        //Application.Quit();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
