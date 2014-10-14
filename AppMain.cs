using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace FlappyDolphin
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static Obstacle[]	obstacles;
		private static Dolphin		dolphin;
		private static Background	background;
		private static int 			Score;	
		private static string		scoreString;
		private static int			gameState;
		
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			dolphin.Dispose();
			foreach(Obstacle obstacle in obstacles)
				obstacle.Dispose();
			background.Dispose();
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			//Set up director and UISystem.
			Score = 0;
			scoreString = Score.ToString(scoreString);
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Height*0.1f - scoreLabel.Height/2);
			scoreLabel.Text = scoreString;
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the flappy dolphin
			dolphin = new Dolphin(gameScene);
			
			//Create some obstacles.
			obstacles = new Obstacle[2];
			obstacles[0] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width*0.5f, gameScene);	
			obstacles[1] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width, gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
			gameState = 1;
		}
		
		public static void Update()
		{
			scoreString = Score.ToString(scoreString);
			scoreLabel.Text = scoreString;
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			//If tapped, inform the dolphin.
			if(touches.Count > 0)
				dolphin.Tapped();
			
			//Update the dolphin.
			dolphin.Update(0.0f);
			
			//loop through the obstacles to see when one collides with the dolphin
			if(gameState == 1)
			{
				for(int i = 0; i < obstacles.Length; i++)
				{
					if(HasCollidedWith(dolphin.getBounds(), obstacles[i].getBounds(i)))
					{
						//dolphin.Alive = false;
						Score++;
					}
				}
			}
			if(dolphin.Alive)
			{
				//Move the background.
				background.Update(0.0f);
							
				//Update the obstacles.
				foreach(Obstacle obstacle in obstacles)
					obstacle.Update(0.0f);
			}
		}
		
		public static bool HasCollidedWith(Bounds2 sp, Bounds2 ob)
		{
			if(sp.Overlaps(ob))
			{
				return true;
			}
			
			return false;
		}
		
		
	}
}
