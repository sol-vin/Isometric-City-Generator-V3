#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using InputEngine.Input;

#endregion

namespace ICG
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Rectangle _screen = new Rectangle(0,0,1000,800);

		public const int MAXX = 30;
		public const int MAXY = 30;
		public const int MAXZ = 7;
		IsometricFactory _if;
		Camera _camera;

		bool _showgrid = true;
		bool _showbuildings = true;

		KeyBoardInput _randomizekey = new KeyBoardInput(Keys.Space);
		KeyBoardInput _showgridkey = new KeyBoardInput(Keys.Q);
		KeyBoardInput _showbuildingskey = new KeyBoardInput(Keys.W);
		KeyBoardInput _showinfluencekey = new KeyBoardInput(Keys.E);
		KeyBoardInput _rotateleftkey = new KeyBoardInput(Keys.A);
		KeyBoardInput _rotaterightkey = new KeyBoardInput(Keys.D);
		KeyBoardInput _generatetestpatternkey = new KeyBoardInput(Keys.R);
		KeyBoardInput _randomizegridkey = new KeyBoardInput(Keys.G);

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = false;		
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
			this.Components.Add (new InputHandler(this));
			_camera =  new Camera(_screen);
			MakeWindow(_screen);				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
			Assets.LoadContent(this);

			_if = new IsometricFactory();
			_if.GenerateGrid();
			_if.GenerateRoads(); //TODO: Fix road generartion
			_if.GenerateBlocks();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				Exit ();
			}

			_camera.Update ();

			if (_randomizekey.Pressed ()) {
				_if.GenerateGrid ();
				_if.GenerateRoads();
				_if.GenerateBlocks ();
			}

			if (_showgridkey.Pressed ())
				_showgrid = !_showgrid;
			if (_showbuildingskey.Pressed ())
				_showbuildings = !_showbuildings;

			if (_showinfluencekey.Pressed () && !_if.InfluenceViewIsOn)
				_if.ShowInfluence ();
			else if (_showinfluencekey.Pressed () && _if.InfluenceViewIsOn)
				_if.HideInfluence ();

			if (_rotateleftkey.Pressed ()) {
				_if.RotateLeft();
			}
			if (_rotaterightkey.Pressed ()) {
				_if.RotateRight();
			}

			if(_generatetestpatternkey.Pressed())
				_if.GenerateTestPattern();

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Gray);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			if(_showgrid)
				_if.DrawGrid(spriteBatch, _camera);
			if(_showbuildings)
				_if.DrawBlocks(spriteBatch,_camera);
			spriteBatch.End();
		
            
			base.Draw (gameTime);
		}

		private void MakeWindow(Rectangle screen)
        {
            if ((screen.Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) && (screen.Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
            {
                graphics.PreferredBackBufferWidth = screen.Width;
                graphics.PreferredBackBufferHeight = screen.Height;
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();
                return;
            }

            return;
        }
	}
}

