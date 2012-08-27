using System;
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
	
	private Texture2D selectedIcon;
	
	private GUIStyle selectedStyle;
	
	private IList<Action<System.Object>> selectionListeners;
	public IList<Action<System.Object>> SelectionListeners {
		get { return selectionListeners; }
	}
	
	private Func<System.Object, bool> isSelected;
	private Func<System.Object, bool> IsSelected {
		set { isSelected = value; }
	}
	
	public ExpandableListView(IELVContentProvider contentProvider, IELVLabelProvider labelProvider, System.Object data) {
		this.contentProvider = contentProvider;
		this.labelProvider = labelProvider;
		this.data = data;
		expandedObjects = new List<System.Object>();
		selectedStyle = new GUIStyle(GUIStyle.none);
		selectionListeners = new List<Action<System.Object>>();
		isSelected = (obj) => { return false; };
		
		openIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/ExpandableListView/icons/open.png", typeof(Texture2D));
		closeIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/ExpandableListView/icons/close.png", typeof(Texture2D));
		selectedIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/ExpandableListView/icons/selected.png", typeof(Texture2D));
		
		selectedStyle.normal.background = selectedIcon;
	}
	
	public void OnGUI () {
		DrawContents(data, 0);
	}
	
	private void DrawContents(System.Object data, int depth) {
		if(data == null) return;
		
		foreach(System.Object obj in contentProvider.GetContents (data)) {
			bool hasChild = contentProvider.isHasChildren(obj);
			bool isOpen = expandedObjects.Contains(obj);
			
			if(isSelected(obj)) {
				GUILayout.BeginHorizontal(selectedStyle);
			} else {
				GUILayout.BeginHorizontal ();
			}
			
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
			
			style.normal.background = null;
			string label = labelProvider.GetLabel(obj);
			if(GUILayout.Button (label, style, GUILayout.ExpandWidth(true))) {
				foreach(Action<System.Object> listener in selectionListeners) {
					listener(obj);
				}
			}
			
			GUILayout.EndHorizontal();
				
			if(isOpen && hasChild) {
				DrawContents(obj, depth + 1);
			}
		}
	}
}