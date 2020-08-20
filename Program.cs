using System;
using System.Collections.Generic;

namespace ActiveObjectPattern
{
    public interface Command
    {
        void execute();
    }

    public class SleepCommand : Command
    {
        private const int MS = 1000;
        private long sleepTime = 0;
        private ActiveObjectEngine engine;
        private Command subCommand;
        private bool started = false;
        private long startTime = 0;
        public SleepCommand(long milliseconds, ActiveObjectEngine engine, Command wakeupCommand)

        {
            this.engine = engine;
            sleepTime = milliseconds;
            subCommand = wakeupCommand;
        }

        public void execute()
        {
            var currentTime = DateTime.Now.Second;

            if (!started)
            {
                started = true;
                startTime = currentTime;
                engine.addCommand(this);
            }
            else if ((currentTime - startTime) * MS < sleepTime)
            {
                engine.addCommand(this);
            }
            else
            {
                engine.addCommand(subCommand);
            }
        }
    }


    public class ActiveObjectEngine
    {
        LinkedList<Command> itsCommands = new LinkedList<Command>();
        public void run()
        {
            while (itsCommands.Count != 0)
            {

                Command c = itsCommands.First.Value;
                itsCommands.Remove(c);
                c.execute();
            }
        }
        public void addCommand(Command c)
        {
            itsCommands.AddLast(c);
        }
    }
    class Program
    {
        public static bool commendExecuted = false;
        class WakeUpCommand : Command
        {
            public void execute()
            {
                commendExecuted = true;
            }
        }

        static void Main(string[] args)
        {
            Command wakeup = new WakeUpCommand();
            var e = new ActiveObjectEngine();
            Command sleepCommand = new SleepCommand(1000, e, wakeup);
            e.addCommand(sleepCommand);
            long start = DateTime.Now.Second;
            e.run();
            long end = DateTime.Now.Second;
            long sleepTime = end - start;
            Console.WriteLine($"SleepTime is {sleepTime}");
        }
    }
}
