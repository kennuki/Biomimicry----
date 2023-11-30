using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Peep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UniversalRenderPipelineAsset urpAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;

        if (urpAsset != null)
        {
            // èCâ¸RenderObjects
            ModifyRenderObjects(urpAsset);
        }
        else
        {
            Debug.LogError("Universal Render Pipeline Asset not found.");
        }
    }
    [System.Serializable]
    public struct RenderFeatureToggle
    {
        public ScriptableRendererFeature feature;
        public bool isEnabled;
    }


    void ModifyRenderObjects(UniversalRenderPipelineAsset urpAsset)
    {
        
        ScriptableRenderer renderer_target = urpAsset.GetRenderer(5);
        ScriptableRenderer renderer_dialog = urpAsset.GetRenderer(6);
        if (Input.GetMouseButtonDown(1))
        {
            //renderer_target.Equals 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
