namespace TimeManager.States
{
    class Default : IState
    {
        ActionHandler _actionHandler;
        ActivitiesManager _activitiesManager;        

        public void OnEnter()
        {
            _actionHandler.DeleteSelection();
        }

        public Default(ActivitiesManager activitiesManager)
        {
            _actionHandler = ActionHandler.GetInstance();
            _activitiesManager = activitiesManager;            
        }

        public void ActivityControl_Click(object sender)
        {
            _activitiesManager.HandleActivityControlClick(sender);
        }
        
        public void WeekGrid_MouseLeave()
        {
            
        }
        
        public void QuarterRectangle_MouseEnter(Quarter quarterSender)
        {

        }

        public void QuarterRectangle_MouseLeftButtonDown(Quarter quarterSender)
        {
            quarterSender.SetSelectionFrame();
            _actionHandler.ConditionallyAddToSelection(quarterSender);
            _actionHandler.SetSIPstate();
        }

        public void Window_MouseLeftButtonDown()
        {

        }

        public void Window_MouseLeftButtonUp()
        {

        }
    }
}
