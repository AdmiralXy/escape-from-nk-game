using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Drawing.Drawing2D;

namespace DM_GameProject
{
    public partial class Form3 : Form
    {
        // Глобальные переменные---------------

        static public int nWalls = 0; // всего препятствий на карте
        static public int nTurrets = 10; // всего турелей на карте
        static int Player_seconds = 0, Player_wasted = 0, Player_shoots = 0, Player_turretKills = 0; // статистика игрока
        static bool isCheating = false; // статистика игрока

        // Глобальные массивы---------------

        static public Rectangle[] WallsCoords = new Rectangle[nWalls]; // массив координат препятствий
        static public Rectangle[] TurretsCoords = new Rectangle[nTurrets]; // массив координат turrets
        static public Image[] PlayerSprites;

        // Глобальные обьекты-----------------

        static GameInteraction Game; // контроллирующий игру объект
        static MyPanel GameField; // поле для рисования на карте
        static Rectangle PlayerHitbox = new Rectangle(); // хитбокс игрока
        static Turret[] Turrets = new Turret[nTurrets];
        static PlayerControl Player;
        static Mine[] Mines = new Mine[2];
        static Glider GliderObj;

        // Классы----------------------------

        class Turret
        {
            private int bulletSpeed;
            private int shootDirection;
            private int positionX;
            private int positionY;
            private readonly int nTurret;
            int ChangeX;
            int ChangeY;
            public PictureBox TurretBox;
            public Rectangle rect_Check; // Rectangle для проверки Intersects пули
            public PictureBox TurretbulletBox; // PictureBox пули
            private int xf;
            private int yf;

            public bool isTurretDead;

            public Turret(int BulletSpeed, int ShootDirection, int PosX, int PosY, Form formAdd, int numberOfTurret)
            {
                nTurret = numberOfTurret;
                bulletSpeed = BulletSpeed;
                shootDirection = ShootDirection;
                positionX = PosX;
                positionY = PosY;
                TurretBox = new PictureBox();
                rect_Check = new Rectangle(0, 0, 15, 15); // Rectangle для проверки Intersects пули
                TurretbulletBox = new PictureBox(); // PictureBox пули
                TurretBox.Location = new Point(PosX, PosY);
                TurretBox.Load("..\\..\\Resources\\turret.png");
                TurretBox.SizeMode = PictureBoxSizeMode.StretchImage;
                TurretBox.Show();
                formAdd.Controls.Add(TurretBox);
                formAdd.Controls.Add(TurretbulletBox);
                TurretbulletBox.Load("..\\..\\Resources\\bullet2.png");
                TurretbulletBox.Size = new Size(15, 15);
                TurretbulletBox.SizeMode = PictureBoxSizeMode.StretchImage;
                isTurretDead = false;
                TurretBox.BackColor = Color.Transparent;
                Calculate();
            }

            public void TurretUpdate(int newX, int newY, int newShootingSpeed, int direction)
            {
                TurretbulletBox.Left = xf;
                TurretbulletBox.Top = yf;
                bulletSpeed = newShootingSpeed;
                positionX = newX;
                positionY = newY;
                TurretBox.Location = new Point(positionX, positionY);
                shootDirection = direction;
                Calculate();
                TurretbulletBox.Show();
                TurretBox.Show();
            }

            public void TurretHide()
            {
                isTurretDead = true;
                TurretbulletBox.Left = TurretBox.Location.X;
                TurretbulletBox.Top = TurretBox.Location.Y;
                TurretbulletBox.Hide();
                TurretBox.Hide();
                TurretsCoords[nTurret] = new Rectangle(0, 0, 0, 0);
            }

            public void TurretDead()
            {
                isTurretDead = true;
                TurretbulletBox.Hide();
                TurretBox.Load("..\\..\\Resources\\turret_dead.png");
                TurretsCoords[nTurret] = new Rectangle(0, 0, 0, 0);
                System.Media.SoundPlayer turrethit = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\turrethit.wav");
                turrethit.Play();
                switch (shootDirection)
                {
                    case 1:
                        TurretBox.Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 2:
                        TurretBox.Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                        break;
                    case 4:
                        TurretBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                }
            }

            private void Calculate()
            {
                int xBulletLoc = 0, yBulletLoc = 0;
                var objectPos = TurretBox.Location;
                xf = 0; yf = 0; ChangeX = 0; ChangeY = 0;
                isTurretDead = false;
                TurretBox.Load("..\\..\\Resources\\turret.png");
                if (shootDirection == 1) // l
                {
                    TurretBox.Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                    TurretBox.Size = new Size(70, 50);
                    xBulletLoc = -20;
                    yBulletLoc = 18;
                    ChangeX = -bulletSpeed;
                    ChangeY = 0;
                }
                else if (shootDirection == 2) // r
                {
                    TurretBox.Image.RotateFlip(RotateFlipType.Rotate90FlipY);
                    TurretBox.Size = new Size(70, 50);
                    xBulletLoc = 50;
                    yBulletLoc = 18;
                    ChangeX = bulletSpeed;
                    ChangeY = 0;
                }
                else if (shootDirection == 3) // u
                {
                    TurretBox.Size = new Size(50, 70);
                    xBulletLoc = 18;
                    yBulletLoc = 20;
                    ChangeX = 0;
                    ChangeY = -bulletSpeed;
                }
                else if (shootDirection == 4) // d
                {
                    TurretBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    TurretBox.Size = new Size(50, 70);
                    xBulletLoc = 18;
                    yBulletLoc = 60;
                    ChangeX = 0;
                    ChangeY = bulletSpeed;
                }
                xf = objectPos.X + xBulletLoc;
                yf = objectPos.Y + yBulletLoc;
                TurretsCoords[nTurret] = new Rectangle(positionX, positionY, TurretBox.Size.Width, TurretBox.Size.Height);
            }

