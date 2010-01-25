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

: . ( n -- )
    <# 0 #s #> type space ;

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
variable score

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
    
: random-robot ( robot -- )
    dup enable-robot
    random-pos rot 2* robots-pos + pos! ;

\ collisions =========================================

: robots-crash ( robot1 robot2 -- )
    2dup <> if
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

: robot-collision ( robot -- )
    robots-count @ 0 do
        i is-robot-enabled if
            dup i robots-crash
        then
    loop drop ;
            
: robots-collision ( -- )
    robots-count @ 0 do
        i is-robot-enabled if
            i robot-collision
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

\ level ==============================================

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

: random-robots ( n -- )
    dup robots-count !
    disable-robots
    0 do
        i random-robot \ TODO safe-random-robot
    loop ;

\ game ===============================================

variable level

: setup-level ( level -- )
    score @ 100 + score !
    dup 5 min safe-teleports !
    dup level !
    2 * 2 + random-robots
    safe-random-player ;

: next-level ( -- )
    level @ 1+ setup-level ;

: print-score ( -- )
    0 0 locate s" SCORE " type score @ . s"  " type
    15 0 locate s" SAFE TELEPORTS " type safe-teleports @ . ;

: print-game ( -- )
    clear-backscreen
    print-score
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
    12 10 locate s" LEVEL " type level @ .
    CHGET drop ;

: new-game ( -- )
    0 score !
    1 setup-level ;

: dec-score ( -- )
    score @ 0 > if
        score @ 1- score !
    then ;

: move-game ( -- )
    all-robots-disabled if
        \ change to next level
        show-next-level-message
        next-level
        print-game
        show-level-message
    else
        player-collision count-enabled-robots 1 = or if
            show-dead-message
            new-game
        else
            \ game step
            move-player
            move-robots
            dec-score
        then
    then ;

: play-game ( -- )
    print-game
    move-game
    recurse ;

: init-game ( -- )
    decimal
    1 r_seed ! \ TODO random seed
    init-screen ;

: main
    init-game
    new-game
    print-game
    show-level-message
    play-game
    abort ;
