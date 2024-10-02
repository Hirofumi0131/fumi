using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (_player.transform.position - transform.position);
        transform.rotation = Quaternion.FromToRotation(Vector2.left, dir);
    }
}
