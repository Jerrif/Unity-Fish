using UnityEngine;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour {
    public Material transitionMaterial;

    // TODO: RESEARCH: do I need to have a flag that stops this from running every frame?
    // (for performance reasons)
    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, transitionMaterial);
    }
}






// from a youtube comment on the video
// https://www.youtube.com/watch?v=LnAoD7hgDxw

/*
Here's some troubleshooting I went through to get this working in a brand new Unity Project (with 2D Template) v2019.3.1f1 (and some Tips for creating Gradients in Photoshop)
EDIT; Note that these step DO NOT apply to a Project with URP; sorry, don't know how to help there, I couldn't get it working.
1. In the Sample Scene, add the "SimpleBlit" script to the Main Camera. 
2. Drag the "BattleTransitions" Material onto the "TransitionMaterial" slot on "SimpleBlit"
3. In the "BattleTransitions" Material itself, select which Texture you wish to use and drop it onto the "Transition Texture" slot. Leave the "Texture" slot alone.
4. If you're using one of the screen-sheering effects, tick the box which says "Distort" otherwise, leave it disabled
At this point, you should see the effect working in the Game-View Tab by dragging the "Cutoff" slider from 0 - 1 (I went nuts because I was expecting it to preview in the Scene view tab; NOT THE CASE!)
5. To manipulate the value of the "Cutoff" at runtime, you can use the following line of code;
TransitionMaterial.SetFloat("_Cutoff", cutoffVal);
- "TransitionMaterial" is the name of the material in the SimpleBlit script
- SetFloat("_Cutoff" is how you modify properties on a material; in particular, a float
- "cutoffVal" is a float I defined which I gradually ramp up to 1f in Update or FixedUpdate. This can obviously be changed to anything you want. For those who are curious, I will include the full modification of the script at the bottom of this comment, but first;

TEXTURE CREATION TIPS =============================
- Create Gradients which go between pure black and Hex-Code "efefef"; I tried making images that went to pure white, and they didn't "complete" (a part of the screen was left uncovered; feel free to try it with a throw away test to see what I mean)
- You can achieve a LOT of cool effects in Photoshop by using the effects under Filter > Distort on top of simple gradients. In particular, I recommend messing around with "Ripple", "Twirl", "Wave", and "Zig-Zag"
- One thing which I did frequently was create an image effect which took up half the screen, then copied it to a new layer, and flipped it to fill the other half of the screen symmetrically

SimpleBlit (Modified; Full script) =============================
using UnityEngine;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;
    public bool transitionIsActive = false;
    public bool fadeIn = true;
    public float transitionRate = 0.01f;

    private float cutoffVal;

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }

    private void FixedUpdate() {
        if (transitionIsActive) {
            if (fadeIn) {
                if (cutoffVal < 1f) {
                    cutoffVal += transitionRate;
                } else {
                    cutoffVal = 1f;
                    transitionIsActive = false;
                }
            } else {
                if (cutoffVal > 0f) {
                    cutoffVal -= transitionRate;
                } else {
                    cutoffVal = 0f;
                    transitionIsActive = false;
                }
            }
            TransitionMaterial.SetFloat("_Cutoff", cutoffVal);
        }
    }
}

*/