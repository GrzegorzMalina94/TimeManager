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
            _actionHandler.ConditionallyAddToSelection(quarterSender);
            quarterSender.SetSelectionFrame();
        }

        public void QuarterRectangle_MouseLeftButtonDown(Quarter quarterSender)
        {

        }

        public void Window_MouseLeftButtonDown()
        {

        }

        public void Window_MouseLeftButtonUp()
        {
            _actionHandler.SetQSstate();
        }
    }
}
