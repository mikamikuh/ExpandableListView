using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ExpandableListView {

	private IELVContentProvider contentProvider;
	
	private IELVLabelProvider labelProvider;
	
	private System.Object data;
	
	private IList<System.Object> expandedObjects;
	
	private Texture2D openIcon;
	
	private Texture2D closeIcon;
	
	public ExpandableListView(IELVContentProvider contentProvider, IELVLabelProvider labelProvider, System.Object data) {
		this.contentProvider = contentProvider;
		this.labelProvider = labelProvider;
		this.data = data;
		expandedObjects = new List<System.Object>();
		
		openIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/ExpandableListView/icons/open.png", typeof(Texture2D));
		closeIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/ExpandableListView/icons/close.png", typeof(Texture2D));
	}
	
	public void OnGUI () {
		DrawContents(data, 0);
	}
	
	private void DrawContents(System.Object data, int depth) {
		if(data == null) return;
		
		foreach(System.Object obj in contentProvider.GetContents (data)) {
			bool hasChild = contentProvider.isHasChildren(obj);
			bool isOpen = expandedObjects.Contains(obj);
			
			GUILayout.BeginHorizontal();
			GUIStyle style = new GUIStyle (GUIStyle.none);
			
			GUILayout.Space (20 * depth);
			if(hasChild) {
				if(isOpen) {
					style.normal.background = openIcon;
					if(GUILayout.Button ("", style,  GUILayout.Width(16),  GUILayout.Height (16))) {
						expandedObjects.Remove(obj);
					}
				} else {
					style.normal.background = closeIcon;
					if(GUILayout.Button ("", style,  GUILayout.Width(16),  GUILayout.Height (16))) {
						expandedObjects.Add (obj);
					}
				}
			}
			
			style.normal.background = labelProvider.GetIcon(obj);
			GUILayout.Button ("", style,  GUILayout.Width(16),  GUILayout.Height (16));
			
			string label = labelProvider.GetLabel(obj);
			EditorGUILayout.LabelField (label);
			GUILayout.EndHorizontal();
				
			if(isOpen && hasChild) {
				DrawContents(obj, depth + 1);
			}
		}
	}
}