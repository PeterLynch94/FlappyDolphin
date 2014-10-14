using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyDolphin
{
	public class Dolphin
	{
		//Private variables.
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private static int			pushAmount = 160;
		private static float		yPositionBeforePush;
		private static bool			rise;
		private static float		angle;
		private static bool			alive;
		private static Bounds2		collisionBox;
		private static Vector2		rotationAngle;
		private static float		fallAmount;
		private static float 		riseAmount;
		private static int 			fallTimer;
		private static float 		scaleX, scaleY;
		public bool Alive { get{return alive;} set{alive = value;} }
		
		//Accessors.
		//public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Dolphin (Scene scene)
		{
			textureInfo  = new TextureInfo("/Application/textures/dolphin.png");
			scaleX = 0.15f;
			scaleY = 0.20f;
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(80.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			sprite.Pivot 	= new Vector2(0.5f, 0.5f);
			sprite.Scale 	= new Vector2(scaleX, scaleY);
			angle = 0.000f;
			rise  = false;
			alive = true;
			fallAmount = 0.0f;
			fallTimer = 0;
			riseAmount = 0.0f;
			rotationAngle = new Vector2(1.0f, 0.0f);
			collisionBox = sprite.Quad.Bounds2();
			
			//Add to the current scene.
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			
			sprite.RotationNormalize = rotationAngle;
			Bounds2 b = collisionBox;
			Vector2 a = sprite.Position;
			b.Add(a);
			collisionBox = b;
			
			if(rise)
			{
				//Amount to rotate the sprite by each time it jumps upwards
				if(rotationAngle.Y < 0.500f)
				rotationAngle.Y += riseAmount;
				
			if( (sprite.Position.Y-yPositionBeforePush) < pushAmount/4)
				{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 5f);
				} else if ((sprite.Position.Y-yPositionBeforePush) < pushAmount/3)
				{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 4f);
				} else if ((sprite.Position.Y-yPositionBeforePush) < pushAmount/2)
				{
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 3f);
				}
				else
				rise = false;
			}
			else
			{
				//Increments fallAmount to make the dolphin fall more convincingly
				if(fallAmount > -1.000f)
				fallAmount += 0.015f;
				
				//Makes the dolphin rotate downwards by fallAmount
				if(rotationAngle.Y > -0.500f)
				rotationAngle.Y -= fallAmount;
				
				//Makes the dolphin fall slower at first, then speed up on its way down
				fallTimer++;
				if(fallTimer <= 10)
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 5);
				else if (fallTimer > 10 && fallTimer <= 20)
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 6);
				else if (fallTimer > 20)
				sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 7);
			}
		}	
		
		public void Tapped()
		{
			if(!rise)
			{
				rise = true;
				if(rotationAngle.Y > 0.350f)
					rotationAngle.Y = 0.350f;
				yPositionBeforePush = sprite.Position.Y;
				fallTimer = 0;
				fallAmount = 0.000f;
				riseAmount = 0.500f;
			}
		}
		
		public Bounds2 getBounds()
		{
			Bounds2 b = collisionBox;
			Vector2 a = sprite.Position;
			b.Add(a);
			collisionBox = b;
			string s = "";
			s = b.ToString();
			Console.WriteLine("Dolphin: " + s);
			
			return collisionBox;
		}
		
			public float getWidth()
		{
			return sprite.Quad.Point10.X;
		}
		
		public float getHeight()
		{
			return sprite.Quad.Point11.Y;
		}
		
		public float getX()
		{
			return sprite.Position.X;
		}
		
		public float getY()
		{
			return sprite.Position.Y;
		}
	}
}