            public bool DoShoot(Rectangle PlayerHitbox)
            {
                if (isTurretDead == true) return false;
                TurretbulletBox.Left = TurretbulletBox.Left + ChangeX;
                TurretbulletBox.Top = TurretbulletBox.Top + ChangeY;
                rect_Check.X = TurretbulletBox.Left + ChangeX;
                rect_Check.Y = TurretbulletBox.Top + ChangeY;
                if (CheckCollision(rect_Check) || TurretbulletBox.Location.X > 1100 || TurretbulletBox.Location.X < -1 || TurretbulletBox.Location.Y > 650 || TurretbulletBox.Location.Y < -1)
                {
                    TurretbulletBox.Hide();
                    TurretbulletBox.Show();
                    TurretbulletBox.Left = xf;
                    TurretbulletBox.Top = yf;
                }
                if (rect_Check.IntersectsWith(PlayerHitbox))
                {
                    TurretbulletBox.Hide();
                    TurretbulletBox.Show();
                    TurretbulletBox.Left = xf;
                    TurretbulletBox.Top = yf;
                    return true;
                }
                return false;
            }

            private bool CheckCollision(Rectangle ThisRectangle)
            {
                for (int i = 0; i < nWalls; i++)
                    if (ThisRectangle.IntersectsWith(WallsCoords[i]))
                        return true;
                for (int i = 0; i < nTurrets; i++)
                    if (i != nTurret && ThisRectangle.IntersectsWith(TurretsCoords[i]))
                        return true;
                return false;
            }

        }

        class PlayerControl
        {
            public int hp;
            private int level;
            public int playerSpeed;
            private int playerShootingSpeed;
            private int xPos; private int yPos;
            private int xf; private int yf; private int xh; private int yh;
            private bool updatePos = true;
            public int moveDirection = 4;
            private int k = 1;
            public int AnimN = 1;
            private bool moveLR = true;
            public bool hitedAnim = false;

            public bool stopShoot = true;
            public int moveX; public int moveY;

            public Label playerInfo = new Label();
            public Label wasted = new Label();
            public Label playerStats = new Label();

            public PictureBox PlayerBox;
            Rectangle RectangleForCheck;
            PictureBox bulletBox;

            public PlayerControl(int lvl, int HP, int x, int y, int speed, int shootingspeed, Form addForm)
            {
                PlayerBox = new PictureBox();
                bulletBox = new PictureBox();
                RectangleForCheck = new Rectangle();
                RectangleForCheck.Size = new Size(50, 50);
                level = lvl;
                hp = HP;
                xPos = x;
                yPos = y;

                playerSpeed = speed;
                playerShootingSpeed = shootingspeed;
                PlayerBox.Size = new Size(53, 56);
                PlayerBox.Location = new Point(x, y);
                PlayerBox.Load("..\\..\\Resources\\Character\\Holding\\Left\\Anim_1.png");
                PlayerBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PlayerBox.BackColor = Color.Transparent;

                bulletBox.Load("..\\..\\Resources\\Character\\Bullet\\PlayerBulletL.png");
                bulletBox.Size = new Size(25, 13);
                bulletBox.SizeMode = PictureBoxSizeMode.StretchImage;
                bulletBox.Hide();

                playerInfo.AutoSize = true;
                playerInfo.Font = new Font("Lester Deco", 21, FontStyle.Bold);
                playerInfo.Location = new Point(30, 30);
                playerInfo.BackColor = Color.Transparent;

                wasted.AutoSize = true;
                wasted.Font = new Font("Arial", 48, FontStyle.Bold);
                wasted.Location = new Point(240, 310);
                wasted.BackColor = Color.Transparent;
                wasted.Text = "WASTED! TRY AGAIN ~(:";

                playerStats.AutoSize = true;
                playerStats.Font = new Font("Arial", 28, FontStyle.Bold);
                playerStats.Location = new Point(435, 125);
                playerStats.BackColor = Color.Transparent;
                playerStats.TextAlign = ContentAlignment.MiddleCenter;
                playerStats.Text = "HELOO!!";

                addForm.Controls.Add(playerStats);
                addForm.Controls.Add(PlayerBox);
                addForm.Controls.Add(bulletBox);
                addForm.Controls.Add(playerInfo);
                addForm.Controls.Add(wasted);
                wasted.Hide();
                playerStats.Hide();

                UpdateScreenInfo();
            }

            public void PlayerUpdate(int newX, int newY, int nLevel)
            {
                bulletBox.Hide();
                stopShoot = true;
                PlayerBox.Location = new Point(newX, newY);
                bulletBox.Location = new Point(newX, newY);
                hp = 10;
                level = nLevel;
            }

            public void Hurt()
            {
                System.Media.SoundPlayer hurt = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\hurt.wav");
                hurt.Play();
                hp = hp - 5;
                if (hp <= 0)
                {
                    Player_wasted++;
                    Game.LoadMap(Game.currentLevel);
                    System.Media.SoundPlayer TurretHit = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\playerdead.wav");
                    TurretHit.Play();
                    wasted.Show();
                }
            }

            public void UpdateScreenInfo()
            {
                playerInfo.Text = "Statistics :" + "\nHP: " + Convert.ToString(hp) + "\nLevel: #" + level;
            }

