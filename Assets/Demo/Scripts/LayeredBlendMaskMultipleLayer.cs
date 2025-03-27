using UnityEngine;
using LayeredBlendMasks.Runtime;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(LayeredBlendMaskMultipleLayer))]
public class LayeredBlendMaskMultipleLayerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying == false)
            return;

        LayeredBlendMaskMultipleLayer multipleLayer = (LayeredBlendMaskMultipleLayer)target;

        for (int i = 0; i < multipleLayer.Layers.Count; i++)
        {
            var layer = multipleLayer.Layers[i];
            bool layerAdded = multipleLayer.LayeredMask.HasLayer(layer.Clip.name);
            if (GUILayout.Button((layerAdded ? "Remove " : "Add ") + layer.Clip.name))
            {
                if (layerAdded)
                {
                    multipleLayer.LayeredMask.RemoveLayer(layer.Clip.name);
                }
                else
                {
                    multipleLayer.LayeredMask.AddLayer(layer.Clip.name, layer.Clip, layer.BlendProfile);
                }
            }
        }
    }
}
#endif


public class LayeredBlendMaskMultipleLayer : MonoBehaviour
{

    public LayeredBlendMask LayeredMask;

    [System.Serializable]
    public class LayerGroup
    {
        public AnimationClip Clip;
        public BlendProfile BlendProfile;
    }

    public List<LayerGroup> Layers = new List<LayerGroup>();

    private bool m_Init = false;

    private void Update()
    {
        if (m_Init == false && LayeredMask.Initialized == true)
        {
            m_Init = true;
            for (int i = 0; i < Layers.Count; i++)
            {
                LayeredMask.AddLayer("Layer " + i, Layers[i].Clip, Layers[i].BlendProfile);
            }
        }
    }
}
