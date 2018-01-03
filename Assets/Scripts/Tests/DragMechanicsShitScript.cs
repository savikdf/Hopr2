using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Face
{
    public Vector3 p0, p0pos;
    public Vector3 p1, p1pos;
    public Vector2 c;
    public Vector3 normal;
    public GameObject Object;
    //Probs better to just store the transform itself *shrugs*
    public Vector3 storedRefrenceRotation, storedRefrencePosition;

    public Face(Vector3 _p0, Vector3 _p1, Vector3 pos, string name)
    {
        Object = new GameObject(name);
        Object.transform.position = pos;
        Object.transform.rotation = new Quaternion() { eulerAngles = new Vector3(90, 0, 0) };
        storedRefrenceRotation = Object.transform.rotation.eulerAngles;
        storedRefrencePosition = Object.transform.position;

        p0pos = _p0;
        p1pos = _p1;

        Vector3 pM0 = Object.transform.localToWorldMatrix * (_p0);
        p0 = (pM0 + Object.transform.position);

        Vector3 pM1 = Object.transform.localToWorldMatrix * (_p1);
        p1 = pM1 + Object.transform.position;

        c = Object.transform.position;


        normal = Object.transform.localToWorldMatrix * CreateNormal();
    }

    Vector3 CreateNormal()
    {
        return Vector3.Normalize(Vector3.Cross(new Vector3(p0.x,p0.y, 0.0f), new Vector3(p1.x, p1.y, 0.0f)));
    }

    public void ApplyTransformMatrix()
    {
        c = storedRefrencePosition;

        normal = Object.transform.localToWorldMatrix * CreateNormal();

        Vector3 pM0 = Object.transform.localToWorldMatrix * (p0pos);
        p0 = pM0 + Object.transform.position;

        Vector3 pM1 = Object.transform.localToWorldMatrix * (p1pos);
        p1 = pM1 + Object.transform.position;

    }

    public void DrawFace()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(p0, 0.05f);
        Gizmos.DrawSphere(p1, 0.05f);
        Gizmos.DrawSphere(c, 0.05f);

        Gizmos.DrawLine(p0, p1);


        Gizmos.DrawRay(c, normal);
    }


    public void Update()
    {
        if (Object.transform.rotation.eulerAngles != storedRefrenceRotation || Object.transform.position != storedRefrencePosition)
        {
            //If we rotate the platform in real time, we gadda update the points
            //and normals to make sure the transform effects them too

            storedRefrenceRotation = Object.transform.rotation.eulerAngles;
            storedRefrencePosition = Object.transform.position;
            ApplyTransformMatrix();
        }
    }
    
}


public class DragMechanicsShitScript : MonoBehaviour
{

    public Face testFace;
    public Face testFace2;
    public Face testFace3;
    public Face testFace4;
    public Face testFace5;

    GameObject playertest;
    Vector2 mousePos;
    public static float GRAVITY = -9.8f;
    public static float SCALEFACTOR = 0.95f;
    public static float BOUNCEDECAY = 0.55f;

    [Range(0, 100)]
    public float force;
    float time;
    public bool isGettingReady, isGrounded, isApplyingGravity; 
    public float buildup, cap, angle;
    public Vector2 Velocity;
    public bool isColliding;

    [Range(0, 10)]
    public float TapRange;

    bool Impulse;

    public GameObject Arrow;
    public Vector3 m_ScreenMosPos;
    
    [Range(0, 10)]
    public int iterations;
    public float dotAngle;


    [Range(0, 1)]
    public float CheckMultiplier;
    public Vector3 intersectionPoint;

    public Vector3 rayCheckOffset;

    Ray CollisionRay;

    List<Face> faces = new List<Face>();

    void Start ()
    {
        testFace = new Face(new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 3.5f, -1), "TestFace");
        faces.Add(testFace);
        testFace2 = new Face(new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(1.5f, 5.5f, -1), "TestFace2");
        faces.Add(testFace2);

        testFace5 = new Face(new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(-1.5f, 5.5f, -1), "TestFace5");
        faces.Add(testFace5);

