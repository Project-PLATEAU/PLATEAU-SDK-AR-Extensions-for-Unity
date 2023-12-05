using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PlateauToolkit.AR
{
    public class PlateauAROcclusionRendererFeature : ScriptableRendererFeature
    {
        RenderObjectsPass m_RenderObjectsPassOpaque;
        RenderObjectsPass m_RenderObjectsPassTransparent;
        RenderObjectsPass m_RenderObjectMaterialOverride;

        [SerializeField] LayerMask m_AROccludeeMask;
        [SerializeField] LayerMask m_AROccluderMask;
        [SerializeField] Material m_AROccluderMaterial;

        public void SetData(LayerMask arOccludeeLayer, LayerMask arOccluderLayer, Material arOccluderMaterial)
        {
            m_AROccludeeMask = arOccludeeLayer;
            m_AROccluderMask = arOccluderLayer;
            m_AROccluderMaterial = arOccluderMaterial;
        }

        public override void Create()
        {
            name = nameof(PlateauAROcclusionRendererFeature);

            m_RenderObjectsPassOpaque = CreateRenderObjectsPass(
                $"{nameof(PlateauAROcclusionRendererFeature)}Opaque",
                RenderQueueType.Opaque,
                m_AROccludeeMask,
                null);

            m_RenderObjectsPassTransparent = CreateRenderObjectsPass(
                $"{nameof(PlateauAROcclusionRendererFeature)}Transparent",
                RenderQueueType.Transparent,
                m_AROccludeeMask,
                null);

            m_RenderObjectMaterialOverride = CreateRenderObjectsPass(
                $"{nameof(PlateauAROcclusionRendererFeature)}OverrideMaterial",
                RenderQueueType.Transparent,
                m_AROccluderMask,
                m_AROccluderMaterial);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_RenderObjectsPassOpaque);
            renderer.EnqueuePass(m_RenderObjectsPassTransparent);
            renderer.EnqueuePass(m_RenderObjectMaterialOverride);
        }

        RenderObjectsPass CreateRenderObjectsPass(
            string name, RenderQueueType queueType, LayerMask layerMask, Material overrideMaterial)
        {
            var renderObjectsPass = new RenderObjectsPass(
                name,
                RenderPassEvent.AfterRenderingTransparents,
                Array.Empty<string>(),
                queueType,
                layerMask,
                new RenderObjects.CustomCameraSettings { overrideCamera = false });

            renderObjectsPass.SetStencilState(
                1, CompareFunction.NotEqual, StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);

            if (overrideMaterial != null)
            {
                renderObjectsPass.overrideMaterial = overrideMaterial;
                renderObjectsPass.overrideMaterialPassIndex = 0;
            }

            return renderObjectsPass;
        }
    }
}