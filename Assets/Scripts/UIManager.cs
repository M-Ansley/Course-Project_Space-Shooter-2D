using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textBox = null;

    private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
        UpdatePoints(0);
    }
 
    private void UpdatePoints(int increment)
    {
        _score += increment;
        textBox.text = "Score: " + _score.ToString();
    }

    private void EnemyDestroyed(int score)
    {
        UpdatePoints(score);
    }


    private void ListenToEvents()
    {

        GameEvents.current.playerKill.AddListener(EnemyDestroyed);
    }
}
