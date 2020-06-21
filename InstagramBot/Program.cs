using System;

namespace InstagramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            ActionClass botActions = new ActionClass(Secrets.userName, Secrets.password);

            botActions.Initiate();
            botActions.GetNoneFollowers();
            //botActions.Logout();
        }
    }
}
