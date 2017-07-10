
namespace Roguelike_AStar
{
    class Point
    {
        int mX;
        int mY;

        public Point()
        {
            mX = 0;
            mY = 0;
        }
        public Point(int x, int y)
        {
            mX = x;
            mY = y;
        }
        public Point(Point p)
        {
            mX = p.mX;
            mY = p.mY;
        }

        public int getX() { return mX; }
        public int getY() { return mY; }
        public void setX(int x) { mX = x; }
        public void setY(int y) { mY = y; }
        public void setPoint(Point p)
        {
            mX = p.mX;
            mY = p.mY;
        }

    }
}
