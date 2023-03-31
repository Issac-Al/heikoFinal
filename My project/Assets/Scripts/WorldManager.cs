using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject[] enemies;
    private GameObject player;
    public GameObject spiritWorld;
    private float distance;
    private bool canUpdate = true;
    // Start is called before the first frame update
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        UnityEngine.Debug.Log(enemies);
        player = GameObject.FindGameObjectWithTag("Player");
        spiritWorld.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetAllDistances();
    }

    void GetAllDistances()
    {
        foreach(GameObject enemy in enemies)
        {
           distance = Mathf.Abs(Vector3.Distance(enemy.transform.position, player.transform.position));
           if(distance > 60 && canUpdate)
            {
                enemy.SetActive(false);
            }
           else enemy.SetActive(true);
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemy == enemies[i])
            {
                enemies[i] = new GameObject();
            }
        }
    }

    public void UpdateEnemies()
    {
        canUpdate = false;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SetActive(true);
        }
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        canUpdate = true;
    }

}
