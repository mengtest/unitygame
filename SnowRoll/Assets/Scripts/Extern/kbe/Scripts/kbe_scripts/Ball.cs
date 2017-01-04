namespace KBEngine
{
    /**
     * @brief 场景中的一个玩家分裂
     */
    public class Ball : KBEngine.GameObject   
    {
		public Ball()
		{

        }
		
		public override void __init__()
		{
            if (null != mEntity_SDK)
            {
                mEntity_SDK.init();
            }
        }

		public override void onDestroy ()
		{
			if(isPlayer())
			{
				KBEngine.Event.deregisterIn(this);
			}

            if (null != mEntity_SDK)
            {
                mEntity_SDK.dispose();
            }
        }

        public override void SetPosition(object old)
        {
            base.SetPosition(old);

            if (null != mEntity_SDK)
            {
                this.mEntity_SDK.setPos(this.position);
            }
        }

        public override void SetDirection(object old)
        {
            base.SetDirection(old);

            if (null != mEntity_SDK)
            {
                this.mEntity_SDK.setRotateEulerAngle(this.direction);
            }
        }

        public virtual void updatePlayer(float x, float y, float z, float yaw)
		{
	    	position.x = x;
	    	position.y = y;
	    	position.z = z;
			
	    	direction.z = yaw;
		}

        // 拥有者是否是 MainPlayer
        public bool isOwnerPlayer()
        {
            bool ret = false;

            System.Int32 ownerId = (System.Int32)getDefinedProperty("OwnerId");

            if (ownerId == KBEngineApp.app.entity_id)
            {
                ret = true;
            }

            return ret;
        }

        // 设置拥有者 Id
        public void set_OwnerId(System.Int32 ownerId)
        {
            if(this.isOwnerPlayer())
            {
                if (null == this.mEntity_SDK)
                {
                    this.mEntity_SDK = new SDK.Lib.PlayerMainChild(null);
                }
                else
                {
                    this.mEntity_SDK = new SDK.Lib.PlayerOtherChild(null);
                }

                this.mEntity_SDK.setEntity_KBE(this);
                this.mEntity_SDK.setPos(this.position);
                (this.mEntity_SDK as SDK.Lib.BeingEntity).setRotateEulerAngle(this.direction);
                this.mEntity_SDK.init();

                SDK.Lib.Player player = SDK.Lib.Ctx.mInstance.mPlayerMgr.getEntityByThisId((uint)ownerId) as SDK.Lib.Player;
                if(null != player)
                {
                    player.addSplitChild(this.mEntity_SDK as SDK.Lib.PlayerChild);
                }
            }
        }

        public override void onEnterWorld()
		{
			base.onEnterWorld();
		}

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();

            if (null != this.mEntity_SDK)
            {
                this.mEntity_SDK.dispose();
            }
        }
    }
}