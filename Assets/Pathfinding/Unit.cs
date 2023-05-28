using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Transform target;
    const float minPathUpdateTime = .01f;
	const float pathUpdateMoveThreshold = 1f;

    Path path;
    public float speed = 1;
	public float turnSpeed = 3;
	public float turnDst = 5;
	private float stoppingDst = 0;
	private bool canMove = true;
	EnemyBase entity;
	private MovementScript movementScript;
	public void setTarget(Transform _target){
		if (_target != target){
			target=_target;
			StopCoroutine("FollowPath");
		}
	} 

    void Start(){
        movementScript = GetComponent<MovementScript>();
		StartCoroutine (UpdatePath ());
		entity = GetComponent<EnemyBase>();
    }

	public void setCanMove(bool _canMove){
		canMove = _canMove;
		// StopCoroutine("FollowPath");
	}

    IEnumerator UpdatePath() {

		if (Time.timeSinceLevelLoad < .3f) {
			yield return new WaitForSeconds (.3f);
		}
		Vector3? targetPosOld = null;
		if (target is not null){
			PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
			targetPosOld = target.position;
		}
		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
		

		while (true) {
			yield return new WaitForSeconds (minPathUpdateTime);
			if (target == null) continue;
			if (targetPosOld is null){
				targetPosOld = target.position;
			} 
			if ((target.position - (Vector3) targetPosOld).sqrMagnitude > sqrMoveThreshold) {
				if (!canMove) continue;
				PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
				targetPosOld = target.position;
			}
		}
	}
    public void OnPathFound(Vector2[] newPath, bool pathSuccesful){
		if (this == null) return;
        if (gameObject != null && pathSuccesful){
            path = new Path(newPath, transform.position, turnDst, stoppingDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {

		bool followingPath = true;
		int pathIndex = 0;

        
		int pathHash = path.GetHashCode();
		float speedPercent = 1;

		while (followingPath) {
			if (!canMove){
				yield return null;
				continue;
			}
			Vector2 pos2D = new Vector2 (transform.position.x, transform.position.y);
			if (path.turnBoundaries.Length == 0){
                followingPath = false;
                break;
            }
            while (path.turnBoundaries [pathIndex].HasCrossedLine (pos2D)) {
				if (pathIndex == path.finishLineIndex) {
					followingPath = false;
					break;
				} else {
					pathIndex++;
				}
			}

			if (followingPath) {

				if (pathIndex >= path.slowDownIndex && stoppingDst > 0) {
					speedPercent = Mathf.Clamp01 (path.turnBoundaries [path.finishLineIndex].DistanceFromPoint (pos2D) / stoppingDst);
					if (speedPercent < 0.1f) {
						followingPath = false;
					}
				}

                var pointPos = new Vector3(path.lookPoints [pathIndex].x,path.lookPoints [pathIndex].y,0);

                var AngleRad = Mathf.Atan2(pointPos.y - this.transform.position.y, pointPos.x - this.transform.position.x);

                var AngleDeg = (180 / Mathf.PI) * AngleRad;

				Quaternion targetRotation = Quaternion.Euler(0, 0, AngleDeg);
				var position = (targetRotation * Vector2.right).normalized*Time.deltaTime*speed;
				if (position.normalized.x < 0)
					entity.WalkFlip(true);
				else if (position.normalized.x > 0)
					entity.WalkFlip(false);
				transform.position = transform.position + position;
				// transform.Translate (Vector3.right * Time.deltaTime * speed * speedPercent, Space.Self);
			}

			yield return null;

            if (path.GetHashCode() != pathHash) {
				followingPath = false;
			}
			entity.SetIsWalking(followingPath);
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			path.DrawWithGizmos ();
		}
	}
}