            public void ShowStats()
            {
                int points = 0;
                if (Player_seconds / 7 <= 30)
                    points = 10;
                else
                    points = 8;
                for (int i = 0; i < Player_wasted; i++)
                    if (i != 0 && i % 2 == 0)
                        points--;
                if (points < 0)
                    points = 0;
                if (isCheating)
                    playerStats.Text =
                    "Game End!\n"
                    + "\nYour Statistics:"
                    + "\n Time - ~" + Convert.ToString(Player_seconds / 7) + "sec per level"
                    + "\nWasted - " + Convert.ToString(Player_wasted) + " times"
                    + "\nShoots - " + Convert.ToString(Player_shoots)
                    + "\nTurrets dead - " + Convert.ToString(Player_turretKills)
                    + "\n\nCHEAT FUNCTIONS - " + Convert.ToString(0) + "/10";
                else
                    playerStats.Text =
                    "Game End!\n"
                    + "\nYour Statistics:"
                    + "\n Time - ~" + Convert.ToString(Player_seconds / 7) + "sec per level"
                    + "\nWasted - " + Convert.ToString(Player_wasted) + " times"
                    + "\nShoots - " + Convert.ToString(Player_shoots)
                    + "\nTurrets dead - " + Convert.ToString(Player_turretKills)
                    + "\n\nTOTAL POINTS - " + Convert.ToString(points) + "/10";
                playerStats.Show();
            }

            public void CreateShoot(Form addForm)
            {
                // инициализация пули
                if (updatePos == true)
                {
                    Player_shoots++;
                    System.Media.SoundPlayer ShootSound = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\shoot.wav");
                    ShootSound.Play();
                    ShootingСalculation(addForm);
                    bulletBox.Location = new Point(xf, yf);
                    bulletBox.Show();
                    updatePos = false;
                }
                bulletBox.Left = bulletBox.Left + xh;
                bulletBox.Top = bulletBox.Top + yh;
                RectangleForCheck = new Rectangle(bulletBox.Location.X, bulletBox.Location.Y, bulletBox.Width, bulletBox.Height);
                int nTurretCollision = CheckCollisionWithTurrets(RectangleForCheck);
                if (nTurretCollision > -1)
                {
                    Player_turretKills++;
                    Turrets[nTurretCollision].TurretDead();
                    updatePos = true;
                    stopShoot = true;
                    bulletBox.Hide();
                }
                if (CheckCollision(RectangleForCheck) || bulletBox.Location.X > 1280 || bulletBox.Location.X < -1 || bulletBox.Location.Y > 720 || bulletBox.Location.Y < -1)
                {
                    updatePos = true;
                    stopShoot = true;
                    bulletBox.Hide();
                }
                if (RectangleForCheck.IntersectsWith(Mines[0].MineRectangle))
                    Mines[0].Dead();
                if (RectangleForCheck.IntersectsWith(Mines[1].MineRectangle))
                    Mines[1].Dead();
            }

            private bool CheckCollision(Rectangle ThisRectangle)
            {
                for (int i = 0; i < nWalls; i++)
                    if (ThisRectangle.IntersectsWith(WallsCoords[i]))
                        return true;
                return false;
            }

            private int CheckCollisionWithTurrets(Rectangle ThisRectangle)
            {
                for (int i = 0; i < nTurrets; i++)
                    if (ThisRectangle.IntersectsWith(TurretsCoords[i]))
                        return i;
                return -1;
            }

            public void PlayerMove()
            {
                switch (moveDirection)
                {
                    case 1:
                        if (AnimN > 8) { AnimN = 1; };
                        if (!hitedAnim)
                            PlayerBox.Load("..\\..\\Resources\\Character\\Running\\Left\\Anim_" + Convert.ToString(AnimN) + ".png");
                        else
                            PlayerBox.Load("..\\..\\Resources\\Character\\Running\\LeftRed\\Anim_" + Convert.ToString(AnimN) + ".png");
                        moveLR = true;
                        break;
                    case 2:
                        if (AnimN > 8) { AnimN = 1; };
                        if (!hitedAnim)
                            PlayerBox.Load("..\\..\\Resources\\Character\\Running\\Right\\Anim_" + Convert.ToString(AnimN) + ".png");
                        else
                            PlayerBox.Load("..\\..\\Resources\\Character\\Running\\RightRed\\Anim_" + Convert.ToString(AnimN) + ".png");
                        moveLR = false;
                        break;
                    case 3:
                        if (AnimN > 8) { AnimN = 1; };
                        if (moveLR)
                            if (!hitedAnim)
                                PlayerBox.Load("..\\..\\Resources\\Character\\Running\\Left\\Anim_" + Convert.ToString(AnimN) + ".png");
                            else
                                PlayerBox.Load("..\\..\\Resources\\Character\\Running\\LeftRed\\Anim_" + Convert.ToString(AnimN) + ".png");
                        else
                            if (!hitedAnim)
                                PlayerBox.Load("..\\..\\Resources\\Character\\Running\\Right\\Anim_" + Convert.ToString(AnimN) + ".png");
                            else
                                PlayerBox.Load("..\\..\\Resources\\Character\\Running\\RightRed\\Anim_" + Convert.ToString(AnimN) + ".png");
                        break;
                    case 4:
                        if (moveLR)
                        {
                            if (AnimN > 13) { AnimN = 1; };
                            if (!hitedAnim)
                                PlayerBox.Load("..\\..\\Resources\\Character\\Holding\\Left\\Anim_" + Convert.ToString(AnimN) + ".png");
                            else
                                PlayerBox.Load("..\\..\\Resources\\Character\\Holding\\LeftRed\\Anim_" + Convert.ToString(AnimN) + ".png");
                        }
                        else
                        {
                            if (AnimN > 13) { AnimN = 1; };
                            if (!hitedAnim)
                                PlayerBox.Load("..\\..\\Resources\\Character\\Holding\\Right\\Anim_" + Convert.ToString(AnimN) + ".png");
                            else
                                PlayerBox.Load("..\\..\\Resources\\Character\\Holding\\RightRed\\Anim_" + Convert.ToString(AnimN) + ".png");
                        }
                        break;
                }
                if (k % 7 == 0)
                    AnimN++;
                k++;
                RectangleForCheck = new Rectangle(PlayerBox.Location, PlayerBox.Size);  // определение прямоугольных областей
                if (!CheckCollision(RectangleForCheck))
                {
                    PlayerBox.Location = new Point(PlayerBox.Location.X + moveX, PlayerBox.Location.Y + moveY);
                    if (moveX < 0 || moveY < 0)
                        Player.wasted.Hide();
                }
                RectangleForCheck = new Rectangle(PlayerBox.Location, PlayerBox.Size);  // определение прямоугольных областей
                if (CheckCollision(RectangleForCheck) || PlayerBox.Location.X > 1210 || PlayerBox.Location.X < -10 || PlayerBox.Location.Y > 710 || PlayerBox.Location.Y < -10)
                {
                    PlayerBox.Location = new Point(PlayerBox.Location.X - moveX, PlayerBox.Location.Y - moveY);
                }
                PlayerHitbox = new Rectangle(PlayerBox.Location.X, PlayerBox.Location.Y, PlayerBox.Width, PlayerBox.Height);
                if (PlayerBox.Location.X < -1)
                {
                    System.Media.SoundPlayer nextlevel = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\nextlevel.wav");
                    nextlevel.Play();
                    Game.NextLevel();
                }
            }

