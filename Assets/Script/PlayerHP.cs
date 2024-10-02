using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField, Header("HPÉAÉCÉRÉì")]
    private GameObject _platerIcon;

    private Player _player;
    private int _beforeHP;
   
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _beforeHP = _player.GetHP();
        _CreatHPIcon();

        humihumi();
    }

    private void _CreatHPIcon()
    {
        for(int i = 0; i < _player.GetHP(); i++)
        {
            GameObject _playerHPOdj = Instantiate(_platerIcon);
            _playerHPOdj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _ShowHPIcon();
    }


    private void _ShowHPIcon()
    {
        if (_beforeHP == _player.GetHP()) return;

        Image[] icons = transform.GetComponentsInChildren<Image>();
        for(int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(i < _player.GetHP());
        }
        _beforeHP = _player.GetHP();
    }

    private void humihumi()
    {
        Debug.Log("humihumi");
    }
}
