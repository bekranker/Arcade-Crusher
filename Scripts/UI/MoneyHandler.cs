using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _monetTMP;
    [SerializeField, Range(0, 10)] private float _earningEffectSize;
}