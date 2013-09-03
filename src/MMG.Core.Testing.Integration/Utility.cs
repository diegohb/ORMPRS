// *************************************************
// MMG.Core.Testing.Integration.Utility.cs
// Last Modified: 08/31/2013 2:53 PM
// Modified By: Bustamante, Diego (bustamd1)
// *************************************************

using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MMG.Core.Testing.Integration
{
    public class Utility
    {
        public const string NorthwindDBConnectionName = "NorthwindDB";
        public const string NorthwindAltDBConnectionName = NorthwindDBConnectionName + "Alt";

        public static void DoTimedAction(Expression<Action> pAction)
        {
            Console.Write("Executing {0} ... ", pAction.Body);
            var timer = new Stopwatch();
            var act = pAction.Compile();
            timer.Start();
            act.Invoke();
            timer.Stop();
            Console.WriteLine("Executed successfully in {0} ms.", timer.ElapsedMilliseconds);
        }
    }
}