using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour {

    public int integrity, durability;
    private List<RoomNode> nodes;
    public IReadOnlyList<RoomNode> roomNodes;
    public Ship ship { get; private set; }

    public void Initialize(Ship ship)
    {
        this.ship = ship;
        nodes = GetComponentsInChildren<RoomNode>().ToList();
        foreach (var node in nodes)
        {
            node.Initialize(this);
        }
        roomNodes = new ReadOnlyCollection<RoomNode>(nodes);
        EvaluateHealth();
    }

    private void EvaluateHealth()
    {
        foreach (var node in nodes)
        {
            Wall wall = node.GetComponent<Wall>();
            if (!wall)
                continue;
            integrity += wall.integrity;
            //durability += wall.durability;
        }
    }
}
