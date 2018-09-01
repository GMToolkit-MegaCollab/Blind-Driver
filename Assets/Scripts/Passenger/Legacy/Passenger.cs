using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "passenger", menuName = "Passenger", order = 1)]
public class Passenger : ScriptableObject {
    [System.Serializable]
    public class Directions
    {
        public AudioClip[] left, right, front;
    }

    public string Name;

    public Directions intense;
    public Directions extreme;
}