            public void ShootingСalculation(Form addForm)
            {
                var cursorPos = addForm.PointToClient(Cursor.Position);
                var objectPos = PlayerBox.Location;
                int a, b;
                if (cursorPos.X - objectPos.X < 0)
                    a = (cursorPos.X - objectPos.X) * -1;
                else
                    a = cursorPos.X - objectPos.X;
                if (cursorPos.Y - objectPos.Y < 0)
                    b = (cursorPos.Y - objectPos.Y) * -1;
                else
                    b = cursorPos.Y - objectPos.Y;

                if (a > b)
                {
                    if (cursorPos.X - objectPos.X < 0)
                    {
                        // Мышь слева от игрока
                        bulletBox.Load("..\\..\\Resources\\Character\\Bullet\\PlayerBulletL.png");
                        bulletBox.Size = new Size(25, 13);
                        xf = objectPos.X + 25; // -5
                        yf = objectPos.Y + 18;
                        xh = -playerShootingSpeed;
                        yh = 0;
                    }
                    else
                    {
                        // Мышь справа от игрока
                        bulletBox.Load("..\\..\\Resources\\Character\\Bullet\\PlayerBulletR.png");
                        bulletBox.Size = new Size(25, 13);
                        xf = objectPos.X; // + 60
                        yf = objectPos.Y + 18;
                        xh = playerShootingSpeed;
                        yh = 0;
                    }
                }
                else
                {
                    if (cursorPos.Y - objectPos.Y < 0)
                    {
                        // Мышь сверху от игрока
                        bulletBox.Load("..\\..\\Resources\\Character\\Bullet\\PlayerBulletT.png");
                        bulletBox.Size = new Size(13, 25);
                        xf = objectPos.X + 18;
                        yf = objectPos.Y + 25; // +30
                        xh = 0;
                        yh = -playerShootingSpeed;
                    }
                    else
                    {
                        // Мышь снизу от игрока
                        bulletBox.Load("..\\..\\Resources\\Character\\Bullet\\PlayerBulletD.png");
                        bulletBox.Size = new Size(13, 25);
                        xf = objectPos.X + 18;
                        yf = objectPos.Y; // +60
                        xh = 0;
                        yh = playerShootingSpeed;
                    }
                }
            }

        }

        class Mine
        {
            int speed;
            int xPos; int yPos;
            public PictureBox MineBox;
            public Rectangle MineRectangle;
            public Rectangle MineActivationRect;
            bool dead = false;
            public bool notActive = true;

            public Mine(int x, int y, int movingSpeed, Form thisForm)
            {
                MineBox = new PictureBox();
                xPos = x;
                yPos = y;
                speed = movingSpeed;
                MineBox.Size = new Size(28, 28);
                MineBox.Location = new Point(x, y);
                MineBox.Load("..\\..\\Resources\\mine.png");
                MineBox.SizeMode = PictureBoxSizeMode.StretchImage;
                MineBox.BackColor = Color.Transparent;
                MineBox.Show();
                MineRectangle = new Rectangle(x, y, MineBox.Size.Width, MineBox.Size.Height);
                thisForm.Controls.Add(MineBox);

            }

            public void Hide()
            {
                MineBox.Hide();
                dead = true;
                notActive = true;
                MineBox.Location = new Point(0, 0);
                MineRectangle = new Rectangle(0, 0, 0, 0);
                MineActivationRect = new Rectangle(0, 0, 0, 0);
            }

            public void Start()
            {
                notActive = false;
            }

            public void Dead()
            {
                if (dead == false)
                {
                    dead = true;
                    MineBox.Load("..\\..\\Resources\\minedead.png");
                    System.Media.SoundPlayer mineExplosion = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\mineExplosion.wav");
                    mineExplosion.Play();
                }
            }

