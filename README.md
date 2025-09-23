# GAME-HW1
1. Rate-limited fire
- Launch torpedoes at fixed time intervals, independent of the frame rate.
How it works:
- At first, the lauch time is controlled by the speed of player click on the return key. A private variable `nextFireTime` stores the next allowed firing time. When player presses the Return key:
    - If the current time is before nextFireTime, no torpedo is fired.
    - Otherwise, a torpedo will be fired.
Like, the ReteOfFire = 4 - fires every 0.25 seconds
          RateOfFire = 2 - fires every 0.5 seconds.
