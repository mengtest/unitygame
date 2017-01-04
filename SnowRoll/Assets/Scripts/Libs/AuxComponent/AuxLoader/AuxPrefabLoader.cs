﻿using LuaInterface;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预制
     */
    public class AuxPrefabLoader : AuxLoaderBase
    {
        protected GameObject mSelfGo;                       // 加载的 GameObject
        protected PrefabRes mPrefabRes;                     // 预制资源
        protected ResInsEventDispatch mResInsEventDispatch; // 实例化的时候使用的分发器
        protected bool mIsInsNeedCoroutine; // 实例化是否需要协程
        protected bool mIsDestroySelf;      // 是否释放自己
        protected bool mIsNeedInsPrefab;    // 是否需要实例化预制

        protected bool mIsSetFakePos;       // 是否初始化的时候设置到很远的位置
        protected bool mIsSetInitOrientPos; // 是否 Instantiate 的时候，设置初始化方向位置信息， UI 是不需要的，UI 的初始化信息都保存在 Prefab 里面，直接从 Prefab 里面读取就行了，如果设置了不对的位置信息，可能位置就不对了
        protected ResInsEventDispatch mInsEventDispatch;

        public AuxPrefabLoader(string path = "", bool isNeedInsPrefab = true, bool isInsNeedCoroutine = true)
            : base(path)
        {
            this.mTypeId = "AuxPrefabLoader";

            this.mIsInsNeedCoroutine = isInsNeedCoroutine;
            this.mIsDestroySelf = true;
            this.mIsNeedInsPrefab = isNeedInsPrefab;

            this.mIsSetInitOrientPos = false;
            this.mIsSetFakePos = false;
        }

        public void setIsInitOrientPos(bool isSet)
        {
            this.mIsSetInitOrientPos = isSet;
        }

        public void setIsFakePos(bool isSet)
        {
            this.mIsSetFakePos = isSet;
        }

        override public void dispose()
        {
            if (this.mIsDestroySelf)
            {
                if (this.mSelfGo != null)
                {
                    UtilApi.DestroyImmediate(this.mSelfGo);
                }
            }
            base.dispose();
        }

        public GameObject selfGo
        {
            get
            {
                return mSelfGo;
            }
            set
            {
                mSelfGo = value;
            }
        }

        public bool isDestroySelf()
        {
            return this.mIsDestroySelf;
        }

        public void setDestroySelf(bool value)
        {
            this.mIsDestroySelf = value;
        }

        override public string getLogicPath()
        {
            if (mPrefabRes != null)
            {
                return mPrefabRes.getLogicPath();
            }

            return mPath;
        }

        override public void syncLoad(string path, MAction<IDispatchObject> evtHandle = null)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if (this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);
                mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndSyncLoadRes(path, null);
                onPrefabLoaded(mPrefabRes);
            }
        }

        // 异步加载对象
        override public void asyncLoad(string path, MAction<IDispatchObject> evtHandle)
        {
            if (needUnload(path))
            {
                unload();
            }

            this.setPath(path);

            if(this.isInvalid())
            {
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, evtHandle);
                mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, onPrefabLoaded);
            }
        }

        override public void asyncLoad(string path, LuaTable luaTable, LuaFunction luaFunction)
        {
            this.setPath(path);

            if (this.isInvalid())
            {
                unload();
                mEvtHandle = new ResEventDispatch();
                mEvtHandle.addEventHandle(null, null, luaTable, luaFunction);
                mPrefabRes = Ctx.mInstance.mPrefabMgr.getAndAsyncLoadRes(path, onPrefabLoaded);
            }
        }

        public void onPrefabLoaded(IDispatchObject dispObj)
        {
            // 一定要从这里再次取值，因为如果这个资源已经加载，可能在返回之前就先调用这个函数，因此这个时候 mPrefabRes 还是空值
            mPrefabRes = dispObj as PrefabRes;
            if(mPrefabRes.hasSuccessLoaded())
            {
                mIsSuccess = true;
                if (mIsNeedInsPrefab)
                {
                    if (mIsInsNeedCoroutine)
                    {
                        mResInsEventDispatch = new ResInsEventDispatch();
                        mResInsEventDispatch.addEventHandle(null, onPrefabIns);

                        if (this.mIsSetFakePos)
                        {
                            mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat, mResInsEventDispatch);
                        }
                        else
                        {
                            mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat, mResInsEventDispatch);
                        }
                    }
                    else
                    {
                        if (this.mIsSetFakePos)
                        {
                            this.selfGo = mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat);
                        }
                        else
                        {
                            this.selfGo = mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos,  UtilMath.ZeroVec3, UtilMath.UnitQuat);
                        }

                        onAllFinish();
                    }
                }
                else
                {
                    onAllFinish();
                }
            }
            else if (mPrefabRes.hasFailed())
            {
                mIsSuccess = false;
                Ctx.mInstance.mPrefabMgr.unload(mPrefabRes.getResUniqueId(), onPrefabLoaded);
                mPrefabRes = null;

                if (mEvtHandle != null)
                {
                    mEvtHandle.dispatchEvent(this);
                }
            }
        }

        public void onPrefabIns(IDispatchObject dispObj)
        {
            mResInsEventDispatch = dispObj as ResInsEventDispatch;
            this.selfGo = mResInsEventDispatch.getInsGO();
            onAllFinish();
        }

        // 所有的资源都加载完成
        public void onAllFinish()
        {
            if (this.mIsNeedInsPrefab)
            {
                if (this.selfGo != null)
                {
                    mIsSuccess = true;
                }
                else
                {
                    mIsSuccess = false;
                }
            }
            else
            {
                if(null != mPrefabRes && mPrefabRes.hasSuccessLoaded())
                {
                    mIsSuccess = true;
                }
                else
                {
                    mIsSuccess = false;
                }
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.dispatchEvent(this);
            }
        }

        override public void unload()
        {
            if(mPrefabRes != null)
            {
                Ctx.mInstance.mPrefabMgr.unload(mPrefabRes.getResUniqueId(), onPrefabLoaded);
                mPrefabRes = null;
            }

            if(mResInsEventDispatch != null)
            {
                mResInsEventDispatch.setIsValid(false);
                mResInsEventDispatch = null;
            }

            if (mEvtHandle != null)
            {
                mEvtHandle.clearEventHandle();
                mEvtHandle = null;
            }
        }

        public GameObject getGameObject()
        {
            return this.mSelfGo;
        }

        // 获取预制模板
        public GameObject getPrefabTmpl()
        {
            GameObject ret = null;
            if(null != this.mPrefabRes)
            {
                ret = this.mPrefabRes.getObject();
            }
            return ret;
        }

        public void setClientDispose(bool isDispose)
        {

        }

        public bool isClientDispose()
        {
            return false;
        }

        public UnityEngine.GameObject InstantiateObject(MAction<IDispatchObject> insHandle = null)
        {
            if(null == this.mInsEventDispatch && null != insHandle)
            {
                this.mInsEventDispatch = new ResInsEventDispatch();
            }
            if(null != insHandle)
            {
                this.mInsEventDispatch.addEventHandle(null, insHandle);
            }

            if (mIsInsNeedCoroutine)
            {
                if (null == this.mResInsEventDispatch)
                {
                    mResInsEventDispatch = new ResInsEventDispatch();
                }
                mResInsEventDispatch.addEventHandle(null, onInstantiateObjectFinish);

                if (this.mIsSetFakePos)
                {
                    mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat, mResInsEventDispatch);
                }
                else
                {
                    mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat, mResInsEventDispatch);
                }
            }
            else
            {
                if (this.mIsSetFakePos)
                {
                    this.selfGo = mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilApi.FAKE_POS, UtilMath.UnitQuat);
                }
                else
                {
                    this.selfGo = mPrefabRes.InstantiateObject(mPrefabRes.getPrefabName(), mIsSetInitOrientPos, UtilMath.ZeroVec3, UtilMath.UnitQuat);
                }

                onInstantiateObjectFinish();
            }

            return this.selfGo;
        }

        public void onInstantiateObjectFinish(IDispatchObject dispObj = null)
        {
            if(null != dispObj)
            {
                this.selfGo = mResInsEventDispatch.getInsGO();
            }

            if (null != this.mInsEventDispatch)
            {
                this.mInsEventDispatch.dispatchEvent(this);
            }
        }
    }
}