using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxSpeed = 4.0f;
    public Transform waypoint;
    public float maxHealth = 100.0f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float dividedSpeed = 0.0f;
    private bool isDead = false;
    private WaypointManager.Path _path;
    private int _currentWaypoint = 0;
    private float _currentHealth = 0.0f;
    private float deathClipLength;


    // Start is called before the first frame update
    void Start()
    {
        ResetEnemy();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        
        _agent.enabled = true;
        if(_agent != null)
        {
            _agent.SetDestination(waypoint.position);
            _agent.speed = maxSpeed;
        }
        dividedSpeed = 1 / maxSpeed;

        AnimationClip[] animations = _animator.runtimeAnimatorController.animationClips;
        if(animations == null || animations.Length <= 0)
        {
            Debug.Log("animations Error");
            return;
        }

        for(int i = 0; i<animations.Length; ++i)
        {
            if(animations[i].name == "Standing React Death Backward")
            {
                deathClipLength = animations[i].length;
                break;
            }
        }

    }

    public void ResetEnemy()
    {
        isDead = false;
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        if (isDead)
            return;

        if(_path == null || _path.Waypoints == null || _path.Waypoints.Count <= _currentWaypoint)
        {
            return;
        }

        Transform destination = _path.Waypoints[_currentWaypoint];
        _agent.SetDestination(destination.position);

        if((transform.position - destination.position).sqrMagnitude < 3.0f * 3.0f)
        {
            _currentWaypoint++;
        }
    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("EnemySpeed", _agent.velocity.magnitude * dividedSpeed);
        _animator.SetBool("isDead", isDead);
    }

    public void Initialize(WaypointManager.Path path)
    {
        _path = path;
    }

    public float GetHealth()
    {
        return _currentHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0.0f && !isDead)
        {
            StartCoroutine("Kill");
        }
    }

    public IEnumerator Kill()
    {
        isDead = true;

        yield return new WaitForSeconds(deathClipLength);
        ServiceLocator.Get<ObjectPoolManager>().RecycleObject(gameObject);
        //Destroy(gameObject);
    }
}