            public void Update(int x, int y, int ARx, int ARy, int ARw, int ARh)
            {
                MineBox.Show();
                MineBox.Load("..\\..\\Resources\\mine.png");
                xPos = x;
                yPos = y;
                dead = false;
                MineBox.Location = new Point(xPos, yPos);
                MineActivationRect = new Rectangle(ARx, ARy, ARw, ARh);
            }

            public bool GoToEnemy()
            {
                if (dead || notActive) return false;
                var minePos = MineBox.Location;
                var objectPos = Player.PlayerBox.Location;
                int a, b;
                if (minePos.X - objectPos.X < 0)
                    a = (minePos.X - objectPos.X) * -1;
                else
                    a = minePos.X - objectPos.X;
                if (minePos.Y - objectPos.Y < 0)
                    b = (minePos.Y - objectPos.Y) * -1;
                else
                    b = minePos.Y - objectPos.Y;

                if (a > b)
                {
                    if (minePos.X - objectPos.X < 0)
                    {
                        // слева от игрока
                        xPos += 5;
                        MineBox.Location = new Point(xPos, yPos);
                        MineRectangle = new Rectangle(xPos, yPos, MineBox.Size.Width, MineBox.Size.Height);
                    }
                    else
                    {
                        // справа от игрока
                        xPos -= 5;
                        MineBox.Location = new Point(xPos, yPos);
                        MineRectangle = new Rectangle(xPos, yPos, MineBox.Size.Width, MineBox.Size.Height);
                    }
                }
                else
                {
                    if (minePos.Y - objectPos.Y < 0)
                    {
                        // сверху от игрока
                        yPos += 5;
                        MineBox.Location = new Point(xPos, yPos);
                        MineRectangle = new Rectangle(xPos, yPos, MineBox.Size.Width, MineBox.Size.Height);
                    }
                    else
                    {
                        // снизу от игрока
                        yPos -= 5;
                        MineBox.Location = new Point(xPos, yPos);
                        MineRectangle = new Rectangle(xPos, yPos, MineBox.Size.Width, MineBox.Size.Height);
                    }
                }
                if (MineRectangle.IntersectsWith(PlayerHitbox))
                {
                    return true;
                }
                return false;
            }
        }

        class Glider
        {
            public double speed;
            public double speedChange;
            public PictureBox GliderBox;
            public PictureBox GliderCrosshair;
            public Rectangle GliderCrosshairRect;
            Rectangle GliderRect;
            Rectangle GliderRectT;
            Rectangle GliderRectB;
            Rectangle GliderRectR;
            Rectangle GliderRectL;
            int TurnMovementX = 1;
            int TurnMovementY = 1;

            public bool updatePos = true;
            public bool stopShoot = false;
            public bool wallIn = false;
            public double angle;
            double Const_angle = 0.0;
            public bool stopCrosshair = false;

            int whichWallInter = -1;

            public Glider(Form thisForm)
            {
                GliderBox = new PictureBox();
                GliderCrosshair = new PictureBox();
                speed = 5;
                speedChange = 1.0;
                GliderBox.Size = new Size(38, 38);
                GliderBox.Location = new Point(0, 0);
                GliderBox.Load("..\\..\\Resources\\glider.png");
                GliderBox.SizeMode = PictureBoxSizeMode.StretchImage;
                GliderBox.BackColor = Color.Transparent;
                GliderCrosshair.Size = new Size(15, 15);
                GliderCrosshair.Load("..\\..\\Resources\\crosshair.png");
                GliderCrosshair.SizeMode = PictureBoxSizeMode.StretchImage;
                GliderCrosshair.BackColor = Color.Transparent;
                GliderBox.Hide();
                GliderCrosshair.Hide();
                thisForm.Controls.Add(GliderCrosshair);
                thisForm.Controls.Add(GliderBox);
            }

            public void HideGlider()
            {
                speed = 0;
                speedChange = 0;
                updatePos = true;
                stopShoot = true;
                TurnMovementX = 1;
                TurnMovementY = 1;
                GliderBox.Hide();
                GliderCrosshair.Hide();
            }

            public void CrosshairToMap(Form thisForm)
            {
                FindAngle(thisForm);
                if (Const_angle != angle)
                {
                    stopCrosshair = false;
                    GliderCrosshair.Location = new Point(PlayerHitbox.Location.X, PlayerHitbox.Location.Y);
                    Const_angle = angle;
                    GliderCrosshairRect = new Rectangle(GliderCrosshair.Location.X, GliderCrosshair.Location.Y, GliderCrosshair.Width, GliderCrosshair.Height);
                }
                if (!stopCrosshair)
                {
                    GliderCrosshairRect.Location = new Point(Convert.ToInt32(GliderCrosshairRect.Left + (5 * Math.Cos(angle)) * TurnMovementX), Convert.ToInt32(GliderCrosshairRect.Top - (5 * Math.Sin(angle)) * TurnMovementY));
                    if (CheckCollision(GliderCrosshairRect, 1) || GliderCrosshairRect.Location.X > 1280 || GliderCrosshairRect.Location.X < -1 || GliderCrosshairRect.Location.Y > 720 || GliderCrosshairRect.Location.Y < -1)
                    {
                        bool areaFinded = false;
                        for (int i = 0; !areaFinded && i < 4; i++) // поиск подходящей позиции по 4ём направлениям
                        {
                            int xm = 0, ym = 0;
                            if (i == 0)
                            {
                                xm = 1;
                                ym = 0;
                            }
                            else if (i == 1)
                            {
                                xm = -1;
                                ym = 0;
                            }
                            else if (i == 2)
                            {
                                xm = 0;
                                ym = 1;
                            }
                            else if (i == 3)
                            {
                                xm = 0;
                                ym = -1;
                            }
                            int defaultPosX = GliderCrosshairRect.Location.X;
                            int defaultPosY = GliderCrosshairRect.Location.Y;
                            for (int j = 0; j < 25; j++)
                            {
                                GliderCrosshairRect.Location = new Point(GliderCrosshairRect.Location.X + xm, GliderCrosshairRect.Location.Y + ym);
                                if (!CheckCollision(GliderCrosshairRect, 1))
                                {
                                    areaFinded = true;
                                    break;
                                }
                            }
                        }
                        GliderCrosshair.Location = new Point(GliderCrosshairRect.Location.X, GliderCrosshairRect.Location.Y);
                        stopCrosshair = true;
                        GliderCrosshair.Show();
                    }
                }
            }

