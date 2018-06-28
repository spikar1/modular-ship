using System.Collections.Generic;
using UnityEngine;

public class RepairKit : MonoBehaviour, ITool {

	public float repairPerSecond = 10f;
	public bool IsCarried { get; set; }

	private readonly List<Wall> wallBuffer = new List<Wall>();
	public void Use() {
		Physics2DHelper.GetAllNear(transform.position, .5f, -1, wallBuffer);
		foreach (var wall in wallBuffer) {
			wall.Repair(Time.deltaTime * repairPerSecond);
		}
	}

	public void OnPickUp(ToolCarrier carrier) {
		IsCarried = true;
		transform.parent = carrier.transform;
		transform.localPosition = new Vector3(0f, 0f, -.5f);
		
	}

	public void OnDropped(ToolCarrier carrier) {
		IsCarried = false;
		transform.parent = carrier.transform.parent;
		transform.position = carrier.transform.position;
	}
}
