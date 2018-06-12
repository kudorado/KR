using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPM : KR.Node<NPM> {

	// Use this for initialization
	void Start () {
		NPM.On("Update", Update);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
