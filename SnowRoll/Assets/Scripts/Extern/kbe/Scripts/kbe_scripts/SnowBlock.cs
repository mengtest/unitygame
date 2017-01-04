namespace KBEngine
{
    public class SnowBlock : KBEngine.GameObject   
    {
    	public Combat combat = null;
    	
    	public static SkillBox skillbox = new SkillBox();
    	
		public SnowBlock()
		{
            mEntity_SDK = new SDK.Lib.SnowBlock();
            mEntity_SDK.setEntity_KBE(this);
        }
		
		public override void __init__()
		{
            mEntity_SDK.init();
        }

		public override void onDestroy ()
		{
			if(isPlayer())
			{
				KBEngine.Event.deregisterIn(this);
			}

            mEntity_SDK.dispose();
        }

        public override void SetPosition(object old)
        {
            base.SetPosition(old);
            this.mEntity_SDK.setPos(this.position);
        }

        public override void SetDirection(object old)
        {
            base.SetDirection(old);
            this.mEntity_SDK.setRotateEulerAngle(this.direction);
        }

        public virtual void updatePlayer(float x, float y, float z, float yaw)
		{
	    	position.x = x;
	    	position.y = y;
	    	position.z = z;
			
	    	direction.z = yaw;
		}
		
		public override void onEnterWorld()
		{
			base.onEnterWorld();
		}

        public override void onLeaveWorld()
        {
            base.onLeaveWorld();
            this.mEntity_SDK.dispose();
        }
    }
}