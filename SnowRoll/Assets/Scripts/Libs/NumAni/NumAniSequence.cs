﻿namespace SDK.Lib
{
    /**
     * @breif 数字动画序列，线性序列，仅支持顺序执行的
     */
    public class NumAniSequence : NumAniSeqBase
    {
        public NumAniSequence()
        {
            mGo = UtilApi.createGameObject("NumAniSeq");
            mGo.transform.parent = Ctx.mInstance.mLayerMgr.mPath2Go[NotDestroyPath.ND_CV_App].transform;
            NumAniSeqBehaviour seqBeh = mGo.AddComponent<NumAniSeqBehaviour>();
            seqBeh.onAniEndDisp = onOneAniEndHandle;
        }

        public void play()
        {
            nextAni();
        }

        override protected void onOneAniEndHandle(NumAniBase ani)
        {
            if (ani.decItweenCount() == 0)      // 如果 ITween 全部播放完成
            {
                endCurAni(ani);
                if (!nextAni())
                {
                    if (m_aniSeqEndDisp != null)
                    {
                        m_aniSeqEndDisp(this);
                    }
                }
            }
        }

        // 成功开始下一个动画返回 true ，否则返回 false
        protected bool nextAni()
        {
            if(m_numAniList.Count > 0)
            {
                if (!m_numAniList[0].isPlaying())
                {
                    m_numAniList[0].play();
                    return true;
                }
            }

            return false;
        }
    }
}