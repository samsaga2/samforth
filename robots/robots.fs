INCLUDE kernel.fs
INCLUDE bios.fs
INCLUDE graphics.fs
INCLUDE keyboard.fs
INCLUDE sprites.fs
INCLUDE tiles.fs
INCLUDE random.fs

\ screen =============================================

32 23 * const screen-size
screen-size carray backscreen

: init-screen ( -- )
    DISSCR
    0 0 15 change-color
    init-graphic-mode
    clear-screen
    redefine-tiles
    ENASCR ;

: clear-backscreen ( -- )
    backscreen screen-size floor-tile fill ;

: print-backscreen ( -- )
    screen-size base-screen 32 + backscreen LDIRVM ;

\ position ===========================================

: random-pos ( -- x y )
    rnd8 32 mod
    rnd8 23 mod ;

: pos! ( x y addr -- )
    2dup 1+ c! nip c! ;

: pos@ ( addr -- x y )
    dup c@ swap 1+ c@ ;

: pos@x ( addr -- x )
    c@ ;

: pos@y ( addr -- y )
    1+ c@ ;

: pos!+x ( +x addr -- )
    swap over c@ + swap c! ;
    
: pos!+y ( +y addr -- )
    1+ swap over c@ + swap c! ;
    
\ player =============================================

variable player-pos
variable safe-teleports

: random-player ( -- )
    random-pos player-pos pos! ;

: print-player ( -- )
    player-tile player-pos pos@ locate-addr backscreen + c! ;
        
\ robots =============================================

50 const max-robots
max-robots array robots-pos
max-robots carray robots-enabled
variable robots-count

: enable-robot ( robot -- )
    1 swap robots-enabled + c! ;

: disable-robot ( robot -- )
    0 swap robots-enabled + c! ;

: is-robot-enabled ( robot -- n )
    robots-enabled + c@ 0 <> ;

: disable-robots ( -- )
    max-robots 0 do
        i disable-robot
    loop ;
    
: random-robot ( robot -- )
    \ TODO check for free cell
    dup enable-robot
    random-pos rot 2* robots-pos + pos! ;

: random-robots ( n -- )
    dup robots-count !
    disable-robots
    0 do
        i random-robot
    loop ;

: move-robot ( robot -- )
    2* robots-pos +
    dup pos@x player-pos pos@x > if
        -1 over pos!+x \ left
    then
    dup pos@x player-pos pos@x < if
        1 over pos!+x \ right
    then
    dup pos@y player-pos pos@y > if
        -1 over pos!+y \ up
    then
    dup pos@y player-pos pos@y < if
        1 over pos!+y \ down
    then
    drop ;

: move-robots ( -- )
    robots-count @ 0 do
        i is-robot-enabled if
            i move-robot
        then
    loop ;

: robot-pos@ ( robot -- x y )
    2* robots-pos + pos@ ;

: print-robot ( tile robot -- )
    robot-pos@ locate-addr backscreen + c! ;

: print-robots ( -- )
    robots-count @ 0 do
        i is-robot-enabled if
            robot-tile i print-robot
        then
    loop ;

: robot-explosion ( robot -- )
    explosion-tile swap print-robot ;

: count-enabled-robots ( -- )
    0
    robots-count @ 0 do
        i is-robot-enabled if
            1+
        then
    loop ;

: all-robots-disabled ( -- )
    count-enabled-robots 0 = ;

\ collisions =========================================

: robots-crash ( robot1 robot2 -- )
    2dup <> if
        \ TODO very slow line
        over 2* robots-pos + @
        over 2* robots-pos + @
        = if
            dup disable-robot robot-explosion
            dup disable-robot robot-explosion
        else
            2drop
        then
    else
        2drop
    then ;

: robots-collision ( -- )
    \ TODO bug when three robots collide?
    robots-count @ 0 do
        i is-robot-enabled if
            robots-count @ 0 do
                i is-robot-enabled if
                    i j robots-crash
                then
            loop
        then
    loop ;

: player-robot-collision ( robot -- )
    2* robots-pos + @ player-pos @ = ;

: player-collision ( -- )
    0 robots-count @ 0 do
        i is-robot-enabled if
            i player-robot-collision if
                drop 1
            then
        then
    loop
    0 <> ;

\ player movement ====================================

: safe-random-player ( -- )
    random-player
    player-collision if
        recurse
    then ;

: teleport-player ( -- )
    safe-teleports @ 0 > if
        safe-random-player
        safe-teleports @ 1- safe-teleports !
    else
        random-player
    then ;

: move-player ( -- )
    CHGET
    dup 28 = if \ right
        player-pos pos@x 31 < if
            1 player-pos pos!+x
        then
    then
    dup 29 = if \ left
        player-pos pos@x 0 > if
            -1 player-pos pos!+x
        then
    then
    dup 30 = if \ up
        player-pos pos@y 0 > if
            -1 player-pos pos!+y
        then
    then
    dup 31 = if \ down
        player-pos pos@y 22 < if
            1 player-pos pos!+y
        then
    then
    32 = if \ teleport
        teleport-player
    then ;

\ game ===============================================

variable level

: setup-level ( level -- )
    dup safe-teleports !
    dup level !
    2 * 2 + random-robots
    random-player ;

: next-level ( -- )
    level @ 1+ setup-level ;

: print-score ( -- )
    0 0 locate s" SCORE XXXXX" type
    15 0 locate s" SAFE TELEPORTS XX" type ;

: print-game ( -- )
    clear-backscreen
    print-score
    \ TODO print safe teleports
    print-player
    print-robots
    robots-collision
    print-backscreen ;

: show-dead-message ( -- )
    13 9 locate s" HA HA" type
    10 10 locate s" PLAYER DEAD" type
    10 11 locate s" ROBOTS COOL" type
    11 13 locate s" GAME OVER" type
    CHGET drop ;

: show-next-level-message ( -- )
    14 10 locate s" SHIT" type
    9 11 locate s" LEVEL COMPLETE" type
    CHGET drop ;

: show-level-message ( -- )
    12 10 locate s" LEVEL XX" type
    CHGET drop ;

: move-game ( -- )
    all-robots-disabled if
        \ change to next level
        show-next-level-message
        next-level
        print-game
        show-level-message
    else
        player-collision if
            show-dead-message
            1 setup-level
        else
            \ game step
            move-player
            move-robots
        then
    then ;

: play-game ( -- )
    print-game
    move-game
    recurse ;

: main
    init-screen
    1 r_seed ! \ TODO random seed
    1 setup-level
    play-game
    abort ;
