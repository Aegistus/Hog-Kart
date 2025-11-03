using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostCooldownUI : MonoBehaviour
{
    [SerializeField] GameObject element;
    [SerializeField] RawImage boostBar;

    CarController car;

    private void Awake()
    {
        car = FindAnyObjectByType<CarController>();
    }

    public void Update()
    {
        if (car.CurrentBoostCooldown <= 0)
        {
            element.SetActive(false);
        }
        else
        {
            if (!element.activeInHierarchy)
            {
                element.SetActive(true);
            }
            boostBar.rectTransform.localScale = new Vector3(1 - (car.CurrentBoostCooldown / car.BoostCooldown), 1, 1);
        }
    }
}
