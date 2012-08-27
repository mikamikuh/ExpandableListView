using System;
using System.Collections;

public interface IELVLabelProvider {
	string GetLabel(Object target);
	UnityEngine.Texture2D GetIcon(Object target);
}