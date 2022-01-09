using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PoolSpawner : MonoBehaviour
{
    #region UI Region

    [Header("UI")]
    [Space(10)]

    [SerializeField] private TextMeshProUGUI PrefabCountText;
    [SerializeField] private TMP_InputField AmountInputField;
    [SerializeField] private Button ApplyButton;

    #endregion

    #region Private variables
    [SerializeField] private int SpawnedObjects;    
    [SerializeField] private int Amount;
    #endregion

    private Pooler Pooler;//instance of pooler

    private void Start()
    {
        Pooler = Pooler.Instance;

        ApplyButton.onClick.AddListener(() => ApplyUpdateToPool());

    }
    /// <summary>
    /// Update pools from pooler
    /// </summary>
    /// <param name="_tag"></param>
    /// <param name="_amount"></param>
    public void UpdateFromPool(string _tag, int _amount)
    {
        Pooler.UpdateFromPool(_tag, _amount);

        SpawnedObjects =  Pooler.GetActivePooledObjects(_tag);

        PrefabCountText.text = SpawnedObjects.ToString();
    }

    private void ApplyUpdateToPool()
    {
        if (int.TryParse(AmountInputField.text, out Amount))
        {
            UpdateFromPool("Cube", Amount);
        }
    }
}
