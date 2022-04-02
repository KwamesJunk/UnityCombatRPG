namespace RPG.Core
{
    public interface IAction {
        void cancel();
        string getName();
    }
}