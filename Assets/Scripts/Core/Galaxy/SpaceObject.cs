[System.Serializable]
public class SpaceObject
{
    public string name;
    public DVector position;
    public DVector rotation;
    public float mass, radius;

   

    public SpaceObject(string name, DVector position, DVector rotation, float mass, float radius)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.mass = mass;
        this.radius = radius;
    }

    public SpaceObject(DVector position, DVector rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public SpaceObject()
    {

    }
}
