using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _deathDisplayText = null;

    [SerializeField]
    private GameObject _restartPromptObj = null;

    [SerializeField]
    private float _flickerDelay = 0.75f;
    
    [SerializeField]
    private string _displayText = "YOU DIED";

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
        _deathDisplayText.text = "";
        _restartPromptObj.SetActive(false);
    }

   private void ListenToEvents()
    {
        GameEvents.current.playerDied += PlayerDied;
    }

    private void PlayerDied()
    {
        _restartPromptObj.SetActive(true);
        StartCoroutine(TextFlicker());
    }

    IEnumerator TextFlicker()
    {
        while (true)
        {
            if (_deathDisplayText.text == "")
            {
                _deathDisplayText.text = _displayText;
            }
            else
            {
                _deathDisplayText.text = "";
            }
            yield return new WaitForSecondsRealtime(_flickerDelay);
        }
    }
}
