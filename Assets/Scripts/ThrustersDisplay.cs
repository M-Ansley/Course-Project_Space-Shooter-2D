using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustersDisplay : MonoBehaviour
{
    [SerializeField]
    private Image _fillImage = null;

   public void SetFillImage(float fillValue)
    {
        _fillImage.fillAmount = fillValue;
    }
}
