using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ErryneiXNode : Node {

	[Input] public SomeClass someInput;
	[Output] public SomeClass someOutput;

	[Serializable]
	public class SomeClass
	{
		public float x;
		public float y;
	}

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}

public class ErryneiXNode2 : ErryneiXNode
{
	public string otherField;
	public GameObject obj;
}