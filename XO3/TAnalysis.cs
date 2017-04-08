using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace XO3
{
    class TAnalysis
    {
        public TAnalysis()
        {

        }

        TPosition Pos;

        public TAnalysisRes What(TPosition Pos)
        {
            this.Pos = Pos;

            return IsWin();
        }

        TAnalysisRes IsWin()
        {
            TAnalysisRes Res = new TAnalysisRes();

            TFinal Finals = new TFinal();

            foreach(TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if(Res.Act == ActionType.Win)
                {
                    return Res;
                }
            }

            foreach (TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if (Res.Act == ActionType.Def)
                {
                    return Res;
                }
            }

            foreach (TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if (Res.Act == ActionType.Neutral)
                {
                    return Res;
                }
            }


            return Res;
        }

        public ActionType WhatResult(TPosition Pos)
        {
            TAnalysisRes Res = new TAnalysisRes();

            TFinal Finals = new TFinal();

            foreach (TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if (Res.Act == ActionType.Win)
                {
                    return ActionType.Win;
                }
            }

            foreach (TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if (Res.Act == ActionType.Def)
                {
                    return ActionType.Def;
                }
            }

            return ActionType.Neutral;
        }

        public ActionType WhatResult2(TPosition Pos)
        {
            TAnalysisRes Res = new TAnalysisRes();

            TFinal Finals = new TFinal();

            foreach (TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if (Res.Act == ActionType.Def)
                {
                    return ActionType.Def;
                }
            }

            foreach (TCoords C in Finals.ForWin)
            {
                Res = C.What(Pos);

                if (Res.Act == ActionType.Win)
                {
                    return ActionType.Win;
                }
            }

            return ActionType.Neutral;
        }


    }

    public enum ActionType { Nil, Neutral, Win, Def };

    class TFinal
    {
        public TCoords[] ForWin; // Все возможные позиции для выигрыша

        public TFinal()
        {
            ForWin = new TCoords[MaxWin];

            ForWin[0] = new TCoords(new TCoord(0, 0), new TCoord(0, 1), new TCoord(0, 2));
            ForWin[1] = new TCoords(new TCoord(1, 0), new TCoord(1, 1), new TCoord(1, 2));
            ForWin[2] = new TCoords(new TCoord(2, 0), new TCoord(2, 1), new TCoord(2, 2));

            ForWin[3] = new TCoords(new TCoord(0, 0), new TCoord(1, 0), new TCoord(2, 0));
            ForWin[4] = new TCoords(new TCoord(0, 1), new TCoord(1, 1), new TCoord(2, 1));
            ForWin[5] = new TCoords(new TCoord(0, 2), new TCoord(1, 2), new TCoord(2, 2));

            ForWin[6] = new TCoords(new TCoord(0, 0), new TCoord(1, 1), new TCoord(2, 2));
            ForWin[7] = new TCoords(new TCoord(0, 2), new TCoord(1, 1), new TCoord(2, 0));
        }

        public int MaxWin { get { return 8; } }
    }

    class TCoords
    {
        public TCoord[] C;
        public TCoords(TCoord C1, TCoord C2, TCoord C3)
        {
            C = new TCoord[3];
            C[0] = C1;
            C[1] = C2;
            C[2] = C3;
        }

        public TAnalysisRes What(TPosition Pos)
        {
            TAnalysisRes Res = new TAnalysisRes();

            int C_X = 0;
            int C_O = 0;
            int C_ = 0;

            foreach(TCoord c in C)
            {
                if (Pos.P[c.i, c.j] == 'X')
                {
                    C_X++;
                }

                if (Pos.P[c.i, c.j] == 'O')
                {
                    C_O++;
                }

                if (Pos.P[c.i, c.j] == ' ')
                {
                    C_++;

                    Res.Act = ActionType.Neutral;
                    Res.Coord.i = c.i;
                    Res.Coord.j = c.j;
                }
            }

            if(C_ == 1)
            {
                if(C_X == 2)
                {
                    Res.Act = ActionType.Def;
                }

                if(C_O == 2)
                {
                    Res.Act = ActionType.Win;
                }
            }

            return Res;
        }
    }

    class TCoord
    {
        public int i;
        public int j;

        public TCoord(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }

    class TAnalysisRes
    {
        public TCoord Coord;
        public ActionType Act;

        public TAnalysisRes()
        {
            Coord = new TCoord(-1, -1);

            Act = ActionType.Nil;
        }
    }
}
