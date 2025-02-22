using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeHandler : MonoBehaviour
{
    [SerializeField] private List<Image> _chargePills = new();
    private int _remainingCells;
    private void Start()
    {
        SetRemainingCell(PlayerPrefs.GetInt("RemainingCellCount", 3));
    }
    private void SetRemainingCell(int value)
    {
        _remainingCells = value;
        for (int i = 0; i < value; i++)
        {
            _chargePills[i].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Energy decreaing
    /// </summary>
    public void Decrease()
    {
        if (!EnoughCharge()) return;
        _remainingCells--;
    }
    /// <summary>
    /// Has enough energy
    /// </summary>
    /// <returns></returns>
    public bool EnoughCharge() => PlayerPrefs.GetInt("RemainingCellCount") > 0;
}