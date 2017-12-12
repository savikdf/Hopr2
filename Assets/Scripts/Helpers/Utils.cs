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

    
}
