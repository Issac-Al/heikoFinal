using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemies : MonoBehaviour
{
    public List<GameObject> enemiesNear;
    private bool hasEntered;
    // Start is called before the first frame update
    void Start()
    {
        enemiesNear = new List<GameObject>();
    }

    // Update is called once per frame

    public void OnTriggerEnter(Collider collider)
    {
        hasEntered = false;
        UnityEngine.Debug.Log("entro al trigger");
        if (collider.transform.CompareTag("Enemy"))
        {
            if (enemiesNear.Count >= 1)
            {
                foreach (GameObject enemy in enemiesNear)
                {
                    if (enemy == collider.gameObject)
                        hasEntered = true;
                }
                if (!hasEntered)
                {
                    enemiesNear.Add(collider.gameObject);
                }
            }
            else enemiesNear.Add(collider.gameObject);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        foreach (GameObject enemy in enemiesNear)
        {
            if (enemy == collider.gameObject)
                enemiesNear.Remove(enemy);
        }
    }

    public void DestroyEnemyInList(GameObject enemy)
    {
        for (int i = 0; i < enemiesNear.Count; i++)
        {
            if (enemy == enemiesNear[i])
            {
                enemiesNear.RemoveAt(i);
            }
        }
    }

    public List<GameObject> ListSize()
    {
        return enemiesNear;
    }
}
