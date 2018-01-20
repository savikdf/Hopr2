using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicsOptions
{
		public float GRAVITY = -9.8f;
        public float SCALEFACTOR = 0.95f;
        public float BOUNCEDECAY = 0.55f;
        [Range(0, 100)]
        public float force = 0.2f;
		public float cap = 25;
		[Range(0, 10)]
        public float TapRange = 5f;
		[Range(0, 10)]
        public float CheckMultiplier = 0.15f;
        public Vector3 rayCheckOffset = new Vector3(0, 0.2f, 0);
		[Range(0, 5)]
        public float RestTime = 1.4f;
        public bool showDebugs;

        public Vector3 resetPoint;

        public Vector3 resetPosition;
}

public class VariableManager : MonoBehaviour 
{
	public PhysicsOptions physicsOptions;

}
