INCLUDE kernel.fs
INCLUDE bios.fs
INCLUDE graphics.fs
INCLUDE keyboard.fs
INCLUDE sprites.fs
INCLUDE tiles.fs
INCLUDE random.fs

32 24 * const screen-size
screen-size carray backscreen

variable player-pos

50 const max-robots
max-robots array robots
variable robots-count

: init-screen ( -- )
    DISSCR
    0 0 15 change-color
    init-graphic-mode
    clear-screen
    redefine-tiles
    ENASCR ;

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

: random-player ( -- )
    random-pos player-pos pos! ;
    
: random-robot ( robot -- )
    random-pos swap robots + pos! ;

: random-robots ( -- )
    robots-count @ 0 do
        i 2* random-robot
    loop ;

: move-robot ( robot -- )
    robots +
    dup pos@x player-pos pos@x > if
        -1 over pos!+x
    then
    dup pos@x player-pos pos@x < if
        1 over pos!+x
    then
    dup pos@y player-pos pos@y > if
        -1 over pos!+y
    then
    dup pos@y player-pos pos@y < if
        1 over pos!+y
    then
    drop ;

: move-robots ( -- )
    robots-count @ 0 do
        i 2* move-robot
    loop ;

: print-player ( -- )
    player-tile player-pos pos@ locate-addr backscreen + c! ;

: print-robot ( robot -- )
    robot-tile swap robots + pos@ locate-addr backscreen + c! ;

: print-robots ( -- )
    robots-count @ 0 do
        i 2* print-robot
    loop ;

: move-player ( -- )
    CHGET
    dup 28 = if
        player-pos pos@x 31 < if
            1 player-pos pos!+x
        then
    then
    dup 29 = if
        player-pos pos@x 0 > if
            -1 player-pos pos!+x
        then
    then
    dup 30 = if
        player-pos pos@y 0 > if
            -1 player-pos pos!+y
        then
    then
    31 = if
        player-pos pos@y 23 < if
            1 player-pos pos!+y
        then
    then ;

: clear-backscreen ( -- )
    backscreen screen-size floor-tile fill ;

: print-backscreen ( -- )
    screen-size base-screen backscreen LDIRVM ;

: play-game ( -- )
    clear-backscreen
    print-player
    print-robots
    print-backscreen
    move-player
    move-robots
    \ TODO test robot collision
    \ TODO test player collision
    recurse ;

: setup-level ( level -- )
    2 * 2 + robots-count c!
    random-robots
    random-player ;

: main
    init-screen
    1 r_seed !
    1 setup-level
    play-game
    abort ;
