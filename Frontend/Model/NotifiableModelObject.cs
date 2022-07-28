namespace Frontend.Model
{
    public abstract class NotifiableModelObject : NotifiableObject
    {
        public BackendController controller;
        
        public BackendController Controller
        {
            get { return controller; }
        }
        public NotifiableModelObject(BackendController controller)
        {
            this.controller = controller;
        }
    }
}
