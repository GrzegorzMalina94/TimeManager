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
        bool MLBttnHasBeenPrssd = false;
        
        public void OnEnter()
        {
            MLBttnHasBeenPrssd = false;
        }

        public QuartersSelected()
        {
            _actionHandler = ActionHandler.GetInstance();
        }

        public void ActivityControl_Click(object sender)
        {
            _actionHandler.AssignActivityToSelectedQuarters((sender as ActivityControl).ActivityName);
            _actionHandler.SetDefaultState();
        }

        public void WeekGrid_MouseLeave()
        {
            
        }

        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {
            
        }

        public void QuarterRectangle_MouseLeftButtonDown(Quarter quarterSender)
        {
            MLBttnHasBeenPrssd = true;
        }

        public void Window_MouseLeftButtonDown()
        {
            MLBttnHasBeenPrssd = true;
        }

        public void Window_MouseLeftButtonUp()
        {
            if(MLBttnHasBeenPrssd)
            _actionHandler.SetDefaultState();
        }
    }
}
