using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.States
{
    class SelectionInProgress : IState
    {
        ActionHandler _actionHandler;

        public void OnEnter()
        {
            _actionHandler.DeleteSelection();
        }

        public SelectionInProgress()
        {
            _actionHandler = ActionHandler.GetInstance();
        }

        public void ActivityControl_Click(object sender)
        {
            
        }
        
        public void WeekGrid_MouseLeave()
        {
            _actionHandler.SetQSstate();
        }

        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {
            _actionHandler.AddToSelection(quarterSender);
            quarterSender.AddFrame();
        }
    }
}
