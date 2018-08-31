using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum NodeType {Room, Corridor}

struct Edge
{
    List<Vector2> vertex;

    public Edge(Vector2 v1, Vector2 v2)
    {
        vertex = new List<Vector2> { v1, v2 };
    }
};

public class PCGSimple : MonoBehaviour
{

    class Node
    {
        public Vector2 position;
        List<Vector2> pathLocuses;

        List<Path> forks = new List<Path>();

        public Node(Vector2 location)
        {
            this.position = location;
        }

    }

    class Room : Node
    {
        public GameObject room;
        public List<Edge> edges;

        public float width;
        public float height;
        public float area;

        public Room(Vector2 location) : base(location)
        {
            this.position = location;
        }
    }

    class Corridor : Node
    {
        List<Corridor> segments;
        List<Vector2> turnLocuses;

        float length;
        float width;

        public Corridor(Vector2 location) : base(location)
        {
            this.position = location;
        }
    }

    class Segment
    {
        Corridor parentCorridor;
        GameObject segment;

        Vector2 head;
        Vector2 tail;
    }

    class Path
    {
        Node pivot;
        Vector2 pivotLocus;

        List<Node> nodes = new List<Node>();
    }

    public const float CONST2D = 5f;
    public GameObject spriteTemplate;

    float numRoomMin = 4f;
    float numRoomMax = 12f;

    float roomEdgeMin = 4f;
    float roomEdgeMax = 7.5f;
    float roomAreaMin = 20f;
    float roomAreaMax = 35f;

    float CorridorLenMin = 1f;
    float CorridorLenMax = 15f;

    float ForkLenMin = 1f;
    float ForkLenMax = 3f;

    float CorridorForkProb = 0.5f;
    float RoomForkProb = 0.25f;

    float numRoom;

    private int GenUniformInt(float min, float max)
    {
        float fnum = Random.Range(min, max + 1);
        if (fnum == max + 1)
            fnum = max;

        return Mathf.FloorToInt(fnum);
    }

    private Room GenRoom(Vector2 position)
    {
        Room Room = new Room(position);

        Room.area = Random.Range(roomAreaMin, roomAreaMax);
        Room.width = Random.Range(roomEdgeMin, roomEdgeMax);
        Room.height = Room.area / Room.width;

        Vector2[] vertices = new Vector2[4];
        Vector2 vertex1 = Room.position + new Vector2(-Room.width / 2,  Room.height / 2) * CONST2D;
        Vector2 vertex2 = Room.position + new Vector2( Room.width / 2,  Room.height / 2) * CONST2D;
        Vector2 vertex3 = Room.position + new Vector2( Room.width / 2, -Room.height / 2) * CONST2D;
        Vector2 vertex4 = Room.position + new Vector2(-Room.width / 2, -Room.height / 2) * CONST2D;

        Edge edge1 = new Edge(vertex1, vertex2);
        Edge edge2 = new Edge(vertex2, vertex3);
        Edge edge3 = new Edge(vertex4, vertex3);
        Edge edge4 = new Edge(vertex1, vertex4);

        Room.edges = new List<Edge> { edge1, edge2, edge3, edge4 };
        Room.room = Instantiate<GameObject>(spriteTemplate, Room.position, Quaternion.identity, gameObject.transform);
        Room.room.transform.localScale = new Vector3(Room.width, Room.height, 0f) * CONST2D;

        // DEBUG    
        Debug.Log(Room.edges[0]);
        Debug.Log(Room.edges);

        return Room;
    }

    // Use this for initialization
    void Start()
    {
        numRoom = GenUniformInt(numRoomMin, numRoomMax);

        Room mainRoom = GenRoom(Vector2.zero);
        Path mainPath;



    }

    // Update is called once per frame
    void Update()
    {

    }
}
