# GAME-HW1
1. Rate-limited fire
- Launch torpedoes at fixed time intervals, independent of the frame rate.
How it works:
- At first, the lauch time is controlled by the speed of player click on the return key. A private variable `nextFireTime` stores the next allowed firing time. When player presses the Return key:
    - If the current time is before nextFireTime, no torpedo is fired.
    - Otherwise, a torpedo will be fired.
Like, the ReteOfFire = 4 - fires every 0.25 seconds
          RateOfFire = 2 - fires every 0.5 seconds.
3. Torpedo reflections
- Torpedoes that hit a boundary will be reflected according to the laws of physics rather than disappearing.
Core:
- Control the maximum number of reflections using the numberReflections variable in the Inspector.
- Use the reflection formula R = I - 2*(I·N)*N
- Each reflection triggers a sound effect
