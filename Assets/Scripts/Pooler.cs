using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Pooler : SingletonMB<Pooler>
{

    public List<ObjectPoolItem> m_Pools;

    private Dictionary<string, ObjectPoolItem> m_poolDictionary;

    #region Public functions

    /// <summary>
    /// return amount of active objects in specific pool
    /// </summary>
    /// <param name="_tag">Tag of ObjectPoolItem</param>
    /// <returns></returns>
    public int GetActivePooledObjects(string _tag)
    {
        return m_poolDictionary[_tag].m_ActiveObjects;
    }

    /// <summary>
    /// Spawn or despawn objects from pool
    /// </summary>
    /// <param name="_tag">Tag of ObjectPoolItem </param>
    /// <param name="_amount">If _amount Postive spawn(_amount) objects from pool,If _amount negative despawn(_amount) objects from pool </param>
    /// <param name="_position">spawn position</param>
    /// <param name="_rotation">spawn rotation</param>
    public void UpdateFromPool(string _tag, int _amount, Vector3 _position = new Vector3(), Vector3? _minPosition = null, Vector3? _maxPosition = null, Quaternion _rotation = new Quaternion())
    {
        if (_amount == 0) return;   

        if (_amount > 0)
        {
            SpawnFromPool(_tag, _amount, _position,_rotation,_minPosition, _maxPosition );
        }
        else
        {
            DespawnFromPool(_tag, _amount);
        }
    }
    #endregion

    /// <summary>
    /// Initialze using SingletonMB initialisation
    /// </summary>
    protected override void Initialize()
    {
        m_poolDictionary = new Dictionary<string, ObjectPoolItem>();

        foreach (ObjectPoolItem pool in m_Pools)
        {
            m_poolDictionary.Add(pool.Tag, pool);

            List<GameObject> objectPool = new List<GameObject>();

            var poolParent = new GameObject(pool.Tag);

            poolParent.transform.SetParent(transform);

            pool.m_Parent = poolParent.transform;

            for (int i = 0; i < pool.amountToPool; i++)
            {

                GameObject poolObject = Instantiate(pool.objectToPool, pool.m_Parent);
                poolObject.SetActive(false);
                objectPool.Add(poolObject);
            }

           
            pool.m_Pool = objectPool;
        }
    }


    #region Private funtions


    /// <summary>
    /// check if pool extandable,and extended it with specific value when needed
    /// </summary>
    /// <param name="_tag">Tag of ObjectPoolItem </param>
    /// <param name="_amount">amount to add</param>
    private void CheckExtandPool(string _tag, int _amount)
    {
        ObjectPoolItem objectPoolItem = m_poolDictionary[_tag];

        if (_amount + objectPoolItem.m_ActiveObjects > objectPoolItem.amountToPool)
        {
            if (objectPoolItem.Expandable)
            {
                int amountToAdd = (_amount + objectPoolItem.m_ActiveObjects) - objectPoolItem.amountToPool;

                AddObjectToPool(objectPoolItem, amountToAdd);
                objectPoolItem.amountToPool = _amount + objectPoolItem.m_ActiveObjects;

            }
            else
            {
                DeSpawnFromPoolFirst(_tag);
            }
        }
    }

    /// <summary>
    /// Add objects (prefabs clone) to pool
    /// </summary>
    /// <param name="_item">ObjectPoolItem</param>
    /// <param name="_amountToAdd">_amountToAdd</param>
    private void AddObjectToPool(ObjectPoolItem _item, int _amountToAdd)
    {
        Debug.Log("AddObjectToPool #1");

        for (int i = 0; i < _amountToAdd; i++)
        {
            GameObject poolObject = Instantiate(_item.objectToPool, _item.m_Parent);
            poolObject.SetActive(false);

            _item.m_Pool.Insert(0, poolObject);
        }
    }
    private GameObject objectToSpawn;

    private void SpawnFromPool(string _tag, int _amount, Vector3 _position,Quaternion _rotation, Vector3? _minPosition = null, Vector3? _maxPosition = null)
    {
        CheckExtandPool(_tag, _amount);

        for (int i = 0; i < _amount; i++)
        {
            SpawnFromPool(_tag, _position, _rotation,_minPosition,_maxPosition);
        }
    }

    private GameObject SpawnFromPool(string _tag, Vector3 _position, Quaternion _rotation, Vector3? _minPosition = null, Vector3? _maxPosition = null)
    {
        if (!m_poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Invalid Tag : " + _tag);
            return null;
        }

        objectToSpawn = m_poolDictionary[_tag].m_Pool[0];

        m_poolDictionary[_tag].m_Pool.Remove(objectToSpawn);

        objectToSpawn.SetActive(true);

        if(_minPosition != null && _maxPosition != null)
        {
           _position =  _position.Random((Vector3)_minPosition,(Vector3) _maxPosition);
        }

        objectToSpawn.transform.position = _position;

        objectToSpawn.transform.rotation = _rotation;   

        m_poolDictionary[_tag].m_Pool.Add(objectToSpawn);

        m_poolDictionary[_tag].m_ActiveObjects++;

        m_poolDictionary[_tag].OnVariableChange?.Invoke(this, EventArgs.Empty);

        return objectToSpawn;
    }

    public void DespawnObjectFromPool(string _tag,GameObject _itemToRemove)
    {
        if (!m_poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Invalid Tag : " + _tag);
            return;
        }
        DeSpawnFromPool(_tag, _itemToRemove);
    }

    private GameObject objectToDeSpawn;

    private void DespawnFromPool(string _tag, int _amount)
    {
        if (!m_poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Invalid Tag : " + _tag);
            return;
        }

        _amount = Mathf.Abs(_amount);

        if (_amount > m_poolDictionary[_tag].m_ActiveObjects)
            _amount = m_poolDictionary[_tag].m_ActiveObjects;


        for (int i = 0; i < _amount; i++)
        {
            DeSpawnFromPool(_tag);
        }
    }

    private void DeSpawnFromPool(string _tag)
    {
        objectToDeSpawn = m_poolDictionary[_tag].m_Pool.Last();

        m_poolDictionary[_tag].m_Pool.Remove(objectToDeSpawn);

        objectToDeSpawn.SetActive(false);

        m_poolDictionary[_tag].m_Pool.Insert(0,objectToDeSpawn);

        m_poolDictionary[_tag].m_ActiveObjects--;
        m_poolDictionary[_tag].OnVariableChange?.Invoke(this, EventArgs.Empty);

    }
    private void DeSpawnFromPoolFirst(string _tag)
    {
        objectToDeSpawn = m_poolDictionary[_tag].m_Pool.First();

        m_poolDictionary[_tag].m_Pool.Remove(objectToDeSpawn);

        objectToDeSpawn.SetActive(false);

        m_poolDictionary[_tag].m_Pool.Insert(m_poolDictionary[_tag].m_Pool.Count-1, objectToDeSpawn);

        m_poolDictionary[_tag].m_ActiveObjects--;

        m_poolDictionary[_tag].OnVariableChange?.Invoke(this, EventArgs.Empty);

    }
    private void DeSpawnFromPool(string _tag,GameObject _itemToRemove)
    {
        if (m_poolDictionary[_tag].m_Pool.Contains(_itemToRemove))
        {
            m_poolDictionary[_tag].m_Pool.Remove(_itemToRemove);

            _itemToRemove.SetActive(false);

            m_poolDictionary[_tag].m_Pool.Insert(0, _itemToRemove);

            m_poolDictionary[_tag].m_ActiveObjects--;

            m_poolDictionary[_tag].OnVariableChange?.Invoke(this, EventArgs.Empty);

        }
    }
    public void SubscribeToPrefabAmountEvent(string _tag,EventHandler Subscriber)
    {
        if (!m_poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Invalid Tag : " + _tag);
            return;
        }
        m_poolDictionary[_tag].OnVariableChange += Subscriber;
    }
    public void UnSubcribeToPrefabAmountEvent(string _tag, EventHandler Subscriber)
    {
        if (!m_poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Invalid Tag : " + _tag);
            return;
        }
        m_poolDictionary[_tag].OnVariableChange -= Subscriber;
    }
    #endregion
}

[System.Serializable]
public class ObjectPoolItem
{
    public string Tag;

    public List<GameObject> m_Pool;

    public GameObject objectToPool;
    public int amountToPool;
    public bool Expandable = true;
    public Transform m_Parent;
    public int m_ActiveObjects;

    public EventHandler OnVariableChange;
        
}

