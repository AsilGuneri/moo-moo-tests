using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Projectile : NetworkBehaviour
{
    public Action OnHit;

    [SerializeField] public float speed = 1;
    [SerializeField] private ParticleSystem onHitParticle;
    [SerializeField] private float onHitParticleDestroySecond;


    [SyncVar] private bool _isMoving;
    [SyncVar] private int _damage;
    [SyncVar] private Transform spawnerTransform;
    [SyncVar] public GameObject Target;


    #region Server
    [Server]
    private void DestroySelf()
    {
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
        if(Target == null || spawnerTransform == null)
        {
            return;
        }
        Target.GetComponent<Health>().TakeDamage(_damage, spawnerTransform);
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
    public void SetupProjectile(GameObject target, int damage, Transform spawnerTransform,Action action = null)
    {
        OnHit = action;
        _isMoving = true;
        Target = target;
        _damage = damage;
        this.spawnerTransform = spawnerTransform;
    }
    [ClientCallback]
    private void Update()
    {
        if (_isMoving && Target == null) DestroySelf();
        if (Target == null || !_isMoving) return;
        if (Vector2.Distance(Extensions.To2D(transform.position), Extensions.To2D(Target.transform.position)) > 0.4f)
        {
            transform.position += Extensions.Vector3WithoutY(Direction(Target.transform.position) * Time.deltaTime * speed);
            Vector3 targetPos = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            transform.LookAt(targetPos);
            return;

        }
        else
        {
            OnHit?.Invoke();
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
