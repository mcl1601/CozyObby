-- Credits:
zSkull162 - Created the UI and Udon programs

Playerbush001 - Helped with the logic

Freepik - Exposure, bloom, temperature, tint, and contrast sprites
KP Arts - Saturation sprite

Vernon Adams - Nunito font


--- Creator's Discord:
(Message if you need help with the prefab)
skull_z162


--- Instructions:
(I reccommend using the USharp version of the prefab. The graph one is there for those who want to edit it, but don't know coding. The graph version is a bit more of a mess, so these instructions are for the USharp version)

- To change the default values:

1. Open the "Volumes" dropdown
2. Select one of the Post-Process Volumes
(for example, Exposure High if you want it to be brighter, or Exposure Low if you want it darker)

3. Change the "Weight" slider of the Post-Process Volume component

Done. All you need to do is change the weight of the volumes, and the rest is automatic. Just make sure you don't turn up the weight of a "Low" and "High" volume of the same type at the same time, as it may break.
(example: Don't use Exposure High and Exposure Low at the same time)


- To change the maximum strength of an effect:

1. Select the Post-Process Volume that you want to be stronger
(example: If you want the Temperature slider to be warmer than the maximum default warmth)

2. Make sure its weight is set to 1, so that you can see its full effect
(Just make sure to turn it back down when you're done)

3. Open the effect's dropdown, which is under "Overrides" in the Post-Process Volume component
4. Find the enabled slider/value, and turn it up or down

Done. By making the effect stronger, the slider will automatically be stronger as well. Just don't turn a Low type volume to the same value as a High volume, because then the slider will give the same effect on both sides.
(example: Don't turn the Temperature Cool temperature value up to be a warm color)