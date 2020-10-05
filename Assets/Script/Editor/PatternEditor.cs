using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Patterns))]
public class PatternEditor : Editor {


	Patterns pattern;

	private void OnEnable() {
		pattern = target as Patterns;

	}
	
	
	
	private void OnGUI() {
		
	}
	
	public override void OnInspectorGUI()
	{

		if (GUILayout.Button("Open Editor"))
		{
			pattern.Initialize();

			var win = EditorWindow.GetWindow<PaternEditorWIndow>(false, "Pattern Editor", true) as PaternEditorWIndow;
			win.Init(in pattern);
		}
	}
}
