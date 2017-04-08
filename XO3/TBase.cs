using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XO3
{
    class TBase
    {
        public TBase()
        {

        }

        public void Run(TPole Pole)
        {
            TAnalysis Analysis = new TAnalysis();

            TAnalysisRes Res = Analysis.What(Pole.Pos);

            if(Res.Act != ActionType.Nil)
            {
                if(Pole.Move(Res.Coord.i, Res.Coord.j) != Result.Error)
                {
                    return;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Pole.Move(i, j) != Result.Error)
                    {
                        return;
                    }
                }
            }
        }
    }
}
