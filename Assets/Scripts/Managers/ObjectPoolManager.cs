using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyContainer;

    private static GameObject _enemiesEmpty;
    private static GameObject _projectilesEmpty;
    private static GameObject _particleEmpty;
    private static GameObject _audioSourceEmpty;

    public enum PoolType
    {
        Enemy,
        Projectile,
        Particle,
        None,
        AudioSource        
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

        _particleEmpty = new GameObject("Particles");
        _particleEmpty.transform.SetParent(_objectPoolEmptyContainer.transform);

        _audioSourceEmpty = new GameObject("AudioSources"); // Add this line
        _audioSourceEmpty.transform.SetParent(Camera.main.transform); // Add this line
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.Enemy:
                return _enemiesEmpty;
            case PoolType.Projectile:
                return _projectilesEmpty;
            case PoolType.Particle:
                return _particleEmpty;
            case PoolType.AudioSource: // Add this case
                return _audioSourceEmpty;
            default:
                return null;
        }
    }

    #region Default SpawnObject Method 
    //uses GameObject, Vector3, and Quaternion
    public static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None) where T : Component
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        T component;

        if (spawnableObject == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObject.name = objectToSpawn.name;
            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
            component = spawnableObject.GetComponent<T>();
            if (component == null)
            {
                component = spawnableObject.AddComponent<T>();
            }
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
            component = spawnableObject.GetComponent<T>();
        }

        return component;
    }

    #endregion
    public static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPosition, PoolType poolType = PoolType.None) where T : Component
    {
        Quaternion defaultRotation = Quaternion.identity;

        return SpawnObject<T>(objectToSpawn, spawnPosition, defaultRotation, poolType);
    }

    public static T SpawnObject<T>(GameObject objectToSpawn, PoolType poolType = PoolType.None) where T : Component
    {
        // Determine a default position - either Vector3.zero or another logic
        Vector3 defaultPosition = Vector3.zero;
        Quaternion defaultRotation = Quaternion.identity;

        // Optionally, adjust defaultPosition based on parent object logic here
        GameObject parentObject = SetParentObject(poolType);
        if (parentObject != null)
        {
            defaultPosition = parentObject.transform.position;
        }

        return SpawnObject<T>(objectToSpawn, defaultPosition, defaultRotation, poolType);
    }

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

        spawnableObject.transform.position = new Vector3(position.x, position.y, 0); ;
        spawnableObject.transform.up = direction;

        return spawnableObject;
    }
    #endregion

    #region SpawnObject Method Overload with parent transform
    //uses GameObject, Vector3, Quaternion & Parent Transform
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, Transform parentTransform)
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
            spawnableObject = Instantiate(objectToSpawn);
            spawnableObject.name = objectToSpawn.name;
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        spawnableObject.transform.position = spawnPosition;
        spawnableObject.transform.rotation = spawnRotation;
        spawnableObject.transform.SetParent(parentTransform);

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
        }
        else
        {
            objectToDespawn.SetActive(false);
            pool.InactiveObjects.Add(objectToDespawn);
        }
    }
    #endregion

    // Overload for DespawnObject Method with timer
    public static void DespawnObject(GameObject objectToDespawn, float timer)
    {
        objectToDespawn.GetComponent<MonoBehaviour>().StartCoroutine(DespawnTimer(objectToDespawn, timer));
    }

    private static IEnumerator DespawnTimer(GameObject objectToDespawn, float timer)
    {
        yield return new WaitForSeconds(timer);
        DespawnObject(objectToDespawn);
    }
    public static void DespawnComponent<T>(MonoBehaviour monoBehaviour, T componentToDespawn, float timer = 0) where T : Component
    {
        if (timer > 0)
        {
            monoBehaviour.StartCoroutine(DespawnTimer(componentToDespawn.gameObject, timer));
        }
        else
        {
            DespawnObject(componentToDespawn.gameObject);
        }
    }

}

public class PooledObjectInfo //This is a helper class to store the inactive objects in a list
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}