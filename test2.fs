INCLUDE kernel.fs
INCLUDE bios.fs

i: tile-pattern
    create
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c,
    parse-name evaluate c, ;i

i: color-pattern
    create
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c,
    parse-name evaluate parse-name evaluate color c, ;i

tile-pattern A-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000110
0b10000110
0b00000000
tile-pattern B-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000110
0b11111110
0b00000000
tile-pattern C-pattern
0b11111100
0b10000110
0b10000000
0b10000000
0b10000000
0b10000110
0b01111110
0b00000000 
tile-pattern D-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b11111110
0b00000000
tile-pattern E-pattern
0b11111100
0b10000000
0b10000000
0b10000000
0b11111110
0b10000000
0b11111110
0b00000000
tile-pattern F-pattern
0b11111100
0b10000000
0b10000000
0b10000000
0b11111110
0b10000000
0b10000000
0b00000000
tile-pattern G-pattern
0b11111100
0b10000110
0b10000000
0b10000000
0b11111100
0b10000110
0b01111110
0b00000000
tile-pattern H-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000110
0b10000110
0b00000000
tile-pattern I-pattern
0b00010000
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00000000
tile-pattern J-pattern
0b11111000
0b00001100
0b00001100
0b00001100
0b00001100
0b10001100
0b01111100
0b00000000
tile-pattern K-pattern
0b10001000
0b10001100
0b10001100
0b10011000
0b11100000
0b10011100
0b10001100
0b00000000
tile-pattern L-pattern
0b11000000
0b11000000
0b11000000
0b11000000
0b11000000
0b11000000
0b11111110
0b00000000
tile-pattern M-pattern
0b11001100
0b10110110
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b00000000
tile-pattern N-pattern
0b11000110
0b11000110
0b10100110
0b10010110
0b10001110
0b10000110
0b10000110
0b00000000
tile-pattern O-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b01111110
0b00000000
tile-pattern P-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10000000
0b10000000
0b00000000
tile-pattern Q-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b10010110
0b10001110
0b01111110
0b00000000
tile-pattern R-pattern
0b11111100
0b10000110
0b10000110
0b10000110
0b11111110
0b10011100
0b10001100
0b00000000
tile-pattern S-pattern
0b11111000
0b10000000
0b10000000
0b10000000
0b11111100
0b00000110
0b11111110
0b00000000
tile-pattern T-pattern
0b11111110
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00011000
0b00000000
tile-pattern U-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b10000110
0b10000110
0b11111110
0b00000000
tile-pattern V-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b10000110
0b01001110
0b00111100
0b00000000
tile-pattern W-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b10110110
0b10110110
0b11111110
0b00000000
tile-pattern X-pattern
0b10000100
0b11000110
0b00101100
0b00010000
0b00101100
0b11000110
0b10000110
0b00000000
tile-pattern Y-pattern
0b10000100
0b10000110
0b10000110
0b10000110
0b01111100
0b00011000
0b00011000
0b00000000
tile-pattern Z-pattern
0b11111100
0b00001110
0b00001100
0b00011000
0b01110000
0b10000000
0b11111110
0b00000000

color-pattern char-color
COLOR-LIGHT-RED  COLOR-BLACK
COLOR-DARK-RED   COLOR-BLACK
COLOR-DARK-RED   COLOR-BLACK
COLOR-DARK-RED   COLOR-BLACK
COLOR-LIGHT-RED  COLOR-BLACK
COLOR-MEDIUM-RED COLOR-BLACK
COLOR-MEDIUM-RED COLOR-BLACK
COLOR-MEDIUM-RED COLOR-BLACK

: char-vaddr ( c -- v-addr )
    3 lshift ;

: char-color-vaddr ( c -- v-addr )
    3 lshift 0x2000 + ;

: ram-to-3banks ( addr vaddr -- )
    2dup swap 8 -rot ram-to-vram
    2dup 2048 + swap 8 -rot ram-to-vram
    4096 + swap 8 -rot ram-to-vram ;

: redefine-pattern ( addr char -- )
    char-vaddr ram-to-3banks ;

: redefine-color ( addr char -- )
    char-color-vaddr ram-to-3banks ;

: redefine-tile ( pattern color char -- )
    2dup redefine-color
    nip redefine-pattern ;

: redefine-char ( pattern char -- )
    ['] char-color swap redefine-tile ;

: clear-tiles ( -- )
    0 0x2000 0x2000 fill-vram ;

: redefine-tiles ( -- )
    clear-tiles
    ['] A-pattern [char] A redefine-char
    ['] B-pattern [char] B redefine-char
    ['] C-pattern [char] C redefine-char 
    ['] D-pattern [char] D redefine-char
    ['] E-pattern [char] E redefine-char
    ['] F-pattern [char] F redefine-char
    ['] G-pattern [char] G redefine-char 
    ['] H-pattern [char] H redefine-char
    ['] I-pattern [char] I redefine-char
    ['] J-pattern [char] J redefine-char
    ['] K-pattern [char] K redefine-char 
    ['] L-pattern [char] L redefine-char
    ['] M-pattern [char] M redefine-char
    ['] N-pattern [char] N redefine-char
    ['] O-pattern [char] O redefine-char 
    ['] P-pattern [char] P redefine-char
    ['] Q-pattern [char] Q redefine-char
    ['] R-pattern [char] R redefine-char
    ['] S-pattern [char] S redefine-char
    ['] T-pattern [char] T redefine-char
    ['] U-pattern [char] U redefine-char 
    ['] V-pattern [char] V redefine-char
    ['] W-pattern [char] W redefine-char
    ['] X-pattern [char] X redefine-char
    ['] Y-pattern [char] Y redefine-char
    ['] Z-pattern [char] Z redefine-char ;

: clear-screen
    0 0x1800 768 fill-vram ;

variable cursor

: locate ( x y -- )
    32 * + 0x1800 + cursor ! ;

: 1+! ( addr -- )
    dup @ 1+ swap ! ;

: emit ( n -- )
    cursor @ vram! 
    cursor 1+! ;

: type ( c-addr +n -- )
    ?dup if
        over + swap do i c@ emit loop
    else drop then ;

: cr
    cursor @ 0b1111111111100000 and 32 + cursor ! ;

: show-message
    0 0 locate
    s" BY" type cr
    s" VICTOR MARZO" type ;

: init-screen
    0 0 15 change-color
    init-graph
    clear-screen
    redefine-tiles ;

: main
    disable-screen
    init-screen
    show-message
    enable-screen
    abort ;
