using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Timers;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class TimeState
    {
        public int Ticks { get; set; } = 0;
        public int TicksSinceLastSignIn { get; set; } = 0;
        public double MillisecondsElapsed { get; set; } = 0;
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        public DateTime TimeSignedOn = DateTime.Now;
        public System.Timers.Timer GameTimer { get; set; } = new System.Timers.Timer(25);
        public event Func<Task> OnChange;
        private readonly ScavengingState _scavengingState;

        public TimeState(ScavengingState scavengingState)
        {
            _scavengingState = scavengingState;
            GameTimer.Elapsed += CalculateElapsedTicks;
            GameTimer.Enabled = true;
            GameTimer.Start();
        }

        private async Task NotifyStateChanged()
        {
            if (OnChange == null) 
                return;
            await OnChange.Invoke();
        }

        public void CalculateElapsedTicks(object source, ElapsedEventArgs e)
        {
            TimeSpan timeElapsed = DateTime.Now - LastUpdateTime;
            LastUpdateTime = DateTime.Now;
            MillisecondsElapsed += timeElapsed.TotalMilliseconds;
            while (MillisecondsElapsed >= 40)
            {
                MillisecondsElapsed -= 40;
                Ticks += 1;
                TicksSinceLastSignIn += 1;
            }
            while (Ticks > 50000)
            {
                Tick(50000);
                Ticks -= 50000;
                System.Threading.Thread.Sleep(100);
            }
            Tick(Ticks);
            Ticks = 0;
            NotifyStateChanged();
        }

        // Update every state affected by time. 25 ticks elapse per second.
        public void Tick(int ticksElapsed)
        {
            _scavengingState.ScavengeTicks(ticksElapsed);
        }

        public void AddTicks(int ticks)
        {
            Ticks += ticks;
            TicksSinceLastSignIn += ticks;
        }
    }
}