using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal pairedPortal;
    [SerializeField] private Camera portalCam;
    [SerializeField] private int recursionLimit = 8;

    private Vector3[] renderPositions;
    private Quaternion[] renderRotations;
    public MeshRenderer meshRenderer;

    private RenderTexture viewTex;

    private Vector3 screenCenter;

    private void Awake()
    {
        screenCenter = meshRenderer.transform.localPosition;
        renderPositions = new Vector3[recursionLimit];
        renderRotations = new Quaternion[recursionLimit];
    }
    public void Render()
    {
        if (!VisibleFromCamera(pairedPortal.meshRenderer, Camera.main))
        {
            pairedPortal.meshRenderer.material.SetInt("_Inactive", 1);
            pairedPortal.meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            return;
        }

        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        if (pairedPortal != null && portalCam != null)
        {
            AssignViewTexture();
            portalCam.projectionMatrix = Camera.main.projectionMatrix;

            Matrix4x4 localToWorldMat = Camera.main.transform.localToWorldMatrix;

            int startIndex = 0;
            for (int i = 0; i < recursionLimit; i++)
            {
                if (i > 0)
                {
                    MinMax2D a = GetRendererScreenBounds(meshRenderer.bounds, portalCam);
                    MinMax2D b = GetRendererScreenBounds(pairedPortal.meshRenderer.bounds, portalCam);

                    if (!a.Intersects(b))
                        break;
                }
                localToWorldMat = transform.localToWorldMatrix * pairedPortal.transform.worldToLocalMatrix * localToWorldMat;

                //store values back to front
                int renderIndex = recursionLimit - i - 1;
                renderPositions[renderIndex] = localToWorldMat.GetColumn(3);
                renderRotations[renderIndex] = localToWorldMat.rotation;

                portalCam.transform.SetPositionAndRotation(renderPositions[renderIndex], renderRotations[renderIndex]);

                //track the renderIndex in case of the loop breaking
                startIndex = renderIndex;
            }

            //pairedPortal.meshRenderer.material.SetInt("_Inactive", 0);

            for (int i = startIndex; i < recursionLimit; i++)
            {
                portalCam.enabled = true;

                portalCam.transform.SetPositionAndRotation(renderPositions[i], renderRotations[i]);
                SetObliqueMatrix();
                portalCam.Render();

                portalCam.enabled = false;

                //float recursionLevel = (i - startIndex) / (recursionLimit - 1f);
                //recursionLevel = 1f - recursionLevel;

                if (i == startIndex)
                    pairedPortal.meshRenderer.material.SetInt("_Inactive", 1);
                else
                    pairedPortal.meshRenderer.material.SetInt("_Inactive", 0);

                //pairedPortal.meshRenderer.material.SetFloat("_Inactive", recursionLevel);
            }
        }

        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
    public void PostRender()
    {
        //PreventClipping(Camera.main.transform.position);
    }
    private void AssignViewTexture()
    {
        if (viewTex == null || viewTex.width != Screen.width || viewTex.height != Screen.height)
        {
            if (viewTex != null)
                viewTex.Release();

            viewTex = new RenderTexture(Screen.width, Screen.height, 0);
            portalCam.targetTexture = viewTex;
            pairedPortal.meshRenderer.material.SetTexture("_MainTex", viewTex);
        }
    }
    private bool VisibleFromCamera(MeshRenderer renderer, Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
    private void SetObliqueMatrix()
    {
        int sign = Math.Sign(Vector3.Dot(transform.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(transform.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(transform.forward) * sign;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal);

        if (Mathf.Abs(camSpaceDst) > 0.2f)
        {
            Vector4 clipPlane = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);
            portalCam.projectionMatrix = Camera.main.CalculateObliqueMatrix(clipPlane);
        }
        else
        {
            portalCam.projectionMatrix = Camera.main.projectionMatrix;
        }
    }
    private static MinMax2D GetRendererScreenBounds(Bounds rendBounds, Camera cam)
    {
        Vector3[] points = new Vector3[8];
        Vector3 c = rendBounds.center;
        Vector3 e = rendBounds.extents;

        points[0] = cam.WorldToScreenPoint(c - e);
        points[1] = cam.WorldToScreenPoint(new Vector3(c.x + e.x, c.y - e.y, c.z - e.z));
        points[2] = cam.WorldToScreenPoint(new Vector3(c.x - e.x, c.y + e.y, c.z - e.z));
        points[3] = cam.WorldToScreenPoint(new Vector3(c.x - e.x, c.y - e.y, c.z + e.z));
        points[4] = cam.WorldToScreenPoint(new Vector3(c.x + e.x, c.y + e.y, c.z - e.z));
        points[5] = cam.WorldToScreenPoint(new Vector3(c.x + e.x, c.y - e.y, c.z + e.z));
        points[6] = cam.WorldToScreenPoint(new Vector3(c.x - e.x, c.y + e.y, c.z + e.z));
        points[7] = cam.WorldToScreenPoint(c + e);

        IEnumerable<Vector3> screenCorners = points;
        float maxX = screenCorners.Max(corner => corner.x);
        float minX = screenCorners.Min(corner => corner.x);
        float maxY = screenCorners.Max(corner => corner.y);
        float minY = screenCorners.Min(corner => corner.y);

        //Vector2 topRight = new Vector2(maxX, maxY);
        //Vector2 topLeft = new Vector2(minX, maxY);
        //Vector2 bottomRight = new Vector2(maxX, minY);
        //Vector2 bottomLeft = new Vector2(minX, minY);

        //float xRatio = 1920f / Screen.width;
        //float yRatio = 1080f / Screen.height;

        //float sizeX = xRatio * (Mathf.Max(topRight.x, bottomRight.x) - Mathf.Max(topLeft.x, bottomLeft.x));
        //float sizeY = yRatio * (Mathf.Max(topRight.y, topLeft.y) - Mathf.Max(bottomRight.y, bottomLeft.y));

        return new MinMax2D(minX, maxX, minY, maxY);
    }
    public struct MinMax2D
    {
        public float minX;
        public float maxX;
        public float minY;
        public float maxY;

        public MinMax2D(float minX, float maxX, float minY, float maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }
        public bool Intersects(MinMax2D other)
        {
            bool intersectsX = maxX > other.minX || minX < other.maxX;
            bool intersectsY = maxY > other.minY || minY < other.maxY;

            return intersectsX && intersectsY;
        }
    }
}