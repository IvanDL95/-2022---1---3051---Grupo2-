using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TGC.MonoGame.TP.Models;

namespace TGC.MonoGame.TP.TGCGame
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
        public Color Background { get; set; }

        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model Model { get; set; }
        private Model RaceCarModel { get; set; }
        private Matrix RaceCarWorld { get; set; }
        private TankModel TankModel { get; set; }
        private Tank Tank { get; set; }
        private Effect Effect { get; set; }
        private Effect OtherEffect { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix TankWorld { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

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

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 250);

            TankModel = new TankModel();
            TankWorld = Matrix.Identity;
            // Tank = new Tank(this);

            RaceCarWorld = Matrix.Identity;

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

            // Cargo el modelo del logo.
            //Model = Content.Load<Model>(ContentFolder3D + "tgc-logo/tgc-logo");
            RaceCarModel = Content.Load<Model>(ContentFolder3D + "vehicles/CombatVehicle/Vehicle");
            // TankModel.Load(Content.Load<Model>(ContentFolder3D + "vehicles/Tank/tank"));

            // Tank.Initialize();

            // Cargo un efecto basico propio declarado en el Content pipeline.
            // En el juego no pueden usar BasicEffect de MG, deben usar siempre efectos propios.
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");

            // Asigno el efecto que cargue a cada parte del mesh.
            // Un modelo puede tener mas de 1 mesh internamente.
            /*
            foreach (var mesh in RaceCarModel.Meshes)
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

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();
            
            var time = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);
/* 
            TankModel.WheelRotation = time * 5;
            TankModel.SteerRotation = (float) Math.Sin(time * 0.75f) * 0.5f;
            TankModel.TurretRotation = (float) Math.Sin(time * 0.333f) * 1.25f;
            TankModel.CannonRotation = (float) Math.Sin(time * 0.25f) * 0.333f - 0.333f;
            TankModel.HatchRotation = MathHelper.Clamp((float) Math.Sin(time * 2) * 2, -1, 0);
 */
            // Basado en el tiempo que paso se va generando una rotacion.
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Tank.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            /*
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
            foreach (ModelMesh mesh in RaceCarModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = World * rotationMatrix;
                    effect.View = View;
                    effect.Projection = Projection;
                }

                mesh.Draw();
            }

            //  No anda
            // RaceCarModel.Draw(RaceCarWorld * rotationMatrix, View, Projection);
            // Calculate the camera matrices.
            var time = Convert.ToSingle(gameTime.TotalGameTime.TotalSeconds);
            // TankModel.Draw(TankWorld * Matrix.CreateRotationY(time * 0.1f), View, Projection);
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