using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// RoboShader custom shader inspector
public class RoboShaderGUI : ShaderGUI {

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
        
        MaterialProperty _Color = ShaderGUI.FindProperty("_Color", properties);
        MaterialProperty _Tint = ShaderGUI.FindProperty("_Tint", properties);
        MaterialProperty _OutlineColor = ShaderGUI.FindProperty("_OutlineColor", properties);
        MaterialProperty _OutlineThickness = ShaderGUI.FindProperty("_OutlineThickness", properties);

        materialEditor.ShaderProperty(_Color, _Color.displayName);
        materialEditor.ShaderProperty(_Tint, _Tint.displayName);
        materialEditor.ShaderProperty(_OutlineColor, _OutlineColor.displayName);
        materialEditor.ShaderProperty(_OutlineThickness, _OutlineThickness.displayName);

    }

}