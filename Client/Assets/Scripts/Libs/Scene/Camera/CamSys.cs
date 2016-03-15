﻿using UnityEngine;

namespace SDK.Lib
{
    public class CamSys
    {
        public BoxCam m_boxCam;
        public DzCam m_dzCam;

        public UICamera m_uiCam;            // 这个不是 UI 相机，这个是场景相机

        protected MCamera m_localCamera;         // 这个是系统摄像机，主要进行裁剪使用的
        protected Camera m_mainCamera;          // 主相机
        protected Camera m_uguiCam;             // UGUI 相机
        protected ThirdCameraController m_cameraController; // 摄像机控制器
        protected CameraMan m_cameraMan;        // 摄像机玩家

        public CamSys()
        {
            
        }

        public MCamera getLocalCamera()
        {
            return m_localCamera;
        }

        public void setLocalCamera(Camera cam)
        {
            m_localCamera = new MCamera(cam.gameObject.transform);
            m_localCamera.setFOVy(new MRadian(UtilMath.DegreesToRadians(cam.fieldOfView)));
            m_localCamera.setFarClipDistance(cam.farClipPlane);
            m_localCamera.setNearClipDistance(cam.nearClipPlane);
            m_localCamera.setAspectRatio(cam.aspect);
        }

        public void setSceneCamera2UICamera()
        {
            m_uiCam.mCam = Ctx.m_instance.m_layerMgr.m_path2Go[NotDestroyPath.ND_CV_UICamera].GetComponent<Camera>();
        }

        public void setSceneCamera2MainCamera()
        {
            m_uiCam.mCam = null;
        }

        public Camera getMainCamera()
        {
            return m_mainCamera;
        }

        public void setMainCamera(Camera camera)
        {
            m_mainCamera = camera;
        }

        public Camera getUGuiCamera()
        {
            return m_uguiCam;
        }

        public void setUGuiCamera(Camera camera)
        {
            m_uguiCam = camera;
        }

        // 设置摄像机 Man Actor
        public void setCameraActor(GameObject go)
        {
            if (m_cameraController == null)
            {
                m_cameraController = new ThirdCameraController(m_mainCamera, go);
            }
            else
            {
                m_cameraController.setTarget(go);
            }

            if (m_cameraMan == null)
            {
                //m_cameraMan = new CameraMan(go);
                m_cameraMan = new TerrainCameraMan(go);
                m_cameraMan.setCameraController(m_cameraController);
            }
            else
            {
                m_cameraMan.setActor(go);
            }
        }

        public MPlane[] getFrustumPlanes()
        {
            return m_localCamera.getFrustumPlanes();
        }

        public void invalidCamera()
        {
            if (m_localCamera != null)
            {
                m_localCamera.invalid();
                m_localCamera.updateVertexData();
            }
        }
    }
}