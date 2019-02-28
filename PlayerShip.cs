using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup
{
    class PlayerShip : Ship
    {
        Colors shipColor;
        Color color;
        Sprites spriteIndex; 
        double time = 0; //pour gérer le temps entre le passage du vaisseau d'un sprite à un autre (obsolète)
        Vector2 velocityMax = new Vector2(0, 3);
        int ellapsedTime = 0;
        List<PlayerShipBullet> listBulletLeft = new List<PlayerShipBullet>(); //liste contenant les projectiles situés à gauche du vaisseau
        List<PlayerShipBullet> listBulletRight = new List<PlayerShipBullet>(); //liste contenant les projectiles situés à droite du vaisseau
        double timeRespawn;        
        int start;
        const string path = "../scores.txt";
        bool hasWritten;

        public Sprites SpriteIndex { get => spriteIndex; set => spriteIndex = value; }
        internal Colors ShipColor { get => shipColor; set => shipColor = value; }
        public List<PlayerShipBullet> ListBulletLeft { get => listBulletLeft; set => listBulletLeft = value; }
        public List<PlayerShipBullet> ListBulletRight { get => listBulletRight; set => listBulletRight = value; }
        public double TimeRespawn { get => timeRespawn; set => timeRespawn = value; }
        public Color Color { get => color; set => color = value; }

        public enum Sprites
        {
            Red = 0, //rectangleSource = new Rectangle(0, 0, 85, 78);
            RedL1 = 1, //rectangleSource = new Rectangle(250, 0, 84, 78);
            RedL2 = 2, //rectangleSource = new Rectangle(334, 0, 81, 78);
            RedR1 = 3, //rectangleSource = new Rectangle(85, 0, 84, 78);
            RedR2 = 4, //rectangleSource = new Rectangle(170, 0, 81, 78);
            Green = 5, //rectangleSource = new Rectangle(415, 0, 85, 78);
            GreenL1 = 6, //rectangleSource = new Rectangle(667, 0, 84, 78);
            GreenL2 = 7, //rectangleSource = new Rectangle(750, 0, 81, 78);
            GreenR1 = 8, //rectangleSource = new Rectangle(501, 0, 84, 78);
            GreenR2 = 9, //rectangleSource = new Rectangle(587, 0, 81, 78);
            Yellow = 10, //rectangleSource = new Rectangle(832, 0, 85, 78);
            YellowL1 = 11, //rectangleSource = new Rectangle(1082, 0, 84, 78);
            YellowL2 = 12, //rectangleSource = new Rectangle(1166, 0, 81, 78);
            YellowR1 = 13, //rectangleSource = new Rectangle(917, 0, 84, 78);
            YellowR2 = 14, //rectangleSource = new Rectangle(1001, 0, 81, 78);
            Blue = 15, //rectangleSource = new Rectangle(1247, 0, 85, 78);
            BlueL1 = 16, //rectangleSource = new Rectangle(1495, 0, 84, 78);
            BlueL2 = 17, //rectangleSource = new Rectangle(1577, 0, 81, 78);
            BlueR1 = 18, //rectangleSource = new Rectangle(1331, 0, 84, 78);
            BlueR2 = 19, //rectangleSource = new Rectangle(1415, 0, 81, 78);
        }

        public enum Colors
        {
            Red=0,
            Green=1,
            Yellow=2,
            Blue=3
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>

        public PlayerShip()
        {
            width = 85;
            height = 78;
            spriteIndex = Sprites.Red;
            shipColor = Colors.Red;
            rectangleSource = new Rectangle(0, 0, width, height);
            velocity = new Vector2(3f, -2.5f);
            origin = new Vector2(width / 2, height / 2);
            position = new Vector2(1280 / 2, 720 / 2);
            InitialisationHitbox();
            isAlive = true;
            color = Color.Red;
        }

        public new void Reset()
        {
            base.Reset();
            width = 85;
            height = 78;
            spriteIndex = Sprites.Red;
            shipColor = Colors.Red;
            rectangleSource = new Rectangle(0, 0, width, height);
            velocity = new Vector2(3f, -2.5f);
            origin = new Vector2(width / 2, height / 2);
            position = new Vector2(1280 / 2, 720 / 2);
            InitialisationHitbox();
            isAlive = true;
            color = Color.Red;
            ellapsedTime = 0;
            time = 0;
            listBulletLeft.Clear();
            listBulletRight.Clear();
        }

        /// <summary>
        /// Gère le déplacement du vaisseau
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="level"></param>

        public void Move(GameTime gameTime, Level level)
        {
            KeyboardState key = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);                      
            MouseState mousePosition = Mouse.GetState();
            Vector2 maxVelocity = new Vector2(3f, -2.5f);
            Vector2 vel = Vector2.Zero;
            Vector2 mouseWorldPosition;

            time += gameTime.ElapsedGameTime.TotalMilliseconds;
            position += velocity;
            
            //gère le mouvement vers la gauche

            if ((key.IsKeyDown(Keys.Left) || key.IsKeyDown(Keys.Q)) || gamePad.DPad.Left == ButtonState.Pressed || gamePad.ThumbSticks.Left.X < 0)
            {
                
                switch ((int)spriteIndex)
                {
                    case 0: //Red
                    case 3: //RedR1
                    case 4: //RedR2
                        spriteIndex = Sprites.RedL1;
                        
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.Y;
                            else
                                vel.X = -maxVelocity.X;
                        rectangleSource = new Rectangle(250, 0, 84, 78);
                        break;

                    case 1: //RedL1
                            if (time >= 100)
                            {
                                spriteIndex = Sprites.RedL2;
                                if (gamePad.ThumbSticks.Left.X < 0)
                                    vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.Y;
                                else
                                    vel.X = -maxVelocity.X;
                                rectangleSource = new Rectangle(334, 0, 81, 78);
                            }
                        break;

                    case 2: //RedL2
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.Y;
                            else
                                vel.X = -maxVelocity.X;
                            
                        time = 0.0;
                        break;

                    case 5: //Green
                    case 8: //GreenR1
                    case 9: //GreenR2
                        spriteIndex = Sprites.GreenL1;
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        
                        rectangleSource = new Rectangle(667, 0, 84, 78);
                        break;

                    case 6: //GreenL1

                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        if (time >= 100)
                        {
                            rectangleSource = new Rectangle(750, 0, 81, 78);
                            spriteIndex = Sprites.GreenL2;
                        }
                        break;

                    case 7: //GreenL2
                        
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        time = 0.0;
                        break;

                    case 10: //Yellow
                    case 13: //YellowR1
                    case 14: //YellowR2
                        spriteIndex = Sprites.YellowL1;
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        rectangleSource = new Rectangle(1082, 0, 84, 78);
                        break;

                    case 11: //YellowL1
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        if (time >= 100)
                        {
                            spriteIndex = Sprites.YellowL2;
                            rectangleSource = new Rectangle(1166, 0, 81, 78);
                        }
                        break;

                    case 12: //YellowL2
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        time = 0.0;
                        break;

                    case 15: //Blue
                    case 18: //BlueR1
                    case 19: //BlueR2
                        spriteIndex = Sprites.BlueL1;
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        rectangleSource = new Rectangle(1495, 0, 84, 78);
                        break;

                    case 16: //BlueL1
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        if (time >= 100)
                        {
                            spriteIndex = Sprites.BlueL2;
                            rectangleSource = new Rectangle(1577, 0, 81, 78);
                        }
                        break;

                    case 17: //BlueL2
                            if (gamePad.ThumbSticks.Left.X < 0)
                                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                            else
                                vel.X = -maxVelocity.X;
                        time = 0.0;
                        break;
                }
            }

            //gère le mouvement vers la droite

            if ((key.IsKeyDown(Keys.Right) || key.IsKeyDown(Keys.D)) || gamePad.DPad.Right == ButtonState.Pressed || gamePad.ThumbSticks.Left.X > 0) //la balle monte lorsqu'on appuie sur la touche 'Espace'
            {
                switch ((int)spriteIndex)
                {
                    case 0: //Red
                    case 1: //RedL1
                    case 2: //RedL2
                        spriteIndex = Sprites.RedR1;
                        
                         if (gamePad.ThumbSticks.Left.X > 0)
                             vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                         else
                             vel.X = maxVelocity.X;
                        rectangleSource = new Rectangle(85, 0, 84, 78);
                        break;

                    case 3: //RedR1
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        if (time >= 100)
                        {
                            spriteIndex = Sprites.RedR2;
                            rectangleSource = new Rectangle(170, 0, 81, 78);
                        }
                        break;

                    case 4: //RedR2
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        time = 0.0;
                        break;

                    case 5: //Green
                    case 6: //GreenL1
                    case 7: //GreenL2
                        spriteIndex = Sprites.GreenR1;
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        rectangleSource = new Rectangle(501, 0, 84, 78);
                        break;

                    case 8: //GreenR1
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        if (time >= 100)
                        {
                            spriteIndex = Sprites.GreenR2;
                            rectangleSource = new Rectangle(587, 0, 81, 78);
                        }
                        break;

                    case 9: //GreenR2
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        time = 0.0;
                        break;

                    case 10: //Yellow
                    case 11: //YellowL1
                    case 12: //YellowL2
                        spriteIndex = Sprites.YellowR1;
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        rectangleSource = new Rectangle(917, 0, 84, 78);
                        break;

                    case 13: //YellowR1
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        if (time >= 100)
                        {
                            spriteIndex = Sprites.YellowR2;
                            rectangleSource = new Rectangle(1001, 0, 81, 78);
                        }
                        break;

                    case 14: //YellowR2
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        
                        time = 0.0;
                        break;

                    case 15: //Blue
                    case 16: //BlueL1
                    case 17: //BlueL2
                        spriteIndex = Sprites.BlueR1;
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        rectangleSource = new Rectangle(1331, 0, 84, 78);
                        break;

                    case 18: //BlueR1
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        if (time >= 100)
                        {
                            spriteIndex = Sprites.BlueR2;
                            rectangleSource = new Rectangle(1415, 0, 81, 78);
                        }
                        break;

                    case 19: //BlueR2
                            if (gamePad.ThumbSticks.Left.X > 0)
                                vel.X = gamePad.ThumbSticks.Left.X * maxVelocity.X;
                            else
                                vel.X = maxVelocity.X;
                        time = 0.0;
                        break;
                }
            }
            //gère le mouvement vers le haut

            if ((key.IsKeyDown(Keys.Up) || key.IsKeyDown(Keys.Z)) || gamePad.DPad.Up == ButtonState.Pressed || gamePad.ThumbSticks.Left.Y > 0)
            {

                if (gamePad.ThumbSticks.Left.Y > 0)
                    vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                else
                    vel.Y = maxVelocity.Y;
                    
            }
            // gère le mouvement vers le bas
            if ((key.IsKeyDown(Keys.Down) || key.IsKeyDown(Keys.S)) || gamePad.DPad.Down == ButtonState.Pressed || gamePad.ThumbSticks.Left.Y < 0)
            {
             
                if (gamePad.ThumbSticks.Left.Y < 0)
                vel = new Vector2(gamePad.ThumbSticks.Left.X * maxVelocity.X, gamePad.ThumbSticks.Left.Y * maxVelocity.Y);
                else
                vel.Y = -maxVelocity.Y;
            }

            //gère le cas où aucun bouton n'est pressé

            if (key.Equals(new KeyboardState()) && ((gamePad.DPad.Left == ButtonState.Released) && (gamePad.DPad.Right == ButtonState.Released)) && (gamePad.ThumbSticks.Left==new Vector2(0,0)))
            {              

                switch ((int)shipColor)
                {
                    //pour le vaisseau rouge

                    case 0:
                        time = 0.0;
                        rectangleSource = new Rectangle(0, 0, 85, 78);
                        spriteIndex = Sprites.Red;
                        break;

                    //pour le vaisseau vert

                    case 1:
                        time = 0.0;
                        rectangleSource = new Rectangle(415, 0, 85, 78);
                        spriteIndex = Sprites.Green;
                        break;

                    //pour le vaisseau jaune

                    case 2:
                        time = 0.0;
                        rectangleSource = new Rectangle(832, 0, 85, 78);
                        spriteIndex = Sprites.Yellow;
                        break;

                    //pour le vaisseau vert

                    case 3:
                        time = 0.0;
                        rectangleSource = new Rectangle(1247, 0, 85, 78);
                        spriteIndex = Sprites.Blue;
                        break;
                }
            }

            //Gestion de la direction du vaisseau par rapport à la souris
            mouseWorldPosition = Vector2.Transform(new Vector2(mousePosition.X, mousePosition.Y), Matrix.Invert(Camera.World));
            if(Shmup.IsInside)
                angle = (float)Math.Atan2(mouseWorldPosition.Y - position.Y, mouseWorldPosition.X - position.X) + (float)(Math.PI /2);


            if (gamePad.ThumbSticks.Right != new Vector2(0,0))
            {
                angle = (float)Math.Atan2(gamePad.ThumbSticks.Right.X, gamePad.ThumbSticks.Right.Y);
                Mouse.SetPosition(-1, -1);
            }
            
            //Ici on gère le fait que le vaisseau ne puisse se déplacer en dehors des limites du niveau
            if(position.X <= 0)
            {
                position.X -= position.X;
            }
            if(position.X >= Level.Width)
            {
                position.X -= position.X - Level.Width;
            }
            if (position.Y <= 0)
            {
                position.Y -= position.Y;
            }
            if (position.Y >= Level.Height)
            {
                position.Y -= position.Y - Level.Height;
            }

            velocity = vel;
            position += velocity;
            InitialisationHitbox();
        }

        /// <summary>
        /// Gère le changement de couleur du vaisseau
        /// </summary>
        public void ChangeColor()
        {
            KeyboardState key = Keyboard.GetState();
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);

            //Si le joueur appuie sur la touche 1(&) du clavier  ou sur le bouton B de la manette
            if (key.IsKeyDown(Keys.D1) || (gamepad.Buttons.B == ButtonState.Pressed))
            {
                switch ((int)spriteIndex)
                {
                    case 0:
                    case 5:
                    case 10:
                    case 15:
                        rectangleSource = new Rectangle(0, 0, 85, 78);
                        spriteIndex = Sprites.Red;
                        shipColor = Colors.Red;
                        color = Color.Red;
                        break;
                    case 1:
                    case 6:
                    case 11:
                    case 16:
                        rectangleSource = new Rectangle(250, 0, 84, 78);
                        spriteIndex = Sprites.RedL1;
                        shipColor = Colors.Red;
                        color = Color.Red;
                        break;

                    case 2:
                    case 7:
                    case 12:
                    case 17:
                        rectangleSource = new Rectangle(334, 0, 81, 78);
                        spriteIndex = Sprites.RedL2;
                        shipColor = Colors.Red;
                        color = Color.Red;
                        break;
                    case 3:
                    case 8:
                    case 13:
                    case 18:
                        rectangleSource = new Rectangle(85, 0, 84, 78);
                        spriteIndex = Sprites.RedR1;
                        shipColor = Colors.Red;
                        color = Color.Red;
                        break;
                    case 4:
                    case 9:
                    case 14:
                    case 19:
                        rectangleSource = new Rectangle(170, 0, 81, 78);
                        spriteIndex = Sprites.RedR2;
                        shipColor = Colors.Red;
                        color = Color.Red;
                        break;
                }
            }
            //Si le joueur appuie sur la touche 2(é) du clavier  ou sur le bouton A de la manette
            if (key.IsKeyDown(Keys.D2) || (gamepad.Buttons.A == ButtonState.Pressed))
            {
                switch ((int)spriteIndex)
                {
                    case 0:
                    case 5:
                    case 10:
                    case 15:
                        rectangleSource = new Rectangle(415, 0, 85, 78);
                        spriteIndex = Sprites.Green;
                        shipColor = Colors.Green;
                        color = Color.Green;
                        break;
                    case 1:
                    case 6:
                    case 11:
                    case 16:
                        rectangleSource = new Rectangle(667, 0, 84, 78);
                        spriteIndex = Sprites.GreenL1;
                        shipColor = Colors.Green;
                        color = Color.Green;
                        break;

                    case 2:
                    case 7:
                    case 12:
                    case 17:
                        rectangleSource = new Rectangle(750, 0, 81, 78);
                        spriteIndex = Sprites.GreenL2;
                        shipColor = Colors.Green;
                        color = Color.Green;
                        break;
                    case 3:
                    case 8:
                    case 13:
                    case 18:
                        rectangleSource = new Rectangle(501, 0, 84, 78);
                        spriteIndex = Sprites.GreenR1;
                        shipColor = Colors.Green;
                        color = Color.Green;
                        break;
                    case 4:
                    case 9:
                    case 14:
                    case 19:
                        rectangleSource = new Rectangle(587, 0, 80, 78);
                        spriteIndex = Sprites.GreenR2;
                        shipColor = Colors.Green;
                        color = Color.Green;
                        break;
                }
            }
            //Si le joueur appuie sur la touche 3(") du clavier  ou sur le bouton Y de la manette
            if (key.IsKeyDown(Keys.D3) || (gamepad.Buttons.Y == ButtonState.Pressed))
            {
                switch ((int)spriteIndex)
                {
                    case 0:
                    case 5:
                    case 10:
                    case 15:
                        rectangleSource = new Rectangle(832, 0, 85, 78);
                        spriteIndex = Sprites.Yellow;
                        shipColor = Colors.Yellow;
                        color = Color.Yellow;
                        break;
                    case 1:
                    case 6:
                    case 11:
                    case 16:
                        rectangleSource = new Rectangle(1082, 0, 84, 78);
                        spriteIndex = Sprites.YellowL1;
                        shipColor = Colors.Yellow;
                        color = Color.Yellow;
                        break;

                    case 2:
                    case 7:
                    case 12:
                    case 17:
                        rectangleSource = new Rectangle(1166, 0, 81, 78);
                        spriteIndex = Sprites.YellowL2;
                        shipColor = Colors.Yellow;
                        color = Color.Yellow;
                        break;
                    case 3:
                    case 8:
                    case 13:
                    case 18:
                        rectangleSource = new Rectangle(917, 0, 84, 78);
                        spriteIndex = Sprites.YellowR1;
                        shipColor = Colors.Yellow;
                        color = Color.Yellow;
                        break;
                    case 4:
                    case 9:
                    case 14:
                    case 19:
                        rectangleSource = new Rectangle(1001, 0, 81, 78);
                        spriteIndex = Sprites.YellowR2;
                        shipColor = Colors.Yellow;
                        color = Color.Yellow;
                        break;
                }
            }
            //Si le joueur appuie sur la touche 4(') du clavier  ou sur le bouton X de la manette
            if (key.IsKeyDown(Keys.D4) || (gamepad.Buttons.X == ButtonState.Pressed))
            {
                switch ((int)spriteIndex)
                {
                    case 0:
                    case 5:
                    case 10:
                    case 15:
                        rectangleSource = new Rectangle(1247, 0, 85, 78);
                        spriteIndex = Sprites.Blue;
                        shipColor = Colors.Blue;
                        color = Color.Blue;
                        break;
                    case 1:
                    case 6:
                    case 11:
                    case 16:
                        rectangleSource = new Rectangle(1495, 0, 84, 78);
                        spriteIndex = Sprites.BlueL1;
                        shipColor = Colors.Blue;
                        color = Color.Blue;
                        break;

                    case 2:
                    case 7:
                    case 12:
                    case 17:
                        rectangleSource = new Rectangle(1577, 0, 81, 78);
                        spriteIndex = Sprites.BlueL2;
                        shipColor = Colors.Blue;
                        color = Color.Blue;
                        break;
                    case 3:
                    case 8:
                    case 13:
                    case 18:
                        rectangleSource = new Rectangle(1331, 0, 84, 78);
                        spriteIndex = Sprites.BlueR1;
                        shipColor = Colors.Blue;
                        color = Color.Blue;
                        break;
                    case 4:
                    case 9:
                    case 14:
                    case 19:
                        rectangleSource = new Rectangle(1415, 0, 81, 78);
                        spriteIndex = Sprites.BlueR2;
                        shipColor = Colors.Blue;
                        color = Color.Blue;
                        break;
                }
            }
        }

        ///<summary>
        ///Gère les projectiles tirés par le vaisseau
        /// </summary>
        
        public void Fire(Bullet shipBullet, GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            if (mouseState.LeftButton == ButtonState.Pressed || gamePad.Triggers.Right == 1)
            {
                if (ellapsedTime == 0)
                {
                    Vector2 bulletPosition1 = new Vector2((float)Math.Cos(angle) * (width/2) + position.X, (float)Math.Sin(angle) * (height/2) + position.Y);
                    Vector2 bulletPosition2 = new Vector2((float)Math.Cos(angle) * (-width / 2) + position.X, (float)Math.Sin(angle) * (-height / 2) + position.Y);

                    PlayerShipBullet shipBullet1 = new PlayerShipBullet(shipBullet, angle, bulletPosition1, (int) shipColor);
                    PlayerShipBullet shipBullet2 = new PlayerShipBullet(shipBullet, angle, bulletPosition2, (int) shipColor);
                    listBulletLeft.Add(shipBullet1);
                    listBulletRight.Add(shipBullet2);
                }
                ellapsedTime++;
            }
            if((mouseState.LeftButton == ButtonState.Released && gamePad.Triggers.Right == 0) || (ellapsedTime > 10))
            {
                ellapsedTime = 0;
            }
        }
       

        /// <summary>
        /// Fonction update principale
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="shipBulleti"></param>
        /// <param name="particle"></param>
        /// <param name="level"></param>
        
        public void Update(GameTime gameTime, PlayerShipBullet shipBulleti, ParticleManager particle, Level level)
        {
            if(isAlive)
            {
                Move(gameTime, level);
                Fire(shipBulleti, gameTime);
                ChangeColor();                
            }
            else
            {
                timeRespawn += gameTime.ElapsedGameTime.TotalSeconds;

                if (!HasExploded)
                {
                    start = particle.Initialize(this, Color.White);
                    HasExploded = true;

                    GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                    SoundEffectInstance explosionInstance = SoundEffectExplosion.CreateInstance();
                    explosionInstance.Volume = 0.05f;
                    explosionInstance.Play();
                }
                else
                {
                    while (start >= particle.Particles.Count)
                        start -= (particle.Capacity / 15);
                    double framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);
                    timeParticle += gameTime.ElapsedGameTime.TotalSeconds;
                    if (timeParticle * framerate <= particle.Particle.TimeToLive +1 )
                        particle.UpdateParticle(start);
                    if (timeParticle >= 0.5)
                        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                }
                ReinitialisationHitbox();
                if (!hasWritten)
                {

                    EcritureFichier();
                    hasWritten = true;
                }
            }
            foreach (PlayerShipBullet shipBullet in listBulletLeft)
                shipBullet.UpdateBullets();
            foreach (PlayerShipBullet shipBullet in listBulletRight)
                shipBullet.UpdateBullets();
        }

       

        public static bool EstHighScore()
        {
            //pour savoir quel est le score le plus élevé et l'enregistrer
            if (File.Exists(path))
            {
                String sLine = File.ReadLines(path).Last();
                string[] line = sLine.Split(',');
                Shmup.HighScore = int.Parse(line[0]);
            }

            if (Shmup.HighScore <= Shmup.Score)
            {
                Shmup.HighScore = Shmup.Score;
                return true;
            }
            return false;
        }



        public void EcritureFichier()
        {
            String dates = DateTime.Now.ToLocalTime().ToString();
            String highscore;


            if (!File.Exists(path))
            {
                highscore = Shmup.Score + ", " + dates + Environment.NewLine;
                File.AppendAllText(path, highscore);
            }
            else
            {
                int countLine = File.ReadAllLines(path).Count();
                if (countLine < 3)
                {
                    highscore = Shmup.Score + ", " + dates + Environment.NewLine;
                    File.AppendAllText(path, highscore);
                }
                else
                {
                    if (EstHighScore())
                    {
                        var lines2 = File.ReadAllLines(path);
                        highscore = Shmup.HighScore + ", " + dates;
                        lines2[2] = highscore;
                        File.WriteAllLines(path, lines2);
                    }
                }
                var lines = File.ReadAllLines(path).OrderByDescending(x => x.Split(',')[0]);
                File.WriteAllLines(path, lines);
            }
        }


        /// <summary>
        /// Fonction Draw principale
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="particle"></param>

        public void Draw(SpriteBatch spriteBatch, ParticleManager particle)
        {           
            if (!isAlive)
                particle.Draw(spriteBatch);
            else
                spriteBatch.Draw(texture, position, rectangleSource, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);                


            foreach (PlayerShipBullet shipBullet in listBulletLeft)
                shipBullet.Draw(spriteBatch);
            foreach (PlayerShipBullet shipBullet in listBulletRight)
                shipBullet.Draw(spriteBatch);
        }       
    }
}

