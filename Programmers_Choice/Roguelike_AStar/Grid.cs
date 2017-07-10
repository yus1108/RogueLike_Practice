
namespace Roguelike_AStar
{
    class Grid
    {
        Point mP;
        int mG;
        int mH;

        public Grid()
        {
            mP = new Point(0, 0);
            mG = 0;
            mH = 0;
        }
        public Grid(Point p, int g, int h)
        {
            mP = new Point(p);
            mG = g;
            mH = h;
        }
        public Grid(Grid g)
        {
            mP = g.mP;
            mG = g.mG;
            mH = g.mH;
        }

        public Point getP() { return mP; }
        public int getG() { return mG; }
        public int getH() { return mH; }
        public void setP(Point p) { mP = new Point(p); }
        public void setG(int g) { mG = g; }
        public void setH(int h) { mH = h; }
        public void setGrid(Grid g)
        {
            mP = g.mP;
            mG = g.mG;
            mH = g.mH;
        }


    }
}
