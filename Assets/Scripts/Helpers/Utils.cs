using UnityEngine;
using System.Collections;


public static class Utils
{
    public static float Norm(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    public static float Lerp(float value, float min, float max)
    {
        return ((max - min) * value) + min;
    }

    public static float map(float value, float sourceMin, float SourceMax, float destMin, float DestMax)
    {
        return Lerp(Norm(value, sourceMin, SourceMax), destMin, DestMax);
    }

    public static float clamp(float value, float min, float max)
    {
        return Mathf.Min(Mathf.Max(value, Mathf.Min(min, max)), Mathf.Max(min, max));
    }

    public static float randomRange(float min, float max)
    {
        return min + Random.value * (max - min);
    }

    public static int randomInt(int min, int max)
    {
        return Mathf.FloorToInt(min + Random.value * (max - min + 1));
    }

    public static int randomDist(int min, int max, int itirations)
    {
        int total = 0;

        for (int i = 0; i < itirations; i++)
        {
            total += (int)Utils.randomRange(min, max);
        }

        return total / itirations;
    }

    public static float distance(Vector2 p1, Vector2 p2)
    {
        float dx = p1.x - p2.x;
        float dy = p1.y - p2.y;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static float distanceXY(float x0, float y0, float x1, float y1)
    {
        float dx = x1 - x0;
        float dy = y1 - y0;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static bool circleCollision(Vector2 c0, Vector2 c1, float rad0, float rad1)
    {
        //returns a bool depending on distance between p1 and p2
        return Utils.distance(c0, c1) <= rad0 + rad1;
    }

    public static bool circlePointCollision(float x, float y, Vector2 vec, Vector2 c0, float rad0)
    {
        //if particle isnt used, use vector points instead
        float circx = c0.x;
        float circy = c0.y;
        float radius = rad0;

        return Utils.distanceXY(x, y, circx, circy) < radius;
    }

    public static bool pointInRect(float x, float y, Vector2 min, Vector2 max)
    {
        //x range check
        //y range check
        //if both are true the point is inside the range

        return Utils.inRange(x, min.x, max.x) &&
               Utils.inRange(y, min.y, max.y);
    }

    public static bool inRange(float value, float min, float max)
    {

        //checks the ranges from x1 to x2 , returning true if the point is within range
        //Mathf.min and mathf.Max are used in the case of negetive values

        //Mathf.min is used when value is smallest value
        //instead of just checking value with min
        //and vice versa
        //if max is negetive it will be the smallest value istead
        return value >= Mathf.Min(min, max) && value <= Mathf.Max(min, max);
    }

    public static bool rangeIntersect(float min0, float max0, float min1, float max1)
    {
        return Mathf.Max(min0, max0) >= Mathf.Min(min1, max1) &&
             Mathf.Min(min0, max0) <= Mathf.Max(min1, max1);
    }

    public static bool rectInterest(Vector2 min0, Vector2 max0, Vector2 min1, Vector2 max2)
    {
        return Utils.rangeIntersect(min0.x, max0.x, min1.x, max2.x) &&
            Utils.rangeIntersect(min0.y, max0.y, min1.y, max2.y);
    }

    public static float dagreesToRads(float dagrees)
    {
        return dagrees / (180 * Mathf.PI);
    }

    public static float RadsToDagrees(float radians)
    {
        return radians * (180 / Mathf.PI);
    }

    public static float roundToPlaces(float value, float places)
    {
        float mult = Mathf.Pow(10, places);
        return Mathf.Round(value * mult) / mult;
    }

    public static int roundNearest(float value, float nearest)
    {
        return (int)(Mathf.Round(value / nearest) * nearest);
    }

    public static Vector2 quadtraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t, Vector2 pFinal = default(Vector2))
    {

        //standard beizer curve function
        //2nd order fuction (power of 2)
        //or Qudtraic function
        pFinal = (pFinal == default(Vector2)) ? new Vector2() : pFinal;

        pFinal.x = Mathf.Pow(1 - t, 2) * p0.x +
            (1 - t) * 2 * t * p1.x +
            t * t * p2.x;

        pFinal.y = Mathf.Pow(1 - t, 2) * p0.y +
           (1 - t) * 2 * t * p1.y +
           t * t * p2.y;

        return pFinal;
    }

    public static Vector2 cubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t, Vector2 pFinal = default(Vector2))
    {
        //shits cubed yo
        pFinal = (pFinal == default(Vector2)) ? new Vector2() : pFinal;

        pFinal.x = Mathf.Pow(1 - t, 3) * p0.x +
                 Mathf.Pow(1 - t, 2) * 3 * t * p1.x +
                 (1 - t) * 3 * t * t * p2.x +
                 t * t * t * p3.x;

        pFinal.y = Mathf.Pow(1 - t, 3) * p0.y +
                Mathf.Pow(1 - t, 2) * 3 * t * p1.y +
                (1 - t) * 3 * t * t * p2.y +
                t * t * t * p3.y;

        return pFinal;
    }

    /// <summary>
    /// t = Time,
    /// b = startValue;
    /// c = change value
    /// d = duration;
    /// 
    /// </summary>
    /// <param name="t">Time</param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static float QuadraticEaseOut(float t, float b, float c, float d)
    {
        //T = current time;
        //b = start value
        //c = return value/Change Value
        //d = duration

        t /= d;
        return -c * t * (t - 2) + b;
    }

    public static Vector2 controlPointAdjust(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        Vector2 cp = new Vector2();
        cp.x = p1.x * 2 - (p0.x + p2.x) / 2;
        cp.y = p1.y * 2 - (p0.y + p2.y) / 2;
        return cp;
    }

    public static Color RandomColor()
    {
        return new Color(Utils.randomRange(0f, 1f), Utils.randomRange(0, 1f), Utils.randomRange(0, 1f), 1f);
    }
    public static bool IsSegmentIntersection(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, ref Vector3 intersection)
    {
        //A1 is the change in Y
        //B1 is the change in X

        //Ax + By = C  => Standard Form

        float A1 = p1.y - p0.y;
        float B1 = p0.x - p1.x; //x values are reversed

        float C1 = A1 * p0.x + B1 * p0.y;

        float A2 = p3.y - p2.y;
        float B2 = p2.x - p3.x;

        float C2 = A2 * p2.x + B2 * p2.y;

        float denominator = A1 * B2 - A2 * B1;

        if (denominator == 0)
            return false;


        float intersectX = (B2 * C1 - B1 * C2) / denominator;
        float intersectY = (A1 * C2 - A2 * C1) / denominator;

        float rx0 = (intersectX - p0.x) / (p1.x - p0.x),
              ry0 = (intersectY - p0.y) / (p1.y - p0.y);
        float rx1 = (intersectX - p2.x) / (p3.x - p2.x),
              ry1 = (intersectY - p2.y) / (p3.y - p2.y);

        //if (((rx0 >= 0.0f && rx0 <= 1.0f) || (ry0 >= 0.0f && ry0 <= 1.0f)) && ((rx1 >= 0.0f && rx1 <= 1.0f) || (ry1 >= 0.0f && ry1 <= 1.0f)))
        //{
        //    intersection = new Vector3(intersectX, intersectY, 0.1f);
        //    return true;
        //}

        if ((inRange(rx0, 0.0f, 1.0f) || inRange(ry0, 0.0f, 1.0f)) && ((inRange(rx1, 0.0f, 1.0f) || inRange(ry1, 0.0f, 1.0f))))
        {
                intersection = new Vector3(intersectX, intersectY, 0.1f);
                return true;
        }

        return false;
    }

        public static bool IsSegmentIntersectionXZ(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, ref Vector3 intersection)
    {
        //A1 is the change in Y
        //B1 is the change in X

        //Ax + By = C  => Standard Form

        float A1 = p1.z - p0.z;
        float B1 = p0.x - p1.x; //x values are reversed

        float C1 = A1 * p0.x + B1 * p0.z;

        float A2 = p3.z - p2.z;
        float B2 = p2.x - p3.x;

        float C2 = A2 * p2.x + B2 * p2.z;

        float denominator = A1 * B2 - A2 * B1;

        if (denominator == 0)
            return false;

        float intersectX = (B2 * C1 - B1 * C2) / denominator;
        float intersectZ = (A1 * C2 - A2 * C1) / denominator;

        float rx0 = (intersectX - p0.x) / (p1.x - p0.x),
              rz0 = (intersectZ - p0.z) / (p1.z - p0.z);
        float rx1 = (intersectX - p2.x) / (p3.x - p2.x),
              rz1 = (intersectZ - p2.z) / (p3.z - p2.z);

        //if (((rx0 >= 0.0f && rx0 <= 1.0f) || (ry0 >= 0.0f && ry0 <= 1.0f)) && ((rx1 >= 0.0f && rx1 <= 1.0f) || (ry1 >= 0.0f && ry1 <= 1.0f)))
        //{
        //    intersection = new Vector3(intersectX, intersectY, 0.1f);
        //    return true;
        //}

        if ((inRange(rx0, 0.0f, 1.0f) || inRange(rz0, 0.0f, 1.0f)) && ((inRange(rx1, 0.0f, 1.0f) || inRange(rz1, 0.0f, 1.0f))))
        {
                intersection.z = intersectZ;
                return true;
        }

        return false;
    }

    public static bool PointInTriangle(Vector3 t0, Vector3 t1, Vector3 t2, Vector3 p)
    {
        //Compute Vectors
        Vector3 v0 = t2 - t0;
        Vector3 v1 = t1 - t0;
        Vector3 v2 = p - t0;

        //Compute dot products
        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);


        //Compute Barycentric coordinates
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        //Check if point is in triangle
        if ((u >= 0) && (v >= 0) && (u + v < 1))
            return true;

        return false;
    }

    public static Vector3 toWorld(Vector3 vector)
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(vector.x, vector.y, Camera.main.farClipPlane));
    }

    public static Vector3 ToView(Vector3 vector)
    {
        return Camera.main.WorldToViewportPoint(new Vector3(vector.x, vector.y, Camera.main.farClipPlane));
    }

}
