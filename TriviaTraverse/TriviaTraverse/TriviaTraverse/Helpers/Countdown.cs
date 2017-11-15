//
//  Countdown.cs
//  Created by Alexey Kinev on 11 Jan 2015.
//
//    Licensed under The MIT License (MIT)
//    http://opensource.org/licenses/MIT
//
//    Copyright (c) 2015 Alexey Kinev <alexey.rudy@gmail.com>
//
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
//using System.Timers;

namespace TriviaTraverse.Helpers
{
    /// <summary>
    /// Countdown timer with periodical ticks.
    /// </summary>
    public class Countdown : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the start date time.
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// Gets the remain time in seconds.
        /// </summary>
        public bool TimerActive
        {
            get { return timerActive; }

            private set
            {
                timerActive = value;
                OnPropertyChanged();
            }
        }

        public double RemainTime
        {
            get { return remainTime; }

            private set
            {
                remainTime = value;
                OnPropertyChanged();
            }
        }

        public double TotalTime
        {
            get { return totalTime; }

            private set
            {
                totalTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when completed.
        /// </summary>
        public event Action Completed;

        /// <summary>
        /// Occurs when ticked.
        /// </summary>
        public event Action Ticked;

        /// <summary>
        /// Occurs after timer is rest.
        /// </summary>
        public event Action ResetSuccess;

        /// <summary>
        /// The timer.
        /// </summary>
        //Timer timer;

        bool timerActive;

        /// <summary>
        /// The remain time.
        /// </summary>
        double remainTime;

        /// <summary>
        /// The remain time total.
        /// </summary>
        double totalTime;

        /// <summary>
        /// Starts the updating with specified period, total time and period are specified in seconds.
        /// </summary>
        public void StartUpdating(double total, double period = 1.0)
        {
            //if (timer != null)
            //{
            //    ResetUpdating();
            //}

            TimerActive = true;
            TotalTime = total;
            RemainTime = total;

            StartDateTime = DateTime.Now;

            //timer = new Timer(period * 1000);
            //timer.Elapsed += (sender, e) => Tick();
            //timer.Enabled = true;

            resetingTime = false;
            Device.StartTimer(TimeSpan.FromSeconds(period), () =>
            {
                return Tick();

                //return true; // True = Repeat again, False = Stop the timer
            });
        }

        /// <summary>
        /// Stops the updating.
        /// </summary>
        public void StopUpdating()
        {
            //if (timer != null)
            //{
            //    timer.Enabled = false;
            //    timer = null;
            //}
            TimerActive = false;
        }

        /// <summary>
        /// Stops the updating.
        /// </summary>
        public void ResetUpdating()
        {
            RemainTime = 0;
            TotalTime = 0;

            TimerActive = false;
        }

        private bool resetingTime = false;
        public void ResetTime()
        {
            resetingTime = true;
        }

        /// <summary>
        /// Updates the time remain.
        /// </summary>
        public bool Tick()
        {
            //Debug.WriteLine("countdown tick");

            if (resetingTime)
            {
                StartDateTime = DateTime.Now;
                RemainTime = TotalTime;
                resetingTime = false;
                var resetSuccess = ResetSuccess;
                resetSuccess();
            }

            if (TimerActive)
            {
                var delta = (DateTime.Now - StartDateTime).TotalSeconds;

                if (delta < TotalTime)
                {
                    RemainTime = TotalTime - delta;

                    var ticked = Ticked;
                    if (ticked != null)
                    {
                        ticked();
                    }
                    return true;
                }
                else
                {
                    RemainTime = 0;

                    var completed = Completed;
                    if (completed != null)
                    {
                        completed();
                    }
                    return false;  //stops the timer
                }
            }
            else
            {
                return false;
            }
        }

        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}