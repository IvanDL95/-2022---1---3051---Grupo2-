﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.TP.Models;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Player;
using TGC.MonoGame.TP.Geometries.Textures;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        /// <summary>
        ///     The folder which the game will search for content.
        /// </summary>
        public const string ContentFolder = "Content";

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        public const int CarsCount = 1;

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            // Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = ContentFolder;
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        /// <summary>
        ///     Sample background color.
        /// </summary>
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Camera GameCamera { get; set; }
        private CombatVehicle PlayerVehicle { get; set; }
        private PlayerInput PlayerInput { get; set; }

        // A Quad to draw the floor
        private QuadPrimitive Quad { get; set; }
        // The Floor Texture
        private Texture2D FloorTexture { get; set; }
        // A Tiling Effect to repeat the floor and wall textures
        private Effect TilingEffect { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Enciendo Back-Face culling
            // Configuro Blend State a Opaco
            var rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.BlendState = BlendState.Opaque;
            // Seria hasta aca.

            // Configuro el tamaño de la pantalla
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            // Configuramos nuestras matrices de la escena.
            GameCamera = new IsometricCamera(
                GraphicsDevice.Viewport.AspectRatio,
                Vector3.One * 2000f,
                -Vector3.Normalize(Vector3.One)
            );

            PlayerVehicle = new CombatVehicle(Content);
            PlayerInput = new PlayerInput(PlayerVehicle);

            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Debug.WriteLine("Verbose Console API");

            // Cargo el modelo del vehículo.
            PlayerVehicle.Load(Vector3.Zero);

            //Vehicle.Load(Content.Load<Model>(ContentFolder3D + "vehicles/CombatVehicle/Vehicle"));

            // Create the Quad
            Quad = new QuadPrimitive(GraphicsDevice);
            FloorTexture = Content.Load<Texture2D>(ContentFolderTextures + "floor/adoquin");

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            //Effect = Content.Load<Effect>(ContentFolderEffects + "BasicEffect");
            TilingEffect = Content.Load<Effect>(ContentFolderEffects + "TextureTiling");
            TilingEffect.Parameters["Tiling"].SetValue(new Vector2(0f, 0f));

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            /*
            foreach (var mesh in Model.Meshes)
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
            foreach (var meshPart in mesh.MeshParts)
                meshPart.Effect = Effect;
            */
            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            KeyboardState kbState = Keyboard.GetState();

            // Capturar Input teclado
            if (kbState.IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();
            
            // Basado en el tiempo que paso.
            var time = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            PlayerInput.Update(elapsedTime, kbState);
            PlayerVehicle.Update(elapsedTime);

            GameCamera.Update(gameTime);

            Debug.WriteLine("Car World forward: " + PlayerVehicle.World.Forward);
            Debug.WriteLine("Car Speed: " + PlayerVehicle.Speed);
            Debug.WriteLine("Car Rotation: " + PlayerVehicle.Rotation);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.Black);
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.

            // Calculate the camera matrices.

            /*
            var rotationMatrix = Matrix.CreateRotationY(Rotation);
            Effect.Parameters["View"].SetValue(View);
            Effect.Parameters["Projection"].SetValue(Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.Black.ToVector3());
            foreach (var mesh in Model.Meshes)
            {
                World = mesh.ParentBone.Transform * rotationMatrix;
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            */
            PlayerVehicle.Draw(GameCamera.View, GameCamera.Projection);
            // Set the WorldViewProjection and Texture for the Floor and draw it
            TilingEffect.Parameters["WorldViewProjection"].SetValue(Matrix.Identity * (GameCamera.View * GameCamera.Projection));
            TilingEffect.Parameters["Texture"].SetValue(FloorTexture);
            Quad.Draw(TilingEffect);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }
    }
}