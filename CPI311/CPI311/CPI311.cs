using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Common;

namespace CPI311
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CPI311 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;        // Drawing 2D stuff
        Texture2D bombTexture;          // Object for a texture
        SpriteFont textFont;            // Object for a font
        BasicEffect effect;
        float imageAngle = 0;           // Rotation Angle
        float scaleAngle = 0;           // Scaling "angle"
        float gameSpeed = 1;
        Common.Plane plane;

        Camera camera;
        Camera thirdCamera;
        Camera firstCamera;
        ModelObject torus;              //center sun
        ModelObject Cube;
        Planet mercury;
        Planet earth;
        Planet moon;
        KeyboardState prevKeyboardState = Keyboard.GetState();
        public CPI311()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bombTexture = Content.Load<Texture2D>("Textures/Bomb");      // Load the texture
            textFont = Content.Load<SpriteFont>("Fonts/SegoeUI");    // Load the font
            torus = new ModelObject();
            torus.Model = Content.Load<Model>("Models/Sphere");
            torus.Texture = Content.Load<Texture2D>("Textures/Stripes");
            torus.Scale *= 5;

            Cube = new ModelObject();
            Cube.Model = Content.Load<Model>("Models/Cube");
            Cube.Scale *= 3;

            mercury = new Planet();
            mercury.Parent = torus;
            mercury.RevolutionRate = MathHelper.PiOver2;
            mercury.Radius = 10;                            //distance from planet to parent
            mercury.Scale *= 2;
            mercury.Model = torus.Model;
            mercury.Texture = torus.Texture;

            earth = new Planet();
            earth.Parent = torus;
            earth.RevolutionRate = MathHelper.PiOver2;
            earth.RotationRate = MathHelper.PiOver2;
            earth.Radius = 20;
            earth.Scale *= 3;
            earth.Model = torus.Model;
            earth.Texture = torus.Texture;

            moon = new Planet();
            moon.Parent = earth;
            moon.RevolutionRate = 2*MathHelper.PiOver2;
            moon.RotationRate =  2*MathHelper.PiOver2;
            moon.Radius = 5;
            moon.Scale *= 1;
            moon.Model = earth.Model;
            moon.Texture = earth.Texture;

            // TODO: use this.Content to load your game content here
            camera = new Camera();
            camera.Position = new Vector3(0, 0, -20);
            camera.AspectRatio = GraphicsDevice.Viewport.AspectRatio;

            firstCamera = new Camera();
            firstCamera.Position = new Vector3(0, 0, -20);
            firstCamera.AspectRatio = GraphicsDevice.Viewport.AspectRatio;

            thirdCamera = new Camera();
            thirdCamera.Position = new Vector3(0, 20, 0);
            thirdCamera.RotateX = 1.5f;
            thirdCamera.AspectRatio = GraphicsDevice.Viewport.AspectRatio;

            plane = new Common.Plane(99);
            plane.Texture = Content.Load<Texture2D>("Textures/Jellyfish");
            plane.Scale *= 50;
            //plane.RotateX = 0.50f;
            plane.Position = new Vector3(0, -5, 0);
           
            effect = new BasicEffect(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            imageAngle += MathHelper.Pi * gameTime.ElapsedGameTime.Milliseconds / 500f;    // Increment the angle
            scaleAngle += MathHelper.Pi * gameTime.ElapsedGameTime.Milliseconds / 500f;    // Increment the scale
            // TODO: Add your update logic here
            torus.RotateY = MathHelper.Pi * gameTime.ElapsedGameTime.Milliseconds * gameSpeed / 1000f;
            mercury.Update(gameTime, gameSpeed);
            earth.Update(gameTime, gameSpeed);
            moon.Update(gameTime, gameSpeed);
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            if(keyboardState.IsKeyDown(Keys.W))
                firstCamera.Position += 0.01f * Vector3.UnitZ;
            if (keyboardState.IsKeyDown(Keys.S))
                firstCamera.Position -= 0.01f * Vector3.UnitZ;
            if (keyboardState.IsKeyDown(Keys.A))
                firstCamera.Position -= 0.01f * Vector3.UnitX;
            if (keyboardState.IsKeyDown(Keys.D))
                firstCamera.Position += 0.01f * Vector3.UnitX;
            if (keyboardState.IsKeyDown(Keys.Up))
                firstCamera.RotateX = 0.01f;
            if (keyboardState.IsKeyDown(Keys.Down))
                firstCamera.RotateX = -0.01f;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                firstCamera.RotateY = -0.01f;
                Cube.RotateY = -0.01f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                firstCamera.RotateY = 0.01f;
                Cube.RotateY = 0.01f;
            }
            if (keyboardState.IsKeyDown(Keys.Add))
                camera.FieldOfView -= 0.01f;
            if (keyboardState.IsKeyDown(Keys.Subtract))
                camera.FieldOfView += 0.01f;
            if (keyboardState.IsKeyDown(Keys.Multiply))//go faster
                gameSpeed += 0.05f;
            if (keyboardState.IsKeyDown(Keys.Divide))
                gameSpeed -= 0.05f;
            gameSpeed = MathHelper.Clamp(gameSpeed, 0, 2);
            if(keyboardState.IsKeyDown(Keys.Tab) && prevKeyboardState.IsKeyUp(Keys.Tab))
            {
                if (camera == firstCamera)
                {
                    camera = thirdCamera;
                }
                else
                    camera = firstCamera;
            }
            prevKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Goldenrod);
            // Set the Depth Stencil State to default for 3D rendering
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            // Setup some colors for lighting
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
            effect.DiffuseColor = new Vector3(0.5f, 0.0f, 0.0f);
            effect.SpecularColor = new Vector3(0.0f, 0.5f, 0.0f);
            effect.SpecularPower = 10;
            effect.LightingEnabled = true; // Enable lighting!
            // Some parameters for the Directional lighting
            effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
            effect.DirectionalLight0.SpecularColor = Vector3.One;
            // Provide the different matrices
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            effect.World = torus.World;
            effect.Texture = torus.Texture;
            effect.TextureEnabled = true;
            torus.Draw(effect);
            //torus.Draw(camera.View, camera.Projection);

            Cube.Position = firstCamera.Position - firstCamera.Forward * Cube.Scale;
            effect.World = Cube.World;
            Cube.Draw(effect);

            effect.World = mercury.World;
            effect.Texture = mercury.Texture;
            mercury.Draw(effect);

            effect.World = earth.World;
            effect.Texture = earth.Texture;
            earth.Draw(effect);

            effect.World = moon.World;
            effect.Texture = moon.Texture;
            effect.DiffuseColor = new Vector3(0, 1, 0) * (moon.RotationPosition / MathHelper.Pi / 2);
            moon.Draw(effect);

            effect.World = plane.World;
            effect.Texture = plane.Texture;
            effect.TextureEnabled = true;
            plane.Draw(effect);
          
            /*spriteBatch.Begin();    // First, start the sprite batch
            // Draw the bomb, simple
            spriteBatch.Draw(bombTexture, new Vector2(100, 100), Color.White);
            // Draw the bomb using scale and rotation
            spriteBatch.Draw(bombTexture, new Vector2(200, 100), // Position
                            null, Color.White, // Blend color
                            imageAngle, // Rotation
                            new Vector2(bombTexture.Width/2, bombTexture.Height/2),
                            // Rotation center, we can use width/2 and height/2
                            (float)Math.Cos(scaleAngle), // Scaling
                            SpriteEffects.None, 0);
            // Draw some string
            spriteBatch.DrawString(textFont, torus.World.ToString(), new Vector2(50, 50), Color.Black);
            // Draw some string with rotation and scale
            spriteBatch.DrawString(textFont, "CPI 311", 
                    new Vector2(50, 300), // The position (top left)
                    Color.Black,    // Text color
                    imageAngle,     // Rotation angle
                    textFont.MeasureString("CSE 311") / 2, // Rotation center
                    // using MeasureString, we can get the width/height
                    (float)Math.Sin(scaleAngle), // Scale
                    SpriteEffects.None, 0);
            spriteBatch.End();*/
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
