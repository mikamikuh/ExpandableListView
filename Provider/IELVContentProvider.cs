using System;
using System.Collections;
using System.Collections.Generic;

public interface IELVContentProvider {
	IList<Object> GetContents(Object target);
	bool isHasChildren(Object target);
}