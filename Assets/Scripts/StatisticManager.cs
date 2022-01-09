using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticManager : SingletonMB<StatisticManager>
{
    [SerializeField] private TextMeshProUGUI PrefabShootedText;
    [SerializeField] private TextMeshProUGUI BulletShootedText;
    [SerializeField] private TextMeshProUGUI AccuracyText;

    private int prefabShooted;
    private int BulletShooted;
    private float Accuracy;

    public void UpdateUI(bool _prefabShooted = false, bool _bulletShooted = false)
    {
        if(_bulletShooted)
        {
            BulletShooted++;
            BulletShootedText.text = BulletShooted.ToString();
        }
        if (_prefabShooted)
        {
            prefabShooted++;

            PrefabShootedText.text = prefabShooted.ToString();
        }
        Accuracy = ((float)prefabShooted / (float)BulletShooted)*100;

        AccuracyText.text = Accuracy.ToString() + " %";

    }


}
