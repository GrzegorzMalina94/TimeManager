using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.States
{
    class QuartersSelected : IState
    {
        ActionHandler _actionHandler;
        
        public void OnEnter()
        {
            
        }

        public QuartersSelected()
        {
            _actionHandler = ActionHandler.GetInstance();
        }

        public void ActivityControl_Click(object sender)
        {
            _actionHandler.AssignActivityToSelectedQuarters((sender as ActivityControl).ActivityName);
        }

        public void WeekGrid_MouseLeave()
        {
            
        }

        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {
            
        }
    }
}
