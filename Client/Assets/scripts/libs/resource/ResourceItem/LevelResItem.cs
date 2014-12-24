﻿using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class LevelResItem : ResItem
    {
        protected string m_levelName;

        public LevelResItem()
        {
            
        }

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        override public void init(LoadItem item)
        {
            // 如果是打包成 AssetBundle ，然后放在本地磁盘，要加载的话，需要 WWW 打开，AssetBundle.CreateFromFile 是不行的
            //string path;
            //path = Application.dataPath + "/" + m_path;
            //AssetBundle asset = AssetBundle.CreateFromFile(path);
            //asset.LoadAll();
            //Object[] resArr = asset.LoadAllAssets();
            if (m_resNeedCoroutine)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(initAssetByCoroutine());
            }
            else
            {
                initAsset();
            }
        }

        override public void reset()
        {
            base.reset();
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void initAsset()
        {
            //string path = Application.dataPath + "/" + m_path;
            //string path = m_path;       // 注意这个是场景打包的时候场景的名字，不是目录，这个场景一定要 To add a level to the build settings use the menu File->Build Settings...
            Application.LoadLevel(m_levelName);

            if (onLoaded != null)
            {
                onLoaded(this);
            }

            clearListener();
        }

        protected IEnumerator initAssetByCoroutine()
        {
            //string path = Application.dataPath + "/" + m_path;
            AsyncOperation asyncOpt = Application.LoadLevelAsync(m_levelName);

            yield return asyncOpt;

            if (null != asyncOpt)
            {
                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }

            clearListener();
        }
    }
}