            public void ReleaseGlider(Form thisForm)
            {
                if (updatePos == true)
                {
                    System.Media.SoundPlayer ShootSound = new System.Media.SoundPlayer(@"..\\..\\Resources\\Sounds\\shoot.wav");
                    ShootSound.Play();
                    FindAngle(thisForm);
                    GliderBox.Location = new Point(PlayerHitbox.Location.X, PlayerHitbox.Location.Y);
                    GliderBox.Show();
                    speed = 5;
                    speedChange = 1.0;
                    updatePos = false;
                }
                GliderBox.Left = Convert.ToInt32(GliderBox.Left + (speed * Math.Cos(angle)) * TurnMovementX);
                GliderBox.Top = Convert.ToInt32(GliderBox.Top - (speed * Math.Sin(angle)) * TurnMovementY);
                GliderRect = new Rectangle(GliderBox.Location.X, GliderBox.Location.Y, GliderBox.Width, GliderBox.Height);
                GliderRectT = new Rectangle(GliderBox.Location.X + 9, GliderBox.Location.Y - 10, GliderBox.Width - 18, 9);
                GliderRectB = new Rectangle(GliderBox.Location.X + 9, GliderBox.Location.Y + 38 + 1, GliderBox.Size.Width - 18, 9);
                GliderRectL = new Rectangle(GliderBox.Location.X - 10, GliderBox.Location.Y + 9, 9, GliderBox.Size.Height - 18);
                GliderRectR = new Rectangle(GliderBox.Location.X + 38 + 1, GliderBox.Location.Y + 9, 9, GliderBox.Size.Height - 18);
                int nTurretCollision = CheckCollisionWithTurrets(GliderRect);
                if (nTurretCollision > -1)
                {
                    speedChange -= 0.1;
                    speed *= speedChange;
                    Player_turretKills++;
                    Turrets[nTurretCollision].TurretDead();
                }
                if (CheckCollision(GliderRect, 0) || GliderBox.Location.X > 1210 || GliderBox.Location.X < -10 || GliderBox.Location.Y > 710 || GliderBox.Location.Y < -10)
                {
                    if (!wallIn)
                    {
                        int xNow = GliderBox.Left;
                        int yNow = GliderBox.Top;
                        int xNext = Convert.ToInt32(GliderBox.Left + (speed * Math.Cos(angle)) * TurnMovementX);
                        int yNext = Convert.ToInt32(GliderBox.Top - (speed * Math.Sin(angle)) * TurnMovementY);
                        if ((CheckCollision(GliderRectT, 1) || CheckCollision(GliderRectB, 1)) && (CheckCollision(GliderRectL, 1) || CheckCollision(GliderRectR, 1))) // удар сверху или снизу и слева или справа
                        {
                            TurnMovementY *= -1;
                            TurnMovementX *= -1;
                        }
                        else if (CheckCollision(GliderRectT, 1) || CheckCollision(GliderRectB, 1)) // удар сверху или снизу
                        {
                            TurnMovementY *= -1;
                        }
                        else if (CheckCollision(GliderRectL, 1) || CheckCollision(GliderRectR, 1)) // удар слева или справа
                        {
                            TurnMovementX *= -1;
                        }
                        speedChange -= 0.1;
                        speed *= speedChange;
                    }
                    wallIn = true;
                }
                else
                {
                    wallIn = false;
                } 
                if (GliderBox.Location.X > 1210 || GliderBox.Location.X < -10 || GliderBox.Location.Y > 710 || GliderBox.Location.Y < -10)
                    whichWallInter = -1;
            }

            public void FindAngle(Form thisForm)
            {
                var cursorPos = thisForm.PointToClient(Cursor.Position);
                var objectPos = new Point(PlayerHitbox.Location.X + 25, PlayerHitbox.Location.Y + 25);
                angle = Math.Atan2((cursorPos.Y - objectPos.Y), (cursorPos.X - objectPos.X)) * -1;
            }

