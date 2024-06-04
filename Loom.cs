using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;
using System.Diagnostics;

public class Loom {
    public static int maxThreads = 8;
    public static int numThreads;

    private static int _count;

    private static bool initialized;

    private static Stopwatch stopwatch;
    private static TimeSpan elapsed;

    public static void Init() {
        initialized = true;

        // Initialize the stopwatch
        stopwatch = new Stopwatch();
        stopwatch.Start();

        // Start a timer to update the UI
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000; // Update UI every second
        timer.Tick += Timer_Tick;
        timer.Start();

        System.Windows.Forms.Timer timer_update = new System.Windows.Forms.Timer();
        timer_update.Interval = 100; // Update UI every second
        timer_update.Tick += Timer_Tick_update;
        timer_update.Start();
    }

    private static void Timer_Tick_update(object sender, EventArgs e) {
        // Get the elapsed time from the stopwatch
        Update();
    }

    private static void Timer_Tick(object sender, EventArgs e) {
        // Get the elapsed time from the stopwatch
        elapsed = stopwatch.Elapsed;
    }

    private static List<Action> _actions = new List<Action>();

    public struct DelayedQueueItem {
        public float time;
        public Action action;
    }

    private static List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    private static List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action action) {
        QueueOnMainThread(action, 0f);
    }

    public static void QueueOnMainThread(Action action, float time) {
        if (time != 0) {
            lock (_delayed) {
                _delayed.Add(new DelayedQueueItem { time = (float)elapsed.TotalSeconds + time, action = action });
            }
        } else {
            lock (_actions) {
                _actions.Add(action);
            }
        }
    }

    public static Thread RunAsync(Action a) {
        while (numThreads >= maxThreads) {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action) {
        try {
            ((Action)action)();
        } catch {
        } finally {
            Interlocked.Decrement(ref numThreads);
        }
    }

    private static List<Action> _currentActions = new List<Action>();

    // Update is called once per frame
    private static void Update() {
        lock (_actions) {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var a in _currentActions) {
            a();
        }
        lock (_delayed) {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(_delayed.Where(d => d.time <= (float)elapsed.TotalSeconds));
            foreach (var item in _currentDelayed)
                _delayed.Remove(item);
        }
        foreach (var delayed in _currentDelayed) {
            delayed.action();
        }
    }
}