using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private HealthController _target;
    private Vector3 _direction;

    public HealthController Target
    {
        get => _target;
        set => _target = value;
    }

    public void GoToPool()
    {
        Target = null;
        Destroy(gameObject);
    }

    
    private void Update()
    {
        if (Target == null) return;
        if (Vector3.Distance(transform.position, Target.transform.position) > 0.5f)
        {
            transform.position += Vector3WithoutY(Direction(Target.transform.position) * Time.deltaTime * speed);
            transform.LookAt(Target.transform.position);
            return;
        }
        else
        {
            GoToPool();
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
