using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyContainer;

    private static GameObject _enemiesEmpty;
    private static GameObject _projectilesEmpty;

    public enum PoolType
    {
        Enemy,
        Projectile,
        None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyContainer = new GameObject("Object Pool Container");

        _enemiesEmpty = new GameObject("Enemies");
        _enemiesEmpty.transform.SetParent(_objectPoolEmptyContainer.transform);

        _projectilesEmpty = new GameObject("Projectiles");
        _projectilesEmpty.transform.SetParent(_objectPoolEmptyContainer.transform);
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Enemy:
                return _enemiesEmpty;
            case PoolType.Projectile:
                return _projectilesEmpty;
            default:
                return null;
        }
    }

    #region Default SpawnObject Method 
    //uses GameObject, Vector3, and Quaternion
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name); //This line finds the pool for the object being spawned

        if (pool == null) //This line creates a new pool if one doesn't exist for the object being spawned
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault(); //This line finds the first inactive object in the pool

        if (spawnableObject == null) //This line creates a new object if there are no inactive objects in the pool
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObject.name = objectToSpawn.name;

            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else //This line reuses an inactive object if there is one in the pool
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);            
        }

        return spawnableObject;
    }
    #endregion

    #region SpawnObject Method Overload with Direction & Position
    //uses GameObject, Vector2, and Vector2 for projectile spawning
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector2 direction, Vector2 position, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObject = Instantiate(objectToSpawn);
            spawnableObject.name = objectToSpawn.name;

            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        spawnableObject.transform.position = position;
        spawnableObject.transform.up = direction;

        return spawnableObject;
    }
    #endregion

    #region DespawnObject Method
    public static void DespawnObject(GameObject objectToDespawn)
    {
        //string goName = objectToDespawn.name.Substring(0, objectToDespawn.name.Length - 7); might not need this since name is set in SpawnObject
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToDespawn.name);

        if (pool == null)
        {
            Debug.LogWarning("No pool exists for " + objectToDespawn.name);
            //These lines create a new pool if one doesn't exist for the object being despawned 
            //pool = new PooledObjectInfo() { LookupString = objectToDespawn.name };
            //ObjectPools.Add(pool);
        }
        else
        {
            objectToDespawn.SetActive(false);
            pool.InactiveObjects.Add(objectToDespawn);
        }
    }
    #endregion
}

public class PooledObjectInfo //This is a helper class to store the inactive objects in a list
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}