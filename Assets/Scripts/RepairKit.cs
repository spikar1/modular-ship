using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : MonoBehaviour, ICarryable {
	
	
	public void Use()
	{
//		this
	}

	public bool TryPickUp(AttachmentCarrier attachmentCarrier)
	{
//		thing
		return false;
	}

	public bool TryPutDown()
	{
//		fixit
		return false;
	}
	
}
