using UnityEngine;

[System.Serializable]
public struct DVector
{
    public decimal x, y, z;

    public DVector(decimal x, decimal y, decimal z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static DVector operator+ (DVector b, DVector c)
    {
        return new DVector(b.x + c.x, b.y + c.y, b.z + c.z);
    }

    public static DVector operator *(DVector b, int c)
    {
        return new DVector(b.x * c, b.y * c, b.z * c);
    }

    public Vector3 toVector()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }
    public string Log()
    {
        return $"[{x.ToString("F2")},{y.ToString("F2")},{z.ToString("F2")}]";
    }

    public decimal Dist(DVector second)
    {
        return Sqrt(Pow(second.x - x, 2) + Pow(second.y - y, 2) + Pow(second.z - z, 2));
    }

    public decimal Pow(decimal num, int n)
    {
        decimal final = num;
        for (int i = 0; i < n-1; i++)
        {
            final *= num;
        }

        return final;
    }

    public decimal Sqrt(decimal x, decimal epsilon = 0.0M)
    {
        if (x < 0) throw new System.OverflowException("Cannot calculate square root from a negative number");

        decimal current = (decimal)System.Math.Sqrt((double)x), previous;
        do
        {
            previous = current;
            if (previous == 0.0M) return 0;
            current = (previous + x / previous) / 2;
        }
        while (System.Math.Abs(previous - current) > epsilon);
        return current;
    }
}
