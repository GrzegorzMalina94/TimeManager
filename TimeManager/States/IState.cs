using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager
{
    interface IState
    {
        void OnEnter();

        void ActivityControl_Click(object sender);

        void WeekGrid_MouseLeave();

        void QuarterRectangle_MouseEnter(Quarter quarterSender);
    }
}
