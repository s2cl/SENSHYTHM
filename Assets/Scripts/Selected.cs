using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selected{

	public static MapParam Song;

	public static void Set(string filename){
		Song = MapParam.ReadWithNotes(filename);
	}

}