            private bool CheckCollision(Rectangle ThisRectangle, int s)
            {
                for (int i = 0; i < nWalls; i++)
                {
                    if (s == 0)
                    {
                        if (i != whichWallInter && ThisRectangle.IntersectsWith(WallsCoords[i]))
                        {
                            whichWallInter = i;
                            return true;
                        }
                    }
                    else
                    {
                        if (ThisRectangle.IntersectsWith(WallsCoords[i]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            private int CheckCollisionWithTurrets(Rectangle ThisRectangle)
            {
                for (int i = 0; i < nTurrets; i++)
                    if (ThisRectangle.IntersectsWith(TurretsCoords[i]))
                        return i;
                return -1;
            }

        }

        public class MyPanel : Panel
        {
            public MyPanel()
            {
                this.DoubleBuffered = true;
            }
        }

        class GameInteraction
        {
            Bitmap Bitmap;
            public int currentLevel;

            public GameInteraction(Form addForm)
            {
                Player = new PlayerControl(1, 10, 1150, 310, 3, 15, addForm);
                for (int i = 0; i < nTurrets; i++)
                {
                    Turrets[i] = new Turret(10, 3, 0, 0, addForm, i);
                }
                Mines[0] = new Mine(1050, 310, 10, addForm);
                Mines[1] = new Mine(1050, 310, 10, addForm);
                GameField = new MyPanel();
                GameField.Parent = addForm;
                GameField.Location = new Point(0, 0);
                GameField.Width = 1280;
                GameField.Height = 720;
                GameField.BackColor = Color.Transparent;
                Bitmap = new Bitmap(GameField.Width, GameField.Height);
                ClearMap();
                GameField.Paint += Draw;
                currentLevel = 1;
                PaintMap(currentLevel);
            }

            private void Draw(object sender, PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                Pen p = new Pen(Color.White, 1);
                g.DrawImage(Bitmap, 0, 0, Bitmap.Width, Bitmap.Height);
            }

            private void PaintMap(int nLevel)
            {
                int lastLevel = 8;
                if (nLevel == lastLevel)
                    Player.ShowStats();
                else if (nLevel == lastLevel + 1)
                    Application.Restart();
                Graphics g = Graphics.FromImage(Bitmap);
                SolidBrush myBrush = new SolidBrush(Color.PeachPuff);
                Brush brushOpacity = new SolidBrush(Color.FromArgb(55, 226, 107, 56));
                StreamReader MapReader;
                Player.PlayerUpdate(1150, 310, currentLevel);
                string path = @"..\\..\\Maps\\" + Convert.ToString(nLevel) + "\\";
                int numberWalls, numberTurrets, numberMines, numberMinesZone;
                MapReader = new StreamReader(path + "Walls", System.Text.Encoding.Default);
                string[] WallsInfo = MapReader.ReadToEnd().Split(' '); numberWalls = Int32.Parse(WallsInfo[0]); nWalls = numberWalls;
                MapReader = new StreamReader(path + "Turrets", System.Text.Encoding.Default);
                string[] TurretsInfo = MapReader.ReadToEnd().Split(' '); numberTurrets = Int32.Parse(TurretsInfo[0]);
                MapReader = new StreamReader(path + "Mines", System.Text.Encoding.Default);
                string[] MinesInfo = MapReader.ReadToEnd().Split(' '); numberMines = Int32.Parse(MinesInfo[0]);
                MapReader = new StreamReader(path + "MinesZone", System.Text.Encoding.Default);
                string[] MinesZoneInfo = MapReader.ReadToEnd().Split(' '); numberMinesZone = Int32.Parse(MinesZoneInfo[0]);
                Array.Resize(ref WallsCoords, nWalls);
                int i = 1;
                for (int iString = 0; iString < numberWalls; iString++)
                {
                    g.FillRectangle(myBrush, new Rectangle(Convert.ToInt32(WallsInfo[i]), Convert.ToInt32(WallsInfo[i + 1]), Convert.ToInt32(WallsInfo[i + 2]), Convert.ToInt32(WallsInfo[i + 3])));
                    WallsCoords[iString] = new Rectangle(Convert.ToInt32(WallsInfo[i]), Convert.ToInt32(WallsInfo[i + 1]), Convert.ToInt32(WallsInfo[i + 2]), Convert.ToInt32(WallsInfo[i + 3]));
                    i += 4;
                }
                i = 1;
                for (int iString = 0; iString < numberTurrets; iString++)
                {
                    Turrets[iString].TurretUpdate(Convert.ToInt32(TurretsInfo[i]), Convert.ToInt32(TurretsInfo[i + 1]), Convert.ToInt32(TurretsInfo[i + 2]), Convert.ToInt32(TurretsInfo[i + 3]));
                    i += 4;
                }
                i = 1;
                for (int iString = 0; iString < numberMines; iString++)
                {
                    Mines[iString].Update(Convert.ToInt32(MinesInfo[i]), Convert.ToInt32(MinesInfo[i + 1]), Convert.ToInt32(MinesInfo[i + 2]), Convert.ToInt32(MinesInfo[i + 3]), Convert.ToInt32(MinesInfo[i + 4]), Convert.ToInt32(MinesInfo[i + 5]));
                    i += 6;
                }
                i = 1;
                for (int iString = 0; iString < numberMinesZone; iString++)
                {
                    g.FillRectangle(brushOpacity, new Rectangle(Convert.ToInt32(MinesZoneInfo[i]), Convert.ToInt32(MinesZoneInfo[i + 1]), Convert.ToInt32(MinesZoneInfo[i + 2]), Convert.ToInt32(MinesZoneInfo[i + 3])));
                    i += 4;
                }
                brushOpacity.Dispose();
                myBrush.Dispose();
                GameField.Invalidate();
            }

            private void ClearMap()
            {
                Graphics g = Graphics.FromImage(Bitmap);
                g.Clear(Color.Transparent);
                Player.PlayerUpdate(1150, 310, currentLevel);
                for (int i = 0; i < nTurrets; i++)
                {
                    Turrets[i].TurretUpdate(0, 0, 0, 1);
                    Turrets[i].TurretHide();
                }
                Mines[0].Hide();
                Mines[1].Hide();
                GliderObj.HideGlider();
                Player.hitedAnim = false;
            }

            public void LoadMap(int nLevel)
            {
                ClearMap();
                currentLevel = nLevel;
                PaintMap(nLevel);
            }

            public void NextLevel()
            {
                currentLevel++;
                LoadMap(currentLevel);
            }
        }

        // -----------------------------

        public Form3()
        {
            InitializeComponent();
            GliderObj = new Glider(this);
            Game = new GameInteraction(this);
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        public void Form3_Load(object sender, EventArgs e)
        {
            byte[] b = Properties.Resources.GameBackground;
            FileInfo fileInfo = new FileInfo("GameBackground.mp3");
            FileStream fs = fileInfo.OpenWrite();
            fs.Write(b, 0, b.Length);
            fs.Close();
            S_Background.URL = fileInfo.Name;
            Player.playerInfo.MouseDown += Form3_MouseDown;
            GameField.MouseDown += Form3_MouseDown;
            GameField.MouseDown += Form3_MouseDown;
            GameField.MouseDown += Form3_MouseDown;
            Player.wasted.MouseDown += Form3_MouseDown;
            Mines[0].MineBox.MouseDown += Form3_MouseDown;
            Mines[1].MineBox.MouseDown += Form3_MouseDown;
            for (int i = 0; i < nTurrets; i++)
            {
                Turrets[i].TurretbulletBox.MouseDown += Form3_MouseDown;
                Turrets[i].TurretBox.MouseDown += Form3_MouseDown;
            }
        }

        public bool CheckCollision(Rectangle ThisRectangle) // истина если есть препятствие
        {
            for (int i = 0; i < nWalls; i++)
                if (ThisRectangle.IntersectsWith(WallsCoords[i]))
                    return true;
            return false;
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e) // управление игроком
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    Player.moveDirection = 1;
                    Player.moveX = -Player.playerSpeed;
                    Player.moveY = 0;
                    break;
                case Keys.D:
                    Player.moveDirection = 2;
                    Player.moveX = Player.playerSpeed;
                    Player.moveY = 0;
                    break;
                case Keys.W:
                    Player.moveDirection = 3;
                    Player.moveX = 0;
                    Player.moveY = -Player.playerSpeed;
                    break;
                case Keys.S:
                    Player.moveDirection = 3;
                    Player.moveX = 0;
                    Player.moveY = Player.playerSpeed;
                    break;
                case Keys.E:
                    CrosshairTimer.Start();
                    break;
                case Keys.Home:
                    Player.hp = 1000;
                    isCheating = true;
                    break;
            }
        }

        private void Form3_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    Player.moveDirection = 4;
                    Player.moveX = 0;
                    Player.AnimN = 11;
                    break;
                case Keys.D:
                    Player.moveDirection = 4;
                    Player.moveX = 0;
                    Player.AnimN = 11;
                    break;
                case Keys.W:
                    Player.moveDirection = 4;
                    Player.moveY = 0;
                    Player.AnimN = 11;
                    break;
                case Keys.S:
                    Player.moveDirection = 4;
                    Player.moveY = 0;
                    Player.AnimN = 11;
                    break;
                case Keys.E:
                    GliderObj.stopShoot = false;
                    GliderTimer.Start();
                    CrosshairTimer.Stop();
                    break;
            }
        }

        private void Form3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            Player.stopShoot = false;
            playerBulletTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < nTurrets; i++)
            {
                if (Turrets[i].DoShoot(PlayerHitbox))
                {
                    PersonHitTimer.Start();
                    Player.Hurt();
                }
            }
        }

