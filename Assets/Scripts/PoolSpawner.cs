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

    [SerializeField] private Vector3 minPosition, MaxPosition;



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
    public void UpdateFromPool(string _tag, int _amount,Vector3? _minPosition, Vector3? _maxPosition)
    {
        Pooler.UpdateFromPool(_tag, _amount, _minPosition : _minPosition,_maxPosition : _maxPosition) ;

        SpawnedObjects =  Pooler.GetActivePooledObjects(_tag);

        PrefabCountText.text = SpawnedObjects.ToString();
    }

    private void ApplyUpdateToPool()
    {
        if (int.TryParse(AmountInputField.text, out Amount))
        {
            UpdateFromPool("Cube", Amount, minPosition,MaxPosition);
        }
    }
}
