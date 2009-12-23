INCLUDE kernel.fs
INCLUDE bios.fs
INCLUDE graphics.fs
INCLUDE keyboard.fs
INCLUDE sprites.fs
INCLUDE tiles.fs
INCLUDE random.fs

\ screen =============================================

32 24 * const screen-size
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
    screen-size base-screen backscreen LDIRVM ;

\ position ===========================================

: random-pos ( -- x y )
    rnd8 32 mod
    rnd8 24 mod ;

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

: random-player ( -- )
    random-pos player-pos pos! ;

: print-player ( -- )
    player-tile player-pos pos@ locate-addr backscreen + c! ;

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
        player-pos pos@y 23 < if
            1 player-pos pos!+y
        then
    then
    32 = if \ teleport
        random-player
    then ;
        
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

: count-enabled-robots ( -- )
    0
    robots-count @ 0 do
        i is-robot-enabled if
            1+
        then
    loop ;

: all-robots-disabled ( -- )
    count-enabled-robots 0 = ;

\ game ===============================================

variable level

: setup-level ( level -- )
    dup level !
    2 * 2 + random-robots
    random-player ;

: next-level ( -- )
    level @ 1+ setup-level ;

: play-game ( -- )
    clear-backscreen
    \ TODO print score
    print-player
    print-robots
    robots-collision
    print-backscreen
    all-robots-disabled if
        \ TODO print congratulations
        next-level
    else
        move-player
        move-robots
    then
    \ TODO test player collision
    recurse ;

: main
    init-screen
    1 r_seed ! \ TODO random seed
    1 setup-level
    play-game
    abort ;