        private void playerBulletTimer_Tick(object sender, EventArgs e)
        {
            if (Player.stopShoot == false)
                Player.CreateShoot(this);
        }

        private void mediaPlayer_Enter(object sender, EventArgs e)
        {

        }

        private void mediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (S_Background.playState == WMPLib.WMPPlayState.wmppsStopped)
                S_Background.Ctlcontrols.play();
        }

        int irotate = 0;

        private void GliderTimer_Tick(object sender, EventArgs e)
        {
            if (!GliderObj.stopShoot)
            {
                if (irotate % 10 == 0)
                    GliderObj.GliderBox.Image.RotateFlip(RotateFlipType.Rotate90FlipX);
                irotate++;
                if (irotate > 100) irotate = 0;
                GliderObj.ReleaseGlider(this);
            }
            if (GliderObj.speed < 1)
                GliderTimer.Stop();
        }

        int hitWait = 0;

        private void PersonHitTimer_Tick(object sender, EventArgs e)
        {
            Player.hitedAnim = true;
            hitWait++;
            if (hitWait == 2)
            {
                PersonHitTimer.Stop();
                hitWait = 0;
                Player.hitedAnim = false;
            }
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void CrosshairTimer_Tick(object sender, EventArgs e)
        {
            if (!GliderObj.stopShoot)
                CrosshairTimer.Stop();
            else
            {
                GliderObj.CrosshairToMap(this);
                while (!GliderObj.stopCrosshair)
                    GliderObj.CrosshairToMap(this);
            }
        }

        private void S_Background_Enter(object sender, EventArgs e)
        {

        }

        private void playerMoveTimer_Tick(object sender, EventArgs e)
        {
            Player.PlayerMove();
            if (Mines[0].MineActivationRect.IntersectsWith(PlayerHitbox))
                Mines[0].notActive = false;
            if (Mines[1].MineActivationRect.IntersectsWith(PlayerHitbox))
                Mines[1].notActive = false;
            Player.UpdateScreenInfo();
        }

        private void mineMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Mines[0].GoToEnemy())
                Player.Hurt();
            if (Mines[1].GoToEnemy())
                Player.Hurt();
        }

        private void totalSecondsTimer_Tick(object sender, EventArgs e)
        {
            Player_seconds++;
        }
    }
}
