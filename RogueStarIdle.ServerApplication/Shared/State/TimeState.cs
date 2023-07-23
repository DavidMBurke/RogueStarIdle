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
        public int TickDuration = 20; //milliseconds
        public int TicksPerSecond = 50;
        public int TicksSinceLastSignIn { get; set; } = 0;
        public double MillisecondsElapsed { get; set; } = 0;
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
        public DateTime TimeSignedOn = DateTime.Now;
        //TODO: Ticks break if timer isn't set to same length as TickDuration, though it shouldn't matter because elapsed ticks calculated
        //      based on time elapsed since last time function was run. Will need to look into why and fix so gametimer can be optimized for performance
        //      and guarantee time consistency.
        public System.Timers.Timer GameTimer { get; set; } = new System.Timers.Timer(20);
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
            // Breaks when adding 10 hours or more and runs Tick multiple times without resetting
            TimeSpan timeElapsed = DateTime.Now - LastUpdateTime;
            LastUpdateTime = DateTime.Now;
            MillisecondsElapsed += timeElapsed.TotalMilliseconds;
            while (MillisecondsElapsed >= TickDuration)
            {
                MillisecondsElapsed -= TickDuration;
                Ticks += 1;
                TicksSinceLastSignIn += 1;
            }
            Tick(Ticks);
            Ticks = 0;
            NotifyStateChanged();
        }

        // Update every state affected by time.
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