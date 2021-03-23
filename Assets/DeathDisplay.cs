using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI deathDisplayText = null;

    private string displayText = "YOU DIED";

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
        deathDisplayText.text = "";
    }

   private void ListenToEvents()
    {
        GameEvents.current.playerDied += PlayerDied;
    }

    private void PlayerDied()
    {
        deathDisplayText.text = displayText;
    }
}
