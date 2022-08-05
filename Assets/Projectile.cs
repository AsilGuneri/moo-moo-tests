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
    [SyncVar] private int _damage;
    [SyncVar] public GameObject _target;


    #region Server
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    [Command(requiresAuthority = false)]
    private void CmdTargetHit()
    {
        _target.GetComponent<Health>().TakeDamage(_damage);
        DestroySelf();
    }
    #endregion
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }
    public void SetupProjectile(GameObject target, int damage)
    {
        _isMoving = true;
        _target = target;
        _damage = damage;
    }
    [ClientCallback]
    private void Update()
    {
        if (_target == null || !_isMoving) return;

        if (Vector2.Distance(Extensions.Vector3ToVector2(transform.position),Extensions.Vector3ToVector2(_target.transform.position)) > 0.4f)
        {
            transform.position += Vector3WithoutY(Direction(_target.transform.position) * Time.deltaTime * speed);
            // transform.LookAt(_target.transform.position);
            Vector3 targetPostition = new Vector3( _target.transform.position.x, 
                                        this.transform.position.y, 
                                        _target.transform.position.z ) ;
            this.transform.LookAt( targetPostition ) ;
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
