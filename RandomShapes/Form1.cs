using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomShapes
{
    public partial class Form1 : Form
    {
        GraphicsPath OpenSHapepath;
        GraphicsPath CloseShapePath;
        GraphicsPath fullSquare;
        int xPath1 = 0;
        int yPath1 = 0;
        int xPath2 = 500;
        int yPath2 = 0;
        int dx = 0;
        int dy = 0;
        bool isOpenSelected = false;
        bool isCloseSelected = false;
        bool isMouseDown = false;
        bool isMerged = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            openShape(g);
            closeShape(g);

        }

        private void openShape(Graphics g)
        {
            OpenSHapepath = new GraphicsPath(FillMode.Alternate);
            
            OpenSHapepath.AddRectangle(new Rectangle(xPath1, yPath1 , 100, 100));
            OpenSHapepath.AddRectangle(new Rectangle(xPath1 + 100, yPath1, 100, 100));
            OpenSHapepath.AddRectangle(new Rectangle(xPath1 + 100, yPath1 + 100, 100, 100));
            OpenSHapepath.AddRectangle(new Rectangle(xPath1 + 200, yPath1, 100, 100));
            OpenSHapepath.CloseFigure();

            g.FillPath(Brushes.Black, OpenSHapepath);
            if (isOpenSelected)
            {
                g.FillPath(Brushes.Red, OpenSHapepath);
            }
            if (isMerged)
            {
                g.FillPath(Brushes.Green, OpenSHapepath);
            }
        }

        private void closeShape(Graphics g)
        {
            this.CloseShapePath = new GraphicsPath();
            Pen pen = new Pen(Brushes.Black, 3);

            CloseShapePath.AddRectangle(new Rectangle(xPath2, yPath2, 100, 100));
            CloseShapePath.AddRectangle(new Rectangle(xPath2 + 100, yPath2 + 100, 100, 100));
            CloseShapePath.AddRectangle(new Rectangle(xPath2, yPath2 + 100, 100, 100));
            CloseShapePath.AddRectangle(new Rectangle(xPath2 + 200, yPath2, 100, 100));
            CloseShapePath.AddRectangle(new Rectangle(xPath2 + 200, yPath2 + 100, 100, 100));
            CloseShapePath.CloseFigure();
            CloseShapePath.FillMode = FillMode.Alternate;
            g.FillPath(Brushes.Black, CloseShapePath);
            if (isCloseSelected)
            {
                g.FillPath(Brushes.Red, CloseShapePath);
            }
            if (isMerged)
            {
                pen.DashStyle = DashStyle.Dash;
                g.FillPath(Brushes.Green, CloseShapePath);
               
                
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            if (checkIntractPath1(e))
            {
                dx = e.Location.X - xPath1;
                dy = e.Location.Y - yPath1;
            } else if (checkIntractPath2(e))
            {
                dx = e.Location.X - xPath2;
                dy = e.Location.Y - yPath2;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
           
            if (checkIntractPath1(e) && isMouseDown)
            {

                xPath1 = e.Location.X - dx;
                yPath1 = e.Location.Y - dy;
                isMerged = checkSnap();
                this.Invalidate();
                if (isMerged)
                {
                    xPath1 = xPath2;
                    yPath1 = yPath2 - 100;
                    fullSquare = new GraphicsPath();
                    fullSquare.AddPath(CloseShapePath , true);
                    fullSquare.AddPath(OpenSHapepath , true);
                    this.Invalidate();
                }
            } else if (checkIntractPath2(e) && isMouseDown)
            {

                xPath2 = e.Location.X - dx;
                yPath2 = e.Location.Y - dy;
                isMerged = checkSnap();
                if (isMerged)
                {
                    xPath1 = xPath2;
                    yPath1 = yPath2 - 100;
                }
                this.Invalidate();

            }

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Matrix matrix = new Matrix();

            if (checkIntractPath1(e))
            {
                isOpenSelected = true;
                isCloseSelected = false;


            }
            else if (checkIntractPath2(e))
            {
                isCloseSelected = true;
                isOpenSelected = false;

            }
            else
            {
                isOpenSelected = false;
                isCloseSelected = false;
                
            }
            this.Invalidate();
        }
        public bool checkIntractPath1(MouseEventArgs mouse)
        {
           
            var rect2 = new System.Drawing.Rectangle(mouse.X, mouse.Y, 10, 10);
            
            return OpenSHapepath.GetBounds().Contains(rect2);

        }
        public bool checkIntractPath2(MouseEventArgs mouse)
        {
           
            var rect2 = new System.Drawing.Rectangle(mouse.X, mouse.Y, 10, 10);
            
            return CloseShapePath.GetBounds().Contains(rect2);

        }
        public bool checkSnap()
        {
           
            var rect1 = new System.Drawing.Rectangle(xPath1 + 100, yPath1 + 100, 100, 100);
            var rect2 = new System.Drawing.Rectangle(xPath2+100, yPath2, 100, 100);
            
            return rect1.IntersectsWith(rect2);

        }

    }
}
