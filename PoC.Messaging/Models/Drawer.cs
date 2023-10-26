using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Messaging.Models
{
    internal class Drawer
    {
        public Drawer(int sizeX, int sizeY)
        {
            MultiplierY = 1;
            MultiplierX = 2;
            SizeX = sizeX * MultiplierX;
            SizeY = sizeY * MultiplierY;
        }

        public int SizeX { get; private set; }
        public int MultiplierX { get; private set; }
        public int SizeY { get; private set; }
        public int MultiplierY { get; private set; }

        public Point LeftTopPoint => new(0, 0);
        public Point RightTopPoint => new(SizeX, 0);
        public Point LeftBottomPoint => new(0, SizeY);
        public Point RightBottomPoint => new(SizeX, SizeY);

        public void Draw()
        {
            DrawLeftBorder();
            DrawRightBorder();
            DrawTopBorder();
            DrawBottomBorder();
        }

        private void DrawLeftBorder()
        {
            WriteVerticalLine(LeftTopPoint, SizeY);
        }
        private void DrawRightBorder()
        {
            WriteVerticalLine(RightTopPoint, SizeY);
        }
        private void DrawTopBorder()
        {
            WriteHorizontalLine(LeftTopPoint, SizeX);
        }
        private void DrawBottomBorder()
        {
            WriteHorizontalLine(LeftBottomPoint, SizeX);
        }

        private static void WriteHorizontalLine(Point point, int numberOfChars)
        {
            SetCursor(point);

            string line = string.Empty;
            for (int i = 0; i < numberOfChars; i++)
            {
                line += "_";
            }

            Console.Write(line);

            SetCursor(point);
        }

        private static void WriteVerticalLine(Point point, int numberOfChars)
        {
            SetCursor(point);

            for (int i = 0; i < numberOfChars; i++)
            {
                Console.CursorTop += 1;
                Console.Write("|");
                Console.CursorLeft -= 1;
            }

            SetCursor(point);
        }

        private static void SetCursor(Point point)
        {
            Console.SetCursorPosition(point.X, point.Y);
        }
    }
}
