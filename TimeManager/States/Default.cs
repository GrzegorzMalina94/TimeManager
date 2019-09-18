using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManager.States
{
    class Default : IState
    {
        ActivitiesManager _activitiesManager;

        public void OnEnter()
        {
            
        }

        public Default(ActivitiesManager activitiesManager)
        {
            _activitiesManager = activitiesManager;
        }

        public void ActivityControl_Click(object sender)
        {
            _activitiesManager.ChangeActivitySelection(sender);
        }
        
        public void WeekGrid_MouseLeave()
        {
            
        }
        
        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {

        }
    }
}