        testFace3 = new Face(new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 8.5f, -1), "TestFace3");
        faces.Add(testFace3);
        testFace4 = new Face(new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 2.5f, -1), "TestFace4");
        testFace4.Object.transform.Rotate(new Vector3(0, 90, 0));
        faces.Add(testFace4);
    }

	void Update ()
    {
       

        if (Velocity.magnitude < 0.1f)
            //isGrounded = true;

        if (Input.GetMouseButtonDown(0))
        {
            //buildup = 0;

            isGettingReady = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            isGettingReady = false;
            getAngle();
            Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -1.0f));

            if (buildup < TapRange)
            {
                buildup = TapRange;
            }

            if(isGrounded)
            Velocity += new Vector2(direction.x, direction.y) * buildup;

            buildup = 0;
            isGrounded = false;
        }

        if(isGettingReady)
        {
            if (isGrounded)
                BuilUp();
        }

        Gravity();
   
        ArrowRotate();

        for(int i = 0; i < faces.Count; i++)
            faces[i].Update();
    }   

    void LateUpdate()
    {
        CollisionRay = new Ray(transform.position, Velocity.normalized);
        GroundCheck();

        for (int i = 0; i < iterations; i++)
        {
            CollisionDetection();
        }

        ApplyForce();

        time += Time.deltaTime;

        faces[1].Object.transform.Rotate(new Vector3(0, Mathf.Sin(time % 180), 0));
        faces[2].Object.transform.Rotate(new Vector3(0, Mathf.Cos(time % 180), 0));

    }

    void NearestPlatform(Face face)
    {
        Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -1.0f));


        dotAngle = Vector2.Dot(Velocity.normalized, testFace.normal);



        intersectionPoint = Utils.SegmentIntersection(face.p0, face.p1,
        (transform.position + rayCheckOffset) + CollisionRay.direction * CheckMultiplier, transform.position);

        if (Utils.IsSegmentIntersection(face.p0, face.p1,
           (transform.position + rayCheckOffset) + CollisionRay.direction * CheckMultiplier, transform.position))
        {

     
            //transform.position = new Vector3(toWorld(ToView(transform.position)).x + .1f, toWorld(ToView(transform.position)).y, transform.position.z);
            //Velocity.x += -Velocity.x * 0.27f;
            //Velocity = Vector2.Reflect(Velocity, (Mathf.Sign(dotAngle) == -1) ? face.normal : -face.normal) * BOUNCEDECAY;
            Velocity = ReflectionTest(Velocity, (Mathf.Sign(dotAngle) == -1) ? face.normal : -face.normal, intersectionPoint) * BOUNCEDECAY;


        }
        //if (Mathf.Abs(Velocity.magnitude) < 0.3f && Mathf.Abs(dotAngle) == 1)
        //    isGrounded = true;

        //One Way Interesection
        //Reverse the order and sign and BAM two way baby
        if (Mathf.Sign(dotAngle) == -1)
        {
            //Debug.Log("Negetive");                    
            //Means the Faces are in Oppsite Directions   
        }

       
    }


    Vector2 ReflectionTest(Vector2 Velocity, Vector3 direction, Vector3 intersectionPoint)
    {

        Vector2 Result = Vector2.Reflect(Velocity, direction);
        Debug.Log(Result.magnitude);

        if (Result.magnitude < 0.12f)
        {
            isGrounded = true;
            Result = Vector2.zero;
            isApplyingGravity = false;
            transform.position = intersectionPoint;
            Debug.Log("stuck");
        }
        else if (Result.magnitude > 0.15f)
        {          
            isGrounded = false;
        }

        return Result;

    }

    bool LineIntresection()
    {
        return true;
    }

    void CollisionDetection()
    {
            for (int j = 0; j < faces.Count; j++)
            {
                NearestPlatform(faces[j]);
            }

            if (ToView(transform.position).x < 0.0f)
            {
                transform.position = new Vector3(toWorld(ToView(transform.position)).x - .1f, toWorld(ToView(transform.position)).y, transform.position.z);
                //Velocity.x += -Velocity.x * 0.27f;
                Velocity = Vector2.Reflect(Velocity, Vector2.right) * BOUNCEDECAY;
            }
            if (ToView(transform.position).x > 1.0f)
            {
                transform.position = new Vector3(toWorld(ToView(transform.position)).x + .1f, toWorld(ToView(transform.position)).y, transform.position.z);
                //Velocity.x += -Velocity.x * 0.27f;
                Velocity = Vector2.Reflect(Velocity, Vector2.left) * BOUNCEDECAY;
            }

            GroundCheck();
    }

    void BuilUp()
    {
        if (buildup < cap)
            buildup += force;
        else
            buildup = cap;
    }

    Vector3 toWorld(Vector3 vector)
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(vector.x, vector.y, Camera.main.farClipPlane));
    }

    Vector3 ToView(Vector3 vector)
    {
        return Camera.main.WorldToViewportPoint(new Vector3(vector.x, vector.y, Camera.main.farClipPlane));
    }

    void GetMousePos()
    {
        Vector3 MousePos = Input.mousePosition;
        MousePos.z = 3913;
        m_ScreenMosPos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Camera.main.farClipPlane));
    }

    void ApplyForce()
    {
        transform.position += (new Vector3(Velocity.x, Velocity.y, 0.0f) * Time.deltaTime) * 3.0f;
    }

    void Gravity()
    {
        if(!isGrounded)
        {
            Velocity += new Vector2(0, GRAVITY * Time.deltaTime) * SCALEFACTOR;
            isApplyingGravity = true;
        }
    }

    void GroundCheck()
    {

        if (ToView(transform.position).y < 0.0f)
        {
            //isGrounded = true;
            //Velocity = Vector2.zero;
            //isApplyingGravity = false;
            //transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            //Debug.Log("What the Duck");
            Velocity = ReflectionTest(Velocity, Vector3.up, intersectionPoint) * BOUNCEDECAY;
        }

        //if (transform.position.y < 0.1f)
        //{
        //    isGrounded = true;
        //    Velocity = Vector2.zero;
        //    isApplyingGravity = false;
        //    transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        //}
        //else
        //{
        //    //isGrounded = false;
        //}
    }



    void getAngle()
    {
        GetMousePos();
        //Hyp Coords

        Vector3 screenPos = Camera.main.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, Camera.main.farClipPlane));

        float dx = m_ScreenMosPos.x - screenPos.x;
        float dy = m_ScreenMosPos.y - screenPos.x;

        angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
    }

    void ArrowRotate()
    {
        getAngle();

        Quaternion rotation = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, angle)
        };
        Arrow.transform.rotation = rotation;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {

            Vector3 direction = Vector3.Normalize(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), -1.0f));
            Gizmos.DrawSphere(direction + transform.position, .05f);

            Ray debugRay = new Ray(transform.position, direction);
            Gizmos.DrawRay(debugRay);




            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(intersectionPoint, .06f);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(CollisionRay);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere((transform.position + rayCheckOffset) + CollisionRay.direction * CheckMultiplier, 0.06f);

            for (int j = 0; j < faces.Count; j++)
            {
                faces[j].DrawFace();
            }
        }

    }
}





