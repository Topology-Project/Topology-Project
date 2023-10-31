using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(State))]
[CanEditMultipleObjects]
public class OperatorHandlerEditor : Editor
{
	SerializedProperty operatorHandler;
    OperatorHandler @operator;
    private OperType operType;

	void OnEnable()
	{
		operatorHandler = serializedObject.FindProperty("operatorHandler");
        // @operator = operatorHandler.value
	}

	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();
		EditorGUILayout.EnumPopup("Oper", operType);
        // operatorHandler.
        // if(operType == OperType.BaseOper) operatorHandler = State.BaseOper;
        // else if(operType == OperType.AddOper) operatorHandler = State.AddOper;
        // else if(operType == OperType.MulOper) operatorHandler = State.MulOper;
		serializedObject.ApplyModifiedProperties();
	}
}
