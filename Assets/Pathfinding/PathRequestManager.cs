using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    PathRequest currentRequest;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    static PathRequestManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    void Awake(){
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[],bool> callback){
        PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    void TryProcessNext(){
        if (!isProcessingPath && pathRequestQueue.Count > 0){
            currentRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentRequest.pathStart,currentRequest.pathEnd);

        }
    }

    public void FinishedProcessingPath(Vector2[] path, bool success){
        currentRequest.callback(path,success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;
        public PathRequest(Vector2 _start, Vector2 _end, Action<Vector2[],bool> _callback){
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
