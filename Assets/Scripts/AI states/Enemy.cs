using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Character
{
    public Transform target;

    [HideInInspector]
    public Vector2 lastKnowPosition = Vector2.zero;
    [HideInInspector]
    public Vector2 nextPatrolPos = Vector2.zero;
    [HideInInspector]
    public Vector3 pathDir, _path;
    public Transform[] PatrolPoints;

    [HideInInspector]
    public Vector3 startPos;

    //State
    IEnemyState state;

    //Pathfinding
    [Header("Pathfinding")]
    private Seeker seeker;

    public Path path;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    public bool reachedEndOfPath;

    [HideInInspector]
    public float dist;

    public override void Start()
    {
        seeker = GetComponent<Seeker>();
        base.Start();

        startPos = transform.position;

        ChangeState(new PatrolState());
    }

    public override void Update()
    {
        if(!dead)
        {
            dist = Vector2.Distance(lastKnowPosition, transform.position);
            PathFinding();
            base.Update();


            if (state != null)
                state.Execute();

            Debug.Log(state);
        }
    }


    public void FixedUpdate()
    {
        if (state != null && !dead)
            state.ExecuteFixed();


    }

    public void ChangeState(IEnemyState _state)
    {
        if (state != null)
        {
            if (state.id.Equals(_state.id))
                return;
            else
                state.Exit();
        }
          


        state = _state;
        state.Enter(this);

    }

    public void NoticeNoise(Vector3 pos, string soundID)
    {
        if (soundID == "gun")
            lastKnowPosition = pos;
    }

    public override void OnDie()
    {
        base.OnDie();
        App.Instance.killed++;
    }

    #region pathfinding
    public void OnPathComplete(Path p)
    {

        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }

    public void PathFinding()
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;

            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            Vector2 pos = (lastKnowPosition.magnitude != 0) ? lastKnowPosition: nextPatrolPos; //patrol, or hunt the player?
            seeker.StartPath(transform.position, pos, OnPathComplete);
        }

        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        _path = path.vectorPath[currentWaypoint];
        pathDir = (_path - transform.position).normalized;

    }

    #endregion


  
}
