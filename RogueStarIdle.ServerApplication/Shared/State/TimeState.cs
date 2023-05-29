using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;
using System;
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
            GameTimer.Elapsed += calculateElapsedTicks;
            GameTimer.Enabled = true;
            GameTimer.Start();
        }

        private async Task NotifyStateChanged()
        {
            if (OnChange == null) 
                return;
            await OnChange.Invoke();
        }

        public void calculateElapsedTicks(object source, ElapsedEventArgs e)
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
            for (int i = 0; i < Ticks; i++)
            {
                Tick();
            }
            NotifyStateChanged();
        }

        // Update every state affected by time. 25 ticks elapse per second.
        public void Tick()
        {
            _scavengingState.ScavengeTicks();
            Ticks -= 1;
        }

        public void addTicks(int ticks)
        {
            Ticks += ticks;
            TicksSinceLastSignIn += ticks;
        }
    }
}