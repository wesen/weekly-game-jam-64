## To use dialog system ##

` Setup Objects in scene `
1. Create canvas if not already in scene
2. Drag "DialogueUI" into canvas object
3. Add "NPC_Dialogue" to scene
4. Add "DialogManager" to scene

` Setup connections between objects `
1. Click "DialogueManager" in scene
   a. Drag "NPC Name" from DialogueUI>DialogueBox to "Name Text" field in script
   b. Drag "Text" from DialogueUI>DialogueBox to "Dialogue Text" field in script
   c. Drag "DialogueBox" Object from DialogueUI to "Animator" field in script
2. Click NPC_Dialogue in scene
   a. Drag "Player" from scene to "Player" field in controller script (Player must have tag "Player" set)
   b. Drag "SpeechBubble" from NPC_Dialog to "Bubble" in controller script
   c. Drag "DialogueTrigger" script component to "Dialogue Trig" field on controller script
3. Click "Continue" object in scene (Canvas>DialogueUI>DialogueBox>Continue)
   a. Ensure OnClick() is set to DialogueManager.DisplayNextSentence

` Add Dialog `
1. Click "NPC_Dialogue" in scene
2. Click fold arrow on "Dialogue Trigger" script to view fields
3. Set "Name" field to this NPCs name
4. Set "Sentences" field to the number of lines that you would like to add
5. Fill textareas with dialogue
6. Click "NPC_Dialogue_Controller" script fold arrow to view fields
7. Set "Talk Reset Delay to the number of frames (Updates) you would like to wait for before the NPC will be available to talk.



# Additional Information #
Press E when in range of NPC to start dialogue
Click "Continue >>" button on dialog box to continue dialogue
"Talk Reset Delay" can be set to 0 for immediate reset, or -1 for no reset.
Be sure to check that your lines display correctly in game. There is no check for overflow!!

