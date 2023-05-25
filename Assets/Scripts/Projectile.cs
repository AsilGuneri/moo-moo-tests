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
    [SyncVar] private Transform spawnerTransform;
    [SyncVar] public GameObject TargetId;


    #region Server
    [Server]
    private void DestroySelf()
    {
        //NetworkServer.Destroy(gameObject);
        //ObjectPooler.Instance.ReturnToPool(PoolTag, gameObject);
        NetworkServer.UnSpawn(gameObject);
        ObjectPooler.Instance.Return(gameObject);
    }
    //private IEnumerator DestroyOnHitParticle()
    //{
    //    yield return new WaitForSeconds(onHitParticleDestroySecond);
    //    NetworkServer.Destroy(onHitParticle.gameObject);
    //}
    [ServerCallback]
    private void CmdTargetHit()
    {
        if(TargetId == null || spawnerTransform == null)
        {
            return;
        }
        TargetId.GetComponent<Health>().TakeDamage(_damage, spawnerTransform);
        if (onHitParticle)
        {
            onHitParticle.transform.parent = null;
            onHitParticle.Play();
            //StartCoroutine(nameof(DestroyOnHitParticle));
        }
       
        DestroySelf();
    }
    #endregion
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }
    [Server]
    public void SetupProjectile(GameObject target, int damage, Transform spawnerTransform)
    {
        _isMoving = true;
        TargetId = target;
        _damage = damage;
        this.spawnerTransform = spawnerTransform;
    }
    [ClientCallback]
    private void Update()
    {
        // if (_isMoving && _target == null) DestroySelf();
        if (TargetId == null || !_isMoving) return;
        //if (!isLocalPlayer) return;
        if (Vector2.Distance(Extensions.To2D(transform.position), Extensions.To2D(TargetId.transform.position)) > 0.4f)
        {
            transform.position += Extensions.Vector3WithoutY(Direction(TargetId.transform.position) * Time.deltaTime * speed);
            Vector3 targetPos = new Vector3(TargetId.transform.position.x, transform.position.y, TargetId.transform.position.z);
            transform.LookAt(targetPos);
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
