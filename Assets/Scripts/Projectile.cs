using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkBehaviour
{
    [SerializeField] public float speed = 1;
    [SerializeField] private ParticleSystem onHitParticle;
    [SerializeField] private float onHitParticleDestroySecond;


    [SyncVar] private bool _isMoving;
    [SyncVar] private int _damage;
    [SyncVar] public GameObject _target;


    #region Server
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    private IEnumerator DestroyOnHitParticle()
    {
        yield return new WaitForSeconds(onHitParticleDestroySecond);
        NetworkServer.Destroy(onHitParticle.gameObject);
    }
    [ServerCallback]
    private void CmdTargetHit()
    {
        _target.GetComponent<Health>().TakeDamage(_damage);
        if (onHitParticle)
        {
            onHitParticle.transform.parent = null;
            onHitParticle.Play();
            StartCoroutine(nameof(DestroyOnHitParticle));
        }
       
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
        if (_isMoving && _target == null) NetworkServer.Destroy(gameObject);
        if (_target == null || !_isMoving) return;

        if (Vector2.Distance(Extensions.Vector3ToVector2(transform.position),Extensions.Vector3ToVector2(_target.transform.position)) > 0.4f)
        {
            transform.position += Extensions.Vector3WithoutY(Direction(_target.transform.position) * Time.deltaTime * speed);
            Vector3 targetPos = new Vector3( _target.transform.position.x, transform.position.y, _target.transform.position.z ) ;
            transform.LookAt(targetPos) ;
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
   
}
