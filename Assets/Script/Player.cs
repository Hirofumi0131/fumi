using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    [SerializeField, Header("体力")]
    private int _hp;
    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    //点滅の仕様_spriteRendererの判定が違うからプレイヤーの足などが点滅しない   変更済み
    [SerializeField, Header("点滅時間")]
    private float _flashTime;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private bool _bJump;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _LookMoveDirec();
    }

    private void _Move()
    {
        _rigid.velocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.velocity.y);
        _anim.SetBool("Walk",_inputDirection.x != 0.0f);
    }

    private void _LookMoveDirec()
    {
        if (_inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_inputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _bJump = false;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamager");
        }
    }

    private void _HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.localScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        if(transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY -0.1f))
        {
            Destroy(enemy);
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
        else
        {
            enemy.GetComponent<Enemy>().PlayerDamage(this);
            StartCoroutine(_Damage());
        }
    }

    IEnumerator _Damage()
    {
        Color color = _spriteRenderer.color;
        for(int i = 0; i < _damageTime; i ++)
        {
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        _spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void _Dead()
    {
        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void _OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        if (!context.performed || _bJump) return;

        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        _bJump = true;
    }

    public void Damage(int damage)
    {
        _hp = Mathf.Max(_hp - damage, 0);
        _Dead();
    }

    public int GetHP()
    {
        return _hp;
    }
}
