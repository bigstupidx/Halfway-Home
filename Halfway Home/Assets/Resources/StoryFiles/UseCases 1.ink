VAR player_name = "unset"
VAR player_gender = "unset"
VAR grace = 0
VAR expression = 0
VAR awareness = 0
VAR stress = 0
VAR fatigue = 0
VAR delusion = 0
VAR doubt = 0

EXTERNAL PlayMusic(string)

-> Knot1

=== Knot1 ===
// Brackets denote a speaker. Any line with quotation marks is applied to the last speaker.
[Christian]"Make me an Ink file that shows what you need."
// The # denotes an Ink tag. Not something you need to worry about adding, but you do need to listen for it.
[Player]"Okay." # relaxed

// local increment
~stress += 1
// global increment
~ModifyStat("stress", 1)
// Any line without quotations uses no speaker and is narration.
Christian leaves.
<Player>"Better get to it!" ~PlayMusic("work_music")
-> END