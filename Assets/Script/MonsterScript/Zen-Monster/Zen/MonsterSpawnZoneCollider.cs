using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MonsterSpawnZoneCollider : MonoBehaviour
{

    private SphereCollider sphereCollider;

    [SerializeField]
    private GameObject monsterSpawnZoneObj;

    [SerializeField]
    private float active_distance = 50.0f;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.isTrigger = true;
        sphereCollider.radius = active_distance;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            monsterSpawnZoneObj.SetActive(true);
            Debug.Log("플레이어가 들어옴");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterSpawnZoneObj.SetActive(false);
            Debug.Log("플레이어가 나감");
        }
    }
}
