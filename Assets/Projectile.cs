using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    [SerializeField] public float speed = 1;
    //private Health _target;
    private Vector3 _direction;
    [SyncVar] private bool _isMoving;
    private Vector3 _targetPos;
    [SyncVar] private GameObject _target;
    [SyncVar] private int _damage;


    #region Server
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    [Command]
    private void CmdTargetHit()
    {
        DestroySelf();
        _target.GetComponent<Health>().TakeDamage(_damage);
    }
    #endregion
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }
    public void SetupProjectile(GameObject target)
    {
        _target = target;
        Debug.Log("asil");
       // _damage = damage;
        _isMoving = true;
    }
    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return; 
        if (_target == null || !_isMoving) return;
        Debug.Log("2");

        if (Vector2.Distance(Extensions.Vector3ToVector2(transform.position),Extensions.Vector3ToVector2(_target.transform.position)) > 2)
        {
            transform.position += Vector3WithoutY(Direction(_target.transform.position) * Time.deltaTime * speed);
            transform.LookAt(_target.transform.position);
            return;
        }
        else
        {
            CmdTargetHit();
            return;
        }


    }
    private Vector3 Direction(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        return direction;
    }
    private Vector3 Vector3WithoutY(Vector3 vector)
    {
        Vector3 newVector = new Vector3(vector.x, 0, vector.z);
        return newVector;
    }
}
