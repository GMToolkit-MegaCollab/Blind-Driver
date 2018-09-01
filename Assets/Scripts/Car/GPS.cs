﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The mad lad responsible for navigation and the XZ plane of existence
public class GPS : PassengerController2 {

    private List<NavMeshSurface> NavMeshSurfaces = new List<NavMeshSurface>();

    private Vector3 target;

    private float elapsed = 1.1f;

    private NavMeshPath path;
    private NavMeshQueryFilter qfilter;

    public float distance;
    public Vector2[] Path;

    public VoiceLine right;
    public VoiceLine left;
    public VoiceLine straight;
    public VoiceLine uTurn;
    public VoiceLine slightRight;
    public VoiceLine slightLeft;

    //Load gameobjects and then create and bake a navmesh and also create a player navmesh agent
    void Start() {
        GameObject parent = new GameObject("Navmesh");
        GameObject ground = new GameObject("Ground - Offroad", new System.Type[] { typeof(BoxCollider), typeof(NavMeshSurface) });
        ground.transform.localScale = new Vector3(100, 1, 100);
        ground.transform.SetParent(parent.transform);

        GameObject Offroad = new GameObject("Offroad", new System.Type[] { typeof(NavMeshModifier) });
        GameObject Road = new GameObject("Roads", new System.Type[] { typeof(NavMeshModifier) });
        GameObject Obstacle = new GameObject("Obstacles");
        //Offroad.GetComponent<NavMeshModifier>().overrideArea = true;
        //Offroad.GetComponent<NavMeshModifier>().area = 4;
        //Road.GetComponent<NavMeshModifier>().overrideArea = true;
        //Road.GetComponent<NavMeshModifier>().area = 3;
        Offroad.transform.SetParent(parent.transform);
        Road.transform.SetParent(parent.transform);
        Obstacle.transform.SetParent(parent.transform);

        Offroad[] Offroadspace = FindObjectsOfType<Offroad>();
        Road[] Roads = FindObjectsOfType<Road>();
        Obstacle[] Obstacles = FindObjectsOfType<Obstacle>();

        for (int i = 0; i < Offroadspace.Length; i++) {
            Offroad item = Offroadspace[i];
            GameObject obj = new GameObject("Offroad " + i, new System.Type[] { typeof(BoxCollider), typeof(NavMeshSurface) });
            obj.transform.SetParent(Offroad.transform);
            obj.transform.position = new Vector3(item.transform.position.x + item.boxcol.offset.x, 0f, item.transform.position.y + item.boxcol.offset.y);
            obj.transform.localScale = new Vector3(item.transform.localScale.x * item.boxcol.size.x, 1f, item.transform.localScale.y * item.boxcol.size.y);
            obj.GetComponent<NavMeshSurface>().useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            NavMeshSurfaces.Add(obj.GetComponent<NavMeshSurface>());
        }

        for (int i = 0; i < Roads.Length; i++) {
            Road item = Roads[i];
            GameObject obj = new GameObject("Road " + i, new System.Type[] { typeof(BoxCollider), typeof(NavMeshSurface) });
            obj.transform.SetParent(Road.transform);
            obj.transform.position = new Vector3(item.transform.position.x + item.boxcol.offset.x, 0.2f, item.transform.position.y + item.boxcol.offset.y);
            obj.transform.localScale = new Vector3(item.transform.localScale.x * item.boxcol.size.x, 1f, item.transform.localScale.y * item.boxcol.size.y);
            obj.GetComponent<NavMeshSurface>().useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            NavMeshSurfaces.Add(obj.GetComponent<NavMeshSurface>());
        }

        for (int i = 0; i < Obstacles.Length; i++) {
            Obstacle item = Obstacles[i];
            GameObject obj = new GameObject("Obstacle " + i, new System.Type[] { typeof(BoxCollider) });
            obj.transform.SetParent(Obstacle.transform);
            obj.transform.position = new Vector3(item.transform.position.x + item.boxcol.offset.x, 1f, item.transform.position.y + item.boxcol.offset.y);
            obj.transform.localScale = new Vector3(item.transform.localScale.x * item.boxcol.size.x, 2f, item.transform.localScale.y * item.boxcol.size.y);
        }

        for (int i = 0; i < NavMeshSurfaces.Count; i++) {
            NavMeshSurfaces[i].agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
            NavMeshSurfaces[i].BuildNavMesh();
        }

        path = new NavMeshPath();
        qfilter = new NavMeshQueryFilter();
        qfilter.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
        qfilter.areaMask = NavMesh.AllAreas;

        if (FindObjectOfType<Target>() == null) throw new System.Exception("You dumbass you forgot to set a target. Make a new Gameobject, put it where you need the target and add a Target component to it.");
        Vector3 pos = FindObjectOfType<Target>().transform.position;
        SetTarget(new Vector3(pos.x, 0f, pos.y));

        //Finally update path so we're done
        UpdatePath();
    }

    void Update() {

        /*elapsed += Time.deltaTime;
		if (elapsed >= 1.0f) {
			elapsed = 0f;
			UpdatePath();
		}*/
        UpdatePath(); //we have enough processing power to do this every tick

#if UNITY_EDITOR
        for (int i = 0; i < path.corners.Length - 1; i++) {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.yellow);
            Debug.DrawLine(Path[i], Path[i + 1], Color.green);
        }
#endif
        if (Path == null || Path.Length < 2)
            return;
        VoiceLine line;
        int ind = 0;
        Vector2 segNow = Path[ind] - (Vector2)transform.position;
        while (segNow.magnitude < 0.5f && Path.Length > ind+2) {
            ind++;
            segNow = Path[ind] - (Vector2)transform.position;
        }
        Debug.DrawRay(transform.position - Vector3.forward, segNow, Color.red);
        Debug.DrawRay(transform.position - Vector3.forward, transform.forward, Color.blue);
        
        if (Vector2.Angle(segNow, transform.forward) > 90) {
            line = uTurn;
        } else {
            Vector2 segAfter = Path[ind + 1] - Path[ind];
            Debug.DrawRay(transform.position - Vector3.forward + (Vector3)segNow, segAfter, Color.red);
            float angleDiff = Vector2.SignedAngle(segNow, segAfter);
            if (angleDiff > 45)
                line = left;
            else if (angleDiff < -45)
                line = right;
            else if (angleDiff > 15)
                line = slightLeft;
            else if (angleDiff < -15)
                line = slightRight;
            else
                line = straight;
        }
        //todo: recalculating
        if (line != null) {
            Debug.Log(line.name);
            if (segNow.magnitude < 1)
                line.Trigger();
        }
    }

    //Sets the target in NAVMESH SPACE!
    public void SetTarget(Vector3 target) {
        this.target = target;
    }

    private void UpdatePath() {
        NavMesh.CalculatePath(new Vector3(this.transform.position.x, 0f, this.transform.position.y), target, qfilter, path);
        Path = new Vector2[path.corners.Length];
        distance = 0f;
        for (int i = 0; i < path.corners.Length; i++) Path[i] = new Vector2(path.corners[i].x, path.corners[i].z);
        for (int i = 1; i < path.corners.Length; i++) distance += Mathf.Sqrt((Path[i].x - Path[i - 1].x) * (Path[i].x - Path[i - 1].x) + (Path[i].y - Path[i - 1].y) * (Path[i].y - Path[i - 1].y));
    }
}

/*
 *CREDITS IF THIS GETS STOLEN
 *Programming: IQuick 143, TrolledWoods
 *Organization: Meesto, IQuick 143, TrolledWoods
 *Voice + sounds: Meesto, TheBasementNerd
 */
